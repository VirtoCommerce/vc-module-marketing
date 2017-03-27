using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Data.Model
{
	public class PromotionUsageEntity : AuditableEntity
	{	
		[StringLength(128)]
		public string ObjectId { get; set; }
		[StringLength(128)]
		public string ObjectType { get; set; }

        [StringLength(64)]
        public string CouponCode { get; set; }

        #region Navigation Properties
        public string PromotionId { get; set; }
        public virtual PromotionEntity Promotion { get; set; }
        #endregion

        public virtual PromotionUsage ToModel(PromotionUsage usage)
        {
            if (usage == null)
                throw new NullReferenceException(nameof(usage));

            usage.Id = this.Id;
            usage.CreatedBy = this.CreatedBy;
            usage.CreatedDate = this.CreatedDate;
            usage.ModifiedBy = this.ModifiedBy;
            usage.ModifiedDate = this.ModifiedDate;
            usage.CouponCode = this.CouponCode;
            usage.ObjectId = this.ObjectId;
            usage.ObjectType = this.ObjectType;
            usage.PromotionId = this.PromotionId;

            return usage;
        }

        public virtual PromotionUsageEntity FromModel(PromotionUsage usage, PrimaryKeyResolvingMap pkMap)
        {
            if (usage == null)
                throw new NullReferenceException(nameof(usage));

            pkMap.AddPair(usage, this);

            this.Id = usage.Id;
            this.CreatedBy = usage.CreatedBy;
            this.CreatedDate = usage.CreatedDate;
            this.ModifiedBy = usage.ModifiedBy;
            this.ModifiedDate = usage.ModifiedDate;
            this.CouponCode = usage.CouponCode;
            this.PromotionId = usage.PromotionId;      
            this.ObjectId = usage.ObjectId;
            this.ObjectType = usage.ObjectType;
           
            return this;
        }

        public virtual void Patch(PromotionUsageEntity target)
        {
            if (target == null)
                throw new NullReferenceException(nameof(target));

            target.ObjectId = this.ObjectId;
            target.ObjectType = this.ObjectType;         
        }

    }
}
