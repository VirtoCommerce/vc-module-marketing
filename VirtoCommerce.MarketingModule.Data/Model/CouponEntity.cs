using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
    public class CouponEntity : AuditableEntity
    {
        public CouponEntity()
        {
        }

        [StringLength(64)]
        [Index("IX_CodeAndPromotionId", IsUnique = true, Order = 1)]
        public string Code { get; set; }

        public int MaxUsesNumber { get; set; }

        public int MaxUsesPerUser { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [NotMapped]
        public long TotalUsesCount { get; set; }

        #region Navigation Properties
        [Index("IX_CodeAndPromotionId", IsUnique = true, Order = 2)]
        public string PromotionId { get; set; }
        public virtual PromotionEntity Promotion { get; set; }
        
        #endregion

        public virtual Coupon ToModel(Coupon coupon)
        {
            if (coupon == null)
            {
                throw new ArgumentNullException(nameof(coupon));
            }

            coupon.Code = this.Code;
            coupon.CreatedBy = this.CreatedBy;
            coupon.CreatedDate = this.CreatedDate;
            coupon.ExpirationDate = this.ExpirationDate;
            coupon.Id = this.Id;
            coupon.MaxUsesNumber = this.MaxUsesNumber;
            coupon.ModifiedBy = this.ModifiedBy;
            coupon.ModifiedDate = this.ModifiedDate;
            coupon.MaxUsesNumber = this.MaxUsesNumber;
            coupon.PromotionId = this.PromotionId;
            coupon.TotalUsesCount = this.TotalUsesCount;
            coupon.MaxUsesPerUser = this.MaxUsesPerUser;
    
            return coupon;
        }

        public virtual CouponEntity FromModel(Coupon coupon, PrimaryKeyResolvingMap pkMap)
        {
            if (coupon == null)
            {
                throw new ArgumentNullException(nameof(coupon));
            }

            pkMap.AddPair(coupon, this);

            this.Code = coupon.Code;
            this.CreatedBy = coupon.CreatedBy;
            this.CreatedDate = coupon.CreatedDate;
            this.ExpirationDate = coupon.ExpirationDate;
            this.Id = coupon.Id;
            this.MaxUsesNumber = coupon.MaxUsesNumber;
            this.MaxUsesPerUser = coupon.MaxUsesPerUser;
            this.ModifiedBy = coupon.ModifiedBy;
            this.ModifiedDate = coupon.ModifiedDate;
            this.MaxUsesNumber = coupon.MaxUsesNumber;
            this.PromotionId = coupon.PromotionId;
            this.TotalUsesCount = coupon.TotalUsesCount;
            return this;
        }

        public virtual void Patch(CouponEntity target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            target.Code = this.Code;
            target.ExpirationDate = this.ExpirationDate;
            target.MaxUsesNumber = this.MaxUsesNumber;
            target.MaxUsesPerUser = this.MaxUsesPerUser;
        }
    }
}
