using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.MarketingModule.Core;
using VirtoCommerce.MarketingModule.Core.Events;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Promotions;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.ExportImport;
using VirtoCommerce.MarketingModule.Data.Handlers;
using VirtoCommerce.MarketingModule.Data.MySql;
using VirtoCommerce.MarketingModule.Data.PostgreSql;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Search;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.MarketingModule.Data.SqlServer;
using VirtoCommerce.MarketingModule.Web.Authorization;
using VirtoCommerce.MarketingModule.Web.ExportImport;
using VirtoCommerce.OrdersModule.Core.Events;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.JsonConverters;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Extensions;
using VirtoCommerce.Platform.Data.MySql.Extensions;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;
using VirtoCommerce.Platform.Data.SqlServer.Extensions;

namespace VirtoCommerce.MarketingModule.Web
{
    [ExcludeFromCodeCoverage]
    public class Module : IModule, IExportSupport, IImportSupport, IHasConfiguration
    {
        private IApplicationBuilder _appBuilder;

        public ManifestModuleInfo ModuleInfo { get; set; }
        public IConfiguration Configuration { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<MarketingDbContext>(options =>
            {
                var databaseProvider = Configuration.GetValue("DatabaseProvider", "SqlServer");
                var connectionString = Configuration.GetConnectionString(ModuleInfo.Id) ?? Configuration.GetConnectionString("VirtoCommerce");

                switch (databaseProvider)
                {
                    case "MySql":
                        options.UseMySqlDatabase(connectionString, typeof(MySqlDataAssemblyMarker), Configuration);
                        break;
                    case "PostgreSql":
                        options.UsePostgreSqlDatabase(connectionString, typeof(PostgreSqlDataAssemblyMarker), Configuration);
                        break;
                    default:
                        options.UseSqlServerDatabase(connectionString, typeof(SqlServerDataAssemblyMarker), Configuration);
                        break;
                }
            });

            serviceCollection.AddTransient<IMarketingRepository, MarketingRepository>();
            serviceCollection.AddTransient<Func<IMarketingRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IMarketingRepository>());

            serviceCollection.AddTransient<IPromotionService, PromotionService>();
            serviceCollection.AddTransient<ICouponService, CouponService>();
            serviceCollection.AddTransient<IPromotionUsageService, PromotionUsageService>();
            serviceCollection.AddTransient<IMarketingDynamicContentEvaluator, DefaultDynamicContentEvaluator>();
            serviceCollection.AddTransient<IDynamicContentService, DynamicContentService>();
            serviceCollection.AddTransient<IPromotionRewardEvaluator, DefaultPromotionRewardEvaluator>();

            serviceCollection.AddTransient<IContentItemsSearchService, ContentItemsSearchService>();
            serviceCollection.AddTransient<IContentPlacesSearchService, ContentPlacesSearchService>();
            serviceCollection.AddTransient<IContentPublicationsSearchService, ContentPublicationsSearchService>();
            serviceCollection.AddTransient<ICouponSearchService, CouponSearchService>();
            serviceCollection.AddTransient<IFolderSearchService, FolderSearchService>();
            serviceCollection.AddTransient<IPromotionSearchService, PromotionSearchService>();
            serviceCollection.AddTransient<IPromotionUsageSearchService, PromotionUsageSearchService>();

            serviceCollection.AddTransient<CsvCouponImporter>();

            serviceCollection.AddTransient<IMarketingPromoEvaluator>(provider =>
            {
                var settingsManager = provider.GetService<ISettingsManager>();
                var platformMemoryCache = provider.GetService<IPlatformMemoryCache>();
                var promotionService = provider.GetService<IPromotionSearchService>();
                var promotionCombinePolicy = settingsManager.GetValue<string>(ModuleConstants.Settings.General.CombinePolicy);

                if (promotionCombinePolicy.EqualsInvariant("CombineStackable"))
                {
                    var promotionRewardEvaluator = provider.GetService<IPromotionRewardEvaluator>();
                    return new CombineStackablePromotionPolicy(promotionService, promotionRewardEvaluator, platformMemoryCache);
                }

                return new BestRewardPromotionPolicy(promotionService, platformMemoryCache);
            });

            serviceCollection.AddTransient<LogChangesChangedEventHandler>();
            serviceCollection.AddTransient<MarketingExportImport>();
            serviceCollection.AddTransient<CouponUsageRecordHandler>();

            serviceCollection.AddTransient<IAuthorizationHandler, MarketingAuthorizationHandler>();
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            _appBuilder = appBuilder;

            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(ModuleConstants.Settings.General.AllSettings, ModuleInfo.Id);

            var permissionsRegistrar = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsRegistrar.RegisterPermissions(ModuleInfo.Id, "Marketing", ModuleConstants.Security.Permissions.AllPermissions);

            //Register Permission scopes
            AbstractTypeFactory<PermissionScope>.RegisterType<MarketingStoreSelectedScope>();
            permissionsRegistrar.WithAvailabeScopesForPermissions([ModuleConstants.Security.Permissions.Read], new MarketingStoreSelectedScope());

            // Register DynamicPromotion override
            var couponSearchService = appBuilder.ApplicationServices.GetRequiredService<ICouponSearchService>();
            var promotionUsageSearchService = appBuilder.ApplicationServices.GetRequiredService<IPromotionUsageSearchService>();
            AbstractTypeFactory<Promotion>.RegisterType<DynamicPromotion>().WithSetupAction(promotion =>
            {
                var dynamicPromotion = (DynamicPromotion)promotion;
                dynamicPromotion.CouponSearchService = couponSearchService;
                dynamicPromotion.PromotionUsageSearchService = promotionUsageSearchService;
            });

            //Create order observer. record order coupon usage
            appBuilder.RegisterEventHandler<OrderChangedEvent, CouponUsageRecordHandler>();
            appBuilder.RegisterEventHandler<PromotionChangedEvent, LogChangesChangedEventHandler>();
            appBuilder.RegisterEventHandler<CouponChangedEvent, LogChangesChangedEventHandler>();

            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                var databaseProvider = Configuration.GetValue("DatabaseProvider", "SqlServer");

                var dbContext = serviceScope.ServiceProvider.GetRequiredService<MarketingDbContext>();
                if (databaseProvider == "SqlServer")
                {
                    dbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationName(ModuleInfo.Id));
                }
                dbContext.Database.Migrate();
            }

            var dynamicPropertyRegistrar = appBuilder.ApplicationServices.GetRequiredService<IDynamicPropertyRegistrar>();
            dynamicPropertyRegistrar.RegisterType<DynamicContentItem>();

            var dynamicContentService = appBuilder.ApplicationServices.GetService<IDynamicContentService>();
            foreach (var id in new[] { ModuleConstants.MarketingConstants.ContentPlacesRootFolderId, ModuleConstants.MarketingConstants.ContentItemRootFolderId })
            {
                var folders = dynamicContentService.GetFoldersByIdsAsync([id]).GetAwaiter().GetResult();
                var rootFolder = folders.FirstOrDefault();
                if (rootFolder == null)
                {
                    rootFolder = new DynamicContentFolder
                    {
                        Id = id,
                        Name = id
                    };
                    dynamicContentService.SaveFoldersAsync([rootFolder]).GetAwaiter().GetResult();
                }
            }

            //Create standard dynamic properties for dynamic content item
            var dynamicPropertyService = appBuilder.ApplicationServices.GetService<IDynamicPropertyService>();
            var contentItemTypeProperty = new DynamicProperty
            {
                Id = "Marketing_DynamicContentItem_Type_Property",
                IsDictionary = true,
                Name = "Content type",
                ObjectType = typeof(DynamicContentItem).FullName,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto",
            };

            dynamicPropertyService.SaveDynamicPropertiesAsync([contentItemTypeProperty]).GetAwaiter().GetResult();

            PolymorphJsonConverter.RegisterTypeForDiscriminator(typeof(PromotionReward), nameof(PromotionReward.Id));

            //Register the resulting trees expressions into the AbstractFactory<IConditionTree> 
            foreach (var conditionTree in AbstractTypeFactory<PromotionConditionAndRewardTreePrototype>.TryCreateInstance().Traverse<IConditionTree>(x => x.AvailableChildren))
            {
                AbstractTypeFactory<IConditionTree>.RegisterType(conditionTree.GetType());
            }
            foreach (var conditionTree in AbstractTypeFactory<DynamicContentConditionTreePrototype>.TryCreateInstance().Traverse<IConditionTree>(x => x.AvailableChildren))
            {
                AbstractTypeFactory<IConditionTree>.RegisterType(conditionTree.GetType());
            }
        }

        public void Uninstall()
        {
            // Method intentionally left empty.
        }

        public Task ExportAsync(Stream outStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            return _appBuilder.ApplicationServices.GetRequiredService<MarketingExportImport>().DoExportAsync(outStream, options, progressCallback, cancellationToken);
        }

        public Task ImportAsync(Stream inputStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            return _appBuilder.ApplicationServices.GetRequiredService<MarketingExportImport>().DoImportAsync(inputStream, options, progressCallback, cancellationToken);
        }
    }
}
