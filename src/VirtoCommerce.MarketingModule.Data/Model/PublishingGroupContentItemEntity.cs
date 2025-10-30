using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model;

public class PublishingGroupContentItemEntity : AuditableEntity
{
    public int Priority { get; set; }

    #region Navigation Properties

    [StringLength(128)]
    public string DynamicContentPublishingGroupId { get; set; }
    public virtual DynamicContentPublishingGroupEntity PublishingGroup { get; set; }

    [StringLength(128)]
    public string DynamicContentItemId { get; set; }
    public virtual DynamicContentItemEntity ContentItem { get; set; }

    #endregion

    public virtual DynamicContentItem ToModel(DynamicContentItem model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (ContentItem != null)
        {
            ContentItem.ToModel(model);
        }

        model.Priority = Priority;

        return model;
    }

    public virtual PublishingGroupContentItemEntity FromModel(DynamicContentItem model, PrimaryKeyResolvingMap pkMap)
    {
        ArgumentNullException.ThrowIfNull(model);

        pkMap.AddPair(model, this);

        DynamicContentItemId = model.Id;
        Priority = model.Priority;

        return this;
    }

    public virtual void Patch(PublishingGroupContentItemEntity target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Priority = Priority;
    }
}
