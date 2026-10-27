<template>
  <VcBlade
    :title="$t(title)"
    :toolbar-items="bladeToolbar"
    :loading="loading"
    width="50%"
  >
    <div class="mk-crumbs">
      <VcBreadcrumbs
        :items="crumbs"
        separated
      />
      <VcHint v-if="pick">{{ $t("MARKETING.PAGES.CONTENT.PICK_HINT") }}</VcHint>
    </div>
    <VcDataTable
      v-model:search-value="searchValue"
      v-model:selection="selection"
      :selection-mode="pick ? undefined : 'multiple'"
      :items="items"
      :total-count="totalCount"
      :pagination="pagination"
      :searchable="true"
      :state-key="stateKey"
      :active-item-id="activeChildParam"
      :row-actions="pick ? undefined : rowActions"
      row-actions-mode="dropdown"
      :empty-state="{ icon: places ? 'lucide-layout-template' : 'lucide-inbox', title: $t('MARKETING.PAGES.CONTENT.EMPTY') }"
      @row-click="(e: { data: Entry }) => onRowClick(e.data)"
      @pagination-click="pagination.goToPage"
    >
      <VcColumn
        id="name"
        :title="$t('MARKETING.PAGES.CONTENT.COLUMNS.NAME')"
        always-visible
      >
        <template #body="{ data }">
          <span class="mk-entry">
            <VcIcon
              :icon="data.objectType === FOLDER ? 'lucide-folder' : places ? 'lucide-layout-template' : 'lucide-file-code-2'"
              size="s"
              class="mk-entry__icon"
            />
            <span class="mk-list__primary">{{ data.name }}</span>
            <VcStatus
              v-if="pick && chosen.has(data.id)"
              variant="success"
              >{{ $t("MARKETING.PAGES.CONTENT.ADDED") }}</VcStatus
            >
          </span>
        </template>
      </VcColumn>
      <VcColumn
        id="description"
        :title="$t('MARKETING.PAGES.CONTENT.COLUMNS.DESCRIPTION')"
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
  useFunctions,
  useLatestRequest,
  useLoading,
  usePermissions,
  usePopup,
  useTableSearch,
  type IBladeToolbar,
} from "@vc-shell/framework";
import { VcBlade, VcBreadcrumbs, VcColumn, VcDataTable, VcHint, VcIcon, VcStatus } from "@vc-shell/framework/ui";
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { MarketingModuleDynamicContentClient, type DynamicContentListEntry } from "../../api_client/virtocommerce.marketing";

defineBlade({
  url: "/dynamic-content-list",
  name: "MarketingContentList",
  permissions: ["marketing:access"],
});

type Entry = DynamicContentListEntry;
const FOLDER = "DynamicContentFolder";

// one folder browser for content items and placeholders (the AngularJS items/ and placeholders/ list blades)
const { t } = useI18n({ useScope: "global" });
const { options, openBlade, exposeToChildren, closeChildren, callParent, activeChildParam } = useBlade<{ kind: "items" | "places"; pick?: boolean }>();
const { debounce } = useFunctions();
const { showConfirmation } = usePopup();
const { hasAccess } = usePermissions();
const { getApiClient } = useApiClient(MarketingModuleDynamicContentClient);

const places = computed(() => options.value?.kind === "places");
// pick mode: the publication's "add placeholders / add content items" steps — browse folders, click an entry to add it
const pick = computed(() => !!options.value?.pick);
const title = computed(() => {
  if (pick.value) return places.value ? "MARKETING.PAGES.CONTENT.ADD_PLACES_TITLE" : "MARKETING.PAGES.CONTENT.ADD_ITEMS_TITLE";
  return places.value ? "MARKETING.PAGES.CONTENT.PLACES_TITLE" : "MARKETING.PAGES.CONTENT.ITEMS_TITLE";
});
// entries already in the publication are marked "Added" (the AngularJS steps hid them)
const chosen = ref(new Set<string>());
const root = computed(() => ({ id: places.value ? "ContentPlace" : "ContentItem", name: t(places.value ? "MARKETING.PAGES.CONTENT.PLACES_ROOT" : "MARKETING.PAGES.CONTENT.ITEMS_ROOT") }));
const stateKey = computed(() => (places.value ? "MARKETING_CONTENT_PLACES" : "MARKETING_CONTENT_ITEMS"));

const PAGE_SIZE = 20;
const items = ref<Entry[]>([]);
const totalCount = ref(0);
const selection = ref<Entry[]>([]);
const path = ref<{ id: string; name: string }[]>([]);
const folderId = computed(() => path.value[path.value.length - 1]?.id ?? root.value.id);

const { searchValue } = useTableSearch({ stateKey: stateKey.value });
const pagination = useDataTablePagination({ stateKey: stateKey.value, pageSize: PAGE_SIZE, totalCount });

const crumbs = computed(() =>
  [root.value, ...path.value].map((f, i) => ({ id: f.id, title: f.name, clickHandler: () => goTo(i) })),
);

const search = useLatestRequest();
const { loading: listLoading, action: load } = useAsync(async () => {
  const client = await getApiClient();
  const criteria = { folderId: folderId.value, keyword: searchValue.value || undefined, skip: pagination.skip, take: PAGE_SIZE };
  const result = await search.latest(
    places.value ? client.dynamicContentPlaceListEntriesSearch(criteria) : client.dynamicContentItemsEntriesSearch(criteria),
  );
  if (!result) return;
  if (pick.value) chosen.value = new Set(await callParent<string[]>("chosen", options.value?.kind));
  items.value = result.results ?? [];
  totalCount.value = result.totalCount ?? 0;
});
const { loading: deleteLoading, action: remove } = useAsync<Entry[]>(async (list) => {
  const client = await getApiClient();
  const folders = list!.filter((x) => x.objectType === FOLDER).map((x) => x.id!);
  const others = list!.filter((x) => x.objectType !== FOLDER).map((x) => x.id!);
  if (folders.length) await client.deleteDynamicContentFolders(folders);
  if (others.length) await (places.value ? client.deleteDynamicContentPlaces(others) : client.deleteDynamicContents(others));
});
const loading = useLoading(listLoading, deleteLoading);

load();
watch(searchValue, () => pagination.setPage(1));
watch([searchValue, () => pagination.skip], debounce(load, 300));

// another folder starts at its first page; from page 2 setPage(1) triggers the skip watcher, which loads
async function reloadFirstPage() {
  if (pagination.skip) pagination.setPage(1);
  else await load();
}

async function goTo(index: number) {
  await closeChildren();
  path.value = path.value.slice(0, index);
  await reloadFirstPage();
}

function openDetails(entry: Entry | undefined, folder: boolean) {
  const itemBlade = places.value ? "MarketingContentPlaceDetails" : "MarketingContentItemDetails";
  openBlade({
    name: folder ? "MarketingContentFolderDetails" : itemBlade,
    param: entry?.id,
    options: { folderId: folderId.value },
  });
}

async function onRowClick(entry: Entry) {
  if (entry.objectType === FOLDER) {
    await closeChildren();
    path.value = [...path.value, { id: entry.id!, name: entry.name ?? "" }];
    await reloadFirstPage();
  } else if (pick.value) {
    if (chosen.value.has(entry.id!)) return;
    await callParent("pick", { kind: options.value?.kind, entry });
    chosen.value = new Set([...chosen.value, entry.id!]);
  } else openDetails(entry, false);
}

async function confirmDelete(list: Entry[]) {
  if (!(await showConfirmation(t("MARKETING.ALERTS.DELETE_CONTENT", { count: list.length })))) return;
  await closeChildren();
  await remove(list);
  selection.value = [];
  await load();
}

const rowActions = (entry: Entry) => [
  {
    id: "manage",
    icon: "lucide-pencil",
    title: t("MARKETING.ACTIONS.MANAGE"),
    type: "info" as const,
    clickHandler: () => openDetails(entry, entry.objectType === FOLDER),
  },
  {
    id: "delete",
    icon: "lucide-trash-2",
    title: t("MARKETING.ACTIONS.DELETE"),
    type: "danger" as const,
    variant: "danger" as const,
    hidden: !hasAccess("marketing:delete"),
    clickHandler: () => confirmDelete([entry]),
  },
];

const bladeToolbar = ref<IBladeToolbar[]>([
  { id: "refresh", title: t("MARKETING.ACTIONS.REFRESH"), icon: "lucide-refresh-cw", clickHandler: () => load() },
  {
    id: "addFolder",
    isVisible: computed(() => !pick.value),
    title: t("MARKETING.ACTIONS.ADD_FOLDER"),
    icon: "lucide-folder-plus",
    permissions: ["marketing:create"],
    clickHandler: () => openDetails(undefined, true),
  },
  {
    id: "add",
    isVisible: computed(() => !pick.value),
    title: computed(() => t(places.value ? "MARKETING.ACTIONS.ADD_PLACE" : "MARKETING.ACTIONS.ADD_ITEM")),
    icon: "lucide-plus",
    permissions: ["marketing:create"],
    clickHandler: () => openDetails(undefined, false),
  },
  {
    id: "delete",
    isVisible: computed(() => !pick.value),
    title: t("MARKETING.ACTIONS.DELETE"),
    icon: "lucide-trash-2",
    permissions: ["marketing:delete"],
    disabled: computed(() => !selection.value.length),
    clickHandler: () => confirmDelete(selection.value),
  },
]);

exposeToChildren({ reload: () => load() });
</script>

<style lang="scss">
.mk-crumbs {
  padding: 12px 16px 0;
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.mk-entry {
  display: inline-flex;
  align-items: center;
  gap: 8px;

  &__icon {
    color: var(--neutrals-500);
  }
}
</style>
