using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
	}
}
