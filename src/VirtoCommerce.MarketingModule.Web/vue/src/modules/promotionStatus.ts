import { PromotionStatus, type Promotion } from "../api_client/virtocommerce.marketing";

// The statuses the promotion search filters by, in the order the filter lists them
export const PROMOTION_STATUSES = [PromotionStatus.Active, PromotionStatus.Upcoming, PromotionStatus.Archived, PromotionStatus.Deactivated];

export type PromotionStatusVariant = "success" | "info" | "info-dark" | "warning";

const VARIANTS: Record<string, PromotionStatusVariant> = {
  [PromotionStatus.Active]: "success",
  [PromotionStatus.Upcoming]: "info",
  [PromotionStatus.Archived]: "info-dark",
  [PromotionStatus.Deactivated]: "warning",
};

/** The status the promotion search filters by: switched off, not started yet, ended, or running now */
export function promotionStatus(promotion: Pick<Promotion, "isActive" | "startDate" | "endDate">, now = Date.now()): PromotionStatus {
  if (!promotion.isActive) return PromotionStatus.Deactivated;
  if (promotion.startDate && new Date(promotion.startDate).getTime() > now) return PromotionStatus.Upcoming;
  if (promotion.endDate && new Date(promotion.endDate).getTime() < now) return PromotionStatus.Archived;
  return PromotionStatus.Active;
}

/** The localization key of a status (MARKETING.PROMOTION_STATUS.*), shared by the filter and the column */
export const promotionStatusKey = (status: PromotionStatus) => `MARKETING.PROMOTION_STATUS.${status.toUpperCase()}`;

export const promotionStatusVariant = (status: PromotionStatus): PromotionStatusVariant => VARIANTS[status] ?? "info";
