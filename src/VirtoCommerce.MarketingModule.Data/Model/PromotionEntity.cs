using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Promotions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.JsonConverters;

namespace VirtoCommerce.MarketingModule.Data.Model;

public class PromotionEntity : AuditableEntity, IHasOuterId, IDataEntity<PromotionEntity, Promotion>
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

    [StringLength(128)]
    public string OuterId { get; set; }

    #region Navigation Properties

    public virtual ObservableCollection<PromotionStoreEntity> Stores { get; set; } = new NullCollection<PromotionStoreEntity>();

    #endregion

    public virtual Promotion ToModel(Promotion model)
    {
        ArgumentNullException.ThrowIfNull(model);

        model.Id = Id;
        model.CreatedBy = CreatedBy;
        model.CreatedDate = CreatedDate;
        model.ModifiedBy = ModifiedBy;
        model.ModifiedDate = ModifiedDate;
        model.OuterId = OuterId;

        model.StartDate = StartDate;
        model.EndDate = EndDate;
        model.Name = Name;
        model.Description = Description;
        model.IsActive = IsActive;
        model.EndDate = EndDate;
        model.Priority = Priority;
        model.IsExclusive = IsExclusive;
        model.MaxPersonalUsageCount = PerCustomerLimit;
        model.MaxUsageCount = TotalLimit;
        model.MaxPersonalUsageCount = PerCustomerLimit;
        model.HasCoupons = HasCoupons;

        if (Stores != null)
        {
            model.StoreIds = Stores.Select(x => x.StoreId).ToList();
        }

        if (model is DynamicPromotion dynamicPromotion)
        {
            dynamicPromotion.IsAllowCombiningWithSelf = IsAllowCombiningWithSelf;
            dynamicPromotion.DynamicExpression = AbstractTypeFactory<PromotionConditionAndRewardTree>.TryCreateInstance();

            if (PredicateVisualTreeSerialized != null)
            {
                dynamicPromotion.DynamicExpression = JsonConvert.DeserializeObject<PromotionConditionAndRewardTree>(PredicateVisualTreeSerialized, new ConditionJsonConverter(), new PolymorphJsonConverter());
            }
        }

        return model;
    }

    public virtual PromotionEntity FromModel(Promotion model, PrimaryKeyResolvingMap pkMap)
    {
        ArgumentNullException.ThrowIfNull(model);

        pkMap.AddPair(model, this);

        Id = model.Id;
        CreatedBy = model.CreatedBy;
        CreatedDate = model.CreatedDate;
        ModifiedBy = model.ModifiedBy;
        ModifiedDate = model.ModifiedDate;
        OuterId = model.OuterId;

        StartDate = model.StartDate ?? DateTime.UtcNow;
        EndDate = model.EndDate;
        Name = model.Name;
        Description = model.Description;
        IsActive = model.IsActive;
        EndDate = model.EndDate;
        Priority = model.Priority;
        IsExclusive = model.IsExclusive;

        PerCustomerLimit = model.MaxPersonalUsageCount;
        TotalLimit = model.MaxUsageCount;
        PerCustomerLimit = model.MaxPersonalUsageCount;

        if (model.StoreIds != null)
        {
            Stores = new ObservableCollection<PromotionStoreEntity>(model.StoreIds.Select(x => new PromotionStoreEntity { StoreId = x, PromotionId = model.Id }));
        }

        if (model is DynamicPromotion dynamicPromotion)
        {
            IsAllowCombiningWithSelf = dynamicPromotion.IsAllowCombiningWithSelf;

            if (dynamicPromotion.DynamicExpression != null)
            {
                PredicateVisualTreeSerialized = JsonConvert.SerializeObject(dynamicPromotion.DynamicExpression, new ConditionJsonConverter(doNotSerializeAvailCondition: true));
            }
        }

        return this;
    }

    public virtual void Patch(PromotionEntity target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.StartDate = StartDate;
        target.EndDate = EndDate;
        target.StoreId = StoreId;
        target.Name = Name;
        target.Description = Description;
        target.IsActive = IsActive;
        target.IsExclusive = IsExclusive;
        target.EndDate = EndDate;
        target.Priority = Priority;
        target.PredicateVisualTreeSerialized = PredicateVisualTreeSerialized;
        target.PerCustomerLimit = PerCustomerLimit;
        target.TotalLimit = TotalLimit;
        target.PerCustomerLimit = PerCustomerLimit;
        target.IsAllowCombiningWithSelf = IsAllowCombiningWithSelf;

        if (!Stores.IsNullCollection())
        {
            var comparer = AnonymousComparer.Create((PromotionStoreEntity entity) => entity.StoreId);
            Stores.Patch(target.Stores, comparer, (sourceEntity, targetEntity) => targetEntity.StoreId = sourceEntity.StoreId);
        }
    }
}
