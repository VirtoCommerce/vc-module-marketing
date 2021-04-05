using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions
{
    public class ProductPromoEntry : ICloneable, ICacheKey
    {
        public ProductPromoEntry()
        {
            Variations = new List<ProductPromoEntry>();
            Attributes = new Dictionary<string, string>();
        }

        public virtual string Code { get; set; }
        public virtual int Quantity { get; set; }
        public virtual int InStockQuantity { get; set; }
        public virtual decimal Price { get; set; }
        public virtual decimal ListPrice { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual string CatalogId { get; set; }
        public virtual string CategoryId { get; set; }
        public virtual string ProductId { get; set; }
        public virtual object Owner { get; set; }
        public virtual string Outline { get; set; }

        public virtual ICollection<ProductPromoEntry> Variations { get; set; }

        public virtual Dictionary<string, string> Attributes { get; set; }

        public virtual object Clone()
        {
            var result = MemberwiseClone() as ProductPromoEntry;
            result.Variations = Variations?.Select(x => x.Clone()).OfType<ProductPromoEntry>().ToList();
            result.Attributes = new Dictionary<string, string>(Attributes);
            return result;
        }

        public virtual string GetCacheKey()
        {
            return string.Join("|", GetCacheKeyComponents().Select(x => x ?? "null").Select(x => x.ToString()));
        }

        public virtual IEnumerable<object> GetCacheKeyComponents()
        {
            yield return Code;
            yield return ProductId;
            yield return Price;
            yield return Quantity;
        }
    }
}
