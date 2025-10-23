using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.MarketingModule.Data.Model;

public class DynamicContentPublishingGroupEntity : AuditableEntity, IHasOuterId, IDataEntity<DynamicContentPublishingGroupEntity, DynamicContentPublication>
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    [StringLength(256)]
    public string Description { get; set; }

    public int Priority { get; set; }

    public bool IsActive { get; set; }

    [StringLength(256)]
    public string StoreId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string ConditionExpression { get; set; }

    public string PredicateVisualTreeSerialized { get; set; }

    [StringLength(128)]
    public string OuterId { get; set; }

    #region Navigation Properties

    public virtual ObservableCollection<PublishingGroupContentItemEntity> ContentItems { get; set; }
        = new NullCollection<PublishingGroupContentItemEntity>();

    public virtual ObservableCollection<PublishingGroupContentPlaceEntity> ContentPlaces { get; set; }
        = new NullCollection<PublishingGroupContentPlaceEntity>();

    #endregion

    public virtual DynamicContentPublication ToModel(DynamicContentPublication model)
    {
        ArgumentNullException.ThrowIfNull(model);

        model.Id = Id;
        model.CreatedBy = CreatedBy;
        model.CreatedDate = CreatedDate;
        model.ModifiedBy = ModifiedBy;
        model.ModifiedDate = ModifiedDate;
        model.OuterId = OuterId;

        model.Name = Name;
        model.Priority = Priority;
        model.IsActive = IsActive;
        model.StoreId = StoreId;
        model.StartDate = StartDate;
        model.EndDate = EndDate;
        model.Description = Description;

        if (ContentItems != null)
        {
            model.ContentItems = ContentItems.Select(x => x.ToModel(AbstractTypeFactory<DynamicContentItem>.TryCreateInstance())).ToList();
        }

        if (ContentPlaces != null)
        {
            // TODO
            model.ContentPlaces = ContentPlaces
                .Where(x => x.ContentPlace != null)
                .Select(x => x.ContentPlace.ToModel(AbstractTypeFactory<DynamicContentPlace>.TryCreateInstance()))
                .ToList();
        }

        model.DynamicExpression = AbstractTypeFactory<DynamicContentConditionTree>.TryCreateInstance();
        if (PredicateVisualTreeSerialized != null)
        {
            model.DynamicExpression = JsonConvert.DeserializeObject<DynamicContentConditionTree>(PredicateVisualTreeSerialized, new ConditionJsonConverter());
        }

        return model;
    }

    public virtual DynamicContentPublishingGroupEntity FromModel(DynamicContentPublication publication, PrimaryKeyResolvingMap pkMap)
    {
        ArgumentNullException.ThrowIfNull(publication);

        pkMap.AddPair(publication, this);

        Id = publication.Id;
        CreatedBy = publication.CreatedBy;
        CreatedDate = publication.CreatedDate;
        ModifiedBy = publication.ModifiedBy;
        ModifiedDate = publication.ModifiedDate;
        OuterId = publication.OuterId;

        Name = publication.Name;
        Priority = publication.Priority;
        IsActive = publication.IsActive;
        StoreId = publication.StoreId;
        StartDate = publication.StartDate;
        EndDate = publication.EndDate;

        Description = publication.Description;

        if (publication.ContentItems != null)
        {
            ContentItems = new ObservableCollection<PublishingGroupContentItemEntity>(publication.ContentItems.Select(
                x =>
                {
                    var result = AbstractTypeFactory<PublishingGroupContentItemEntity>.TryCreateInstance().FromModel(x, pkMap);
                    result.DynamicContentPublishingGroupId = Id;

                    return result;
                }));
        }

        if (publication.ContentPlaces != null)
        {
            ContentPlaces = new ObservableCollection<PublishingGroupContentPlaceEntity>(publication.ContentPlaces.Select(
                x =>
                {
                    var result = AbstractTypeFactory<PublishingGroupContentPlaceEntity>.TryCreateInstance().FromModel(x, pkMap);
                    result.DynamicContentPublishingGroupId = Id;

                    return result;
                }));
        }

        if (publication.DynamicExpression != null)
        {
            PredicateVisualTreeSerialized = JsonConvert.SerializeObject(publication.DynamicExpression, new ConditionJsonConverter(doNotSerializeAvailCondition: true));
        }

        return this;
    }

    public virtual void Patch(DynamicContentPublishingGroupEntity target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Description = Description;
        target.Name = Name;
        target.Priority = Priority;
        target.IsActive = IsActive;
        target.StoreId = StoreId;
        target.StartDate = StartDate;
        target.EndDate = EndDate;
        target.PredicateVisualTreeSerialized = PredicateVisualTreeSerialized;

        if (!ContentItems.IsNullCollection())
        {
            var itemComparer = AnonymousComparer.Create((PublishingGroupContentItemEntity x) => x.DynamicContentItemId);
            ContentItems.Patch(target.ContentItems, itemComparer, (sourceProperty, targetProperty) => { sourceProperty.Patch(targetProperty); });
        }

        if (!ContentPlaces.IsNullCollection())
        {
            var itemComparer = AnonymousComparer.Create((PublishingGroupContentPlaceEntity x) => x.DynamicContentPlaceId);
            ContentPlaces.Patch(target.ContentPlaces, itemComparer, (sourceProperty, targetProperty) => { sourceProperty.Patch(targetProperty); });
        }
    }
}
