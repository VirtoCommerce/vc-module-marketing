<template>
  <VcBlade
    :title="item.name || (id ? $t('MARKETING.PAGES.PUBLICATION.TITLE') : $t('MARKETING.PAGES.PUBLICATION.NEW_TITLE'))"
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
              :label="$t('MARKETING.PAGES.PUBLICATIONS.COLUMNS.NAME')"
              :placeholder="$t('MARKETING.PAGES.PUBLICATION.NAME_PLACEHOLDER')"
              required
              :disabled="readonly"
            />
          </VcCol>
          <VcCol class="mk-form__switches">
            <VcSwitch
              v-model="item.isActive"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.ACTIVE')"
              :disabled="readonly"
            />
          </VcCol>
        </VcRow>
        <VcTextarea
          v-model="item.description"
          :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.DESCRIPTION')"
          :placeholder="$t('MARKETING.PAGES.PUBLICATION.DESCRIPTION_PLACEHOLDER')"
          :disabled="readonly"
        />
        <VcRow class="tw-gap-4">
          <VcCol>
            <VcSelect
              v-model="item.storeId"
              :label="$t('MARKETING.PAGES.PUBLICATIONS.COLUMNS.STORE')"
              :placeholder="$t('MARKETING.PAGES.PUBLICATION.STORE_PLACEHOLDER')"
              :options="searchStores"
              option-value="id"
              option-label="name"
              searchable
              required
              :disabled="readonly"
            />
          </VcCol>
          <VcCol>
            <VcDatePicker
              v-model="item.startDate"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.START')"
              clearable
              :disabled="readonly"
            />
          </VcCol>
          <VcCol>
            <VcDatePicker
              v-model="item.endDate"
              :label="$t('MARKETING.PAGES.PROMOTION.FIELDS.END')"
              clearable
              :disabled="readonly"
            />
          </VcCol>
        </VcRow>
        <VcSelect
          v-model="item.contentPlaces"
          :label="$t('MARKETING.PAGES.CONTENT.PLACES_TITLE')"
          :placeholder="$t('MARKETING.PAGES.PUBLICATION.PLACES_PLACEHOLDER')"
          :options="placeOptions"
          option-value="id"
          :option-label="entryLabel"
          :emit-value="false"
          multiple
          searchable
          required
          :disabled="readonly"
        >
          <template #append>
            <VcButton
              v-if="!readonly"
              class="mk-browse"
              variant="outline"
              icon="lucide-folder-open"
              data-test-id="browse-places"
              @click="browse('places')"
              >{{ $t("MARKETING.PAGES.PUBLICATION.BROWSE") }}</VcButton
            >
          </template>
        </VcSelect>
        <VcSelect
          v-model="item.contentItems"
          :label="$t('MARKETING.PAGES.CONTENT.ITEMS_TITLE')"
          :placeholder="$t('MARKETING.PAGES.PUBLICATION.ITEMS_PLACEHOLDER')"
          :options="itemOptions"
          option-value="id"
          :option-label="entryLabel"
          :emit-value="false"
          multiple
          searchable
          required
          :disabled="readonly"
        >
          <template #append>
            <VcButton
              v-if="!readonly"
              class="mk-browse"
              variant="outline"
              icon="lucide-folder-open"
              data-test-id="browse-items"
              @click="browse('items')"
              >{{ $t("MARKETING.PAGES.PUBLICATION.BROWSE") }}</VcButton
            >
          </template>
        </VcSelect>

        <VcCard
          v-if="item.dynamicExpression"
          :header="$t('MARKETING.PAGES.PUBLICATION.CONDITIONS')"
          class="mk-conditions"
        >
          <ExpressionTree
            :tree="item.dynamicExpression"
            :disabled="readonly"
          />
        </VcCard>
      </VcForm>
    </VcContainer>
  </VcBlade>
</template>

<script setup lang="ts">
import { useApiClient, useAsync, useBlade, useBladeForm, useLoading, usePermissions, usePopup, type IBladeToolbar } from "@vc-shell/framework";
import { VcBlade, VcButton, VcCard, VcCol, VcContainer, VcDatePicker, VcForm, VcInput, VcRow, VcSelect, VcSwitch, VcTextarea } from "@vc-shell/framework/ui";
import { computed, onMounted, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { MarketingModuleDynamicContentClient, type DynamicContentPublication } from "../../api_client/virtocommerce.marketing";
import ExpressionTree from "../expressions/ExpressionTree.vue";
import { searchStores } from "../lookups";

defineBlade({
  url: "/dynamic-content-publication",
  name: "MarketingPublicationDetails",
  permissions: ["marketing:access"],
});

const { t } = useI18n({ useScope: "global" });
const { param, callParent, closeSelf, openBlade, exposeToChildren } = useBlade();
const { showConfirmation } = usePopup();
const { hasAccess } = usePermissions();
const { getApiClient } = useApiClient(MarketingModuleDynamicContentClient);

const item = ref<DynamicContentPublication>({});
const id = ref(param.value);
const readonly = computed(() => !hasAccess(id.value ? "marketing:update" : "marketing:create"));

const { loading: itemLoading, action: load } = useAsync(async () => {
  const client = await getApiClient();
  item.value = id.value ? await client.getDynamicContentPublicationById(id.value) : await client.getNewDynamicPublication();
});
const { loading: saveLoading, action: save } = useAsync(async () => {
  const client = await getApiClient();
  if (id.value) await client.updateDynamicContentPublication(item.value);
  else id.value = (await client.createDynamicContentPublication(item.value)).id;
  item.value = await client.getDynamicContentPublicationById(id.value!);
});
const { loading: deleteLoading, action: remove } = useAsync(async () => {
  const client = await getApiClient();
  await client.deleteDynamicContentPublications([id.value!]);
});
const loading = useLoading(itemLoading, saveLoading, deleteLoading);

type Entry = { id?: string; name?: string; path?: string };
const entryLabel = (x: Entry) => x.name ?? x.id ?? "";

async function placeOptions(keyword?: string, skip?: number) {
  const client = await getApiClient();
  const r = await client.dynamicContentPlacesSearch({ keyword: keyword || undefined, skip, take: 20 });
  return { results: r.results ?? [], totalCount: r.totalCount ?? 0 };
}
async function itemOptions(keyword?: string, skip?: number) {
  const client = await getApiClient();
  const r = await client.dynamicContentItemsSearch({ keyword: keyword || undefined, skip, take: 20 });
  return { results: r.results ?? [], totalCount: r.totalCount ?? 0 };
}

// an unnamed publication of one placeholder and one item is named "<placeholder>_<item>" (as in the AngularJS blade)
watch(
  () => [item.value.contentPlaces?.length, item.value.contentItems?.length],
  () => {
    if (item.value.name?.trim() || readonly.value) return;
    const name = [item.value.contentPlaces, item.value.contentItems]
      .map((list) => (list?.length === 1 ? list[0].name : ""))
      .filter(Boolean)
      .join("_");
    if (name) item.value.name = name;
  },
);

// "Add placeholders" / "Add content items" steps: the folder browser in pick mode adds what is clicked here
type Kind = "places" | "items";
const listOf = (kind: Kind) => (kind === "places" ? (item.value.contentPlaces ??= []) : (item.value.contentItems ??= []));
const browse = (kind: Kind) => openBlade({ name: "MarketingContentList", options: { kind, pick: true } });
exposeToChildren({
  chosen: (kind: Kind) => listOf(kind).map((x) => x.id),
  pick({ kind, entry }: { kind: Kind; entry: Entry }) {
    const list = listOf(kind) as Entry[];
    if (!list.some((x) => x.id === entry.id)) list.push({ ...entry });
  },
});

const form = useBladeForm({
  data: item,
  canSaveOverride: computed(
    () => !!item.value.name?.trim() && !!item.value.storeId && !!item.value.contentItems?.length && !!item.value.contentPlaces?.length,
  ),
  closeConfirmMessage: computed(() => t("MARKETING.ALERTS.CLOSE_CONFIRMATION")),
});

const bladeToolbar = ref<IBladeToolbar[]>([
  {
    id: "save",
    title: computed(() => t(id.value ? "MARKETING.ACTIONS.SAVE" : "MARKETING.ACTIONS.CREATE")),
    icon: "lucide-save",
    disabled: computed(() => !form.canSave.value || readonly.value),
    async clickHandler() {
      await save();
      form.setBaseline();
      await callParent("reload");
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
    id: "delete",
    title: t("MARKETING.ACTIONS.DELETE"),
    icon: "lucide-trash-2",
    permissions: ["marketing:delete"],
    isVisible: computed(() => !!id.value),
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

<style lang="scss">
.mk-browse {
  margin-left: 8px;
  white-space: nowrap;
}
</style>
