import { ref, watch } from "vue";
import { PromotionStatus } from "../api_client/virtocommerce.marketing";

// Saved promotion filters of the AngularJS list ($localStorage.promotionSearchFilters), shared by the list and filter blades.
// Stored in this browser only, as the AngularJS filters were (same pattern as the Orders plugin).
export interface SavedPromotionFilter {
  id: string;
  name: string;
  status?: PromotionStatus;
  storeIds?: string[];
  predefined?: boolean;
}

const KEY = "vc-marketing.promotionFilters";
const CURRENT_KEY = "vc-marketing.promotionFilterId";

function read<T>(key: string, fallback: T): T {
  try {
    return (JSON.parse(localStorage.getItem(key) ?? "") as T) ?? fallback;
  } catch {
    return fallback;
  }
}

const filters = ref<SavedPromotionFilter[]>(read(KEY, []));
const currentId = ref<string | undefined>(read<string | undefined>(CURRENT_KEY, undefined));
watch(filters, (v) => localStorage.setItem(KEY, JSON.stringify(v)), { deep: true });
watch(currentId, (v) => (v ? localStorage.setItem(CURRENT_KEY, JSON.stringify(v)) : localStorage.removeItem(CURRENT_KEY)));

// the statuses a promotion can be filtered by
export const PROMOTION_STATUSES = [PromotionStatus.Active, PromotionStatus.Upcoming, PromotionStatus.Archived, PromotionStatus.Deactivated];

// the AngularJS predefined filters: one per promotion status
export const predefinedFilters = (): SavedPromotionFilter[] =>
  [...PROMOTION_STATUSES, PromotionStatus.All].map(
    (status) => ({ id: status.toLowerCase(), name: status, status, predefined: true }),
  );

export function useSavedFilters() {
  const find = (id?: string) => (id ? [...predefinedFilters(), ...filters.value].find((f) => f.id === id) : undefined);
  return { filters, currentId, find };
}
