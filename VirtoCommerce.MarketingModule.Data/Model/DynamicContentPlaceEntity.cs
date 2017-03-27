using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
	public class DynamicContentPlaceEntity : AuditableEntity
	{
		[Required]
		[StringLength(128)]
		public string Name { get; set; }

		[StringLength(256)]
		public string Description { get; set; }

		[StringLength(2048)]
		public string ImageUrl { get; set; }

		#region Navigation Properties
		public string FolderId { get; set; }

		public virtual DynamicContentFolderEntity Folder { get; set; }
        #endregion

        public virtual DynamicContentPlace ToModel(DynamicContentPlace place)
        {
            if (place == null)
                throw new NullReferenceException(nameof(place));

            place.Id = this.Id;
            place.CreatedBy = this.CreatedBy;
            place.CreatedDate = this.CreatedDate;
            place.Description = this.Description;
            place.ModifiedBy = this.ModifiedBy;
            place.ModifiedDate = this.ModifiedDate;
            place.Name = this.Name;
            place.FolderId = this.FolderId;
            place.ImageUrl = this.ImageUrl;
     
            if (this.Folder != null)
            {
                place.Folder = this.Folder.ToModel(AbstractTypeFactory<DynamicContentFolder>.TryCreateInstance());
            }
            return place;
        }

        public virtual DynamicContentPlaceEntity FromModel(DynamicContentPlace place, PrimaryKeyResolvingMap pkMap)
        {
            if (place == null)
                throw new NullReferenceException(nameof(place));

            pkMap.AddPair(place, this);

            this.Id = place.Id;
            this.CreatedBy = place.CreatedBy;
            this.CreatedDate = place.CreatedDate;
            this.Description = place.Description;
            this.ModifiedBy = place.ModifiedBy;
            this.ModifiedDate = place.ModifiedDate;
            this.Name = place.Name;
            this.FolderId = place.FolderId;
            this.ImageUrl = place.ImageUrl;

            return this;
        }

        public virtual void Patch(DynamicContentPlaceEntity target)
        {
            if (target == null)
                throw new NullReferenceException(nameof(target));

            target.Name = this.Name;
            target.Description = this.Description;
            target.FolderId = this.FolderId;
            target.ImageUrl = this.ImageUrl;
        }
    }
}
