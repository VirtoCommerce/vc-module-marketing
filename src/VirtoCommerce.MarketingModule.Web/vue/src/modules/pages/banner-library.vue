<template>
  <VcBlade
    :title="$t('MARKETING.MENU.DYNAMIC_CONTENT')"
    :toolbar-items="bladeToolbar"
    :loading="loading"
    width="50%"
  >
    <div class="mk-lib">
      <div class="mk-lib__head">
        <!-- the Sites page's tab bar look -->
        <div
          role="tablist"
          class="mk-lib__tabs"
        >
          <button
            v-for="name in TABS"
            :key="name"
            type="button"
            role="tab"
            class="mk-lib__tab"
            :class="{ 'mk-lib__tab--active': tab === name }"
            :aria-selected="tab === name"
            @click="tab = name"
          >
            {{ $t(`MARKETING.LIBRARY.TABS.${name.toUpperCase()}`) }}
          </button>
        </div>
      </div>
      <p class="mk-lib__intro">{{ $t("MARKETING.LIBRARY.INTRO") }}</p>
    </div>

    <template v-if="tab === 'banners'">
      <div class="mk-lib__search">
        <VcInput
          v-model="keyword"
          :placeholder="$t('MARKETING.LIBRARY.SEARCH_BANNERS')"
          :aria-label="$t('MARKETING.LIBRARY.SEARCH_BANNERS')"
          size="small"
          clearable
        />
      </div>
      <div
        v-if="rows.length"
        class="mk-cards"
      >
        <button
          v-for="item in rows"
          :key="item.id"
          type="button"
          class="mk-card"
          :class="{ 'mk-card--active': item.id === activeChildParam }"
          @click="openDetails(item.id)"
        >
          <VcImage
            class="mk-card__image"
            :src="imageOf(item)"
            size="auto"
            aspect="16x9"
            background="contain"
            empty-icon="lucide-image"
          />
          <span class="mk-card__name">{{ item.name }}</span>
          <span class="mk-card__meta">{{ cardMeta(item) }}</span>
        </button>
      </div>
      <p
        v-else-if="!loading"
        class="mk-lib__empty"
      >
        {{ $t("MARKETING.LIBRARY.EMPTY_BANNERS") }}
      </p>
      <VcPagination
        v-if="pagination.pages > 1"
        class="mk-lib__pages"
        :pages="pagination.pages"
        :current-page="pagination.currentPage"
        @item-click="pagination.goToPage"
      />
    </template>

    <VcDataTable
      v-else-if="tab === 'slots'"
      v-model:search-value="keyword"
      :items="rows"
      :total-count="totalCount"
      :pagination="pagination"
      :searchable="true"
      :active-item-id="activeChildParam"
      :empty-state="{ icon: 'lucide-layout-template', title: $t('MARKETING.LIBRARY.EMPTY_SLOTS') }"
      @row-click="(e: { data: DynamicContentPlace }) => openDetails(e.data.id)"
      @pagination-click="pagination.goToPage"
    >
      <VcColumn
        id="name"
        :title="$t('MARKETING.LIBRARY.COLUMNS.SLOT')"
        body-class="mk-list__primary"
        always-visible
      />
      <VcColumn
        id="description"
        :title="$t('MARKETING.LIBRARY.COLUMNS.WHERE')"
      />
      <VcColumn
        id="banners"
        :title="$t('MARKETING.LIBRARY.COLUMNS.BANNERS')"
        align="end"
      >
        <template #body="{ data }">{{ usage.banners.get(data.id)?.size ?? 0 }}</template>
      </VcColumn>
    </VcDataTable>

    <!-- the dynamic content publications: a banner in a slot, on a site, for a time and an audience -->
    <VcDataTable
      v-else
      v-model:search-value="keyword"
      :items="placements"
      :total-count="totalCount"
      :pagination="pagination"
      :searchable="true"
      :active-item-id="activeChildParam"
      :empty-state="{ icon: 'lucide-calendar-range', title: $t('MARKETING.LIBRARY.EMPTY_PLACEMENTS') }"
      @row-click="(e: { data: DynamicContentPublication }) => openDetails(e.data.id)"
      @pagination-click="pagination.goToPage"
    >
      <VcColumn
        id="name"
        :title="$t('MARKETING.LIBRARY.COLUMNS.PLACEMENT')"
        body-class="mk-list__primary"
        always-visible
      />
      <VcColumn
        id="storeId"
        :title="$t('MARKETING.LIBRARY.COLUMNS.SITE')"
      />
      <VcColumn
        id="status"
        :title="$t('MARKETING.LIBRARY.COLUMNS.STATUS')"
        always-visible
      >
        <template #body="{ data }">
          <div class="tw-inline-flex">
            <VcStatus :variant="placementStatus(data).variant">{{ $t("MARKETING.LIBRARY.STATUS." + placementStatus(data).key) }}</VcStatus>
          </div>
        </template>
      </VcColumn>
      <VcColumn
        id="startDate"
        :title="$t('MARKETING.LIBRARY.COLUMNS.STARTS')"
        type="date"
      />
      <VcColumn
        id="endDate"
        :title="$t('MARKETING.LIBRARY.COLUMNS.ENDS')"
        type="date"
      />
    </VcDataTable>
  </VcBlade>
</template>

<script setup lang="ts">
import { useApiClient, useAsync, useBlade, useDataTablePagination, useFunctions, useLatestRequest, useLoading, type IBladeToolbar } from "@vc-shell/framework";
import { VcBlade, VcColumn, VcDataTable, VcImage, VcInput, VcPagination, VcStatus } from "@vc-shell/framework/ui";
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import {
  MarketingModuleDynamicContentClient,
  type DynamicContentItem,
  type DynamicContentPlace,
  type DynamicContentPublication,
} from "../../api_client/virtocommerce.marketing";
import { imageOf } from "../banners";

// Banners: the content items as cards, the content places ("slots") and the publications ("placements": a banner in a
// slot on a site, for a time and an audience) as rows; details open the existing blades
defineBlade({
  url: "/dynamic-content",
  name: "MarketingBannerLibrary",
  isWorkspace: true,
  permissions: ["marketing:access"],
  menuItem: {
    title: "MARKETING.MENU.DYNAMIC_CONTENT",
    icon: "lucide-image",
    priority: 11,
  },
});

const TABS = ["banners", "slots", "placements"] as const;
type Tab = (typeof TABS)[number];

const { t } = useI18n({ useScope: "global" });
const { openBlade, exposeToChildren, activeChildParam } = useBlade();
const { debounce } = useFunctions();
const { getApiClient } = useApiClient(MarketingModuleDynamicContentClient);

const PAGE_SIZE = 24;
const tab = ref<Tab>("banners");
const keyword = ref("");
// places have the items' list fields, so one list serves both tabs
const rows = ref<DynamicContentItem[]>([]);
const placements = ref<DynamicContentPublication[]>([]);
const totalCount = ref(0);
const pagination = useDataTablePagination({ pageSize: PAGE_SIZE, totalCount });

const search = useLatestRequest();
const { loading: listLoading, action: load } = useAsync(async () => {
  const client = await getApiClient();
  const criteria = { keyword: keyword.value || undefined, skip: pagination.skip, take: PAGE_SIZE };
  if (tab.value === "placements") {
    const placed = await search.latest(client.dynamicContentPublicationsSearch({ ...criteria, sort: "name" }));
    if (!placed) return;
    placements.value = placed.results ?? [];
    totalCount.value = placed.totalCount ?? 0;
    return;
  }
  const result = await search.latest(
    tab.value === "banners" ? client.dynamicContentItemsSearch(criteria) : client.dynamicContentPlacesSearch(criteria),
  );
  if (!result) return;
  rows.value = result.results ?? [];
  totalCount.value = result.totalCount ?? 0;
});

// which sites show each banner and which banners each slot carries, read off the publications
// ponytail: the first 500 publications; a server-side count when a catalog has more
const usage = ref({ sites: new Map<string, Set<string>>(), banners: new Map<string, Set<string>>() });
const { loading: usageLoading, action: loadUsage } = useAsync(async () => {
  const client = await getApiClient();
  const result = await client.dynamicContentPublicationsSearch({ take: 500 });
  const sites = new Map<string, Set<string>>();
  const banners = new Map<string, Set<string>>();
  const add = (map: Map<string, Set<string>>, key?: string, value?: string) => {
    if (key && value) map.set(key, (map.get(key) ?? new Set()).add(value));
  };
  for (const publication of result.results ?? []) {
    for (const item of publication.contentItems ?? []) {
      add(sites, item.id, publication.storeId);
      for (const place of publication.contentPlaces ?? []) add(banners, place.id, item.id);
    }
  }
  usage.value = { sites, banners };
});
const loading = useLoading(listLoading, usageLoading);

/** Showing, Scheduled, Ended or Off: the publication's switch and dates against now */
function placementStatus(p: DynamicContentPublication): { key: string; variant: "success" | "info" | "info-dark" } {
  const now = Date.now();
  if (!p.isActive) return { key: "OFF", variant: "info-dark" };
  if (p.startDate && new Date(p.startDate).getTime() > now) return { key: "SCHEDULED", variant: "info" };
  if (p.endDate && new Date(p.endDate).getTime() < now) return { key: "ENDED", variant: "info-dark" };
  return { key: "SHOWING", variant: "success" };
}

const cardMeta = (item: DynamicContentItem) =>
  [item.contentType, t("MARKETING.LIBRARY.USED_ON", usage.value.sites.get(item.id!)?.size ?? 0)].filter(Boolean).join(" · ");

load();
loadUsage();
watch(tab, () => {
  rows.value = [];
  placements.value = [];
  keyword.value = "";
});
watch([tab, keyword], () => pagination.setPage(1));
watch([tab, keyword, () => pagination.skip], debounce(load, 300));

// new entries go to the root folder, as from the folder browser's root
function openDetails(id?: string) {
  if (tab.value === "placements") return openBlade({ name: "MarketingPublicationDetails", param: id });
  const banners = tab.value === "banners";
  openBlade({
    name: banners ? "MarketingContentItemDetails" : "MarketingContentPlaceDetails",
    param: id,
    options: { folderId: banners ? "ContentItem" : "ContentPlace" },
  });
}

const ADD: Record<Tab, string> = {
  banners: "MARKETING.LIBRARY.ADD_BANNER",
  slots: "MARKETING.LIBRARY.ADD_SLOT",
  placements: "MARKETING.LIBRARY.ADD_PLACEMENT",
};

const bladeToolbar = ref<IBladeToolbar[]>([
  {
    id: "refresh",
    title: t("MARKETING.ACTIONS.REFRESH"),
    icon: "lucide-refresh-cw",
    clickHandler: () => Promise.all([load(), loadUsage()]),
  },
  {
    id: "add",
    title: computed(() => t(ADD[tab.value])),
    icon: "lucide-plus",
    permissions: ["marketing:create"],
    clickHandler: () => openDetails(),
  },
  {
    // folders, bulk delete and the rest of the folder browser
    id: "folders",
    title: t("MARKETING.LIBRARY.FOLDERS"),
    icon: "lucide-folder-open",
    // placements have no folders
    isVisible: computed(() => tab.value !== "placements"),
    clickHandler: () => openBlade({ name: "MarketingContentList", options: { kind: tab.value === "banners" ? "items" : "places" } }),
  },
]);

exposeToChildren({ reload: () => Promise.all([load(), loadUsage()]) });
</script>

<style lang="scss">
.mk-lib {
  padding: 16px 16px 4px;

  &__head {
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 12px;
    border-bottom: 1px solid var(--neutrals-200);
  }

  &__tabs {
    display: flex;
    gap: 24px;
  }

  &__tab {
    height: 40px;
    padding: 0;
    margin-bottom: -1px;
    border: 0;
    border-bottom: 2px solid transparent;
    background: none;
    font: inherit;
    font-size: 15px;
    color: var(--neutrals-500);
    cursor: pointer;

    &--active {
      color: var(--neutrals-900);
      font-weight: 700;
      border-bottom-color: var(--primary-500);
    }
  }

  &__intro {
    margin: 10px 0 0;
    font-size: 13px;
    color: var(--neutrals-500);
  }

  &__search {
    padding: 12px 16px 0;
    max-width: 360px;
  }

  &__empty {
    padding: 32px 16px;
    text-align: center;
    color: var(--neutrals-500);
  }

  &__pages {
    padding: 0 16px 16px;
  }
}

.mk-cards {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
  gap: 16px;
  padding: 16px;
}

.mk-card {
  display: flex;
  flex-direction: column;
  gap: 4px;
  padding: 8px 8px 12px;
  text-align: left;
  border: 1px solid var(--neutrals-200);
  border-radius: 8px;
  background: var(--blade-background-color);
  cursor: pointer;
  transition: border-color 0.15s;

  &:hover {
    border-color: var(--primary-500);
  }

  &--active {
    border-color: var(--primary-500);
    background: var(--primary-50);
  }

  &:focus-visible {
    outline: 2px solid var(--primary-500);
    outline-offset: 2px;
  }

  &__image {
    margin-bottom: 6px;
    border-radius: 6px;
    overflow: hidden;
    background: var(--neutrals-100);
  }

  &__name {
    font-weight: 500;
    color: var(--neutrals-900);
    overflow-wrap: anywhere;
  }

  &__meta {
    font-size: 12px;
    color: var(--neutrals-500);
  }
}
</style>
