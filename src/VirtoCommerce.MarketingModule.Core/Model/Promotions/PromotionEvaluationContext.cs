using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions
{
    public class PromotionEvaluationContext : EvaluationContextBase, ICacheKey
    {
        public string[] RefusedGiftIds { get; set; }

        public string StoreId { get; set; }

        public string Currency { get; set; }

        public Currency CurrencyObject { get; set; }

        /// <summary>
        /// Contains User Id.
        /// This property will be deleted after update to .NET8. Use UserId property instead.
        /// </summary>
        public string CustomerId { get; set; }

        private string _userId;
        public string UserId
        {
            get
            {
                if (_userId is null && CustomerId is not null)
                {
                    _userId = CustomerId;
                }

                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        public string ContactId { get; set; }
        public string OrganizaitonId { get; set; }

        public bool IsRegisteredUser { get; set; }

        /// <summary>
        /// Has user made any orders
        /// </summary>
        public bool IsFirstTimeBuyer { get; set; }

        public bool IsEveryone { get; set; } = true;

        //Cart subtotal (incorrect property name cannot change for back compatibility reasons)
        public decimal CartTotal { get; set; }

        /// <summary>
        /// Current shipment method
        /// </summary>
        public string ShipmentMethodCode { get; set; }
        public string ShipmentMethodOption { get; set; }
        public decimal ShipmentMethodPrice { get; set; }
        public string[] AvailableShipmentMethodCodes { get; set; }

        /// <summary>
        /// Current payment method
        /// </summary>
        public string PaymentMethodCode { get; set; }
        public decimal PaymentMethodPrice { get; set; }
        public string[] AvailablePaymentMethodCodes { get; set; }

        /// <summary>
        /// Entered coupon
        /// </summary>
        public string Coupon { get; set; }
        /// <summary>
        /// Entered multiple coupons
        /// </summary>
        private ICollection<string> _coupons;
        public ICollection<string> Coupons
        {
            get
            {
                if (_coupons == null && !string.IsNullOrEmpty(Coupon))
                {
                    _coupons = new List<string>() { Coupon };
                }
                return _coupons;
            }
            set
            {
                _coupons = value;
            }
        }
        /// <summary>
        /// List of product promo in cart
        /// </summary>
        public ICollection<ProductPromoEntry> CartPromoEntries { get; set; } = new List<ProductPromoEntry>();
        /// <summary>
        /// List of products for promo evaluation
        /// </summary>
        public ICollection<ProductPromoEntry> PromoEntries { get; set; } = new List<ProductPromoEntry>();
        /// <summary>
        /// Single catalog product promo entry 
        /// </summary>
        public ProductPromoEntry PromoEntry { get; set; }

        public virtual string GetCacheKey()
        {
            var keyValues = GetCacheKeyComponents()
                .Select(x => x is string ? $"'{x}'" : x)
                .Select(x => x is ICacheKey cacheKey ? cacheKey.GetCacheKey() : x?.ToString());

            return string.Join("|", keyValues);
        }

        public virtual IEnumerable<object> GetCacheKeyComponents()
        {
            yield return IsRegisteredUser;
            yield return IsFirstTimeBuyer;
            yield return IsEveryone;

            yield return Language;
            yield return StoreId;
            yield return Currency;
            yield return UserId;
            yield return CartTotal;
            yield return Coupon;

            yield return ShipmentMethodCode;
            yield return ShipmentMethodOption;
            yield return ShipmentMethodPrice;

            yield return PaymentMethodCode;
            yield return PaymentMethodPrice;

            foreach (var entry in GetCollectionComponents(Coupons))
            {
                yield return entry;
            }

            foreach (var entry in GetCollectionComponents(UserGroups))
            {
                yield return entry;
            }

            foreach (var entry in GetCollectionComponents(Attributes))
            {
                yield return entry;
            }

            yield return PromoEntry;

            foreach (var entry in GetCollectionComponents(PromoEntries))
            {
                yield return entry;
            }

            foreach (var entry in GetCollectionComponents(CartPromoEntries))
            {
                yield return entry;
            }
        }

        protected virtual IEnumerable<object> GetCollectionComponents<T>(IEnumerable<T> entries)
        {
            if (entries == null)
            {
                yield return null;
            }
            else
            {
                yield return '[';

                foreach (var entry in entries)
                {
                    yield return entry;
                }

                yield return ']';
            }
        }

        public virtual PromotionEvaluationContext Clone()
        {
            return MemberwiseClone() as PromotionEvaluationContext;
        }
    }
}
