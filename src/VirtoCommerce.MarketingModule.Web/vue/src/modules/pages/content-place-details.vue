<template>
  <VcBlade
    :title="place.name || $t('MARKETING.PAGES.CONTENT_PLACE.NEW_TITLE')"
    :toolbar-items="bladeToolbar"
    :loading="loading"
    width="50%"
  >
    <VcContainer>
      <VcForm class="mk-form tw-space-y-4">
        <VcInput
          v-model="place.name"
          :label="$t('MARKETING.PAGES.CONTENT.COLUMNS.NAME')"
          :placeholder="$t('MARKETING.PAGES.CONTENT_PLACE.NAME_PLACEHOLDER')"
          required
          :disabled="readonly"
        />
        <VcTextarea
          v-model="place.description"
          :label="$t('MARKETING.PAGES.CONTENT.COLUMNS.DESCRIPTION')"
          :placeholder="$t('MARKETING.PAGES.CONTENT_PLACE.DESCRIPTION_PLACEHOLDER')"
          required
          :disabled="readonly"
        />
        <VcCard :header="$t('MARKETING.PAGES.CONTENT_PLACE.IMAGE')">
          <div class="tw-p-4">
            <VcImageUpload
              :image="place.imageUrl ? { url: place.imageUrl, name: place.name } : undefined"
              accept=".jpg,.png,.jpeg,.webp,.gif,.svg"
              :loading="uploading"
              :disabled="readonly"
              @upload="upload"
              @remove="place.imageUrl = undefined"
            />
          </div>
        </VcCard>
      </VcForm>
    </VcContainer>
  </VcBlade>
</template>

<script setup lang="ts">
import { useApiClient, useAsync, useBlade, useBladeForm, useLoading, usePermissions, usePopup, type IBladeToolbar } from "@vc-shell/framework";
import { VcBlade, VcCard, VcContainer, VcForm, VcImageUpload, VcInput, VcTextarea } from "@vc-shell/framework/ui";
import { computed, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";
import { MarketingModuleDynamicContentClient, type DynamicContentPlace } from "../../api_client/virtocommerce.marketing";
import { uploadAsset } from "../lookups";

defineBlade({
  url: "/dynamic-content-place",
  name: "MarketingContentPlaceDetails",
  permissions: ["marketing:access"],
});

const { t } = useI18n({ useScope: "global" });
const { param, options, callParent, closeSelf, setError, clearError } = useBlade<{ folderId: string }>();
const { showConfirmation } = usePopup();
const { hasAccess } = usePermissions();
const { getApiClient } = useApiClient(MarketingModuleDynamicContentClient);

const place = ref<DynamicContentPlace>({ folderId: options.value?.folderId });
const id = ref(param.value);
const readonly = computed(() => !hasAccess(id.value ? "marketing:update" : "marketing:create"));
const uploading = ref(false);

const { loading: itemLoading, action: load } = useAsync(async () => {
  if (!id.value) return;
  const client = await getApiClient();
  place.value = await client.getDynamicContentPlaceById(id.value);
});
const { loading: saveLoading, action: save } = useAsync(async () => {
  const client = await getApiClient();
  if (id.value) await client.updateDynamicContentPlace(place.value);
  else id.value = (await client.createDynamicContentPlace(place.value)).id;
});
const { loading: deleteLoading, action: remove } = useAsync(async () => {
  const client = await getApiClient();
  await client.deleteDynamicContentPlaces([id.value!]);
});
const loading = useLoading(itemLoading, saveLoading, deleteLoading);

// the placeholder picture goes to the placeholders-images assets folder (as in the AngularJS blade)
async function upload(files: FileList) {
  clearError();
  uploading.value = true;
  try {
    place.value.imageUrl = (await uploadAsset(files[0], "placeholders-images")).url;
  } catch (e) {
    setError(e);
  } finally {
    uploading.value = false;
  }
}

const form = useBladeForm({
  data: place,
  canSaveOverride: computed(() => !!place.value.name?.trim() && !!place.value.description?.trim()),
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
      if (!(await showConfirmation(t("MARKETING.ALERTS.DELETE_NAMED", { name: place.value.name })))) return;
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
