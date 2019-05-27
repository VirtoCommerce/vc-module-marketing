using System;
using System.Linq;
using System.Web.Http;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.MarketingModule.Data.Handlers;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.MarketingModule.Web.ExportImport;
using VirtoCommerce.MarketingModule.Web.JsonConverters;
using VirtoCommerce.MarketingModule.Web.Security;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Serialization;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using DynamicContentItem = VirtoCommerce.Domain.Marketing.Model.DynamicContentItem;

namespace VirtoCommerce.MarketingModule.Web
{
    public class Module : ModuleBase, ISupportExportImportModule
    {
        private readonly string _connectionString = ConfigurationHelper.GetConnectionStringValue("VirtoCommerce.Marketing") ?? ConfigurationHelper.GetConnectionStringValue("VirtoCommerce");
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public override void SetupDatabase()
        {
            using (var context = new MarketingRepositoryImpl(_connectionString, _container.Resolve<AuditableInterceptor>()))
            {
                var initializer = new SetupDatabaseInitializer<MarketingRepositoryImpl, VirtoCommerce.MarketingModule.Data.Migrations.Configuration>();
                initializer.InitializeDatabase(context);
            }
        }

        public override void Initialize()
        {
            _container.RegisterType<IMarketingRepository>(new InjectionFactory(c => new MarketingRepositoryImpl(_connectionString, new EntityPrimaryKeyGeneratorInterceptor(), _container.Resolve<AuditableInterceptor>())));

            var promotionExtensionManager = new DefaultMarketingExtensionManagerImpl();

            _container.RegisterInstance<IMarketingExtensionManager>(promotionExtensionManager);
            _container.RegisterType<IPromotionService, PromotionServiceImpl>();
            _container.RegisterType<ICouponService, CouponService>();
            _container.RegisterType<IPromotionUsageService, PromotionUsageService>();
            _container.RegisterType<IMarketingDynamicContentEvaluator, DefaultDynamicContentEvaluatorImpl>();
            _container.RegisterType<IDynamicContentService, DynamicContentServiceImpl>();

            _container.RegisterType<IPromotionSearchService, MarketingSearchServiceImpl>();
            _container.RegisterType<ICouponService, CouponService>();
            _container.RegisterType<IDynamicContentSearchService, MarketingSearchServiceImpl>();


            var settingsManager = _container.Resolve<ISettingsManager>();
            var promotionCombinePolicy = settingsManager.GetValue("Marketing.Promotion.CombinePolicy", "BestReward");
            if (promotionCombinePolicy.EqualsInvariant("CombineStackable"))
            {
                _container.RegisterType<IMarketingPromoEvaluator, CombineStackablePromotionPolicy>();
            }
            else
            {
                _container.RegisterType<IMarketingPromoEvaluator, BestRewardPromotionPolicy>();
            }

            var eventHandlerRegistrar = _container.Resolve<IHandlerRegistrar>();
            //Create order observer. record order coupon usage
            eventHandlerRegistrar.RegisterHandler<OrderChangedEvent>(async (message, token) => await _container.Resolve<CouponUsageRecordHandler>().Handle(message));

            AbstractTypeFactory<DynamicPromotion>.RegisterType<DynamicPromotion>().WithFactory(() => _container.Resolve<DynamicPromotion>());
            AbstractTypeFactory<MarketingExportImport>.RegisterType<MarketingExportImport>().WithFactory(() => _container.Resolve<MarketingExportImport>());
        }

        public override void PostInitialize()
        {
            EnsureRootFoldersExist(new[] { VirtoCommerce.MarketingModule.Web.Model.MarketingConstants.ContentPlacesRootFolderId, VirtoCommerce.MarketingModule.Web.Model.MarketingConstants.CotentItemRootFolderId });

            //Create standard dynamic properties for dynamic content item
            var dynamicPropertyService = _container.Resolve<IDynamicPropertyService>();
            var contentItemTypeProperty = new DynamicProperty
            {
                Id = "Marketing_DynamicContentItem_Type_Property",
                IsDictionary = true,
                Name = "Content type",
                ObjectType = typeof(DynamicContentItem).FullName,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto",
            };

            dynamicPropertyService.SaveProperties(new[] { contentItemTypeProperty });

            var securityScopeService = _container.Resolve<IPermissionScopeService>();
            securityScopeService.RegisterSope(() => new MarketingSelectedStoreScope());


            //Next lines allow to use polymorph types in API controller methods
            var httpConfiguration = _container.Resolve<HttpConfiguration>();
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new PolymorphicMarketingJsonConverter(_container.Resolve<IMarketingExtensionManager>(),
                _container.Resolve<IExpressionSerializer>()));
        }

        #endregion

        #region ISupportExportImportModule Members

        public void DoExport(System.IO.Stream outStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var exportJob = AbstractTypeFactory<MarketingExportImport>.TryCreateInstance();
            exportJob.DoExport(outStream, progressCallback);
        }

        public void DoImport(System.IO.Stream inputStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var exportJob = AbstractTypeFactory<MarketingExportImport>.TryCreateInstance();
            exportJob.DoImport(inputStream, progressCallback);
        }

        public string ExportDescription
        {
            get
            {
                var settingManager = _container.Resolve<ISettingsManager>();
                return settingManager.GetValue("Marketing.ExportImport.Description", String.Empty);
            }
        }
        #endregion


        private void EnsureRootFoldersExist(string[] ids)
        {
            var dynamicContentService = _container.Resolve<IDynamicContentService>();
            foreach (var id in ids)
            {
                var rootFolder = dynamicContentService.GetFoldersByIds(new[] { id }).FirstOrDefault();
                if (rootFolder == null)
                {
                    rootFolder = new Domain.Marketing.Model.DynamicContentFolder
                    {
                        Id = id,
                        Name = id
                    };
                    dynamicContentService.SaveFolders(new[] { rootFolder });
                }
            }
        }
    }
}
