using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.MarketingModule.Data.Model;

public class DynamicContentPlaceEntity : AuditableEntity, IDataEntity<DynamicContentPlaceEntity, DynamicContentPlace>
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    [StringLength(256)]
    public string Description { get; set; }

    [StringLength(2048)]
    public string ImageUrl { get; set; }

    #region Navigation Properties

    [StringLength(128)]
    public string FolderId { get; set; }
    public virtual DynamicContentFolderEntity Folder { get; set; }

    #endregion

    public virtual DynamicContentPlace ToModel(DynamicContentPlace model)
    {
        ArgumentNullException.ThrowIfNull(model);

        model.Id = Id;
        model.CreatedBy = CreatedBy;
        model.CreatedDate = CreatedDate;
        model.ModifiedBy = ModifiedBy;
        model.ModifiedDate = ModifiedDate;

        model.Name = Name;
        model.FolderId = FolderId;
        model.ImageUrl = ImageUrl;
        model.Description = Description;

        if (Folder != null)
        {
            model.Folder = Folder.ToModel(AbstractTypeFactory<DynamicContentFolder>.TryCreateInstance());
        }

        return model;
    }

    public virtual DynamicContentPlaceEntity FromModel(DynamicContentPlace model, PrimaryKeyResolvingMap pkMap)
    {
        ArgumentNullException.ThrowIfNull(model);

        pkMap.AddPair(model, this);

        Id = model.Id;
        CreatedBy = model.CreatedBy;
        CreatedDate = model.CreatedDate;
        ModifiedBy = model.ModifiedBy;
        ModifiedDate = model.ModifiedDate;

        Name = model.Name;
        FolderId = model.FolderId;
        ImageUrl = model.ImageUrl;
        Description = model.Description;

        return this;
    }

    public virtual void Patch(DynamicContentPlaceEntity target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Name = Name;
        target.Description = Description;
        target.FolderId = FolderId;
        target.ImageUrl = ImageUrl;
    }
}
