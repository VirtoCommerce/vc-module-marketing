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
		public string Code { get; set; }
		public int Quantity { get; set; }
        public int InStockQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Discount { get; set; }
		public string CatalogId { get; set; }
		public string CategoryId { get; set; }
		public string ProductId { get; set; }
		public object Owner { get; set; }
		public string Outline { get; set; }

		public ICollection<ProductPromoEntry> Variations { get; set; }

		public Dictionary<string, string> Attributes { get; set; }

        public virtual object Clone()
        {
            var result = MemberwiseClone() as ProductPromoEntry;
            result.Variations = Variations?.Select(x => x.Clone()).OfType<ProductPromoEntry>().ToList();
            result.Attributes = new Dictionary<string, string>(Attributes);
            return result;
        }

        public string GetCacheKey()
        {
            return string.Join("|", GetCacheKeyComponents().Select(x => x ?? "null").Select(x => x.ToString()));
        }

        public IEnumerable<object> GetCacheKeyComponents()
        {
            yield return Code;
            yield return ProductId;
            yield return Price;
            yield return ListPrice;
            yield return Discount;
            yield return Quantity;
            yield return InStockQuantity;
        }
    }
}
