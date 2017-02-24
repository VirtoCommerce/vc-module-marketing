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
                throw new ArgumentNullException(nameof(dbEntity));

            var result = DynamicPromotion.CreateInstance(expressionSerializer);
            result.InjectFrom(dbEntity);
            result.StartDate = dbEntity.StartDate;
            result.EndDate = dbEntity.EndDate;

            if (!string.IsNullOrEmpty(result.PredicateVisualTreeSerialized))
            {
                //Temporary back data compatibility fix for serialized expressions
                result.PredicateVisualTreeSerialized = result.PredicateVisualTreeSerialized.Replace("VirtoCommerce.DynamicExpressionModule.", "VirtoCommerce.DynamicExpressionsModule.");
            }

            if (!string.IsNullOrEmpty(result.PredicateSerialized))
            {
                //Temporary back data compatibility fix for serialized expressions
                result.PredicateSerialized = result.PredicateSerialized.Replace("VirtoCommerce.DynamicExpressionModule.", "VirtoCommerce.DynamicExpressionsModule.");
            }

            result.Coupons = dbEntity.Coupons.Select(x => x.Code).ToArray();
            result.Store = dbEntity.StoreId;
            result.MaxUsageCount = dbEntity.TotalLimit;
            result.MaxPersonalUsageCount = dbEntity.PerCustomerLimit;

            return result;
        }

        public static dataModel.Promotion ToDataModel(this coreModel.Promotion promotion)
        {
            if (promotion == null)
                throw new ArgumentNullException(nameof(promotion));

            var result = new dataModel.Promotion { StartDate = DateTime.UtcNow };
            result.InjectFrom(promotion);

            result.StartDate = promotion.StartDate ?? DateTime.UtcNow;
            result.StoreId = promotion.Store;

            if (promotion.Coupons != null)
            {
                result.Coupons = new ObservableCollection<dataModel.Coupon>(promotion.Coupons.Select(x => new dataModel.Coupon { Code = x }));
            }

            result.TotalLimit = promotion.MaxUsageCount;
            result.PerCustomerLimit = promotion.MaxPersonalUsageCount;

            return result;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this dataModel.Promotion source, dataModel.Promotion target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var patchInjection = new PatchInjection<dataModel.Promotion>(x => x.Name, x => x.Description, x => x.Priority, x => x.CouponCode, x => x.StoreId,
                                                                         x => x.StartDate, x => x.EndDate, x => x.IsActive, x => x.TotalLimit, x => x.PerCustomerLimit,
                                                                         x => x.PredicateSerialized, x => x.PredicateVisualTreeSerialized, x => x.RewardsSerialized);

            target.InjectFrom(patchInjection, source);
            target.StartDate = source.StartDate;
            target.EndDate = source.EndDate;

            if (!source.Coupons.IsNullCollection())
            {
                source.Coupons.Patch(target.Coupons, (sourceCoupon, targetCoupon) => { });
            }
        }
    }
}
