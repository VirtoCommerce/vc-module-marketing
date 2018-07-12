using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
    public class PromotionStoreEntity : Entity
    {
        public string PromotionId { get; set; }

        public virtual PromotionEntity Promotion { get; set; }

        [StringLength(128)]
        [Required]
        [Index]
        public string StoreId { get; set; }
    }
}
