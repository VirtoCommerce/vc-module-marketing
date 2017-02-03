using System;
using System.Collections.ObjectModel;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using dataModel = VirtoCommerce.MarketingModule.Data.Model;

namespace VirtoCommerce.MarketingModule.Data.Converters
{
    public static class ContentPublicationConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <returns></returns>
        public static coreModel.DynamicContentPublication ToCoreModel(this dataModel.DynamicContentPublishingGroup dbEntity)
        {
            if (dbEntity == null)
                throw new ArgumentNullException("dbEntity");

            var retVal = new coreModel.DynamicContentPublication();
            retVal.InjectFrom(dbEntity);

            retVal.PredicateSerialized = dbEntity.ConditionExpression;
            retVal.PredicateVisualTreeSerialized = dbEntity.PredicateVisualTreeSerialized;
            if (!string.IsNullOrEmpty(retVal.PredicateVisualTreeSerialized))
            {
                //Temporary back data compatibility fix for serialized expressions
                retVal.PredicateVisualTreeSerialized = retVal.PredicateVisualTreeSerialized.Replace("VirtoCommerce.DynamicExpressionModule.", "VirtoCommerce.DynamicExpressionsModule.");
            }
            if (!string.IsNullOrEmpty(retVal.PredicateSerialized))
            {
                //Temporary back data compatibility fix for serialized expressions
                retVal.PredicateSerialized = retVal.PredicateSerialized.Replace("VirtoCommerce.DynamicExpressionModule.", "VirtoCommerce.DynamicExpressionsModule.");
            }
            if (dbEntity.ContentItems != null)
            {
                retVal.ContentItems = dbEntity.ContentItems.Select(x => x.ContentItem.ToCoreModel()).ToList();
            }
            if (dbEntity.ContentPlaces != null)
            {
                retVal.ContentPlaces = dbEntity.ContentPlaces.Select(x => x.ContentPlace.ToCoreModel()).ToList();
            }

            return retVal;
        }

        public static dataModel.DynamicContentPublishingGroup ToDataModel(this coreModel.DynamicContentPublication publication)
        {
            if (publication == null)
                throw new ArgumentNullException("publication");

            var retVal = new dataModel.DynamicContentPublishingGroup();
            retVal.InjectFrom(publication);

            retVal.ConditionExpression = publication.PredicateSerialized;
            retVal.PredicateVisualTreeSerialized = publication.PredicateVisualTreeSerialized;

            if (publication.ContentItems != null)
            {
                retVal.ContentItems = new ObservableCollection<dataModel.PublishingGroupContentItem>(publication.ContentItems.Select(x => new dataModel.PublishingGroupContentItem { DynamicContentPublishingGroupId = retVal.Id, DynamicContentItemId = x.Id }));
            }
            if (publication.ContentPlaces != null)
            {
                retVal.ContentPlaces = new ObservableCollection<dataModel.PublishingGroupContentPlace>(publication.ContentPlaces.Select(x => new dataModel.PublishingGroupContentPlace { DynamicContentPublishingGroupId = retVal.Id, DynamicContentPlaceId = x.Id }));
            }
            return retVal;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this dataModel.DynamicContentPublishingGroup source, dataModel.DynamicContentPublishingGroup target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<dataModel.DynamicContentPublishingGroup>(x => x.StoreId, x => x.Name, x => x.Description, x => x.IsActive,
                                                                                                  x => x.StartDate, x => x.EndDate, x => x.PredicateVisualTreeSerialized, x => x.ConditionExpression);

            target.InjectFrom(patchInjection, source);
            target.StartDate = source.StartDate;
            target.EndDate = source.EndDate;

            if (!source.ContentItems.IsNullCollection())
            {
                var itemComparer = AnonymousComparer.Create((dataModel.PublishingGroupContentItem x) => x.DynamicContentItemId);
                source.ContentItems.Patch(target.ContentItems, itemComparer, (sourceProperty, targetProperty) => { });
            }

            if (!source.ContentPlaces.IsNullCollection())
            {
                var itemComparer = AnonymousComparer.Create((dataModel.PublishingGroupContentPlace x) => x.DynamicContentPlaceId);
                source.ContentPlaces.Patch(target.ContentPlaces, itemComparer, (sourceProperty, targetProperty) => { });
            }
        }
    }
}
