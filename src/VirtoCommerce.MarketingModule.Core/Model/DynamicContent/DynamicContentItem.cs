using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.MarketingModule.Core.Model
{
    public class DynamicContentItem : DynamicContentListEntry, IsHasFolder, IHasDynamicProperties
    {
        public string ContentType { get; set; }
        public int Priority { get; set; }
        /// <summary>
        /// all parent folders ids concatenated (1;21;344)
        /// </summary>
        public string Outline => Folder?.Outline;
        /// <summary>
        /// all parent folders names concatenated (Root\Child\Child2)
        /// </summary>
        public string Path => Folder?.Path;

        #region IHasFolder Members
        public string FolderId { get; set; }
        public DynamicContentFolder Folder { get; set; }
        #endregion

        #region IHasDynamicProperties Members

        public override string ObjectType => GetType().FullName;

        public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }

        #endregion

        #region ICloneable members

        public override object Clone()
        {
            var result = base.Clone() as DynamicContentItem;

            if (Folder != null)
            {
                result.Folder = Folder.Clone() as DynamicContentFolder;
            }

            if (DynamicProperties != null)
            {
                result.DynamicProperties = new ObservableCollection<DynamicObjectProperty>(
                    DynamicProperties.Select(x => x.Clone() as DynamicObjectProperty));
            }

            return result;
        }

        #endregion
    }
}
