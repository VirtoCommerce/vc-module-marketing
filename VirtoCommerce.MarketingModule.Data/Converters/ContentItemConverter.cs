using System;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using dataModel = VirtoCommerce.MarketingModule.Data.Model;

namespace VirtoCommerce.MarketingModule.Data.Converters
{
    public static class ContentItemConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <returns></returns>
        public static coreModel.DynamicContentItem ToCoreModel(this dataModel.DynamicContentItem dbEntity)
        {
            if (dbEntity == null)
                throw new ArgumentNullException("dbEntity");

            var retVal = new coreModel.DynamicContentItem();
            retVal.InjectFrom(dbEntity);

            retVal.ContentType = dbEntity.ContentTypeId;

            if (dbEntity.Folder != null)
            {
                retVal.Folder = dbEntity.Folder.ToCoreModel();
            }

            return retVal;
        }

        public static dataModel.DynamicContentItem ToDataModel(this coreModel.DynamicContentItem contentItem, PrimaryKeyResolvingMap pkMap)
        {
            if (contentItem == null)
                throw new ArgumentNullException("contentItem");

            var retVal = new dataModel.DynamicContentItem();
            pkMap.AddPair(contentItem, retVal);
            retVal.InjectFrom(contentItem);
            retVal.ContentTypeId = contentItem.ContentType;


            return retVal;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this dataModel.DynamicContentItem source, dataModel.DynamicContentItem target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<dataModel.DynamicContentItem>(x => x.Name, x => x.Description, x => x.FolderId, x => x.ImageUrl, x => x.ContentTypeId);

            target.InjectFrom(patchInjection, source);
        }
    }
}
