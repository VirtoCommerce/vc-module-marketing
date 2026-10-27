<template>
  <!-- number -->
  <VcInput
    v-if="f.kind === 'number'"
    class="mk-field mk-field--num"
    size="small"
    :type="f.int ? 'integer' : 'number'"
    :aria-label="pathLabel(f.path)"
    :model-value="get(f.path) as number"
    :disabled="disabled"
    @update:model-value="(v) => set(f.path, toNumber(v))"
  />

  <!-- text / textarea (labelled ones take a full row) -->
  <template v-else-if="f.kind === 'text'">
    <VcTextarea
      v-if="f.multiline"
      class="mk-field mk-field--row"
      :label="tr(f.label)"
      :placeholder="tr(f.placeholder)"
      :model-value="(get(f.path) as string) ?? ''"
      :disabled="disabled"
      @update:model-value="(v) => set(f.path, v)"
    />
    <VcInput
      v-else
      class="mk-field"
      :class="f.label ? 'mk-field--row' : 'mk-field--text'"
      :size="f.label ? 'default' : 'small'"
      :label="tr(f.label)"
      :aria-label="f.label ? undefined : tr(f.placeholder) || pathLabel(f.path)"
      :placeholder="tr(f.placeholder)"
      :model-value="(get(f.path) as string) ?? ''"
      :disabled="disabled"
      @update:model-value="(v) => set(f.path, v)"
    />
  </template>

  <!-- one of a fixed list -->
  <VcSelect
    v-else-if="f.kind === 'choice'"
    class="mk-field mk-field--choice"
    size="small"
    :options="f.options"
    option-value="value"
    :option-label="optionLabel"
    :clearable="false"
    :model-value="f.bool ? String(!!get(f.path)) : (get(f.path) as string)"
    :disabled="disabled"
    @update:model-value="(v) => set(f.path, f.bool ? v === 'true' : v)"
  />

  <!-- compare condition + value (+ second value for "between") -->
  <template v-else-if="f.kind === 'range'">
    <VcSelect
      class="mk-field mk-field--choice"
      size="small"
      :options="f.options ?? qtyCompare"
      option-value="value"
      :option-label="optionLabel"
      :clearable="false"
      :model-value="get('compareCondition') as string"
      :disabled="disabled"
      @update:model-value="onCompare"
    />
    <VcInput
      class="mk-field mk-field--num"
      size="small"
      :type="f.int ? 'integer' : 'number'"
      :aria-label="pathLabel(f.path)"
      :model-value="get(f.path) as number"
      :disabled="disabled"
      @update:model-value="(v) => set(f.path, toNumber(v))"
    />
    <template v-if="get('compareCondition') === 'Between'">
      <span>{{ $t("MARKETING.EXPRESSIONS.TEXT.AND") }}</span>
      <VcInput
        class="mk-field mk-field--num"
        size="small"
        :type="f.int ? 'integer' : 'number'"
        :aria-label="pathLabel(f.second)"
        :model-value="get(f.second) as number"
        :disabled="disabled"
        @update:model-value="(v) => set(f.second, toNumber(v))"
      />
    </template>
  </template>

  <!-- dictionaries: currencies, methods, languages, countries, groups, time zones -->
  <VcSelect
    v-else-if="f.kind === 'lookup'"
    class="mk-field"
    :class="f.multiple ? 'mk-field--wide' : 'mk-field--lookup'"
    size="small"
    searchable
    :multiple="f.multiple"
    :options="lookupOptions"
    option-value="id"
    option-label="name"
    :placeholder="$t(lookupPlaceholders[f.source])"
    :model-value="(get(f.path) as string | string[] | null) ?? (f.multiple ? [] : null)"
    :disabled="disabled"
    @update:model-value="(v) => set(f.path, f.multiple && !(v as string[])?.length ? null : (v ?? null))"
  />

  <!-- a catalog category -->
  <VcSelect
    v-else-if="f.kind === 'category'"
    class="mk-field mk-field--wide"
    size="small"
    searchable
    :options="categoryOptions"
    option-value="id"
    option-label="name"
    :placeholder="$t('MARKETING.EXPRESSIONS.PLACEHOLDERS.SELECT_CATEGORY')"
    :model-value="(get(p('categoryId')) as string) ?? null"
    :disabled="disabled"
    @update:model-value="onCategory"
  />

  <!-- a product -->
  <VcSelect
    v-else-if="f.kind === 'product'"
    class="mk-field mk-field--wide"
    size="small"
    searchable
    :options="productOptions"
    option-value="id"
    :option-label="productLabel"
    :placeholder="$t('MARKETING.EXPRESSIONS.PLACEHOLDERS.SELECT_PRODUCT')"
    :model-value="(get(p('productId')) as string) ?? null"
    :disabled="disabled"
    @update:model-value="onProduct"
  />

  <!-- several products -->
  <VcSelect
    v-else-if="f.kind === 'products'"
    class="mk-field mk-field--wide"
    size="small"
    searchable
    multiple
    :options="productsOptions"
    option-value="id"
    :option-label="productLabel"
    :placeholder="$t('MARKETING.EXPRESSIONS.PLACEHOLDERS.SELECT_PRODUCTS')"
    :model-value="productIds"
    :disabled="disabled"
    @update:model-value="onProducts"
  />

  <!-- ids only (exclusions) -->
  <VcSelect
    v-else-if="f.kind === 'ids'"
    class="mk-field mk-field--ids"
    size="small"
    searchable
    multiple
    :options="idsOptions"
    option-value="id"
    :option-label="productLabel"
    :placeholder="tr(f.placeholder)"
    :model-value="(get(f.path) as string[]) ?? []"
    :disabled="disabled"
    @update:model-value="(v) => set(f.path, (v as string[]) ?? [])"
  />
</template>

<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { VcInput, VcSelect, VcTextarea } from "@vc-shell/framework/ui";
import { qtyCompare, type ChoiceOption, type Part } from "./defs";
import { lookups, searchCategories, searchProducts, type LookupSource, type Option, type Page } from "../lookups";

type Node = Record<string, unknown>;
type FieldPart = Exclude<Part, string>;

const props = defineProps<{ node: Node; part: FieldPart; disabled?: boolean }>();
const { t } = useI18n({ useScope: "global" });

// the template reads the part through one loose shape (each v-if branch only touches its own kind's keys)
type Loose = {
  kind: FieldPart["kind"];
  path: string;
  second: string;
  source: LookupSource;
  int?: boolean;
  bool?: boolean;
  multiple?: boolean;
  multiline?: boolean;
  label?: string;
  placeholder?: string;
  options?: ChoiceOption[];
};
const f = computed(() => props.part as unknown as Loose);

// dotted paths address nested objects, e.g. "product.productId" in the "for every N items" rewards
function get(path: string): unknown {
  return path.split(".").reduce<unknown>((o, k) => (o == null ? undefined : (o as Node)[k]), props.node);
}
function set(path: string, value: unknown) {
  const keys = path.split(".");
  const last = keys.pop()!;
  let target = props.node;
  for (const k of keys) {
    target[k] ??= {};
    target = target[k] as Node;
  }
  target[last] = value;
}
const prefix = () => ("prefix" in props.part && props.part.prefix) || "";
const p = (name: string) => prefix() + name;

const lookupPlaceholders: Record<LookupSource, string> = {
  currencies: "MARKETING.EXPRESSIONS.PLACEHOLDERS.SELECT_CURRENCY",
  shippingMethods: "MARKETING.EXPRESSIONS.PLACEHOLDERS.SELECT_SHIPPING_METHOD",
  paymentMethods: "MARKETING.EXPRESSIONS.PLACEHOLDERS.SELECT_PAYMENT_METHOD",
  languages: "MARKETING.EXPRESSIONS.PLACEHOLDERS.SELECT_LANGUAGE",
  memberGroups: "MARKETING.EXPRESSIONS.PLACEHOLDERS.SELECT_GROUPS",
  countries: "MARKETING.EXPRESSIONS.PLACEHOLDERS.SELECT_COUNTRY",
  timeZones: "MARKETING.EXPRESSIONS.PLACEHOLDERS.SELECT_TIME_ZONE",
};

// labels and placeholders are i18n keys (or, for nodes defs.ts doesn't know, property names shown as they are)
const tr = (key?: string) => (key ? t(key) : undefined);
const optionLabel = (x: ChoiceOption) => t(x.label);

// accessible names of the inputs without a label or placeholder (the sentence around them says what they are)
const pathLabels: Record<string, string> = {
  amount: "AMOUNT",
  maxLimit: "MAX_AMOUNT",
  itemLimit: "MAX_ITEMS",
  numItem: "QUANTITY",
  quantity: "QUANTITY",
  forNthQuantity: "NTH_QUANTITY",
  inEveryNthQuantity: "EVERY_NTH",
  subTotal: "SUBTOTAL",
  numItemSecond: "UPPER_VALUE",
  quantitySecond: "UPPER_VALUE",
  subTotalSecond: "UPPER_VALUE",
  value: "VALUE",
};
const pathLabel = (path: string) => {
  const key = pathLabels[path.split(".").pop()!];
  return key ? t(`MARKETING.EXPRESSIONS.FIELDS.${key}`) : path;
};

// the API stores non-nullable numbers: an emptied field is 0, as the AngularJS inputs required a value
const toNumber = (v: unknown) => (v === null || v === undefined || v === "" ? 0 : Number(v));

function onCompare(value: unknown) {
  set("compareCondition", value);
  const part = props.part as Extract<FieldPart, { kind: "range" }>;
  if (value === "Between") set(part.second, get(part.path));
}

async function lookupOptions(keyword?: string) {
  const all = await lookups[(props.part as Extract<FieldPart, { kind: "lookup" }>).source]();
  const k = keyword?.toLowerCase();
  const results = k ? all.filter((x: Option) => x.name.toLowerCase().includes(k)) : all;
  return { results, totalCount: results.length };
}

// options seen in the latest search, to copy a picked entry's name/code onto the node
const seen = new Map<string, Option>();
function remember(page: Page) {
  page.results.forEach((x) => seen.set(x.id, x));
  return page;
}
const productLabel = (x: Option) => (x.code ? `${x.name} (${x.code})` : x.name);

// resolves the selected ids' labels from the names stored on the node; asks the catalog only for unnamed ids
async function withKnown(ids: string[] | undefined, keyword: string | undefined, known: Option[], search: typeof searchProducts, skip?: number) {
  if (ids?.length && !keyword) {
    const missing = ids.filter((id) => !known.some((x) => x.id === id && x.name));
    const fetched = missing.length ? (await search(undefined, 0, missing)).results : [];
    const results = ids.map((id) => known.find((x) => x.id === id && x.name) ?? fetched.find((x) => x.id === id) ?? { id, name: id });
    return remember({ results, totalCount: results.length });
  }
  return remember(await search(keyword, skip));
}

const categoryOptions = (keyword?: string, skip?: number, ids?: string[]) =>
  withKnown(ids, keyword, [{ id: get(p("categoryId")) as string, name: get(p("categoryName")) as string }], searchCategories, skip);

function onCategory(id: unknown) {
  set(p("categoryId"), id ?? null);
  set(p("categoryName"), (id && seen.get(id as string)?.name) ?? null);
}

const productOptions = (keyword?: string, skip?: number, ids?: string[]) =>
  withKnown(
    ids,
    keyword,
    [{ id: get(p("productId")) as string, name: get(p("productName")) as string, code: get(p("productCode")) as string }],
    searchProducts,
    skip,
  );

function onProduct(id: unknown) {
  const x = id ? seen.get(id as string) : undefined;
  set(p("productId"), id ?? null);
  set(p("productName"), x?.name ?? null);
  // productCode isn't part of every node's shape: only keep it current where the node has it
  if (get(p("productCode")) !== undefined) set(p("productCode"), x?.code ?? null);
}

// ConditionEntryIs keeps a legacy single productId too: show it until the selection is changed
const productIds = computed(() => {
  const ids = get("productIds") as string[] | null;
  if (ids?.length) return ids;
  const single = get("productId") as string | null;
  return single ? [single] : [];
});

// the products the node names already: productIds with their stored names and codes
function knownProducts(): Option[] {
  const names = (get("productNames") as string[] | null) ?? [];
  const codes = (get("productCodes") as string[] | null) ?? [];
  return ((get("productIds") as string[] | null) ?? []).map((id, i) => ({ id, name: names[i], code: codes[i] }));
}

const productsOptions = (keyword?: string, skip?: number, ids?: string[]) => {
  const known = knownProducts();
  if (get("productId")) known.push({ id: get("productId") as string, name: get("productName") as string, code: get("productCode") as string });
  return withKnown(ids, keyword, known, searchProducts, skip);
};

function onProducts(value: unknown) {
  const ids = (value as string[] | null) ?? [];
  const known = new Map(knownProducts().map((x) => [x.id, x]));
  const pick = (id: string) => seen.get(id) ?? known.get(id);
  set("productIds", ids.length ? ids : null);
  set("productNames", ids.length ? ids.map((id) => pick(id)?.name ?? null) : null);
  if (get("productCodes") !== undefined) set("productCodes", ids.length ? ids.map((id) => pick(id)?.code ?? null) : null);
  // the selection replaces a legacy single product
  if (get("productId")) {
    set("productId", null);
    set("productName", null);
    if (get("productCode") !== undefined) set("productCode", null);
  }
}

const idsOptions = (keyword?: string, skip?: number, ids?: string[]) =>
  withKnown(ids, keyword, [], (props.part as Extract<FieldPart, { kind: "ids" }>).of === "category" ? searchCategories : searchProducts, skip);
</script>

<style lang="scss">
.mk-field {
  flex: 0 0 auto;
  &--num {
    width: 96px;
  }
  &--text {
    width: 180px;
  }
  &--choice {
    width: 168px;
  }
  &--lookup {
    width: 220px;
  }
  &--wide {
    width: 320px;
  }
  &--ids {
    width: 240px;
  }
  &--row {
    flex-basis: 100%;
  }
}
</style>
