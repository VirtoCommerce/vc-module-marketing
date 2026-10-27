<template>
  <VcBlade
    :title="item.name || $t('MARKETING.PAGES.CONTENT_ITEM.NEW_TITLE')"
    :toolbar-items="bladeToolbar"
    :loading="loading"
    width="50%"
  >
    <VcContainer>
      <VcForm class="mk-form tw-space-y-4">
        <VcInput
          v-model="item.name"
          :label="$t('MARKETING.PAGES.CONTENT.COLUMNS.NAME')"
          :placeholder="$t('MARKETING.PAGES.CONTENT_ITEM.NAME_PLACEHOLDER')"
          required
          :disabled="readonly"
        />
        <VcTextarea
          v-model="item.description"
          :label="$t('MARKETING.PAGES.CONTENT.COLUMNS.DESCRIPTION')"
          :placeholder="$t('MARKETING.PAGES.CONTENT_ITEM.DESCRIPTION_PLACEHOLDER')"
          required
          :disabled="readonly"
        />
        <VcCard :header="$t('MARKETING.PAGES.CONTENT_ITEM.PROPERTIES')">
          <!-- multilingual values are edited one language at a time (as Catalog's property values) -->
          <template
            v-if="hasMultilingual && languages.length > 1"
            #actions
          >
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
            <DynamicPropertyField
              v-for="property in sortedProperties"
              :key="property.id ?? property.name"
              :property="property"
              :language="language"
              :disabled="readonly"
            />
            <div
              v-if="!sortedProperties.length"
              class="tw-text-sm"
            >
              {{ $t("MARKETING.PAGES.CONTENT_ITEM.NO_PROPERTIES") }}
            </div>
          </div>
        </VcCard>
      </VcForm>
    </VcContainer>
  </VcBlade>
</template>

<script setup lang="ts">
import {
  DynamicPropertiesClient,
  useApiClient,
  useAsync,
  useBlade,
  useBladeForm,
  useLoading,
  usePermissions,
  usePopup,
  type IBladeToolbar,
} from "@vc-shell/framework";
import { VcBlade, VcCard, VcContainer, VcForm, VcInput, VcSelect, VcTextarea } from "@vc-shell/framework/ui";
import { computed, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";
import { MarketingModuleDynamicContentClient, type DynamicContentItem, type DynamicObjectProperty } from "../../api_client/virtocommerce.marketing";
import DynamicPropertyField from "../components/DynamicPropertyField.vue";
import { useLanguagePicker } from "../lookups";

defineBlade({
  url: "/dynamic-content-item",
  name: "MarketingContentItemDetails",
  permissions: ["marketing:access"],
});

const OBJECT_TYPE = "VirtoCommerce.MarketingModule.Core.Model.DynamicContentItem";

const { t } = useI18n({ useScope: "global" });
const { param, options, callParent, closeSelf } = useBlade<{ folderId: string }>();
const { showConfirmation } = usePopup();
const { hasAccess } = usePermissions();
const { getApiClient } = useApiClient(MarketingModuleDynamicContentClient);
const { getApiClient: getPropertiesClient } = useApiClient(DynamicPropertiesClient);

const item = ref<DynamicContentItem>({ folderId: options.value?.folderId, dynamicProperties: [] });
const id = ref(param.value);
const readonly = computed(() => !hasAccess(id.value ? "marketing:update" : "marketing:create"));
const { languages, language } = useLanguagePicker();
const hasMultilingual = computed(() => (item.value.dynamicProperties ?? []).some((p) => p.isMultilingual));

const sortedProperties = computed(() => [...(item.value.dynamicProperties ?? [])].sort((a, b) => (a.name ?? "").localeCompare(b.name ?? "")));

// every property registered for content items is editable; values of the item's other properties are kept as they are
const { loading: itemLoading, action: load } = useAsync(async () => {
  const [client, propertiesClient] = await Promise.all([getApiClient(), getPropertiesClient()]);
  const [loaded, registered] = await Promise.all([
    id.value ? client.getDynamicContentById(id.value) : Promise.resolve({ ...item.value }),
    propertiesClient.searchDynamicProperties({ objectType: OBJECT_TYPE, take: 1000 }),
  ]);
  const own = loaded.dynamicProperties ?? [];
  // the platform's property definitions, in the marketing API's shape (same JSON)
  const meta = (registered.results ?? []) as DynamicObjectProperty[];
  // the item carries only names and values: the registered property supplies its type (dictionary, array, ...)
  const merged = own.map((p) => {
    const reg = meta.find((x) => x.id === p.id);
    return reg ? { ...p, ...reg, values: p.values ?? [] } : p;
  });
  const missing = meta.filter((p) => !own.some((x) => x.id === p.id)).map((p) => ({ ...p, values: [] }));
  item.value = { ...loaded, dynamicProperties: [...merged, ...missing] };
});
const { loading: saveLoading, action: save } = useAsync(async () => {
  const client = await getApiClient();
  if (id.value) await client.updateDynamicContent(item.value);
  else id.value = (await client.createDynamicContent(item.value)).id;
});
const { loading: deleteLoading, action: remove } = useAsync(async () => {
  const client = await getApiClient();
  await client.deleteDynamicContents([id.value!]);
});
const loading = useLoading(itemLoading, saveLoading, deleteLoading);

const form = useBladeForm({
  data: item,
  canSaveOverride: computed(() => !!item.value.name?.trim() && !!item.value.description?.trim()),
  closeConfirmMessage: computed(() => t("MARKETING.ALERTS.CLOSE_CONFIRMATION")),
});

const bladeToolbar = ref<IBladeToolbar[]>([
  {
    id: "save",
    title: computed(() => t(param.value ? "MARKETING.ACTIONS.SAVE" : "MARKETING.ACTIONS.CREATE")),
    icon: "lucide-save",
    disabled: computed(() => !form.canSave.value || readonly.value),
    async clickHandler() {
      const created = !id.value;
      await save();
      form.setBaseline();
      await callParent("reload");
      if (created) await closeSelf();
    },
  },
  {
    id: "reset",
    title: t("MARKETING.ACTIONS.RESET"),
    icon: "lucide-undo-2",
    isVisible: computed(() => !!param.value),
    disabled: computed(() => !form.isModified.value),
    async clickHandler() {
      await load();
      form.setBaseline();
    },
  },
  {
    id: "delete",
    title: t("MARKETING.ACTIONS.DELETE"),
    icon: "lucide-trash-2",
    permissions: ["marketing:delete"],
    isVisible: computed(() => !!param.value),
    async clickHandler() {
      if (!(await showConfirmation(t("MARKETING.ALERTS.DELETE_NAMED", { name: item.value.name })))) return;
      await remove();
      form.setBaseline();
      await callParent("reload");
      await closeSelf();
    },
  },
]);

onMounted(async () => {
  await load();
  form.setBaseline();
});
</script>
