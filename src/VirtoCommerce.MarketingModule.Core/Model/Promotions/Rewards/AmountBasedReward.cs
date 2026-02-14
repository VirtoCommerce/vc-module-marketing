using System;
using VirtoCommerce.CoreModule.Core.Currency;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions
{
    public abstract class AmountBasedReward : PromotionReward
    {
        protected AmountBasedReward(string rewardType)
            : base(rewardType)
        {
        }

        public RewardAmountType AmountType { get; set; }

        /// <summary>
        /// Reward amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The max reward amount limit (Not to exceed $S)
        /// </summary>
        public decimal MaxLimit { get; set; }

        /// <summary>
        /// The max  quantity limit (No more than Q items)
        /// </summary>
        public int Quantity { get; set; }

        //For N in every Y items
        public int ForNthQuantity { get; set; }
        public int InEveryNthQuantity { get; set; }

        public bool RoundAmountPerItem { get; set; }

        /// <summary>
        ///  Get per item reward amount for given items quantity and price
        /// </summary>
        /// <param name="price">Price per item</param>
        /// <param name="quantity">Total items quantity</param>
        /// <returns></returns>
        [Obsolete("Use GetAmountPerItem(decimal price, int quantity, Currency currency) or GetTotalAmount(decimal price, int quantity, Currency currency)", DiagnosticId = "VC0010", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
        public virtual decimal GetRewardAmount(decimal price, int quantity)
        {
            var totalAmount = GetTotalAmount(price, quantity);

            return totalAmount / quantity;
        }

        /// <summary>
        /// Get per item reward amount for given items quantity and price
        /// </summary>
        /// <param name="price">Price per item</param>
        /// <param name="quantity">Total items quantity</param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public virtual decimal GetAmountPerItem(decimal price, int quantity, Currency currency)
        {
            ArgumentNullException.ThrowIfNull(currency);

            var totalAmount = GetTotalAmount(price, quantity);
            var amountPerItem = totalAmount / quantity;

            if (RoundAmountPerItem)
            {
                amountPerItem = currency.RoundingPolicy.RoundMoney(amountPerItem, currency);
            }

            return amountPerItem;
        }

        /// <summary>
        ///  Get total reward amount for given items quantity and price
        /// </summary>
        /// <param name="price">Price per item</param>
        /// <param name="quantity">Total items quantity</param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public virtual decimal GetTotalAmount(decimal price, int quantity, Currency currency)
        {
            ArgumentNullException.ThrowIfNull(currency);

            var totalAmount = GetTotalAmount(price, quantity);

            if (RoundAmountPerItem)
            {
                var amountPerItem = totalAmount / quantity;
                amountPerItem = currency.RoundingPolicy.RoundMoney(amountPerItem, currency);
                totalAmount = amountPerItem * quantity;
            }

            return totalAmount;
        }


        protected virtual decimal GetTotalAmount(decimal price, int quantity)
        {
            if (price < 0)
            {
                throw new ArgumentException($"The {nameof(price)} cannot be negative", nameof(price));
            }

            if (quantity < 0)
            {
                throw new ArgumentException($"The {nameof(quantity)} cannot be negative", nameof(quantity));
            }

            var workQuantity = quantity = Math.Max(1, quantity);

            if (ForNthQuantity > 0 && InEveryNthQuantity > 0)
            {
                workQuantity = workQuantity / InEveryNthQuantity * ForNthQuantity;
            }

            if (Quantity > 0)
            {
                workQuantity = Math.Min(Quantity, workQuantity);
            }

            var totalAmount = Amount * workQuantity;

            if (AmountType == RewardAmountType.Relative)
            {
                totalAmount = totalAmount * price * 0.01m;
            }

            var totalCost = price * quantity;

            // Use total cost as MaxLimit if it is not explicitly set
            var workMaxLimit = MaxLimit > 0 ? MaxLimit : totalCost;

            // Do not allow MaxLimit to be greater than total cost (to prevent reward amount from being greater than price)
            workMaxLimit = Math.Min(workMaxLimit, totalCost);

            totalAmount = Math.Min(workMaxLimit, totalAmount);

            return totalAmount;
        }
    }
}
