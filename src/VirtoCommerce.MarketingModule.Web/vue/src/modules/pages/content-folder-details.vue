<template>
  <VcBlade
    :title="folder.name || $t('MARKETING.PAGES.CONTENT_FOLDER.NEW_TITLE')"
    :toolbar-items="bladeToolbar"
    :loading="loading"
    width="50%"
  >
    <VcContainer>
      <VcForm class="mk-form tw-space-y-4">
        <VcInput
          v-model="folder.name"
          :label="$t('MARKETING.PAGES.CONTENT.COLUMNS.NAME')"
          :placeholder="$t('MARKETING.PAGES.CONTENT_FOLDER.NAME_PLACEHOLDER')"
          required
          :disabled="readonly"
        />
        <VcTextarea
          v-model="folder.description"
          :label="$t('MARKETING.PAGES.CONTENT.COLUMNS.DESCRIPTION')"
          :placeholder="$t('MARKETING.PAGES.CONTENT_FOLDER.DESCRIPTION_PLACEHOLDER')"
          required
          :disabled="readonly"
        />
      </VcForm>
    </VcContainer>
  </VcBlade>
</template>

<script setup lang="ts">
import { useApiClient, useAsync, useBlade, useBladeForm, useLoading, usePermissions, usePopup, type IBladeToolbar } from "@vc-shell/framework";
import { VcBlade, VcContainer, VcForm, VcInput, VcTextarea } from "@vc-shell/framework/ui";
import { computed, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";
import { MarketingModuleDynamicContentClient, type DynamicContentFolder } from "../../api_client/virtocommerce.marketing";

defineBlade({
  url: "/dynamic-content-folder",
  name: "MarketingContentFolderDetails",
  permissions: ["marketing:access"],
});

// a folder of content items or placeholders (both kinds share the folders API)
const { t } = useI18n({ useScope: "global" });
const { param, options, callParent, closeSelf } = useBlade<{ folderId: string }>();
const { showConfirmation } = usePopup();
const { hasAccess } = usePermissions();
const { getApiClient } = useApiClient(MarketingModuleDynamicContentClient);

const folder = ref<DynamicContentFolder>({ parentFolderId: options.value?.folderId });
const id = ref(param.value);
const readonly = computed(() => !hasAccess(id.value ? "marketing:update" : "marketing:create"));

const { loading: itemLoading, action: load } = useAsync(async () => {
  if (!id.value) return;
  const client = await getApiClient();
  folder.value = await client.getDynamicContentFolderById(id.value);
});
const { loading: saveLoading, action: save } = useAsync(async () => {
  const client = await getApiClient();
  if (id.value) await client.updateDynamicContentFolder(folder.value);
  else id.value = (await client.createDynamicContentFolder(folder.value)).id;
});
const { loading: deleteLoading, action: remove } = useAsync(async () => {
  const client = await getApiClient();
  await client.deleteDynamicContentFolders([id.value!]);
});
const loading = useLoading(itemLoading, saveLoading, deleteLoading);

const form = useBladeForm({
  data: folder,
  canSaveOverride: computed(() => !!folder.value.name?.trim() && !!folder.value.description?.trim()),
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
      if (!(await showConfirmation(t("MARKETING.ALERTS.DELETE_FOLDER", { name: folder.value.name })))) return;
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
