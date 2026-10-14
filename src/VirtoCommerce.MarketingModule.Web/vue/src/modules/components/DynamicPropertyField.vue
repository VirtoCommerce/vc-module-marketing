<template>
  <!-- the framework control (as Catalog's PropertyValues); values stay in the platform shape, in place on property.values -->
  <VcDynamicProperty
    :property="base"
    :model-value="modelValue"
    :value-type="base.valueType"
    :name="property.name ?? property.id ?? ''"
    :display-names="property.displayNames?.map((d) => ({ name: d.name, languageCode: d.locale }))"
    :required="!!property.isRequired"
    :dictionary="base.dictionary"
    :multivalue="base.multivalue"
    :multilanguage="base.multilanguage"
    :current-language="language"
    :options-getter="dictionaryItems"
    :disabled="disabled"
    :class="{ 'mk-code': type === 'Html' }"
    @update:model-value="onChange"
  />
</template>

<script setup lang="ts">
import { computed } from "vue";
import { DynamicPropertiesClient, useApiClient } from "@vc-shell/framework";
import { VcDynamicProperty } from "@vc-shell/framework/ui";
import type { DynamicObjectProperty, DynamicPropertyObjectValue } from "../../api_client/virtocommerce.marketing";

// one platform dynamic property (DynamicObjectProperty: values carry `locale`, flags are isArray/isDictionary/isMultilingual)
// rendered by VcDynamicProperty, whose control contract uses the catalog shape (dictionary/multivalue/multilanguage)
const props = defineProps<{ property: DynamicObjectProperty; language: string; disabled?: boolean }>();

type Value = DynamicPropertyObjectValue;
type DictionaryItem = { id: string; alias: string; value: string };
const { getApiClient } = useApiClient(DynamicPropertiesClient);

// properties of an item that are no longer registered come back with valueType "Undefined": use their values' type
const type = computed(() =>
  String(props.property.valueType && props.property.valueType !== "Undefined" ? props.property.valueType : (props.property.values?.[0]?.valueType ?? "ShortText")),
);
// the platform types the control doesn't know map onto the ones it does
const controlType = computed(() => ({ Decimal: "Number", Html: "LongText", Image: "ShortText" })[type.value] ?? type.value);
const base = computed(() => ({
  id: props.property.id ?? props.property.name,
  name: props.property.name,
  valueType: controlType.value,
  dictionary: !!props.property.isDictionary,
  multivalue: !!props.property.isArray,
  multilanguage: !!props.property.isMultilingual && !props.property.isDictionary,
  values: props.property.values,
}));

const values = () => props.property.values ?? [];
const locale = () => (base.value.multilanguage ? props.language : undefined);
const sameLocale = (v: Value) => (v.locale ?? undefined) === locale();

// dictionary values are stored by item id (valueId); a value whose item is gone still shows by its value
let dictionary: Promise<DictionaryItem[]> | undefined;
async function dictionaryItems(_propertyId?: string, keyword?: string) {
  dictionary ??= getApiClient()
    .then((client) => client.getAllDictionaryItems(props.property.id ?? ""))
    .then((items) => (items ?? []).map((x) => ({ id: x.id!, alias: x.name ?? x.id!, value: x.name ?? x.id! })))
    .catch(() => []);
  const items = [...(await dictionary)];
  for (const v of values()) if (v.valueId && !items.some((x) => x.id === v.valueId)) items.push({ id: v.valueId, alias: String(v.value ?? v.valueId), value: String(v.value ?? v.valueId) });
  const k = keyword?.toLowerCase();
  return k ? items.filter((x) => x.alias.toLowerCase().includes(k)) : items;
}

const modelValue = computed(() => {
  const own = values().filter(sameLocale);
  if (base.value.dictionary) {
    const ids = own.map((v) => v.valueId).filter(Boolean) as string[];
    return base.value.multivalue ? own.map((v) => ({ id: v.valueId, alias: v.value, value: v.value })) : (ids[0] ?? null);
  }
  // free multivalue: VcMultivalue labels text chips by `title`, number chips by `value`
  if (base.value.multivalue) return own.map((v) => ({ ...v, title: v.value }));
  return own[0]?.value ?? (controlType.value === "Boolean" ? false : null);
});

function onChange(ev: unknown) {
  const next = (ev && typeof ev === "object" && "value" in ev ? (ev as { value: unknown }).value : ev) as unknown;
  const others = values().filter((v) => !sameLocale(v));
  const valueType = type.value as Value["valueType"];
  let own: Value[];
  if (base.value.dictionary) {
    // the control sends the loaded dictionary along: new picks take their label from it
    const dict = ((ev as { dictionary?: DictionaryItem[] })?.dictionary ?? []) as DictionaryItem[];
    const picked = ([] as unknown[]).concat(next ?? []).map((x) => (typeof x === "object" && x ? (x as DictionaryItem).id : (x as string)));
    own = picked
      .filter(Boolean)
      .map((id) => values().find((v) => v.valueId === id) ?? { valueId: id, value: dict.find((x) => x.id === id)?.alias ?? id, valueType });
  } else if (base.value.multivalue) {
    own = ((next as (Value & { title?: unknown })[]) ?? []).map(({ title, ...v }) => ({ ...v, value: v.value ?? title, valueType: v.valueType ?? valueType }));
  } else {
    const empty = next === null || next === undefined || next === "";
    const existing = values().find(sameLocale);
    own = empty ? [] : [existing ? { ...existing, value: next } : { value: next, locale: locale(), valueType }];
  }
  props.property.values = [...others, ...own];
}
</script>

<style lang="scss">
.mk-code textarea {
  font-family: ui-monospace, SFMono-Regular, Menlo, Consolas, monospace;
  font-size: 12px;
}
</style>
