using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
    public class PromotionEntity : AuditableEntity
    {
        [StringLength(128)]
        public string StoreId { get; set; }

        [StringLength(128)]
        public string CatalogId { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Priority { get; set; }

        public bool IsExclusive { get; set; }
        public bool IsAllowCombiningWithSelf { get; set; }

        [NotMapped]
        public bool HasCoupons { get; set; }

        public string PredicateSerialized { get; set; }

        public string PredicateVisualTreeSerialized { get; set; }

        public string RewardsSerialized { get; set; }

        public int PerCustomerLimit { get; set; }

        public int TotalLimit { get; set; }

        public virtual ObservableCollection<PromotionStoreEntity> Stores { get; set; } = new NullCollection<PromotionStoreEntity>();

        public virtual Promotion ToModel(DynamicPromotion promotion)
        {
            if (promotion == null)
            {
                throw new ArgumentNullException(nameof(promotion));
            }

            promotion.Id = this.Id;
            promotion.CreatedBy = this.CreatedBy;
            promotion.CreatedDate = this.CreatedDate;
            promotion.ModifiedBy = this.ModifiedBy;
            promotion.ModifiedDate = this.ModifiedDate;
            promotion.StartDate = this.StartDate;
            promotion.EndDate = this.EndDate;
            promotion.Store = this.StoreId;
            promotion.Name = this.Name;
            promotion.Description = this.Description;
            promotion.IsActive = this.IsActive;
            promotion.EndDate = this.EndDate;
            promotion.Priority = this.Priority;
            promotion.IsExclusive = this.IsExclusive;
            promotion.IsAllowCombiningWithSelf = this.IsAllowCombiningWithSelf;
            promotion.PredicateVisualTreeSerialized = this.PredicateVisualTreeSerialized;
            promotion.PredicateSerialized = this.PredicateSerialized;
            promotion.RewardsSerialized = this.RewardsSerialized;
            promotion.MaxPersonalUsageCount = this.PerCustomerLimit;
            promotion.MaxUsageCount = this.TotalLimit;
            promotion.MaxPersonalUsageCount = this.PerCustomerLimit;
            promotion.HasCoupons = this.HasCoupons;


            if (!string.IsNullOrEmpty(promotion.PredicateVisualTreeSerialized))
            {
                //Temporary back data compatibility fix for serialized expressions
                promotion.PredicateVisualTreeSerialized = promotion.PredicateVisualTreeSerialized.Replace("VirtoCommerce.DynamicExpressionModule.", "VirtoCommerce.DynamicExpressionsModule.");
            }
            if (!string.IsNullOrEmpty(promotion.PredicateSerialized))
            {
                //Temporary back data compatibility fix for serialized expressions
                promotion.PredicateSerialized = promotion.PredicateSerialized.Replace("VirtoCommerce.DynamicExpressionModule.", "VirtoCommerce.DynamicExpressionsModule.");
            }

            if (this.Stores != null)
            {
                promotion.StoreIds = this.Stores.Select(x => x.StoreId).ToList();
            }

            return promotion;
        }

        public virtual PromotionEntity FromModel(DynamicPromotion promotion, PrimaryKeyResolvingMap pkMap)
        {
            if (promotion == null)
            {
                throw new ArgumentNullException(nameof(promotion));
            }

            pkMap.AddPair(promotion, this);

            this.Id = promotion.Id;
            this.CreatedBy = promotion.CreatedBy;
            this.CreatedDate = promotion.CreatedDate;
            this.ModifiedBy = promotion.ModifiedBy;
            this.ModifiedDate = promotion.ModifiedDate;
            this.StartDate = promotion.StartDate ?? DateTime.UtcNow;
            this.EndDate = promotion.EndDate;
            this.StoreId = promotion.Store;
            this.Name = promotion.Name;
            this.Description = promotion.Description;
            this.IsActive = promotion.IsActive;
            this.EndDate = promotion.EndDate;
            this.Priority = promotion.Priority;
            this.IsExclusive = promotion.IsExclusive;
            this.IsAllowCombiningWithSelf = promotion.IsAllowCombiningWithSelf;
            this.PredicateVisualTreeSerialized = promotion.PredicateVisualTreeSerialized;
            this.PredicateSerialized = promotion.PredicateSerialized;
            this.RewardsSerialized = promotion.RewardsSerialized;
            this.PerCustomerLimit = promotion.MaxPersonalUsageCount;
            this.TotalLimit = promotion.MaxUsageCount;
            this.PerCustomerLimit = promotion.MaxPersonalUsageCount;

            if (promotion.StoreIds != null)
            {
                this.Stores = new ObservableCollection<PromotionStoreEntity>(promotion.StoreIds.Select(x => new PromotionStoreEntity { StoreId = x, PromotionId = promotion.Id }));
            }

            return this;
        }

        public virtual void Patch(PromotionEntity target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            target.StartDate = this.StartDate;
            target.EndDate = this.EndDate;
            target.StoreId = this.StoreId;
            target.Name = this.Name;
            target.Description = this.Description;
            target.IsActive = this.IsActive;
            target.IsExclusive = this.IsExclusive;
            target.EndDate = this.EndDate;
            target.Priority = this.Priority;
            target.PredicateVisualTreeSerialized = this.PredicateVisualTreeSerialized;
            target.PredicateSerialized = this.PredicateSerialized;
            target.RewardsSerialized = this.RewardsSerialized;
            target.PerCustomerLimit = this.PerCustomerLimit;
            target.TotalLimit = this.TotalLimit;
            target.PerCustomerLimit = this.PerCustomerLimit;
            target.IsAllowCombiningWithSelf = this.IsAllowCombiningWithSelf;

            if (!Stores.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((PromotionStoreEntity entity) => entity.StoreId);
                Stores.Patch(target.Stores, comparer, (sourceEntity, targetEntity) => targetEntity.StoreId = sourceEntity.StoreId);
            }
        }
    }
}
