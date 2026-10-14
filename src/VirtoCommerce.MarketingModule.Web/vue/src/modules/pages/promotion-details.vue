<template>
  <VcBlade
    :title="title"
    :toolbar-items="bladeToolbar"
    :loading="loading"
    width="70%"
  >
    <VcContainer>
      <VcForm class="mk-form tw-space-y-4">
        <VcRow class="tw-gap-4">
          <VcCol>
            <VcInput
              v-model="item.name"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.NAME')"
              :placeholder="$t('MARKETING.PAGES.PROMOTION.FIELDS.NAME_PLACEHOLDER')"
              :hint="$t('MARKETING.PAGES.PROMOTION.FIELDS.NAME_HINT')"
              maxlength="128"
              required
              :error-message="nameError"
              :disabled="readonly"
            />
          </VcCol>
          <VcCol class="mk-form__switches">
            <VcSwitch
              v-model="item.isActive"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.ACTIVE')"
              :disabled="readonly"
            />
            <VcSwitch
              v-model="item.isPublic"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.PUBLIC')"
              :disabled="readonly"
            />
          </VcCol>
        </VcRow>
        <VcRow class="tw-gap-4">
          <VcCol>
            <VcDatePicker
              v-model="item.startDate"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.START')"
              :hint="$t('MARKETING.PAGES.PROMOTION.FIELDS.START_HINT')"
              clearable
              :disabled="readonly"
            />
          </VcCol>
          <VcCol>
            <VcDatePicker
              v-model="item.endDate"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.END')"
              :hint="$t('MARKETING.PAGES.PROMOTION.FIELDS.END_HINT')"
              clearable
              :disabled="readonly"
            />
          </VcCol>
        </VcRow>
        <VcRow class="tw-gap-4">
          <VcCol>
            <VcSelect
              v-model="storeIds"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.STORES')"
              :placeholder="$t('MARKETING.PAGES.PROMOTION.FIELDS.STORES_PLACEHOLDER')"
              :hint="$t('MARKETING.PAGES.PROMOTION.FIELDS.STORES_HINT')"
              :options="storeOptions"
              option-value="id"
              option-label="name"
              multiple
              searchable
              :disabled="readonly"
            />
          </VcCol>
          <VcCol>
            <VcSelect
              :model-value="String(!!item.isExclusive)"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.EXCLUSIVITY')"
              :hint="$t('MARKETING.PAGES.PROMOTION.FIELDS.EXCLUSIVITY_HINT')"
              :options="exclusivity"
              option-value="value"
              option-label="label"
              :clearable="false"
              :disabled="readonly"
              @update:model-value="(v) => (item.isExclusive = v === 'true')"
            />
          </VcCol>
        </VcRow>
        <VcRow class="tw-gap-4">
          <VcCol>
            <VcInput
              v-if="showPriority"
              v-model="item.priority"
              type="integer"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.PRIORITY')"
              :hint="$t('MARKETING.PAGES.PROMOTION.FIELDS.PRIORITY_HINT')"
              :disabled="readonly || item.isExclusive"
            />
            <VcSwitch
              v-else
              v-model="item.isAllowCombiningWithSelf"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.COMBINE_WITH_SELF')"
              :hint="$t('MARKETING.PAGES.PROMOTION.FIELDS.COMBINE_WITH_SELF_HINT')"
              :disabled="readonly"
            />
          </VcCol>
          <VcCol>
            <VcSwitch
              v-if="showPriority"
              v-model="item.isAllowCombiningWithSelf"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.COMBINE_WITH_SELF')"
              :hint="$t('MARKETING.PAGES.PROMOTION.FIELDS.COMBINE_WITH_SELF_HINT')"
              :disabled="readonly"
            />
          </VcCol>
        </VcRow>
        <VcTextarea
          v-model="item.description"
          :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.DESCRIPTION')"
          :placeholder="$t('MARKETING.PAGES.PROMOTION.FIELDS.DESCRIPTION_PLACEHOLDER')"
          :disabled="readonly"
        />

        <VcCard
          v-if="item.type === 'DynamicPromotion' && item.dynamicExpression"
          :header="$t('MARKETING.PAGES.PROMOTION.CONDITIONS')"
          class="mk-conditions"
        >
          <ExpressionTree
            :tree="item.dynamicExpression"
            :disabled="readonly"
          />
        </VcCard>

        <VcCard :header="$t('MARKETING.PAGES.PROMOTION.LOCALIZATION')">
          <template #actions>
            <VcSelect
              v-model="language"
              class="mk-form__lang"
              size="small"
              :options="languages"
              option-value="id"
              option-label="name"
              :clearable="false"
              :aria-label="$t('MARKETING.PAGES.PROMOTION.FIELDS.LANGUAGE')"
            />
          </template>
          <div class="tw-p-4 tw-space-y-4">
            <VcInput
              :model-value="localized('localizedLabel')"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.LABEL')"
              :placeholder="$t('MARKETING.PAGES.PROMOTION.FIELDS.LABEL_PLACEHOLDER')"
              multilanguage
              :current-language="language"
              maxlength="1024"
              :disabled="readonly"
              @update:model-value="(v) => setLocalized('localizedLabel', v)"
            />
            <VcInput
              :model-value="localized('localizedDisplayName')"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.DISPLAY_NAME')"
              :placeholder="$t('MARKETING.PAGES.PROMOTION.FIELDS.DISPLAY_NAME_PLACEHOLDER')"
              multilanguage
              :current-language="language"
              maxlength="1024"
              :disabled="readonly"
              @update:model-value="(v) => setLocalized('localizedDisplayName', v)"
            />
            <VcTextarea
              :model-value="localized('localizedDescription')"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.LOCALIZED_DESCRIPTION')"
              :placeholder="$t('MARKETING.PAGES.PROMOTION.FIELDS.LOCALIZED_DESCRIPTION_PLACEHOLDER')"
              multilanguage
              :current-language="language"
              :disabled="readonly"
              @update:model-value="(v) => setLocalized('localizedDescription', v)"
            />
          </div>
        </VcCard>
      </VcForm>
    </VcContainer>
  </VcBlade>
</template>

<script setup lang="ts">
import {
  defineBladeContext,
  SettingClient,
  useApiClient,
  useAsync,
  useBlade,
  useBladeForm,
  useBladeWidgets,
  useLoading,
  usePermissions,
  type IBladeToolbar,
} from "@vc-shell/framework";
import {
  VcBlade,
  VcCard,
  VcCol,
  VcContainer,
  VcDatePicker,
  VcForm,
  VcInput,
  VcRow,
  VcSelect,
  VcSwitch,
  VcTextarea,
} from "@vc-shell/framework/ui";
import { computed, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";
import { MarketingModulePromotionClient, type Promotion } from "../../api_client/virtocommerce.marketing";
import ExpressionTree from "../expressions/ExpressionTree.vue";
import { treeErrors } from "../expressions/defs";
import { searchOrdersByPromotion, searchStores, useLanguagePicker } from "../lookups";

defineBlade({
  url: "/promotion",
  name: "MarketingPromotionDetails",
  permissions: ["marketing:access"],
});

type LocalizedField = "localizedLabel" | "localizedDisplayName" | "localizedDescription";

const { t } = useI18n({ useScope: "global" });
const { param, options, openBlade, callParent, exposeToChildren } = useBlade<{ clone?: Promotion }>();
const { hasAccess } = usePermissions();
const { getApiClient } = useApiClient(MarketingModulePromotionClient);
const { getApiClient: getSettingsClient } = useApiClient(SettingClient);

const item = ref<Promotion>({});
// the id of the promotion shown (set after creating a new one)
const id = ref<string | undefined>(param.value);
const isNew = computed(() => !id.value);
const readonly = computed(() => !hasAccess(isNew.value ? "marketing:create" : "marketing:update"));

const couponCount = ref(0);
const ordersCount = ref<number>();

const { loading: itemLoading, action: load } = useAsync(async () => {
  const client = await getApiClient();
  if (id.value) {
    item.value = await client.getPromotionById(id.value);
  } else if (options.value?.clone) {
    // the AngularJS clone: same promotion without identity and name
    const copy = JSON.parse(JSON.stringify(options.value.clone)) as Promotion;
    item.value = { ...copy, id: undefined, name: undefined, createdDate: undefined, createdBy: undefined, modifiedDate: undefined, modifiedBy: undefined };
    if (!item.value.dynamicExpression && copy.id) item.value.dynamicExpression = (await client.getPromotionById(copy.id)).dynamicExpression;
  } else {
    item.value = await client.getNewDynamicPromotion();
  }
});

const { loading: saveLoading, action: save } = useAsync(async () => {
  const client = await getApiClient();
  if (id.value) await client.updatePromotions(item.value);
  else id.value = (await client.createPromotion(item.value)).id;
  item.value = await client.getPromotionById(id.value!);
});

const loading = useLoading(itemLoading, saveLoading);

const nameError = computed(() => (form.isReady.value && !item.value.name?.trim() ? t("MARKETING.VALIDATION.REQUIRED") : undefined));
const errors = computed(() => treeErrors(item.value.dynamicExpression as never));

const form = useBladeForm({
  data: item,
  canSaveOverride: computed(() => !!item.value.name?.trim() && !errors.value.length),
  closeConfirmMessage: computed(() => t("MARKETING.ALERTS.CLOSE_CONFIRMATION")),
});

const title = computed(() => item.value.name || (isNew.value ? t("MARKETING.PAGES.PROMOTION.NEW_TITLE") : t("MARKETING.PAGES.PROMOTION.TITLE")));

// combine policy "CombineStackable" uses priorities (the AngularJS blade, which read the setting itself, shows the field only then)
const showPriority = ref(false);
getSettingsClient()
  .then((client) => client.getGlobalSetting("Marketing.Promotion.CombinePolicy"))
  .then((setting) => (showPriority.value = setting?.value === "CombineStackable"))
  .catch(() => undefined);

const exclusivity = computed(() => [
  { value: "false", label: t("MARKETING.PAGES.PROMOTION.EXCLUSIVITY.COMBINED") },
  { value: "true", label: t("MARKETING.PAGES.PROMOTION.EXCLUSIVITY.EXCLUSIVE") },
]);

// unknown store ids (deleted stores) stay selected and are labelled, as the AngularJS "UNKNOWN" marker did
const storeIds = computed({
  get: () => item.value.storeIds ?? [],
  set: (v: string[]) => (item.value.storeIds = v),
});
async function storeOptions(keyword?: string, skip?: number, ids?: string[]) {
  const page = await searchStores(keyword, skip, ids);
  if (ids?.length && !keyword) {
    const missing = ids.filter((x) => !page.results.some((s) => s.id === x)).map((x) => ({ id: x, name: `${x} (${t("MARKETING.UNKNOWN")})` }));
    return { results: [...page.results, ...missing], totalCount: page.totalCount + missing.length };
  }
  return page;
}

// localized label / display name / description, edited one language at a time
const { languages, language } = useLanguagePicker();
const localized = (field: LocalizedField) => (item.value[field]?.values ?? {})[language.value] ?? "";
function setLocalized(field: LocalizedField, value: unknown) {
  item.value[field] ??= { values: {} };
  const target = item.value[field] as { values?: Record<string, string> };
  target.values ??= {};
  target.values[language.value] = String(value ?? "");
}

async function refreshCounts() {
  if (!id.value) return;
  const client = await getApiClient();
  [couponCount.value, ordersCount.value] = await Promise.all([
    client.searchCoupons({ promotionId: id.value, skip: 0, take: 0 }).then((r) => r.totalCount ?? 0),
    // Orders is an optional module: no count, no widget
    searchOrdersByPromotion(id.value, 0, 0)
      .then((r) => r.totalCount ?? 0)
      .catch(() => undefined),
  ]);
}

const bladeToolbar = ref<IBladeToolbar[]>([
  {
    id: "save",
    title: computed(() => t(isNew.value ? "MARKETING.ACTIONS.CREATE" : "MARKETING.ACTIONS.SAVE")),
    icon: "lucide-save",
    disabled: computed(() => !form.canSave.value || readonly.value),
    async clickHandler() {
      const created = isNew.value;
      await save();
      form.setBaseline();
      // coupons and orders change in other blades: only a new promotion needs its counts (and widgets)
      if (created) await refreshCounts();
      callParent("reload");
    },
  },
  {
    id: "reset",
    title: t("MARKETING.ACTIONS.RESET"),
    icon: "lucide-undo-2",
    disabled: computed(() => !form.isModified.value),
    async clickHandler() {
      await load();
      form.setBaseline();
    },
  },
  {
    id: "clone",
    title: t("MARKETING.ACTIONS.CLONE"),
    icon: "lucide-copy-plus",
    permissions: ["marketing:create"],
    isVisible: computed(() => !isNew.value),
    disabled: computed(() => form.isModified.value),
    // the list opens the copy in this blade's place, as the AngularJS clone did
    clickHandler: () => callParent("clone", item.value),
  },
]);

useBladeWidgets([
  {
    id: "MarketingCoupons",
    icon: "lucide-ticket",
    title: "MARKETING.WIDGETS.COUPONS",
    badge: couponCount,
    disabled: computed(() => isNew.value),
    onClick: () => openBlade({ name: "MarketingCoupons", param: id.value, options: { promotion: item.value } }),
  },
  {
    id: "MarketingPromotionUsage",
    icon: "lucide-receipt-text",
    title: "MARKETING.WIDGETS.USAGE",
    badge: computed(() => ordersCount.value ?? 0),
    isVisible: computed(() => !isNew.value && ordersCount.value !== undefined),
    onClick: () => openBlade({ name: "MarketingPromotionOrders", param: id.value, options: { promotion: item.value } }),
  },
]);

// the promotion, for widgets other modules add to this blade (registerExternalWidget with targetBlades: ["MarketingPromotionDetails"])
defineBladeContext({ item });

onMounted(() =>
  Promise.all([
    load().then(() => (options.value?.clone ? form.markReady() : form.setBaseline())),
    refreshCounts(),
  ]),
);

exposeToChildren({ refreshCounts });
</script>
