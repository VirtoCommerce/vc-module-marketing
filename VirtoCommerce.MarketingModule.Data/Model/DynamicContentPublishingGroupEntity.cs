using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
    public class DynamicContentPublishingGroupEntity : AuditableEntity
    {
        public DynamicContentPublishingGroupEntity()
        {
            ContentItems = new NullCollection<PublishingGroupContentItemEntity>();
            ContentPlaces = new NullCollection<PublishingGroupContentPlaceEntity>();
        }

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

        #region Navigation Properties
        public virtual ObservableCollection<PublishingGroupContentItemEntity> ContentItems { get; set; }

        public virtual ObservableCollection<PublishingGroupContentPlaceEntity> ContentPlaces { get; set; }
        #endregion


        public virtual DynamicContentPublication ToModel(DynamicContentPublication publication)
        {
            if (publication == null)
            {
                throw new ArgumentNullException(nameof(publication));
            }

            publication.Id = this.Id;
            publication.CreatedBy = this.CreatedBy;
            publication.CreatedDate = this.CreatedDate;
            publication.Description = this.Description;
            publication.ModifiedBy = this.ModifiedBy;
            publication.ModifiedDate = this.ModifiedDate;
            publication.Name = this.Name;
            publication.Priority = this.Priority;
            publication.IsActive = this.IsActive;
            publication.StoreId = this.StoreId;
            publication.StartDate = this.StartDate;
            publication.EndDate = this.EndDate;
            publication.PredicateSerialized = this.ConditionExpression;
            publication.PredicateVisualTreeSerialized = this.PredicateVisualTreeSerialized;

            if (!string.IsNullOrEmpty(publication.PredicateVisualTreeSerialized))
            {
                //Temporary back data compatibility fix for serialized expressions
                publication.PredicateVisualTreeSerialized = publication.PredicateVisualTreeSerialized.Replace("VirtoCommerce.DynamicExpressionModule.", "VirtoCommerce.DynamicExpressionsModule.");
            }
            if (!string.IsNullOrEmpty(publication.PredicateSerialized))
            {
                //Temporary back data compatibility fix for serialized expressions
                publication.PredicateSerialized = publication.PredicateSerialized.Replace("VirtoCommerce.DynamicExpressionModule.", "VirtoCommerce.DynamicExpressionsModule.");
            }
            if (this.ContentItems != null)
            {
                publication.ContentItems = this.ContentItems.Select(x => x.ToModel(AbstractTypeFactory<DynamicContentItem>.TryCreateInstance())).ToList();
            }
            if (this.ContentPlaces != null)
            {
                publication.ContentPlaces = this.ContentPlaces.Select(x => x.ContentPlace.ToModel(AbstractTypeFactory<DynamicContentPlace>.TryCreateInstance())).ToList();
            }

            return publication;
        }

        public virtual DynamicContentPublishingGroupEntity FromModel(DynamicContentPublication publication, PrimaryKeyResolvingMap pkMap)
        {
            if (publication == null)
            {
                throw new ArgumentNullException(nameof(publication));
            }
            pkMap.AddPair(publication, this);

            this.Id = publication.Id;
            this.CreatedBy = publication.CreatedBy;
            this.CreatedDate = publication.CreatedDate;
            this.Description = publication.Description;
            this.ModifiedBy = publication.ModifiedBy;
            this.ModifiedDate = publication.ModifiedDate;
            this.Name = publication.Name;
            this.Priority = publication.Priority;
            this.IsActive = publication.IsActive;
            this.StoreId = publication.StoreId;
            this.StartDate = publication.StartDate;
            this.EndDate = publication.EndDate;
            this.ConditionExpression = publication.PredicateSerialized;
            this.PredicateVisualTreeSerialized = publication.PredicateVisualTreeSerialized;

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
            return this;
        }

        public virtual void Patch(DynamicContentPublishingGroupEntity target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            target.Description = this.Description;
            target.Name = this.Name;
            target.Priority = this.Priority;
            target.IsActive = this.IsActive;
            target.StoreId = this.StoreId;
            target.StartDate = this.StartDate;
            target.EndDate = this.EndDate;
            target.ConditionExpression = this.ConditionExpression;
            target.PredicateVisualTreeSerialized = this.PredicateVisualTreeSerialized;

            if (!this.ContentItems.IsNullCollection())
            {
                var itemComparer = AnonymousComparer.Create((PublishingGroupContentItemEntity x) => x.DynamicContentItemId);
                this.ContentItems.Patch(target.ContentItems, itemComparer, (sourceProperty, targetProperty) => { sourceProperty.Patch(targetProperty); });
            }

            if (!this.ContentPlaces.IsNullCollection())
            {
                var itemComparer = AnonymousComparer.Create((PublishingGroupContentPlaceEntity x) => x.DynamicContentPlaceId);
                this.ContentPlaces.Patch(target.ContentPlaces, itemComparer, (sourceProperty, targetProperty) => { sourceProperty.Patch(targetProperty); });
            }
        }
    }
}
