using System;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using dataModel = VirtoCommerce.MarketingModule.Data.Model;

namespace VirtoCommerce.MarketingModule.Data.Converters
{
    public static class CouponConverter
    {
        public static coreModel.Coupon ToCoreModel(this dataModel.Coupon dataModel)
        {
            if (dataModel == null)
            {
                throw new ArgumentNullException("dataModel");
            }

            var coreModel = new coreModel.Coupon
            {
                Code = dataModel.Code,
                CreatedBy = dataModel.CreatedBy,
                CreatedDate = dataModel.CreatedDate,
                Id = dataModel.Id,
                MaxUsesNumber = dataModel.MaxUsesNumber,
                ModifiedBy = dataModel.ModifiedBy,
                ModifiedDate = dataModel.ModifiedDate,
                PromotionId = dataModel.PromotionId
            };

            coreModel.Code = dataModel.Code;
            coreModel.MaxUsesNumber = dataModel.MaxUsesNumber;

            return coreModel;
        }

        public static dataModel.Coupon ToDataModel(this coreModel.Coupon coreModel)
        {
            if (coreModel == null)
            {
                throw new ArgumentNullException("coreModel");
            }

            var dataModel = new dataModel.Coupon
            {
                Code = coreModel.Code,
                CreatedBy = coreModel.CreatedBy,
                CreatedDate = coreModel.CreatedDate,
                Id = coreModel.Id,
                MaxUsesNumber = coreModel.MaxUsesNumber,
                ModifiedBy = coreModel.ModifiedBy,
                ModifiedDate = coreModel.ModifiedDate,
                PromotionId = coreModel.PromotionId
            };

            return dataModel;
        }
    }
}