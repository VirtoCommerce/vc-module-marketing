using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
    public static class PromotionRewardConverter
    {
        public static webModel.PromotionReward ToWebModel(this coreModel.PromotionReward reward)
        {
            var retVal = AbstractTypeFactory<webModel.PromotionReward>.TryCreateInstance();
            retVal.InjectFrom(reward);
            retVal.RewardType = reward.GetType().Name;
            return retVal;
        }
    
    }
}
