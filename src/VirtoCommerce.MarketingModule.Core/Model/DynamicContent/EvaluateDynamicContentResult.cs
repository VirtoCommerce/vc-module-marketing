using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MarketingModule.Core.Model.DynamicContent
{
    public class EvaluateDynamicContentResult
    {
        public IList<DynamicContentItem> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
