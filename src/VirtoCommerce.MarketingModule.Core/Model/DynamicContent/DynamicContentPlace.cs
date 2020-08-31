using VirtoCommerce.MarketingModule.Core.Model.DynamicContent;

namespace VirtoCommerce.MarketingModule.Core.Model
{
    public class DynamicContentPlace : DynamicContentListEntry, IsHasFolder
    {
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

        #region ICloneable members

        public override object Clone()
        {
            var result = base.Clone() as DynamicContentPlace;

            if (Folder != null)
            {
                result.Folder = Folder.Clone() as DynamicContentFolder;
            }

            return result;
        }

        #endregion
    }
}
