// Helpers for the Banners screen
import { DynamicPropertyValueType, type DynamicContentItem } from "../api_client/virtocommerce.marketing";

const IMAGE_URL = /^(https?:)?\/\/|^\/|\.(png|jpe?g|gif|webp|svg|avif)(\?|$)/i;

// a banner's picture: its own imageUrl, else the first image-like value of an Image property (or one named "...image...")
export function imageOf(item?: DynamicContentItem): string | undefined {
  if (item?.imageUrl) return item.imageUrl;
  return (item?.dynamicProperties ?? [])
    .flatMap((p) => (p.values ?? []).filter((v) => v.valueType === DynamicPropertyValueType.Image || /image/i.test(p.name ?? "")))
    .map((v) => v.value)
    .find((v): v is string => typeof v === "string" && IMAGE_URL.test(v));
}
