<template>
  <VcBlade
    :title="isNew ? $t('MARKETING.PAGES.FILTER.NEW_TITLE') : filter.name"
    :toolbar-items="bladeToolbar"
    width="40%"
  >
    <VcContainer>
      <VcForm class="mk-form tw-space-y-4">
        <VcInput
          v-model="filter.name"
          :label="$t('MARKETING.PAGES.FILTER.NAME')"
          :error-message="filter.name?.trim() ? undefined : $t('MARKETING.VALIDATION.REQUIRED')"
          required
        />
        <VcSelect
          v-model="filter.status"
          :label="$t('MARKETING.PAGES.PROMOTIONS.FILTERS.STATUS')"
          :placeholder="$t('MARKETING.PAGES.FILTER.ALL')"
          :options="statuses"
          option-value="value"
          option-label="label"
        />
        <VcSelect
          v-model="filter.storeIds"
          :label="$t('MARKETING.PAGES.PROMOTIONS.FILTERS.STORE')"
          :placeholder="$t('MARKETING.PAGES.FILTER.ALL')"
          :options="searchStores"
          option-value="id"
          option-label="name"
          multiple
          searchable
        />
      </VcForm>
    </VcContainer>
  </VcBlade>
</template>

<script setup lang="ts">
// filter-detail: a named, saved promotion filter (stored in this browser, like the AngularJS $localStorage filters)
import { useBlade, useBladeForm, type IBladeToolbar } from "@vc-shell/framework";
import { VcBlade, VcContainer, VcForm, VcInput, VcSelect } from "@vc-shell/framework/ui";
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";
import { searchStores } from "../lookups";
import { PROMOTION_STATUSES, useSavedFilters, type SavedPromotionFilter } from "../savedFilters";

defineBlade({
  url: "/promotion-filter",
  name: "MarketingPromotionFilter",
  permissions: ["marketing:access"],
});

const { t } = useI18n({ useScope: "global" });
const { options, callParent, closeSelf } = useBlade<{ filterId?: string }>();
const { filters, currentId } = useSavedFilters();

const statuses = computed(() =>
  PROMOTION_STATUSES.map((s) => ({
    value: s,
    label: t(`MARKETING.PROMOTION_STATUS.${s.toUpperCase()}`),
  })),
);

const stored = (id?: string) => filters.value.find((f) => f.id === id);
const copy = (f: SavedPromotionFilter) => JSON.parse(JSON.stringify(f)) as SavedPromotionFilter;
const initial = stored(options.value?.filterId);
const filter = ref<SavedPromotionFilter>(initial ? copy(initial) : { id: String(Date.now()), name: t("MARKETING.PAGES.FILTER.UNNAMED") });
const isNew = computed(() => !stored(filter.value.id));

const form = useBladeForm({ data: filter });
form.setBaseline();

const bladeToolbar = ref<IBladeToolbar[]>([
  {
    id: "apply",
    title: t("MARKETING.PAGES.FILTER.APPLY"),
    icon: "lucide-filter",
    disabled: computed(() => !filter.value.name?.trim()),
    async clickHandler() {
      const value = copy({ ...filter.value, status: filter.value.status || undefined, storeIds: filter.value.storeIds?.length ? filter.value.storeIds : undefined });
      const index = filters.value.findIndex((f) => f.id === value.id);
      if (index >= 0) filters.value.splice(index, 1, value);
      else filters.value.unshift(value);
      currentId.value = value.id;
      form.setBaseline();
      await callParent("reloadFirstPage");
    },
  },
  {
    id: "reset",
    title: t("MARKETING.ACTIONS.RESET"),
    icon: "lucide-undo-2",
    disabled: computed(() => !form.isModified.value),
    clickHandler() {
      const saved = stored(filter.value.id);
      if (saved) filter.value = copy(saved);
      form.setBaseline();
    },
  },
  {
    id: "delete",
    title: t("MARKETING.ACTIONS.DELETE"),
    icon: "lucide-trash-2",
    isVisible: computed(() => !isNew.value),
    async clickHandler() {
      filters.value = filters.value.filter((f) => f.id !== filter.value.id);
      if (currentId.value === filter.value.id) currentId.value = undefined;
      form.setBaseline();
      await callParent("reloadFirstPage");
      await closeSelf();
    },
  },
]);
</script>
