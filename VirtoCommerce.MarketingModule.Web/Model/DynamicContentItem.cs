using System.Collections.Generic;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.MarketingModule.Web.Model
{
    /// <summary>
    /// Represent content entry for presentation (Images, Html, Banner etc) 
    /// </summary>
    public class DynamicContentItem : DynamicContentListEntry
    {
        public string ContentType { get; set; }
        public string FolderId { get; set; }
        /// <summary>
        /// all parent folders ids concatenated (1;21;344)
        /// </summary>
        public string Outline { get; set; }
        /// <summary>
        /// all parent folders names concatenated (Root\Child\Child2)
        /// </summary>
        public string Path { get; set; }

        public int Priority { get; set; }

        public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
    }
}
