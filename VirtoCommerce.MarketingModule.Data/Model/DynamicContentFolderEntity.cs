using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
	public class DynamicContentFolderEntity : AuditableEntity
	{
		public DynamicContentFolderEntity()
		{
			ContentItems = new NullCollection<DynamicContentItemEntity>();
			ContentPlaces = new NullCollection<DynamicContentPlaceEntity>();
		}

		[Required]
		[StringLength(128)]
		public string Name { get; set; }

		[StringLength(256)]
		public string Description { get; set; }

		[StringLength(2048)]
		public string ImageUrl { get; set; }

		#region Navigation Properties
		public string ParentFolderId { get; set; }
		public virtual DynamicContentFolderEntity ParentFolder { get; set; }

		public virtual ObservableCollection<DynamicContentItemEntity> ContentItems { get; set; }
		public virtual ObservableCollection<DynamicContentPlaceEntity> ContentPlaces { get; set; }
        #endregion

        public virtual DynamicContentFolder ToModel(DynamicContentFolder folder)
        {
            if (folder == null)
                throw new NullReferenceException(nameof(folder));

            folder.Id = this.Id;
            folder.CreatedBy = this.CreatedBy;
            folder.CreatedDate = this.CreatedDate;
            folder.Description = this.Description;
            folder.ModifiedBy = this.ModifiedBy;
            folder.ModifiedDate = this.ModifiedDate;
            folder.Name = this.Name;
            folder.ParentFolderId = this.ParentFolderId;            
            
            if (this.ParentFolder != null)
            {
                folder.ParentFolder = this.ParentFolder.ToModel(AbstractTypeFactory<DynamicContentFolder>.TryCreateInstance());
            }      
            return folder;
        }

        public virtual DynamicContentFolderEntity FromModel(DynamicContentFolder folder, PrimaryKeyResolvingMap pkMap)
        {
            if (folder == null)
                throw new NullReferenceException(nameof(folder));

            pkMap.AddPair(folder, this);

            this.Id = folder.Id;
            this.CreatedBy = folder.CreatedBy;
            this.CreatedDate = folder.CreatedDate;
            this.Description = folder.Description;
            this.ModifiedBy = folder.ModifiedBy;
            this.ModifiedDate = folder.ModifiedDate;
            this.Name = folder.Name;
            this.ParentFolderId = folder.ParentFolderId;

            return this;
        }

        public virtual void Patch(DynamicContentFolderEntity target)
        {
            if (target == null)
                throw new NullReferenceException(nameof(target));

            target.Name = this.Name;
            target.Description = this.Description;            
        }

    }
}
