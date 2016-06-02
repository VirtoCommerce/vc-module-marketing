using System;
using System.Collections.ObjectModel;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Serialization;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using dataModel = VirtoCommerce.MarketingModule.Data.Model;

namespace VirtoCommerce.MarketingModule.Data.Converters
{
    public static class PromotionConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <returns></returns>
        public static coreModel.Promotion ToCoreModel(this dataModel.Promotion dbEntity, IExpressionSerializer expressionSerializer)
        {
            if (dbEntity == null)
                throw new ArgumentNullException("dbEntity");

            var retVal = new DynamicPromotion(expressionSerializer);
            retVal.InjectFrom(dbEntity);
            if(!string.IsNullOrEmpty(retVal.PredicateVisualTreeSerialized))
            {
                //Temporary back data compatibility fix for serialized expressions
                retVal.PredicateVisualTreeSerialized = retVal.PredicateVisualTreeSerialized.Replace("VirtoCommerce.DynamicExpressionModule.", "VirtoCommerce.DynamicExpressionsModule.");
            }
            retVal.Coupons = dbEntity.Coupons.Select(x => x.Code).ToArray();
            retVal.Store = dbEntity.StoreId;
            retVal.MaxUsageCount = dbEntity.TotalLimit;
            retVal.MaxPersonalUsageCount = dbEntity.PerCustomerLimit;
            return retVal;
        }

        public static dataModel.Promotion ToDataModel(this coreModel.Promotion promotion)
        {
            if (promotion == null)
                throw new ArgumentNullException("promotion");

            var retVal = new dataModel.Promotion { StartDate = DateTime.UtcNow };
            retVal.InjectFrom(promotion);

            retVal.StartDate = promotion.StartDate ?? DateTime.UtcNow;
            retVal.StoreId = promotion.Store;
            if (promotion.Coupons != null)
            {
                retVal.Coupons = new ObservableCollection<dataModel.Coupon>(promotion.Coupons.Select(x => new dataModel.Coupon { Code = x }));
            }
            retVal.TotalLimit = promotion.MaxUsageCount;
            retVal.PerCustomerLimit = promotion.MaxPersonalUsageCount;

            return retVal;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this dataModel.Promotion source, dataModel.Promotion target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<dataModel.Promotion>(x => x.Name, x => x.Description, x => x.Priority, x => x.CouponCode, x => x.StoreId,
                                                                           x => x.StartDate, x => x.EndDate, x => x.IsActive, x => x.TotalLimit, x => x.PerCustomerLimit, x => x.PredicateSerialized,
                                                                           x => x.PredicateVisualTreeSerialized, x => x.RewardsSerialized);

            target.InjectFrom(patchInjection, source);

            if (!source.Coupons.IsNullCollection())
            {
                source.Coupons.Patch(target.Coupons, (sourceCoupon, targetCoupon) => { });
            }
        }
    }
}
