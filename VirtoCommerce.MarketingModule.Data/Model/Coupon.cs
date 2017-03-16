using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
    public class Coupon : AuditableEntity
    {
        [StringLength(64)]
        public string Code { get; set; }

        public int MaxUsesNumber { get; set; }

        #region Navigation Properties

        [StringLength(128)]
        [ForeignKey("Promotion")]
        public string PromotionId { get; set; }
        public virtual Promotion Promotion { get; set; }
        #endregion
    }
}