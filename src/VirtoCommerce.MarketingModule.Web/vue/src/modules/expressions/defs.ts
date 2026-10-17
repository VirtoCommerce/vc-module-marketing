// How each dynamic-expression node is rendered: a sentence of literal text and editable fields bound to the
// node's own properties (the API objects are edited in place and saved back untouched otherwise).
// Ported from the AngularJS templates in Scripts/dynamicConditions/templates.html (marketing) and
// vc-module-core Scripts/dynamicConditions/all-templates.html (shared geo/shopper conditions).
import type { LookupSource } from "../lookups";

export interface ChoiceOption {
  value: string;
  label: string;
}
export type Part =
  | string
  | { kind: "number"; path: string; int?: boolean; width?: number }
  | { kind: "text"; path: string; placeholder?: string; label?: string; multiline?: boolean }
  | { kind: "choice"; path: string; options: ChoiceOption[]; bool?: boolean }
  // compare condition select + value; a second value when "Between"
  | { kind: "range"; path: string; second: string; int?: boolean; options?: ChoiceOption[] }
  | { kind: "lookup"; path: string; source: LookupSource; multiple?: boolean }
  // categoryId/categoryName (+ prefix for nested objects)
  | { kind: "category"; prefix?: string }
  // productId/productName/productCode (+ prefix for nested objects like "product.")
  | { kind: "product"; prefix?: string }
  // productIds/productNames/productCodes
  | { kind: "products" }
  // plain id lists without names (excludingCategoryIds / excludingProductIds)
  | { kind: "ids"; path: string; of: "category" | "product"; placeholder?: string };

export interface Def {
  /** name in the "Add ..." menu */
  label?: string;
  /** group in the "Add ..." menu (dynamic content conditions) */
  group?: string;
  parts: Part[];
  /** label of the button that adds a child */
  addLabel?: string;
  /** validation: at least one child required */
  requireChild?: string;
}

const opts = (...pairs: [string, string][]): ChoiceOption[] => pairs.map(([value, label]) => ({ value, label }));

const allAny: Part = { kind: "choice", path: "all", bool: true, options: opts(["true", "all"], ["false", "any"]) };
// "not" is shown as the truth value of the block: not=false reads "true"
const trueFalse: Part = { kind: "choice", path: "not", bool: true, options: opts(["false", "true"], ["true", "false"]) };
const round: Part = {
  kind: "choice",
  path: "roundAmountPerItem",
  bool: true,
  options: opts(["true", "round"], ["false", "don't round"]),
};
const yesNo = (path: string): Part => ({ kind: "choice", path, bool: true, options: opts(["true", "yes"], ["false", "no"]) });

// ConditionInStockQuantity / cart item counts / subtotal
export const qtyCompare = opts(["Exactly", "exactly"], ["AtLeast", "at least"], ["IsLessThanOrEqual", "exactly or less than"], ["Between", "between"]);
// shopper age / time zone
const valueCompare = opts(
  // the server's default for new age/time zone conditions; same as "greater than or equals"
  ["AtLeast", "at least"],
  ["IsMatching", "exactly"],
  ["IsNotMatching", "anything but"],
  ["IsGreaterThan", "greater than"],
  ["IsGreaterThanOrEqual", "greater than or equals"],
  ["IsLessThan", "less than"],
  ["IsLessThanOrEqual", "less than or equals"],
);
const match: Part = {
  kind: "choice",
  path: "matchCondition",
  options: opts(["Contains", "containing"], ["NotContains", "not containing"], ["Matching", "being exactly"], ["NotMatching", "being anything but"]),
};

const num = (path: string): Part => ({ kind: "number", path });
const int = (path: string): Part => ({ kind: "number", path, int: true });
const text = (path: string, placeholder?: string): Part => ({ kind: "text", path, placeholder });
const product: Part = { kind: "product" };
const category: Part = { kind: "category" };

export const defs: Record<string, Def> = {
  // ---- promotion: customer
  BlockCustomerCondition: {
    parts: ["Customers matching", allAny, "of the following criteria:"],
    addLabel: "Add user group",
    requireChild: "Your promotion must have at least one eligibility criterion",
  },
  ConditionIsEveryone: { label: "Everyone", parts: ["Everyone"] },
  ConditionIsFirstTimeBuyer: { label: "First time customers", parts: ["First time buyer"] },
  ConditionIsRegisteredUser: { label: "Registered users", parts: ["Registered user"] },
  UserGroupsContainsCondition: {
    label: "User group contains ...",
    group: "Shopper profile",
    parts: ["User group contains", text("group", "Enter group")],
  },
  UserGroupIsCondition: {
    label: "User group is ...",
    group: "Shopper profile",
    parts: ["User group is", { kind: "lookup", path: "groups", source: "memberGroups", multiple: true }],
  },

  // ---- promotion: catalog
  BlockCatalogCondition: { parts: ["If", allAny, "of these catalog conditions are", trueFalse], addLabel: "Add condition" },
  ConditionCategoryIs: { label: "Specific category", parts: ["Category:", category] },
  ConditionEntryIs: {
    label: "Specific product",
    parts: ["Product:", { kind: "products" }, "Apply to all product variants:", yesNo("applyToAllVariants")],
  },
  ConditionCodeContains: { label: "Product code contains...", parts: ["Product code contains", text("keyword", "Enter code")] },
  ConditionCurrencyIs: { label: "Currency is...", parts: ["Currency is", { kind: "lookup", path: "currency", source: "currencies" }] },
  ConditionInStockQuantity: {
    label: "In stock quantity is...",
    parts: ["In stock quantity is", { kind: "range", path: "quantity", second: "quantitySecond", int: true }, "item(s)"],
  },
  ConditionHasNoSalePrice: {
    label: "Apply only to full price items and not sales items",
    parts: ["Apply only to full price items and not sales items"],
  },

  // ---- promotion: cart
  BlockCartCondition: { parts: ["If", allAny, "of these cart conditions are", trueFalse], addLabel: "Add condition" },
  ConditionAtNumItemsInCart: {
    label: "Number of items in the shopping cart",
    parts: [{ kind: "range", path: "numItem", second: "numItemSecond", int: true }, "items are in the shopping cart"],
  },
  ConditionAtNumItemsInCategoryAreInCart: {
    label: "Number of items out of a category in the shopping cart",
    parts: [{ kind: "range", path: "numItem", second: "numItemSecond", int: true }, "items from this category:", category, "are in shopping cart"],
  },
  ConditionAtNumItemsOfEntryAreInCart: {
    label: "Number of specific product items in the shopping cart",
    parts: [{ kind: "range", path: "numItem", second: "numItemSecond", int: true }, "specific product items:", product, "are in the shopping cart"],
  },
  PaymentIsCondition: { label: "Payment type is...", parts: ["Payment type is", { kind: "lookup", path: "paymentMethod", source: "paymentMethods" }] },
  ShippingIsCondition: { label: "Shipping type is...", parts: ["Shipping type is", { kind: "lookup", path: "shippingMethod", source: "shippingMethods" }] },
  ConditionCartSubtotalLeast: {
    label: "Cart subtotal is...",
    parts: ["Cart subtotal is", { kind: "range", path: "subTotal", second: "subTotalSecond" }],
  },

  // ---- promotion: rewards
  BlockReward: {
    parts: ["Get the following rewards:"],
    addLabel: "Add reward",
    requireChild: "Your promotion must have at least one reward",
  },
  RewardCartGetOfAbsSubtotal: { label: "$... off cart subtotal", parts: ["$", num("amount"), "off cart subtotal"] },
  RewardCartGetOfRelSubtotal: {
    label: "...% off cart subtotal, no more than $...",
    parts: [num("amount"), "% off cart subtotal, no more than $", num("maxLimit")],
  },
  RewardItemGetFreeNumItemOfProduct: { label: "... free items of ... product", parts: [int("numItem"), "free items of this product:", product] },
  RewardItemGetOfAbs: { label: "$... off", parts: ["$", num("amount"), "off for", product] },
  RewardItemGetOfAbsForNum: {
    label: "$... off for ... specific product items",
    parts: ["$", num("amount"), "off for", int("numItem"), "items of this product:", product, ",", round, "amount per item"],
  },
  RewardItemGetOfRel: {
    label: "...% off for product ..., no more than $...",
    parts: [num("amount"), "% off for", product, ", no more than $", num("maxLimit"), ",", round, "amount per item"],
  },
  RewardItemGetOfRelForNum: {
    label: "...% off for ... specific product items, no more than $...",
    parts: [num("amount"), "% off, no more than $", num("maxLimit"), "for", int("numItem"), "items of this product:", product, ",", round, "amount per item"],
  },
  RewardItemGiftNumItem: {
    label: "... items of ... product as a gift",
    parts: [
      int("quantity"),
      "items of this product:",
      product,
      "as a gift. Details:",
      { kind: "text", path: "name", label: "Name", placeholder: "Enter name" },
      { kind: "text", path: "measureUnit", label: "Measurement unit", placeholder: "Specify measurement" },
      { kind: "text", path: "imageUrl", label: "Image URL", placeholder: "Enter URL" },
      { kind: "text", path: "description", label: "Description", placeholder: "Enter description", multiline: true },
    ],
  },
  RewardShippingGetOfAbsShippingMethod: {
    label: "$... off for shipping at ...",
    parts: ["$", num("amount"), "off for shipping of this type:", { kind: "lookup", path: "shippingMethod", source: "shippingMethods" }],
  },
  RewardShippingGetOfRelShippingMethod: {
    label: "% off for shipping at ..., no more than $...",
    parts: [num("amount"), "% off for shipping of this type:", { kind: "lookup", path: "shippingMethod", source: "shippingMethods" }, ", no more than $", num("maxLimit")],
  },
  RewardPaymentGetOfAbs: {
    label: "$... off for using ... payment method",
    parts: ["$", num("amount"), "off for using this payment method:", { kind: "lookup", path: "paymentMethod", source: "paymentMethods" }],
  },
  RewardPaymentGetOfRel: {
    label: "...% off for using ... payment method, no more than $...",
    parts: [num("amount"), "% off for using this payment method:", { kind: "lookup", path: "paymentMethod", source: "paymentMethods" }, ", no more than $", num("maxLimit")],
  },
  RewardItemForEveryNumInGetOfRel: {
    label: "...% off for ... of every ... specific product items",
    parts: [
      "For",
      int("forNthQuantity"),
      "of every",
      int("inEveryNthQuantity"),
      "items of this product:",
      { kind: "product", prefix: "product." },
      "receive",
      num("amount"),
      "% off, no more than",
      int("itemLimit"),
      "items and no more than $",
      num("maxLimit"),
      ",",
      round,
      "amount per item",
    ],
  },
  RewardItemForEveryNumOtherItemInGetOfRel: {
    label: "...% off for ... items of a specific product per every ... items of another product",
    parts: [
      "For",
      int("forNthQuantity"),
      "items of this product:",
      { kind: "product", prefix: "product." },
      "of every",
      int("inEveryNthQuantity"),
      "items of this product:",
      { kind: "product", prefix: "conditionalProduct." },
      "receive",
      num("amount"),
      "% off, no more than",
      int("itemLimit"),
      "items and no more than $",
      num("maxLimit"),
    ],
  },

  // ---- dynamic content publishing
  BlockContentCondition: { parts: ["If", allAny, "of these conditions are true"], addLabel: "Add condition" },
  DynamicContentConditionCategoryIs: { label: "Specific category", group: "Catalog", parts: ["Category:", category] },
  DynamicContentConditionProductIs: { label: "Specific product", group: "Catalog", parts: ["Product:", { kind: "products" }] },
  TagsContainsCondition: { label: "Tags contain ...", parts: ["Tags contain", text("tag", "Enter tag")] },
  ConditionStoreSearchedPhrase: {
    label: "Shopper searched for ... in the store",
    group: "Browse behavior",
    parts: ["Shopper searched for a phrase:", match, text("value")],
  },
  ConditionLanguageIs: {
    label: "User language is set to...",
    group: "Browse behavior",
    parts: ["The user language is set to", { kind: "lookup", path: "value", source: "languages" }],
  },
  ConditionAgeIs: {
    label: "Shopper age is ...",
    group: "Shopper profile",
    parts: ["Shopper age is", { kind: "choice", path: "compareCondition", options: valueCompare }, int("value")],
  },
  ConditionGenderIs: {
    label: "Shopper gender is ...",
    group: "Shopper profile",
    parts: ["Shopper gender is", { kind: "choice", path: "value", options: opts(["female", "female"], ["male", "male"]) }],
  },
  ConditionGeoTimeZone: {
    label: "Time zone",
    group: "Location",
    parts: [
      "Browsing from a time zone that is",
      { kind: "choice", path: "compareCondition", options: valueCompare },
      { kind: "lookup", path: "value", source: "timeZones" },
    ],
  },
  ConditionGeoZipCode: { label: "Zip/postal code", group: "Location", parts: ["Browsing from a location with ZIP/postal code:", match, text("value")] },
  ConditionGeoCity: { label: "City", group: "Location", parts: ["Browsing from this city:", text("value")] },
  ConditionGeoCountry: {
    label: "Country",
    group: "Location",
    parts: ["Browsing from this country:", { kind: "lookup", path: "value", source: "countries" }],
  },
  ConditionGeoState: { label: "Region/state/province", group: "Location", parts: ["Browsing from this state or province:", text("value")] },
};

const SKIP = new Set(["id", "availableChildren", "children"]);
const humanize = (id: string) => id.replace(/([a-z])([A-Z])/g, "$1 $2");

// Any node the table doesn't know (e.g. registered by another module): its id plus an editor per primitive property
export function defOf(node: Record<string, unknown>): Def {
  const id = String(node.id ?? "");
  const known = defs[id];
  if (known) return known;
  const parts: Part[] = [humanize(id)];
  for (const [key, value] of Object.entries(node)) {
    if (SKIP.has(key) || key.startsWith("excluding")) continue;
    if (typeof value === "number") parts.push(key, { kind: "number", path: key });
    else if (typeof value === "boolean") parts.push(key, yesNo(key));
    else if (typeof value === "string" || value === null) parts.push({ kind: "text", path: key, placeholder: key });
  }
  return { label: humanize(id), parts };
}

type TreeNode = { id?: string; children?: TreeNode[] };

// the AngularJS getValidationError rules: blocks that need at least one child
export function treeErrors(node: TreeNode | undefined): string[] {
  if (!node) return [];
  const own = node.id && defs[node.id]?.requireChild && !node.children?.length ? [defs[node.id].requireChild!] : [];
  return [...own, ...(node.children ?? []).flatMap(treeErrors)];
}
