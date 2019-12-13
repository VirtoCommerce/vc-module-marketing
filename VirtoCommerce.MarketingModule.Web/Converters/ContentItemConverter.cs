using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
    public static class ContentItemConverter
    {
        public static webModel.DynamicContentItem ToWebModel(this coreModel.DynamicContentItem content)
        {
            var retVal = AbstractTypeFactory<webModel.DynamicContentItem>.TryCreateInstance();
            retVal.InjectFrom(content);
            if(content.Folder != null)
            {
                retVal.Outline = content.Folder.Outline;
                retVal.Path = content.Folder.Path;
            }
            retVal.DynamicProperties = content.DynamicProperties;
            return retVal;
        }
    }
}
