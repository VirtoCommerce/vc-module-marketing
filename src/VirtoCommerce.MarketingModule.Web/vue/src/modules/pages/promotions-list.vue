<template>
  <VcBlade
    :title="$t('MARKETING.PAGES.PROMOTIONS.TITLE')"
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
      search-bar
      :state-key="'MARKETING_PROMOTIONS'"
      :active-item-id="activeChildParam"
      :show-all-columns="expanded"
      :global-filters="globalFilters"
      :row-actions="rowActions"
      row-actions-mode="dropdown"
      :empty-state="{ icon: 'lucide-flag', title: $t('MARKETING.PAGES.PROMOTIONS.EMPTY') }"
      @row-click="(e: { data: Promotion }) => openDetails(e.data)"
      @pagination-click="pagination.goToPage"
      @filter="onFilter"
    >
      <VcColumn
        id="name"
        :title="$t('MARKETING.PAGES.PROMOTIONS.COLUMNS.NAME')"
        body-class="mk-list__primary"
        sortable
        always-visible
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
        id="hasCoupons"
        :title="$t('MARKETING.PAGES.PROMOTIONS.COLUMNS.COUPONS')"
        type="status-icon"
      />
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
      <VcColumn
        id="description"
        :title="$t('MARKETING.PAGES.PROMOTIONS.COLUMNS.DESCRIPTION')"
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
  usePermissions,
  usePopup,
  useTableSearch,
  type IBladeToolbar,
} from "@vc-shell/framework";
import { VcBlade, VcColumn, VcDataTable, VcStatus } from "@vc-shell/framework/ui";
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { MarketingModulePromotionClient, PromotionStatus, type Promotion } from "../../api_client/virtocommerce.marketing";
import { searchStores } from "../lookups";

defineBlade({
  url: "/marketing",
  name: "MarketingPromotions",
  isWorkspace: true,
  permissions: ["marketing:access"],
  menuItem: {
    title: "MARKETING.MENU.PROMOTIONS",
    icon: "lucide-flag",
    priority: 10,
  },
});

const { t } = useI18n({ useScope: "global" });
const { openBlade, exposeToChildren, closeChildren, activeChildParam, expanded } = useBlade();
const { debounce } = useFunctions();
const { showConfirmation } = usePopup();
const { hasAccess } = usePermissions();
const { getApiClient } = useApiClient(MarketingModulePromotionClient);

const PAGE_SIZE = 20;
const items = ref<Promotion[]>([]);
const totalCount = ref(0);
const selection = ref<Promotion[]>([]);

const { sortField, sortOrder, sortExpression } = useDataTableSort({ stateKey: "MARKETING_PROMOTIONS", initialField: "name", initialDirection: "ASC" });
const { searchValue } = useTableSearch({ stateKey: "MARKETING_PROMOTIONS" });
const pagination = useDataTablePagination({ stateKey: "MARKETING_PROMOTIONS", pageSize: PAGE_SIZE, totalCount });

// The AngularJS predefined filters and saved-filter blade: a promotion's status (the API takes one) and its
// stores (the API takes a list). The search bar keeps both in the blade URL and saves named views itself.
const PROMOTION_STATUSES = [PromotionStatus.Active, PromotionStatus.Upcoming, PromotionStatus.Archived, PromotionStatus.Deactivated];
const filters = ref<{ status?: PromotionStatus; storeIds?: string[] }>({});
const stores = ref<{ id: string; name: string }[]>([]);
searchStores(undefined, 0).then((r) => (stores.value = r.results));

const globalFilters = computed(() => [
  {
    id: "status",
    label: t("MARKETING.PAGES.PROMOTIONS.FILTERS.STATUS"),
    filter: {
      options: PROMOTION_STATUSES.map((s) => ({
        value: s,
        label: t(`MARKETING.PROMOTION_STATUS.${s.toUpperCase()}`),
      })),
    },
  },
  {
    id: "storeIds",
    label: t("MARKETING.PAGES.PROMOTIONS.FILTERS.STORE"),
    filter: { options: stores.value.map((s) => ({ value: s.id, label: s.name })), multiple: true },
  },
]);

function onFilter(event: { filters: Record<string, unknown> }) {
  const storeIds = event.filters.storeIds as string[] | string | undefined;
  filters.value = {
    status: (event.filters.status as PromotionStatus) || undefined,
    storeIds: storeIds ? ([] as string[]).concat(storeIds) : undefined,
  };
  reloadFirstPage();
}

function reloadFirstPage() {
  if (pagination.skip) pagination.setPage(1);
  else load();
}

const search = useLatestRequest();
const { loading: listLoading, action: load } = useAsync(async () => {
  const client = await getApiClient();
  const { status, storeIds } = filters.value;
  const result = await search.latest(
    client.promotionsSearch({
      keyword: searchValue.value || undefined,
      sort: sortExpression.value,
      skip: pagination.skip,
      take: PAGE_SIZE,
      responseGroup: "none",
      status,
      storeIds: storeIds?.length ? storeIds : undefined,
    }),
  );
  if (!result) return;
  items.value = result.results ?? [];
  totalCount.value = result.totalCount ?? 0;
});

const { loading: deleteLoading, action: remove } = useAsync<string[]>(async (ids) => {
  const client = await getApiClient();
  await client.deletePromotions(ids);
});
const loading = useLoading(listLoading, deleteLoading);

load();
watch(searchValue, () => pagination.setPage(1));
watch([sortExpression, searchValue, () => pagination.skip], debounce(load, 300));

async function confirmDelete(list: Promotion[]) {
  if (!(await showConfirmation(t("MARKETING.ALERTS.DELETE_PROMOTIONS", { count: list.length })))) return;
  await closeChildren();
  await remove(list.map((x) => x.id!));
  selection.value = [];
  await load();
}


function openDetails(item?: Promotion, clone?: Promotion) {
  openBlade({ name: "MarketingPromotionDetails", param: item?.id, options: clone ? { clone } : undefined });
}

// the AngularJS row context menu: manage, copy id, clone, delete
const rowActions = (item: Promotion) => [
  { id: "manage", icon: "lucide-pencil", title: t("MARKETING.ACTIONS.MANAGE"), type: "info" as const, clickHandler: () => openDetails(item) },
  {
    id: "copyId",
    icon: "lucide-copy",
    title: t("MARKETING.ACTIONS.COPY_ID"),
    type: "info" as const,
    clickHandler: () => navigator.clipboard.writeText(item.id ?? ""),
  },
  {
    id: "clone",
    icon: "lucide-copy-plus",
    title: t("MARKETING.ACTIONS.CLONE"),
    type: "info" as const,
    hidden: !hasAccess("marketing:create"),
    clickHandler: () => openDetails(undefined, item),
  },
  {
    id: "delete",
    icon: "lucide-trash-2",
    title: t("MARKETING.ACTIONS.DELETE"),
    type: "danger" as const,
    variant: "danger" as const,
    hidden: !hasAccess("marketing:delete"),
    clickHandler: () => confirmDelete([item]),
  },
];

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
    clickHandler: () => confirmDelete(selection.value),
  },
]);

exposeToChildren({ reload: () => load(), reloadFirstPage, clone: (promotion: Promotion) => openDetails(undefined, promotion) });
</script>
