<template>
  <VcBlade
    :title="$t('MARKETING.PAGES.IMPORT.TITLE')"
    :toolbar-items="bladeToolbar"
    :loading="uploading"
    width="50%"
  >
    <VcContainer>
      <VcForm class="mk-form tw-space-y-4">
        <VcSelect
          v-model="delimiter"
          :label="$t('MARKETING.PAGES.IMPORT.DELIMITER')"
          :placeholder="$t('MARKETING.PAGES.IMPORT.DELIMITER_PLACEHOLDER')"
          :options="delimiters"
          option-value="value"
          option-label="label"
          required
        />
        <VcFileUpload
          :label="$t('MARKETING.PAGES.IMPORT.FILE')"
          accept=".csv"
          :disabled="!delimiter"
          :loading="uploading"
          @upload="upload"
        />
        <VcField
          v-if="fileUrl"
          :label="$t('MARKETING.PAGES.IMPORT.UPLOADED')"
          :model-value="fileUrl"
        />
        <VcDatePicker
          v-model="expirationDate"
          :label="$t('MARKETING.PAGES.COUPON.FIELDS.EXPIRATION')"
          :hint="$t('MARKETING.PAGES.COUPON.FIELDS.EXPIRATION_HINT')"
          clearable
        />

        <VcCard
          v-if="progress"
          :header="$t('MARKETING.PAGES.IMPORT.PROGRESS')"
        >
          <div class="tw-p-4 tw-space-y-2 tw-text-sm">
            <div>{{ progress.description }}</div>
            <template v-if="progress.totalCount">
              <VcProgress :value="((progress.processedCount ?? 0) * 100) / progress.totalCount" />
              <div>
                {{ $t("MARKETING.PAGES.IMPORT.PROCESSED", { processed: progress.processedCount ?? 0, total: progress.totalCount }) }}
              </div>
            </template>
            <VcBanner
              v-if="progress.errors?.length"
              variant="danger"
              icon="lucide-circle-alert"
            >
              <div
                v-for="(error, i) in progress.errors"
                :key="i"
              >
                {{ error }}
              </div>
            </VcBanner>
            <VcStatus
              v-if="progress.finished"
              :variant="progress.errorCount ? 'warning' : 'success'"
              >{{ $t("MARKETING.PAGES.IMPORT.FINISHED") }}</VcStatus
            >
          </div>
        </VcCard>
      </VcForm>
    </VcContainer>
  </VcBlade>
</template>

<script setup lang="ts">
import { useApiClient, useBlade, useBladeNotifications, type IBladeToolbar } from "@vc-shell/framework";
import {
  VcBanner,
  VcBlade,
  VcCard,
  VcContainer,
  VcDatePicker,
  VcField,
  VcFileUpload,
  VcForm,
  VcProgress,
  VcSelect,
  VcStatus,
} from "@vc-shell/framework/ui";
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";
import { MarketingModulePromotionClient, type ImportNotification } from "../../api_client/virtocommerce.marketing";
import { uploadAsset } from "../lookups";

defineBlade({
  url: "/coupon-import",
  name: "MarketingCouponImport",
  permissions: ["marketing:update"],
});

const { t } = useI18n({ useScope: "global" });
const { param, callParent, setError, clearError } = useBlade();
const { getApiClient } = useApiClient(MarketingModulePromotionClient);

const delimiters = computed(() => [
  { value: " ", label: t("MARKETING.PAGES.IMPORT.DELIMITERS.SPACE") },
  { value: ",", label: t("MARKETING.PAGES.IMPORT.DELIMITERS.COMMA") },
  { value: ";", label: t("MARKETING.PAGES.IMPORT.DELIMITERS.SEMICOLON") },
  { value: "\t", label: t("MARKETING.PAGES.IMPORT.DELIMITERS.TAB") },
]);
const delimiter = ref<string>();
const expirationDate = ref<Date | string | null>(null);
const fileUrl = ref<string>();
const uploading = ref(false);
const progress = ref<ImportNotification>();
const running = computed(() => !!progress.value && !progress.value.finished);

// the CSV goes to the temporary assets folder first, as in the AngularJS blade
async function upload(files: FileList) {
  clearError();
  uploading.value = true;
  try {
    const asset = await uploadAsset(files[0], "tmp");
    fileUrl.value = asset.relativeUrl ?? asset.url;
  } catch (e) {
    setError(e);
  } finally {
    uploading.value = false;
  }
}

// progress arrives as CouponCsvImport push notifications
useBladeNotifications<ImportNotification & { notifyType?: string }>({
  types: ["CouponCsvImport"],
  filter: (msg) => msg.id === progress.value?.id,
  onMessage(msg) {
    progress.value = { ...progress.value, ...msg };
    if (msg.finished) callParent("reload");
  },
});

const bladeToolbar = ref<IBladeToolbar[]>([
  {
    id: "start",
    title: t("MARKETING.PAGES.IMPORT.START"),
    icon: "lucide-play",
    disabled: computed(() => !fileUrl.value || delimiter.value === undefined || running.value),
    async clickHandler() {
      clearError();
      try {
        const client = await getApiClient();
        progress.value = await client.importCoupons({
          fileUrl: fileUrl.value,
          delimiter: delimiter.value,
          promotionId: param.value,
          expirationDate: (expirationDate.value || undefined) as Date | undefined,
        });
      } catch (e) {
        setError(e);
      }
    },
  },
]);
</script>
