using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingSampleModule.Web.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.MarketingSampleModule.Web
{
    public class Module : IModule
    {
        public ManifestModuleInfo ModuleInfo { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            // Put the prototype override in this method as:
            // 1. All required types were registered by VirtoCommerce.Marketing already;
            // 2. PostInitialize would be executed too late: after VirtoCommerce.Marketing traverses the prototype.
            AbstractTypeFactory<PromotionConditionAndRewardTreePrototype>.OverrideType<PromotionConditionAndRewardTreePrototype, SamplePromotionConditionAndRewardTreePrototype>();
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            // do nothing in here
        }

        public void Uninstall()
        {
            // do nothing in here
        }
    }
}
