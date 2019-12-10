using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
    public static class ContentFolderConverter
    {
        public static webModel.DynamicContentFolder ToWebModel(this coreModel.DynamicContentFolder folder)
        {
            var retVal = AbstractTypeFactory<webModel.DynamicContentFolder>.TryCreateInstance();
            retVal.InjectFrom(folder);
            return retVal;
        }

        public static coreModel.DynamicContentFolder ToCoreModel(this webModel.DynamicContentFolder folder)
        {
            var retVal = AbstractTypeFactory<coreModel.DynamicContentFolder>.TryCreateInstance();
            retVal.InjectFrom(folder);
            return retVal;
        }
    }
}
