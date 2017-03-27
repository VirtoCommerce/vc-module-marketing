using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System;
using VirtoCommerce.Platform.Core.Common;
using System.Collections.Generic;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Data.Model
{
	public class DynamicContentItemEntity : AuditableEntity
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
        public string FolderId { get; set; }

		public virtual DynamicContentFolderEntity Folder { get; set; }

        #endregion

        public virtual DynamicContentItem ToModel(DynamicContentItem item)
        {
            if (item == null)
                throw new NullReferenceException(nameof(item));

            item.Id = this.Id;
            item.CreatedBy = this.CreatedBy;
            item.CreatedDate = this.CreatedDate;
            item.Description = this.Description;
            item.ModifiedBy = this.ModifiedBy;
            item.ModifiedDate = this.ModifiedDate;
            item.Name = this.Name;
            item.FolderId = this.FolderId;
            item.ImageUrl = this.ImageUrl;
            item.ContentType = this.ContentTypeId;

            if (this.Folder != null)
            {
                item.Folder = this.Folder.ToModel(AbstractTypeFactory<DynamicContentFolder>.TryCreateInstance());
            }
            return item;
        }

        public virtual DynamicContentItemEntity FromModel(DynamicContentItem item, PrimaryKeyResolvingMap pkMap)
        {
            if (item == null)
                throw new NullReferenceException(nameof(item));

            pkMap.AddPair(item, this);

            this.Id = item.Id;
            this.CreatedBy = item.CreatedBy;
            this.CreatedDate = item.CreatedDate;
            this.Description = item.Description;
            this.ModifiedBy = item.ModifiedBy;
            this.ModifiedDate = item.ModifiedDate;
            this.Name = item.Name;
            this.FolderId = item.FolderId;
            this.ImageUrl = item.ImageUrl;
            this.ContentTypeId = item.ContentType;

            return this;
        }

        public virtual void Patch(DynamicContentItemEntity target)
        {
            if (target == null)
                throw new NullReferenceException(nameof(target));

            target.Name = this.Name;
            target.Description = this.Description;
            target.FolderId = this.FolderId;
            target.ContentTypeId = this.ContentTypeId;
            target.ImageUrl = this.ImageUrl;
        }
    }
}
