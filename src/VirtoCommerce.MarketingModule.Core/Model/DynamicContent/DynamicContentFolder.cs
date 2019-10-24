namespace VirtoCommerce.MarketingModule.Core.Model
{
    public class DynamicContentFolder : DynamicContentListEntry
    {

        /// <summary>
        /// all parent folders names concatenated (Root\Child\Child2)
        /// </summary>
        public string Path => ParentFolder == null ? Name : ParentFolder.Path + "\\" + Name;
        /// <summary>
        /// all parent folders ids concatenated (1;21;344)
        /// </summary>
        public string Outline => ParentFolder == null ? Id : ParentFolder.Outline + ";" + Id;

        public string ParentFolderId { get; set; }
        public DynamicContentFolder ParentFolder { get; set; }

        public override string ObjectType => nameof(DynamicContentFolder);

        #region ICloneable members

        public override object Clone()
        {
            var result = base.Clone() as DynamicContentFolder;

            if (ParentFolder != null)
            {
                result.ParentFolder = ParentFolder.Clone() as DynamicContentFolder;
            }

            return result;
        }

        #endregion
    }
}
