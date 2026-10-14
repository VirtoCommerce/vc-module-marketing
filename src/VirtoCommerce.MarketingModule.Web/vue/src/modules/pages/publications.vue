<template>
  <VcBlade
    :title="$t('MARKETING.PAGES.PUBLICATIONS.TITLE')"
    :toolbar-items="bladeToolbar"
    :loading="loading"
    width="50%"
  >
    <VcDataTable
      v-model:search-value="searchValue"
      v-model:sort-field="sortField"
      v-model:sort-order="sortOrder"
      v-model:selection="selection"
      selection-mode="multiple"
      :items="items"
      :total-count="totalCount"
      :pagination="pagination"
      :searchable="true"
      :state-key="'MARKETING_PUBLICATIONS'"
      :active-item-id="activeChildParam"
      :show-all-columns="expanded"
      :empty-state="{ icon: 'lucide-layout-template', title: $t('MARKETING.PAGES.PUBLICATIONS.EMPTY') }"
      @row-click="(e: { data: DynamicContentPublication }) => openDetails(e.data)"
      @pagination-click="pagination.goToPage"
    >
      <VcColumn
        id="name"
        :title="$t('MARKETING.PAGES.PUBLICATIONS.COLUMNS.NAME')"
        body-class="mk-list__primary"
        sortable
        always-visible
      />
      <VcColumn
        id="storeId"
        :title="$t('MARKETING.PAGES.PUBLICATIONS.COLUMNS.STORE')"
        sortable
      />
      <VcColumn
        id="isActive"
        :title="$t('MARKETING.PAGES.PROMOTIONS.COLUMNS.STATUS')"
        sortable
        always-visible
      >
        <template #body="{ data }">
          <div class="tw-inline-flex">
            <VcStatus :variant="data.isActive ? 'success' : 'info'">{{
              $t(data.isActive ? "MARKETING.STATUS.ACTIVE" : "MARKETING.STATUS.INACTIVE")
            }}</VcStatus>
          </div>
        </template>
      </VcColumn>
      <VcColumn
        id="startDate"
        :title="$t('MARKETING.PAGES.PROMOTIONS.COLUMNS.START')"
        type="date"
        sortable
      />
      <VcColumn
        id="endDate"
        :title="$t('MARKETING.PAGES.PROMOTIONS.COLUMNS.END')"
        type="date"
        sortable
      />
    </VcDataTable>
  </VcBlade>
</template>

<script setup lang="ts">
import {
  useApiClient,
  useAsync,
  useBlade,
  useDataTablePagination,
  useDataTableSort,
  useFunctions,
  useLatestRequest,
  useLoading,
  usePopup,
  useTableSearch,
  type IBladeToolbar,
} from "@vc-shell/framework";
import { VcBlade, VcColumn, VcDataTable, VcStatus } from "@vc-shell/framework/ui";
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { MarketingModuleDynamicContentClient, type DynamicContentPublication } from "../../api_client/virtocommerce.marketing";

// Dynamic content: publications (what shows in which placeholder, when); content items and placeholders open from the toolbar
defineBlade({
  url: "/dynamic-content",
  name: "MarketingPublications",
  isWorkspace: true,
  permissions: ["marketing:access"],
  menuItem: {
    title: "MARKETING.MENU.DYNAMIC_CONTENT",
    icon: "lucide-layout-template",
    priority: 11,
  },
});

const { t } = useI18n({ useScope: "global" });
const { openBlade, exposeToChildren, closeChildren, activeChildParam, expanded } = useBlade();
const { debounce } = useFunctions();
const { showConfirmation } = usePopup();
const { getApiClient } = useApiClient(MarketingModuleDynamicContentClient);

const PAGE_SIZE = 20;
const items = ref<DynamicContentPublication[]>([]);
const totalCount = ref(0);
const selection = ref<DynamicContentPublication[]>([]);

const { sortField, sortOrder, sortExpression } = useDataTableSort({ stateKey: "MARKETING_PUBLICATIONS", initialField: "name", initialDirection: "ASC" });
const { searchValue } = useTableSearch({ stateKey: "MARKETING_PUBLICATIONS" });
const pagination = useDataTablePagination({ stateKey: "MARKETING_PUBLICATIONS", pageSize: PAGE_SIZE, totalCount });

const search = useLatestRequest();
const { loading: listLoading, action: load } = useAsync(async () => {
  const client = await getApiClient();
  const result = await search.latest(
    client.dynamicContentPublicationsSearch({
      keyword: searchValue.value || undefined,
      sort: sortExpression.value,
      skip: pagination.skip,
      take: PAGE_SIZE,
    }),
  );
  if (!result) return;
  items.value = result.results ?? [];
  totalCount.value = result.totalCount ?? 0;
});
const { loading: deleteLoading, action: remove } = useAsync<string[]>(async (ids) => {
  const client = await getApiClient();
  await client.deleteDynamicContentPublications(ids);
});
const loading = useLoading(listLoading, deleteLoading);

load();
watch(searchValue, () => pagination.setPage(1));
watch([sortExpression, searchValue, () => pagination.skip], debounce(load, 300));

function openDetails(publication?: DynamicContentPublication) {
  openBlade({ name: "MarketingPublicationDetails", param: publication?.id });
}

function openContent(kind: "items" | "places") {
  openBlade({ name: "MarketingContentList", options: { kind } });
}

const bladeToolbar = ref<IBladeToolbar[]>([
  { id: "refresh", title: t("MARKETING.ACTIONS.REFRESH"), icon: "lucide-refresh-cw", clickHandler: () => load() },
  {
    id: "add",
    title: t("MARKETING.ACTIONS.ADD"),
    icon: "lucide-plus",
    permissions: ["marketing:create"],
    clickHandler: () => openDetails(),
  },
  {
    id: "delete",
    title: t("MARKETING.ACTIONS.DELETE"),
    icon: "lucide-trash-2",
    permissions: ["marketing:delete"],
    disabled: computed(() => !selection.value.length),
    async clickHandler() {
      if (!(await showConfirmation(t("MARKETING.ALERTS.DELETE_PUBLICATIONS", { count: selection.value.length })))) return;
      await closeChildren();
      await remove(selection.value.map((x) => x.id!));
      selection.value = [];
      await load();
    },
  },
  {
    id: "places",
    title: t("MARKETING.PAGES.CONTENT.PLACES_TITLE"),
    icon: "lucide-layout-template",
    separator: "left",
    clickHandler: () => openContent("places"),
  },
  {
    id: "items",
    title: t("MARKETING.PAGES.CONTENT.ITEMS_TITLE"),
    icon: "lucide-file-code-2",
    clickHandler: () => openContent("items"),
  },
]);

exposeToChildren({ reload: () => load() });
</script>
