<template>
  <VcBlade
    :title="coupon.code || $t('MARKETING.PAGES.COUPON.NEW_TITLE')"
    :toolbar-items="bladeToolbar"
    :loading="loading"
    width="50%"
  >
    <VcContainer>
      <VcForm class="mk-form tw-space-y-4">
        <VcRow class="tw-gap-4">
          <VcCol>
            <VcInput
              v-model="coupon.code"
              :label="$t('MARKETING.PAGES.COUPON.FIELDS.CODE')"
              :placeholder="$t('MARKETING.PAGES.COUPON.FIELDS.CODE_PLACEHOLDER')"
              :hint="$t('MARKETING.PAGES.COUPON.FIELDS.CODE_HINT')"
              :error-message="codeError"
              required
            />
          </VcCol>
          <VcCol>
            <VcDatePicker
              v-model="coupon.expirationDate"
              :label="$t('MARKETING.PAGES.COUPON.FIELDS.EXPIRATION')"
              :hint="$t('MARKETING.PAGES.COUPON.FIELDS.EXPIRATION_HINT')"
              clearable
            />
          </VcCol>
        </VcRow>
        <VcRow class="tw-gap-4">
          <VcCol>
            <VcInput
              v-model="coupon.maxUsesNumber"
              type="integer"
              :label="$t('MARKETING.PAGES.COUPON.FIELDS.MAX_USES')"
              :hint="$t('MARKETING.PAGES.COUPON.FIELDS.MAX_USES_HINT')"
              required
            />
          </VcCol>
          <VcCol>
            <VcInput
              v-model="coupon.maxUsesPerUser"
              type="integer"
              :label="$t('MARKETING.PAGES.COUPON.FIELDS.MAX_PER_USER')"
              :hint="$t('MARKETING.PAGES.COUPON.FIELDS.MAX_PER_USER_HINT')"
              required
            />
          </VcCol>
        </VcRow>
        <VcRow class="tw-gap-4">
          <VcCol>
            <VcSelect
              v-model="coupon.memberId"
              :label="$t('MARKETING.PAGES.COUPON.FIELDS.MEMBER')"
              :placeholder="$t('MARKETING.PAGES.COUPON.FIELDS.MEMBER_PLACEHOLDER')"
              :options="searchMembers"
              option-value="id"
              option-label="name"
              searchable
            />
          </VcCol>
          <VcCol>
            <VcField
              v-if="param"
              :label="$t('MARKETING.PAGES.COUPONS.COLUMNS.USED')"
              :model-value="String(coupon.totalUsesCount ?? 0)"
            />
          </VcCol>
        </VcRow>
      </VcForm>
    </VcContainer>
  </VcBlade>
</template>

<script setup lang="ts">
import { useApiClient, useAsync, useBlade, useBladeForm, useLoading, type IBladeToolbar } from "@vc-shell/framework";
import { VcBlade, VcCol, VcContainer, VcDatePicker, VcField, VcForm, VcInput, VcRow, VcSelect } from "@vc-shell/framework/ui";
import { computed, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";
import { MarketingModulePromotionClient, type Coupon } from "../../api_client/virtocommerce.marketing";
import { searchMembers, settingValues } from "../lookups";

defineBlade({
  url: "/coupon",
  name: "MarketingCouponDetails",
  permissions: ["marketing:access"],
});

const { t } = useI18n({ useScope: "global" });
const { param, options, callParent, closeSelf } = useBlade<{ promotionId: string }>();
const { getApiClient } = useApiClient(MarketingModulePromotionClient);

const coupon = ref<Coupon>({ promotionId: options.value?.promotionId, maxUsesNumber: 0, maxUsesPerUser: 0 });

// Marketing.Coupon.CodeValidationPattern; an empty or invalid pattern leaves validation to the server
const pattern = ref<RegExp>();
settingValues("Marketing.Coupon.CodeValidationPattern").then((v) => {
  try {
    if (v?.[0]) pattern.value = new RegExp(String(v[0]));
  } catch {
    /* invalid pattern: server-side validation only */
  }
});
const codeError = computed(() =>
  coupon.value.code && pattern.value && !pattern.value.test(coupon.value.code) ? t("MARKETING.VALIDATION.CODE_PATTERN") : undefined,
);

const { loading: itemLoading, action: load } = useAsync(async () => {
  if (!param.value) return;
  const client = await getApiClient();
  coupon.value = await client.getCoupon(param.value);
});
const { loading: saveLoading, action: save } = useAsync(async () => {
  const client = await getApiClient();
  await client.addCoupons([coupon.value]);
});
const loading = useLoading(itemLoading, saveLoading);

const form = useBladeForm({
  data: coupon,
  canSaveOverride: computed(
    () => !!coupon.value.code?.trim() && !codeError.value && coupon.value.maxUsesNumber != null && coupon.value.maxUsesPerUser != null,
  ),
  closeConfirmMessage: computed(() => t("MARKETING.ALERTS.CLOSE_CONFIRMATION")),
});

const bladeToolbar = ref<IBladeToolbar[]>([
  {
    id: "save",
    title: computed(() => t(param.value ? "MARKETING.ACTIONS.SAVE" : "MARKETING.ACTIONS.CREATE")),
    icon: "lucide-save",
    permissions: ["marketing:update"],
    disabled: computed(() => !form.canSave.value),
    async clickHandler() {
      await save();
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
