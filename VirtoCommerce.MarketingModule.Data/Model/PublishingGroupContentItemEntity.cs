using System;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
    public class PublishingGroupContentItemEntity : AuditableEntity
    {
        #region Navigation Properties
        public string DynamicContentPublishingGroupId { get; set; }
        public virtual DynamicContentPublishingGroupEntity PublishingGroup { get; set; }

        public string DynamicContentItemId { get; set; }
        public virtual DynamicContentItemEntity ContentItem { get; set; }				
        #endregion

        public int Priority { get; set; }

        public virtual DynamicContentItem ToModel(DynamicContentItem contentItem)
        {
            if (contentItem == null)
            {
                throw new ArgumentNullException(nameof(contentItem));
            }

            ContentItem.ToModel(contentItem);

            contentItem.Priority = Priority;

            return contentItem;
        }

        public virtual PublishingGroupContentItemEntity FromModel(DynamicContentItem contentItem, PrimaryKeyResolvingMap pkMap)
        {
            if (contentItem == null)
            {
                throw new ArgumentNullException(nameof(contentItem));
            }

            pkMap.AddPair(contentItem, this);

            DynamicContentItemId = contentItem.Id;
            Priority = contentItem.Priority;

            return this;
        }

        public virtual void Patch(PublishingGroupContentItemEntity target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            target.Priority = Priority;
        }
    }
}
