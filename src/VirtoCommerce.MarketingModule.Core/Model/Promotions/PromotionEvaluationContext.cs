using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions
{
    public class PromotionEvaluationContext : EvaluationContextBase, ICacheKey
    {
        public virtual string[] RefusedGiftIds { get; set; }

        public string StoreId { get; set; }

        public virtual string Currency { get; set; }

        /// <summary>
        /// Customer id
        /// </summary>
        public virtual string CustomerId { get; set; }

        public virtual bool IsRegisteredUser { get; set; }

        /// <summary>
        /// Has user made any orders
        /// </summary>
        public virtual bool IsFirstTimeBuyer { get; set; }

        public virtual bool IsEveryone { get; set; } = true;

        //Cart subtotal (incorrect property name cannot change for back compatibility reasons)
        public virtual decimal CartTotal { get; set; }

        /// <summary>
        /// Current shipment method
        /// </summary>
        public virtual string ShipmentMethodCode { get; set; }
        public virtual string ShipmentMethodOption { get; set; }
        public virtual decimal ShipmentMethodPrice { get; set; }
        public virtual string[] AvailableShipmentMethodCodes { get; set; }

        /// <summary>
        /// Current payment method
        /// </summary>
        public virtual string PaymentMethodCode { get; set; }
        public virtual decimal PaymentMethodPrice { get; set; }
        public virtual string[] AvailablePaymentMethodCodes { get; set; }

        /// <summary>
        /// Entered coupon
        /// </summary>
        public virtual string Coupon { get; set; }
        /// <summary>
        /// Entered multiple coupons
        /// </summary>
        private ICollection<string> _coupons;
        public virtual ICollection<string> Coupons
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
        public virtual ICollection<ProductPromoEntry> CartPromoEntries { get; set; } = new List<ProductPromoEntry>();
        /// <summary>
        /// List of products for promo evaluation
        /// </summary>
        public virtual ICollection<ProductPromoEntry> PromoEntries { get; set; } = new List<ProductPromoEntry>();
        /// <summary>
        /// Single catalog product promo entry 
        /// </summary>
        public virtual ProductPromoEntry PromoEntry { get; set; }

        public virtual string GetCacheKey()
        {
            return string.Join("|", GetCacheKeyComponents().Select(x => x ?? "null").Select(x => x is ICacheKey cacheKey ? cacheKey.GetCacheKey() : x.ToString()));
        }

        public virtual IEnumerable<object> GetCacheKeyComponents()
        {
            yield return IsRegisteredUser;
            yield return IsFirstTimeBuyer;
            yield return IsEveryone;

            yield return Language;
            yield return StoreId;
            yield return Currency;
            yield return CustomerId;
            yield return CartTotal;
            yield return Coupon;

            yield return ShipmentMethodCode;
            yield return ShipmentMethodOption;
            yield return ShipmentMethodPrice;

            yield return PaymentMethodCode;
            yield return PaymentMethodPrice;

            yield return string.Join('&', Coupons ?? Array.Empty<string>());
            yield return string.Join('&', UserGroups ?? Array.Empty<string>());

            if (!Attributes.IsNullOrEmpty())
            {
                foreach (var attribute in Attributes)
                {
                    yield return $"{attribute.Key}-{attribute.Value}";
                }
            }

            yield return PromoEntry;

            if (!PromoEntries.IsNullOrEmpty())
            {
                foreach (var entry in PromoEntries)
                {
                    yield return entry;
                }
            }

            if (!CartPromoEntries.IsNullOrEmpty())
            {
                foreach (var entry in CartPromoEntries)
                {
                    yield return entry;
                }
            }
        }
    }
}
