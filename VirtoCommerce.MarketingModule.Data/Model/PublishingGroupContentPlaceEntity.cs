using System;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
    public class PublishingGroupContentPlaceEntity : AuditableEntity
    {
        #region Navigation Properties
        public string DynamicContentPublishingGroupId { get; set; }
        public virtual DynamicContentPublishingGroupEntity PublishingGroup { get; set; }

        public string DynamicContentPlaceId { get; set; }
        public virtual DynamicContentPlaceEntity ContentPlace { get; set; }
                
        #endregion

        public virtual PublishingGroupContentPlaceEntity FromModel(DynamicContentPlace contentPlace, PrimaryKeyResolvingMap pkMap)
        {
            if (contentPlace == null)
            {
                throw new ArgumentNullException(nameof(contentPlace));
            }

            pkMap.AddPair(contentPlace, this);

            DynamicContentPlaceId = contentPlace.Id;

            return this;
        }

        public virtual DynamicContentPlace ToModel(DynamicContentPlace contentPlace)
        {
            if (contentPlace == null)
            {
                throw new ArgumentNullException(nameof(contentPlace));
            }

            ContentPlace.ToModel(contentPlace);

            return contentPlace;
        }

        public virtual void Patch(PublishingGroupContentPlaceEntity target)
        {

        }
    }
}
