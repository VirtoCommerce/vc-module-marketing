<template>
  <VcBlade
    :title="$t('MARKETING.WIDGETS.USAGE')"
    :loading="loading"
    width="50%"
  >
    <VcDataTable
      :items="items"
      :total-count="totalCount"
      :pagination="pagination"
      :state-key="'MARKETING_PROMOTION_ORDERS'"
      :active-item-id="activeChildParam"
      :show-all-columns="false"
      :empty-state="{ icon: 'lucide-file-text', title: $t('MARKETING.PAGES.USAGE.EMPTY') }"
      @row-click="(e: { data: PromotionOrder }) => openOrder(e.data)"
      @pagination-click="pagination.goToPage"
    >
      <VcColumn
        id="number"
        :title="$t('MARKETING.PAGES.USAGE.COLUMNS.NUMBER')"
        body-class="mk-list__primary"
        always-visible
      />
      <VcColumn
        id="customerName"
        :title="$t('MARKETING.PAGES.USAGE.COLUMNS.CUSTOMER')"
      />
      <VcColumn
        id="status"
        :title="$t('MARKETING.PAGES.USAGE.COLUMNS.STATUS')"
        always-visible
      >
        <template #body="{ data }">
          <div class="tw-inline-flex"><VcStatus variant="info">{{ data.status }}</VcStatus></div>
        </template>
      </VcColumn>
      <VcColumn
        id="createdDate"
        :title="$t('MARKETING.PAGES.USAGE.COLUMNS.CREATED')"
        type="date"
      />
      <VcColumn
        id="total"
        :title="$t('MARKETING.PAGES.USAGE.COLUMNS.TOTAL')"
        align="end"
        always-visible
      >
        <template #body="{ data }">
          <span class="vc-table-cell-money">{{ n(data.total ?? 0, { minimumFractionDigits: 2, maximumFractionDigits: 2 }) }} {{ data.currency }}</span>
        </template>
      </VcColumn>
    </VcDataTable>
  </VcBlade>
</template>

<script setup lang="ts">
import { useAsync, useBlade, useDataTablePagination } from "@vc-shell/framework";
import { VcBlade, VcColumn, VcDataTable, VcStatus } from "@vc-shell/framework/ui";
import { ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { searchOrdersByPromotion, type PromotionOrder } from "../lookups";

defineBlade({
  url: "/promotion-orders",
  name: "MarketingPromotionOrders",
  permissions: ["marketing:access"],
});

// the AngularJS "Usage history" widget: orders the promotion was applied to
const { n } = useI18n({ useScope: "global" });
const { param, openBlade, setError, activeChildParam } = useBlade();
const PAGE_SIZE = 20;
const items = ref<PromotionOrder[]>([]);
const totalCount = ref(0);
const pagination = useDataTablePagination({ stateKey: "MARKETING_PROMOTION_ORDERS", pageSize: PAGE_SIZE, totalCount });

const { loading, action: load } = useAsync(async () => {
  const r = await searchOrdersByPromotion(param.value!, pagination.skip, PAGE_SIZE, "createdDate:desc");
  items.value = r.results ?? [];
  totalCount.value = r.totalCount ?? 0;
});
load();
watch(() => pagination.skip, () => load());

// the order opens in the Orders module's blade
async function openOrder(order: PromotionOrder) {
  try {
    await openBlade({ name: "OrdersDetails", param: order.id });
  } catch (e) {
    setError(e);
  }
}
</script>
