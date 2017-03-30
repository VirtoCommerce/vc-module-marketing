using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.MarketingModule.Web.Security
{
    public class MarketingSelectedStoreScope : PermissionScope
    {
        public override bool IsScopeAvailableForPermission(string permission)
        {
            return permission == MarketingPredefinedPermissions.Read
                || permission == MarketingPredefinedPermissions.Update
                || permission == MarketingPredefinedPermissions.Create;

        }

        public override IEnumerable<string> GetEntityScopeStrings(object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var promotion = entity as Promotion;
            if (promotion != null)
            {
                return new[] { base.Type + ":" + promotion.Store };
            }
            return Enumerable.Empty<string>();
        }
    }
}