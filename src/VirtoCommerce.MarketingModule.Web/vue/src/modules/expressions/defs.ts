// How each dynamic-expression node is rendered: a sentence of literal text and editable fields bound to the
// node's own properties (the API objects are edited in place and saved back untouched otherwise).
// Ported from the AngularJS templates in Scripts/dynamicConditions/templates.html (marketing) and
// vc-module-core Scripts/dynamicConditions/all-templates.html (shared geo/shopper conditions).
// Every text below is an i18n key (MARKETING.EXPRESSIONS in locales/*.json); the components translate it.
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

const k = (key: string) => `MARKETING.EXPRESSIONS.${key}`;
const opts = (...pairs: [string, string][]): ChoiceOption[] => pairs.map(([value, label]) => ({ value, label: k(`OPTIONS.${label}`) }));

const allAny: Part = { kind: "choice", path: "all", bool: true, options: opts(["true", "ALL"], ["false", "ANY"]) };
// "not" is shown as the truth value of the block: not=false reads "true"
const trueFalse: Part = { kind: "choice", path: "not", bool: true, options: opts(["false", "TRUE"], ["true", "FALSE"]) };
const round: Part = {
  kind: "choice",
  path: "roundAmountPerItem",
  bool: true,
  options: opts(["true", "ROUND"], ["false", "DONT_ROUND"]),
};
const yesNo = (path: string): Part => ({ kind: "choice", path, bool: true, options: opts(["true", "YES"], ["false", "NO"]) });

// ConditionInStockQuantity / cart item counts / subtotal
export const qtyCompare = opts(["Exactly", "EXACTLY"], ["AtLeast", "AT_LEAST"], ["IsLessThanOrEqual", "EXACTLY_OR_LESS"], ["Between", "BETWEEN"]);
// shopper age / time zone
const valueCompare = opts(
  // the server's default for new age/time zone conditions; same as "greater than or equals"
  ["AtLeast", "AT_LEAST"],
  ["IsMatching", "EXACTLY"],
  ["IsNotMatching", "ANYTHING_BUT"],
  ["IsGreaterThan", "GREATER_THAN"],
  ["IsGreaterThanOrEqual", "GREATER_OR_EQUAL"],
  ["IsLessThan", "LESS_THAN"],
  ["IsLessThanOrEqual", "LESS_OR_EQUAL"],
);
const match: Part = {
  kind: "choice",
  path: "matchCondition",
  options: opts(["Contains", "CONTAINING"], ["NotContains", "NOT_CONTAINING"], ["Matching", "BEING_EXACTLY"], ["NotMatching", "BEING_ANYTHING_BUT"]),
};

const num = (path: string): Part => ({ kind: "number", path });
const int = (path: string): Part => ({ kind: "number", path, int: true });
const text = (path: string, placeholder?: string): Part => ({ kind: "text", path, placeholder: placeholder && k(`PLACEHOLDERS.${placeholder}`) });
const product: Part = { kind: "product" };
const category: Part = { kind: "category" };
// sentence fragments, "Add ..." menu names and groups
const say = (key: string) => k(`TEXT.${key}`);
const menu = (key: string) => k(`NAMES.${key}`);
const group = (key: string) => k(`GROUPS.${key}`);
const dollar = say("CURRENCY");
const comma = say("COMMA");

export const defs: Record<string, Def> = {
  // ---- promotion: customer
  BlockCustomerCondition: {
    parts: [say("CUSTOMERS_MATCHING"), allAny, say("OF_FOLLOWING_CRITERIA")],
    addLabel: k("ADD_USER_GROUP"),
    requireChild: k("NO_CRITERION"),
  },
  ConditionIsEveryone: { label: menu("EVERYONE"), parts: [say("EVERYONE")] },
  ConditionIsFirstTimeBuyer: { label: menu("FIRST_TIME_CUSTOMERS"), parts: [say("FIRST_TIME_BUYER")] },
  ConditionIsRegisteredUser: { label: menu("REGISTERED_USERS"), parts: [say("REGISTERED_USER")] },
  UserGroupsContainsCondition: {
    label: menu("USER_GROUP_CONTAINS"),
    group: group("SHOPPER_PROFILE"),
    parts: [say("USER_GROUP_CONTAINS"), text("group", "ENTER_GROUP")],
  },
  UserGroupIsCondition: {
    label: menu("USER_GROUP_IS"),
    group: group("SHOPPER_PROFILE"),
    parts: [say("USER_GROUP_IS"), { kind: "lookup", path: "groups", source: "memberGroups", multiple: true }],
  },

  // ---- promotion: catalog
  BlockCatalogCondition: { parts: [say("IF"), allAny, say("CATALOG_CONDITIONS_ARE"), trueFalse], addLabel: k("ADD_CONDITION") },
  ConditionCategoryIs: { label: menu("SPECIFIC_CATEGORY"), parts: [say("CATEGORY"), category] },
  ConditionEntryIs: {
    label: menu("SPECIFIC_PRODUCT"),
    parts: [say("PRODUCT"), { kind: "products" }, say("ALL_VARIANTS"), yesNo("applyToAllVariants")],
  },
  ConditionCodeContains: { label: menu("CODE_CONTAINS"), parts: [say("CODE_CONTAINS"), text("keyword", "ENTER_CODE")] },
  ConditionCurrencyIs: { label: menu("CURRENCY_IS"), parts: [say("CURRENCY_IS"), { kind: "lookup", path: "currency", source: "currencies" }] },
  ConditionInStockQuantity: {
    label: menu("IN_STOCK_QUANTITY"),
    parts: [say("IN_STOCK_QUANTITY"), { kind: "range", path: "quantity", second: "quantitySecond", int: true }, say("ITEMS")],
  },
  ConditionHasNoSalePrice: {
    label: menu("FULL_PRICE_ONLY"),
    parts: [say("FULL_PRICE_ONLY")],
  },

  // ---- promotion: cart
  BlockCartCondition: { parts: [say("IF"), allAny, say("CART_CONDITIONS_ARE"), trueFalse], addLabel: k("ADD_CONDITION") },
  ConditionAtNumItemsInCart: {
    label: menu("ITEMS_IN_CART"),
    parts: [{ kind: "range", path: "numItem", second: "numItemSecond", int: true }, say("ITEMS_IN_CART")],
  },
  ConditionAtNumItemsInCategoryAreInCart: {
    label: menu("CATEGORY_ITEMS_IN_CART"),
    parts: [{ kind: "range", path: "numItem", second: "numItemSecond", int: true }, say("ITEMS_FROM_CATEGORY"), category, say("ARE_IN_CART")],
  },
  ConditionAtNumItemsOfEntryAreInCart: {
    label: menu("PRODUCT_ITEMS_IN_CART"),
    parts: [{ kind: "range", path: "numItem", second: "numItemSecond", int: true }, say("SPECIFIC_PRODUCT_ITEMS"), product, say("ARE_IN_CART")],
  },
  PaymentIsCondition: { label: menu("PAYMENT_IS"), parts: [say("PAYMENT_IS"), { kind: "lookup", path: "paymentMethod", source: "paymentMethods" }] },
  ShippingIsCondition: { label: menu("SHIPPING_IS"), parts: [say("SHIPPING_IS"), { kind: "lookup", path: "shippingMethod", source: "shippingMethods" }] },
  ConditionCartSubtotalLeast: {
    label: menu("CART_SUBTOTAL"),
    parts: [say("CART_SUBTOTAL_IS"), { kind: "range", path: "subTotal", second: "subTotalSecond" }],
  },

  // ---- promotion: rewards
  BlockReward: {
    parts: [say("GET_REWARDS")],
    addLabel: k("ADD_REWARD"),
    requireChild: k("NO_REWARD"),
  },
  RewardCartGetOfAbsSubtotal: { label: menu("CART_ABS_OFF"), parts: [dollar, num("amount"), say("OFF_CART_SUBTOTAL")] },
  RewardCartGetOfRelSubtotal: {
    label: menu("CART_REL_OFF"),
    parts: [num("amount"), say("PCT_OFF_CART_SUBTOTAL_MAX"), num("maxLimit")],
  },
  RewardItemGetFreeNumItemOfProduct: { label: menu("FREE_ITEMS"), parts: [int("numItem"), say("FREE_ITEMS_OF_PRODUCT"), product] },
  RewardItemGetOfAbs: { label: menu("ITEM_ABS_OFF"), parts: [dollar, num("amount"), say("OFF_FOR"), product] },
  RewardItemGetOfAbsForNum: {
    label: menu("ITEM_ABS_OFF_FOR_NUM"),
    parts: [dollar, num("amount"), say("OFF_FOR"), int("numItem"), say("ITEMS_OF_PRODUCT"), product, comma, round, say("AMOUNT_PER_ITEM")],
  },
  RewardItemGetOfRel: {
    label: menu("ITEM_REL_OFF"),
    parts: [num("amount"), say("PCT_OFF_FOR"), product, say("NO_MORE_THAN_AMOUNT"), num("maxLimit"), comma, round, say("AMOUNT_PER_ITEM")],
  },
  RewardItemGetOfRelForNum: {
    label: menu("ITEM_REL_OFF_FOR_NUM"),
    parts: [num("amount"), say("PCT_OFF_MAX"), num("maxLimit"), say("FOR_ITEMS"), int("numItem"), say("ITEMS_OF_PRODUCT"), product, comma, round, say("AMOUNT_PER_ITEM")],
  },
  RewardItemGiftNumItem: {
    label: menu("GIFT"),
    parts: [
      int("quantity"),
      say("ITEMS_OF_PRODUCT"),
      product,
      say("AS_GIFT"),
      { kind: "text", path: "name", label: k("FIELDS.NAME"), placeholder: k("PLACEHOLDERS.ENTER_NAME") },
      { kind: "text", path: "measureUnit", label: k("FIELDS.MEASURE_UNIT"), placeholder: k("PLACEHOLDERS.ENTER_MEASURE_UNIT") },
      { kind: "text", path: "imageUrl", label: k("FIELDS.IMAGE_URL"), placeholder: k("PLACEHOLDERS.ENTER_URL") },
      { kind: "text", path: "description", label: k("FIELDS.DESCRIPTION"), placeholder: k("PLACEHOLDERS.ENTER_DESCRIPTION"), multiline: true },
    ],
  },
  RewardShippingGetOfAbsShippingMethod: {
    label: menu("SHIPPING_ABS_OFF"),
    parts: [dollar, num("amount"), say("OFF_FOR_SHIPPING"), { kind: "lookup", path: "shippingMethod", source: "shippingMethods" }],
  },
  RewardShippingGetOfRelShippingMethod: {
    label: menu("SHIPPING_REL_OFF"),
    parts: [num("amount"), say("PCT_OFF_FOR_SHIPPING"), { kind: "lookup", path: "shippingMethod", source: "shippingMethods" }, say("NO_MORE_THAN_AMOUNT"), num("maxLimit")],
  },
  RewardPaymentGetOfAbs: {
    label: menu("PAYMENT_ABS_OFF"),
    parts: [dollar, num("amount"), say("OFF_FOR_PAYMENT"), { kind: "lookup", path: "paymentMethod", source: "paymentMethods" }],
  },
  RewardPaymentGetOfRel: {
    label: menu("PAYMENT_REL_OFF"),
    parts: [num("amount"), say("PCT_OFF_FOR_PAYMENT"), { kind: "lookup", path: "paymentMethod", source: "paymentMethods" }, say("NO_MORE_THAN_AMOUNT"), num("maxLimit")],
  },
  RewardItemForEveryNumInGetOfRel: {
    label: menu("EVERY_NTH_OFF"),
    parts: [
      say("FOR"),
      int("forNthQuantity"),
      say("OF_EVERY"),
      int("inEveryNthQuantity"),
      say("ITEMS_OF_PRODUCT"),
      { kind: "product", prefix: "product." },
      say("RECEIVE"),
      num("amount"),
      say("PCT_OFF_MAX_ITEMS"),
      int("itemLimit"),
      say("ITEMS_AND_MAX_AMOUNT"),
      num("maxLimit"),
      comma,
      round,
      say("AMOUNT_PER_ITEM"),
    ],
  },
  RewardItemForEveryNumOtherItemInGetOfRel: {
    label: menu("EVERY_NTH_OTHER_OFF"),
    parts: [
      say("FOR"),
      int("forNthQuantity"),
      say("ITEMS_OF_PRODUCT"),
      { kind: "product", prefix: "product." },
      say("OF_EVERY"),
      int("inEveryNthQuantity"),
      say("ITEMS_OF_PRODUCT"),
      { kind: "product", prefix: "conditionalProduct." },
      say("RECEIVE"),
      num("amount"),
      say("PCT_OFF_MAX_ITEMS"),
      int("itemLimit"),
      say("ITEMS_AND_MAX_AMOUNT"),
      num("maxLimit"),
    ],
  },

  // ---- dynamic content publishing
  BlockContentCondition: { parts: [say("IF"), allAny, say("CONDITIONS_ARE_TRUE")], addLabel: k("ADD_CONDITION") },
  DynamicContentConditionCategoryIs: { label: menu("SPECIFIC_CATEGORY"), group: group("CATALOG"), parts: [say("CATEGORY"), category] },
  DynamicContentConditionProductIs: { label: menu("SPECIFIC_PRODUCT"), group: group("CATALOG"), parts: [say("PRODUCT"), { kind: "products" }] },
  TagsContainsCondition: { label: menu("TAGS_CONTAIN"), parts: [say("TAGS_CONTAIN"), text("tag", "ENTER_TAG")] },
  ConditionStoreSearchedPhrase: {
    label: menu("SEARCHED_PHRASE"),
    group: group("BROWSE_BEHAVIOR"),
    parts: [say("SEARCHED_PHRASE"), match, text("value")],
  },
  ConditionLanguageIs: {
    label: menu("LANGUAGE_IS"),
    group: group("BROWSE_BEHAVIOR"),
    parts: [say("LANGUAGE_IS"), { kind: "lookup", path: "value", source: "languages" }],
  },
  ConditionAgeIs: {
    label: menu("AGE_IS"),
    group: group("SHOPPER_PROFILE"),
    parts: [say("AGE_IS"), { kind: "choice", path: "compareCondition", options: valueCompare }, int("value")],
  },
  ConditionGenderIs: {
    label: menu("GENDER_IS"),
    group: group("SHOPPER_PROFILE"),
    parts: [say("GENDER_IS"), { kind: "choice", path: "value", options: opts(["female", "FEMALE"], ["male", "MALE"]) }],
  },
  ConditionGeoTimeZone: {
    label: menu("TIME_ZONE"),
    group: group("LOCATION"),
    parts: [
      say("FROM_TIME_ZONE"),
      { kind: "choice", path: "compareCondition", options: valueCompare },
      { kind: "lookup", path: "value", source: "timeZones" },
    ],
  },
  ConditionGeoZipCode: { label: menu("ZIP_CODE"), group: group("LOCATION"), parts: [say("FROM_ZIP_CODE"), match, text("value")] },
  ConditionGeoCity: { label: menu("CITY"), group: group("LOCATION"), parts: [say("FROM_CITY"), text("value")] },
  ConditionGeoCountry: {
    label: menu("COUNTRY"),
    group: group("LOCATION"),
    parts: [say("FROM_COUNTRY"), { kind: "lookup", path: "value", source: "countries" }],
  },
  ConditionGeoState: { label: menu("STATE"), group: group("LOCATION"), parts: [say("FROM_STATE"), text("value")] },
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
