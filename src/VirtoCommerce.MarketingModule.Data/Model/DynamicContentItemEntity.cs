using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.MarketingModule.Data.Model;

public class DynamicContentItemEntity : AuditableEntity, IDataEntity<DynamicContentItemEntity, DynamicContentItem>
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    [StringLength(256)]
    public string Description { get; set; }

    /// <summary>
    /// available values in DynamicContentType enum
    /// </summary>
    [StringLength(64)]
    public string ContentTypeId { get; set; }

    public bool IsMultilingual { get; set; }

    [StringLength(2048)]
    public string ImageUrl { get; set; }

    #region Navigation Properties

    [StringLength(128)]
    public string FolderId { get; set; }
    public virtual DynamicContentFolderEntity Folder { get; set; }

    public ObservableCollection<DynamicContentItemDynamicPropertyObjectValueEntity> DynamicPropertyObjectValues { get; set; }
        = new NullCollection<DynamicContentItemDynamicPropertyObjectValueEntity>();

    #endregion

    public virtual DynamicContentItem ToModel(DynamicContentItem model)
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
        model.ContentType = ContentTypeId;
        model.Description = Description;

        if (Folder != null)
        {
            model.Folder = Folder.ToModel(AbstractTypeFactory<DynamicContentFolder>.TryCreateInstance());
        }

        model.DynamicProperties = DynamicPropertyObjectValues
            .GroupBy(x => x.PropertyId)
            .Select(g =>
            {
                var property = AbstractTypeFactory<DynamicObjectProperty>.TryCreateInstance();
                property.Id = g.Key;
                property.Name = g.First().PropertyName;
                property.Values = g.Select(x => x.ToModel(AbstractTypeFactory<DynamicPropertyObjectValue>.TryCreateInstance())).ToArray();
                return property;
            })
            .ToArray();

        return model;
    }

    public virtual DynamicContentItemEntity FromModel(DynamicContentItem model, PrimaryKeyResolvingMap pkMap)
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
        ContentTypeId = model.ContentType;
        Description = model.Description;

        if (model.DynamicProperties != null)
        {
            ContentTypeId = model.GetDynamicPropertyValue<string>("Content type", null);

            DynamicPropertyObjectValues = new ObservableCollection<DynamicContentItemDynamicPropertyObjectValueEntity>(
                model.DynamicProperties
                    .SelectMany(p => p.Values
                        .Select(v => AbstractTypeFactory<DynamicContentItemDynamicPropertyObjectValueEntity>.TryCreateInstance().FromModel(v, model, p)))
                    .OfType<DynamicContentItemDynamicPropertyObjectValueEntity>());
        }

        return this;
    }

    public virtual void Patch(DynamicContentItemEntity target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Name = Name;
        target.Description = Description;
        target.FolderId = FolderId;
        target.ContentTypeId = ContentTypeId;
        target.ImageUrl = ImageUrl;

        if (!DynamicPropertyObjectValues.IsNullCollection())
        {
            DynamicPropertyObjectValues.Patch(target.DynamicPropertyObjectValues, (sourceValues, targetValues) => sourceValues.Patch(targetValues));
        }
    }
}
