<template>
  <VcBlade
    :title="$t('MARKETING.WIDGETS.COUPONS')"
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
      :search-placeholder="$t('MARKETING.PAGES.COUPONS.SEARCH')"
      :state-key="'MARKETING_COUPONS'"
      :active-item-id="activeChildParam"
      :empty-state="{ icon: 'lucide-ticket', title: $t('MARKETING.PAGES.COUPONS.EMPTY') }"
      @row-click="(e: { data: Coupon }) => openDetails(e.data)"
      @pagination-click="pagination.goToPage"
    >
      <VcColumn
        id="code"
        :title="$t('MARKETING.PAGES.COUPONS.COLUMNS.CODE')"
        body-class="mk-list__primary"
        sortable
        always-visible
      />
      <VcColumn
        id="totalUsesCount"
        :title="$t('MARKETING.PAGES.COUPONS.COLUMNS.USED')"
        align="end"
        sortable
        always-visible
      >
        <template #body="{ data }">{{ data.totalUsesCount ?? 0 }}</template>
      </VcColumn>
      <VcColumn
        id="maxUsesNumber"
        :title="$t('MARKETING.PAGES.COUPONS.COLUMNS.MAX_USES')"
        align="end"
        sortable
      >
        <template #body="{ data }">{{ data.maxUsesNumber || $t("MARKETING.UNLIMITED") }}</template>
      </VcColumn>
      <VcColumn
        id="expirationDate"
        :title="$t('MARKETING.PAGES.COUPONS.COLUMNS.EXPIRES')"
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
import { VcBlade, VcColumn, VcDataTable } from "@vc-shell/framework/ui";
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { MarketingModulePromotionClient, type Coupon } from "../../api_client/virtocommerce.marketing";

defineBlade({
  url: "/coupons",
  name: "MarketingCoupons",
  permissions: ["marketing:access"],
});

const { t } = useI18n({ useScope: "global" });
const { param, openBlade, callParent, exposeToChildren, closeChildren, activeChildParam } = useBlade();
const { debounce } = useFunctions();
const { showConfirmation } = usePopup();
const { getApiClient } = useApiClient(MarketingModulePromotionClient);

const PAGE_SIZE = 20;
const items = ref<Coupon[]>([]);
const totalCount = ref(0);
const selection = ref<Coupon[]>([]);

const { sortField, sortOrder, sortExpression } = useDataTableSort({ stateKey: "MARKETING_COUPONS", initialField: "code", initialDirection: "ASC" });
const { searchValue } = useTableSearch({ stateKey: "MARKETING_COUPONS" });
const pagination = useDataTablePagination({ stateKey: "MARKETING_COUPONS", pageSize: PAGE_SIZE, totalCount });

const search = useLatestRequest();
const { loading: listLoading, action: load } = useAsync(async () => {
  const client = await getApiClient();
  const result = await search.latest(
    client.searchCoupons({
      promotionId: param.value,
      code: searchValue.value || undefined,
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
  await client.deleteCoupons(ids);
});
const loading = useLoading(listLoading, deleteLoading);

load();
watch(searchValue, () => pagination.setPage(1));
watch([sortExpression, searchValue, () => pagination.skip], debounce(load, 300));

// a change here also updates the promotion's coupon widget
async function reload() {
  await Promise.all([load(), callParent("refreshCounts")]);
}

function openDetails(coupon?: Coupon) {
  openBlade({ name: "MarketingCouponDetails", param: coupon?.id, options: { promotionId: param.value } });
}

const bladeToolbar = ref<IBladeToolbar[]>([
  {
    id: "add",
    title: t("MARKETING.ACTIONS.ADD"),
    icon: "lucide-plus",
    permissions: ["marketing:update"],
    clickHandler: () => openDetails(),
  },
  {
    id: "import",
    title: t("MARKETING.ACTIONS.IMPORT"),
    icon: "lucide-file-up",
    permissions: ["marketing:update", "platform:asset:create"],
    clickHandler: () => openBlade({ name: "MarketingCouponImport", param: param.value }),
  },
  { id: "refresh", title: t("MARKETING.ACTIONS.REFRESH"), icon: "lucide-refresh-cw", clickHandler: () => reload() },
  {
    id: "delete",
    title: t("MARKETING.ACTIONS.DELETE"),
    icon: "lucide-trash-2",
    permissions: ["marketing:delete"],
    disabled: computed(() => !selection.value.length),
    async clickHandler() {
      if (!(await showConfirmation(t("MARKETING.ALERTS.DELETE_COUPONS", { count: selection.value.length })))) return;
      await closeChildren();
      await remove(selection.value.map((x) => x.id!));
      selection.value = [];
      await reload();
    },
  },
]);

exposeToChildren({ reload });
</script>
