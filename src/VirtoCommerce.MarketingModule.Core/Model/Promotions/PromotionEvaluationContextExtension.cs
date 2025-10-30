using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions;

public static class PromotionEvaluationContextExtension
{
    #region Dynamic expression evaluation helper methods

    public static int GetCartItemsQuantity(this PromotionEvaluationContext context, string[] excludingCategoryIds, string[] excludingProductIds)
    {
        var result = context.CartPromoEntries.ExcludeCategories(excludingCategoryIds)
            .ExcludeProducts(excludingProductIds)
            .Sum(x => x.Quantity);
        return result;
    }

    public static int GetCartItemsOfCategoryQuantity(this PromotionEvaluationContext context, string categoryId, string[] excludingCategoryIds, string[] excludingProductIds)
    {
        var result = context.CartPromoEntries.InCategories([categoryId])
            .ExcludeCategories(excludingCategoryIds)
            .ExcludeProducts(excludingProductIds)
            .Sum(x => x.Quantity);
        return result;
    }

    public static int GetCartItemsOfProductQuantity(this PromotionEvaluationContext context, string productId)
    {
        var result = context.CartPromoEntries.InProducts([productId])
            .Sum(x => x.Quantity);
        return result;
    }

    public static decimal GetCartTotalWithExcludings(this PromotionEvaluationContext context, string[] excludingCategoryIds, string[] excludingProductIds)
    {
        var result = context.CartPromoEntries.ExcludeCategories(excludingCategoryIds)
            .ExcludeProducts(excludingProductIds)
            .Sum(x => x.Price * x.Quantity);

        return result;
    }

    public static bool IsItemInCategory(this PromotionEvaluationContext context, string categoryId, string[] excludingCategoryIds, string[] excludingProductIds)
    {
        var result = new[] { context.PromoEntry }.InCategories([categoryId])
            .ExcludeCategories(excludingCategoryIds)
            .ExcludeProducts(excludingProductIds)
            .Any();
        return result;
    }

    public static bool IsItemCodeContains(this PromotionEvaluationContext context, string code)
    {
        var result = context.PromoEntry != null && !string.IsNullOrEmpty(context.PromoEntry.Code);
        if (result)
        {
            result = context.PromoEntry.Code.IndexOf(code, StringComparison.OrdinalIgnoreCase) != -1;
        }
        return result;
    }

    public static bool IsAnyLineItemExtendedTotalNew(this PromotionEvaluationContext context, decimal lineItemTotal, decimal lineItemTotalSecond, string compareCondition, string[] excludingCategoryIds, string[] excludingProductIds)
    {
        if (compareCondition.EqualsIgnoreCase(ConditionOperation.Exactly))
        {
            return context.CartPromoEntries.Where(x => x.Price * x.Quantity == lineItemTotal)
                .ExcludeCategories(excludingCategoryIds)
                .ExcludeProducts(excludingProductIds)
                .Any();
        }

        if (compareCondition.EqualsIgnoreCase(ConditionOperation.AtLeast))
        {
            return context.CartPromoEntries.Where(x => x.Price * x.Quantity >= lineItemTotal)
                .ExcludeCategories(excludingCategoryIds)
                .ExcludeProducts(excludingProductIds)
                .Any();
        }

        if (compareCondition.EqualsIgnoreCase(ConditionOperation.IsLessThanOrEqual))
        {
            return context.CartPromoEntries.Where(x => x.Price * x.Quantity <= lineItemTotal)
                .ExcludeCategories(excludingCategoryIds)
                .ExcludeProducts(excludingProductIds)
                .Any();
        }

        if (compareCondition.EqualsIgnoreCase(ConditionOperation.Between))
        {
            return context.CartPromoEntries.Where(x => x.Price * x.Quantity >= lineItemTotal && x.Quantity <= lineItemTotalSecond)
                .ExcludeCategories(excludingCategoryIds)
                .ExcludeProducts(excludingProductIds)
                .Any();
        }

        throw new Exception("CompareCondition has incorrect value.");
    }

    public static bool IsItemInProduct(this PromotionEvaluationContext context, string productId)
    {
        return new[] { context.PromoEntry }.InProducts([productId]).Any();
    }

    public static bool IsItemInProducts(this PromotionEvaluationContext context, string[] productIds)
    {
        return new[] { context.PromoEntry }.InProducts(productIds).Any();
    }

    public static bool IsParentItemInProduct(this PromotionEvaluationContext context, string productId)
    {
        return new[] { context.PromoEntry }.ParentInProducts([productId]).Any();
    }

    public static bool IsParentItemInProducts(this PromotionEvaluationContext context, string[] productIds)
    {
        return new[] { context.PromoEntry }.ParentInProducts(productIds).Any();
    }

    public static bool IsItemsInStockQuantityNew(this PromotionEvaluationContext context, string compareCondition, int quantity, int quantitySecond)
    {
        if (compareCondition.EqualsIgnoreCase(ConditionOperation.Exactly))
        {
            return context.PromoEntry.InStockQuantity == quantity;
        }

        if (compareCondition.EqualsIgnoreCase(ConditionOperation.AtLeast))
        {
            return context.PromoEntry.InStockQuantity >= quantity;
        }

        if (compareCondition.EqualsIgnoreCase(ConditionOperation.IsLessThanOrEqual))
        {
            return context.PromoEntry.InStockQuantity <= quantity;
        }

        if (compareCondition.EqualsIgnoreCase(ConditionOperation.Between))
        {
            return context.PromoEntry.InStockQuantity >= quantity && context.PromoEntry.InStockQuantity <= quantitySecond;
        }

        throw new Exception("CompareCondition has incorrect value.");
    }

    #endregion

    #region ProductPromoEntry extensions

    public static IEnumerable<ProductPromoEntry> InCategory(this IEnumerable<ProductPromoEntry> entries, string categoryId)
    {
        var result = entries.InCategories([categoryId]);
        return result;
    }

    public static IEnumerable<ProductPromoEntry> InCategories(this IEnumerable<ProductPromoEntry> entries, string[] categoryIds)
    {
        categoryIds = categoryIds.Where(x => x != null).ToArray();
        var promotionEntries = entries as ProductPromoEntry[] ?? entries.ToArray();
        return categoryIds.Any() ? promotionEntries.Where(x => ProductInCategories(x, categoryIds)) : promotionEntries;
    }

    public static IEnumerable<ProductPromoEntry> InProduct(this IEnumerable<ProductPromoEntry> entries, string productId)
    {
        var result = entries.InProducts([productId]);
        return result;
    }

    public static IEnumerable<ProductPromoEntry> InProducts(this IEnumerable<ProductPromoEntry> entries, string[] productIds)
    {
        productIds = productIds.Where(x => x != null).ToArray();
        var promotionEntries = entries as IList<ProductPromoEntry> ?? entries.ToList();
        return productIds.Any() ? promotionEntries.Where(x => ProductInProducts(x, productIds)) : promotionEntries;
    }

    public static IEnumerable<ProductPromoEntry> ParentInProducts(this IEnumerable<ProductPromoEntry> entries, string[] productIds)
    {
        productIds = productIds.Where(x => x != null).ToArray();
        var promotionEntries = entries as IList<ProductPromoEntry> ?? entries.ToList();
        return productIds.Length != 0 ? promotionEntries.Where(x => ParentProductInProducts(x, productIds)) : promotionEntries;
    }

    public static IEnumerable<ProductPromoEntry> ExcludeCategories(this IEnumerable<ProductPromoEntry> entries, string[] categoryIds)
    {
        var result = entries.Where(x => !ProductInCategories(x, categoryIds));
        return result;
    }

    public static IEnumerable<ProductPromoEntry> ExcludeProducts(this IEnumerable<ProductPromoEntry> entries, string[] productIds)
    {
        var result = entries.Where(x => !ProductInProducts(x, productIds));
        return result;
    }

    public static bool ProductInCategories(this ProductPromoEntry entry, ICollection<string> categoryIds)
    {
        var result = categoryIds.Contains(entry.CategoryId, StringComparer.OrdinalIgnoreCase);
        if (!result && entry.Outline != null)
        {
            result = entry.Outline.Split(';', '/', '\\').Intersect(categoryIds, StringComparer.OrdinalIgnoreCase).Any();
        }
        return result;
    }

    public static bool ProductInProducts(this ProductPromoEntry entry, IEnumerable<string> productIds)
    {
        return productIds.Contains(entry.ProductId, StringComparer.OrdinalIgnoreCase);
    }

    public static bool ParentProductInProducts(this ProductPromoEntry entry, IEnumerable<string> productIds)
    {
        return productIds.Contains(entry.ParentId, StringComparer.OrdinalIgnoreCase);
    }

    #endregion
}
