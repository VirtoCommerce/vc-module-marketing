// Small typed fetches to other modules' / platform APIs used by the marketing pickers
// (same-origin cookie auth, the way the generated clients authenticate in the host).
import { ref } from "vue";

export interface Option {
  id: string;
  name: string;
  code?: string;
}
export interface Page<T = Option> {
  results: T[];
  totalCount: number;
}

export async function http<T>(url: string, body?: unknown, method = body === undefined ? "GET" : "POST"): Promise<T> {
  const res = await fetch(url, {
    method,
    headers: body === undefined ? { Accept: "application/json" } : { "Content-Type": "application/json", Accept: "application/json" },
    body: body === undefined ? undefined : JSON.stringify(body),
  });
  if (!res.ok) throw new Error(`${res.status} ${res.statusText}: ${url}`);
  const text = await res.text();
  return (text ? JSON.parse(text) : undefined) as T;
}

const qs = (name: string, values: string[]) => values.map((v) => `${name}=${encodeURIComponent(v)}`).join("&");

// caches a parameterless loader for the session (methods, currencies, settings...)
function once<T>(load: () => Promise<T>): () => Promise<T> {
  let p: Promise<T> | undefined;
  return () =>
    (p ??= load().catch((e) => {
      p = undefined;
      return Promise.reject(e);
    }));
}

const PAGE = 20;

type CatalogEntry = { id: string; name: string; code?: string };
const toOption = (x: CatalogEntry): Option => ({ id: x.id, name: x.name, code: x.code });

export async function searchProducts(keyword?: string, skip = 0, ids?: string[]): Promise<Page> {
  if (ids?.length && !keyword) {
    const items = await http<CatalogEntry[]>(`/api/catalog/products?${qs("ids", ids)}&respGroup=ItemInfo`);
    return { results: (items ?? []).map(toOption), totalCount: items?.length ?? 0 };
  }
  const r = await http<{ items?: CatalogEntry[]; totalCount?: number }>("/api/catalog/search/products", {
    keyword: keyword || undefined,
    skip,
    take: PAGE,
    responseGroup: "ItemInfo",
  });
  return { results: (r.items ?? []).map(toOption), totalCount: r.totalCount ?? 0 };
}

export async function searchCategories(keyword?: string, skip = 0, ids?: string[]): Promise<Page> {
  if (ids?.length && !keyword) {
    const items = await http<CatalogEntry[]>(`/api/catalog/categories?${qs("ids", ids)}&respGroup=Info`);
    return { results: (items ?? []).map(toOption), totalCount: items?.length ?? 0 };
  }
  const r = await http<{ items?: CatalogEntry[]; totalCount?: number }>("/api/catalog/search/categories", {
    keyword: keyword || undefined,
    skip,
    take: PAGE,
  });
  return { results: (r.items ?? []).map(toOption), totalCount: r.totalCount ?? 0 };
}

export async function searchStores(keyword?: string, skip = 0, ids?: string[]): Promise<Page> {
  const r = await http<{ results?: Option[]; totalCount?: number }>("/api/stores/search", {
    keyword: keyword || undefined,
    storeIds: ids?.length ? ids : undefined,
    skip,
    take: ids?.length ? ids.length : PAGE,
    responseGroup: "None",
  });
  return { results: (r.results ?? []).map((s) => ({ id: s.id, name: s.name || s.id })), totalCount: r.totalCount ?? 0 };
}

export async function searchMembers(keyword?: string, skip = 0, ids?: string[]): Promise<Page> {
  const r = await http<{ results?: Option[]; totalCount?: number }>("/api/members/search", {
    keyword: keyword || undefined,
    objectIds: ids?.length ? ids : undefined,
    memberTypes: ["Organization", "Contact"],
    deepSearch: true,
    skip,
    take: ids?.length ? ids.length : PAGE,
    responseGroup: "Info",
  });
  return { results: (r.results ?? []).map((m) => ({ id: m.id, name: m.name || m.id })), totalCount: r.totalCount ?? 0 };
}

// orders that used a promotion (the AngularJS "Usage history" widget); Orders is an optional module
export interface PromotionOrder {
  id: string;
  number?: string;
  customerName?: string;
  storeId?: string;
  status?: string;
  createdDate?: string;
  total?: number;
  currency?: string;
}
export function searchOrdersByPromotion(promotionId: string, skip = 0, take = PAGE, sort?: string) {
  return http<{ results?: PromotionOrder[]; totalCount?: number }>("/api/order/customerOrders/search", {
    promotionIds: [promotionId],
    responseGroup: "Default",
    sort,
    skip,
    take,
  });
}

export const settingValues = (name: string) => http<unknown[]>(`/api/platform/settings/values/${encodeURIComponent(name)}`);

// uploads one file to a platform assets folder (POST api/assets?folderUrl=)
export async function uploadAsset(file: File, folderUrl: string) {
  const body = new FormData();
  body.append("file", file);
  const res = await fetch(`/api/assets?folderUrl=${encodeURIComponent(folderUrl)}`, { method: "POST", body });
  if (!res.ok) throw new Error(`${file.name}: ${res.status} ${res.statusText}`);
  const [asset] = (await res.json()) as { relativeUrl?: string; url?: string }[];
  return asset ?? {};
}

const codes = (list: { code?: string; name?: string }[] | undefined): Option[] => {
  const seen = new Set<string>();
  return (list ?? [])
    .filter((x) => x.code && !seen.has(x.code) && seen.add(x.code))
    .map((x) => ({ id: x.code!, name: x.code! }));
};

// lists used by the expression editor's dropdowns
export const lookups = {
  currencies: once(() => http<{ code?: string; name?: string }[]>("/api/currencies").then(codes)),
  shippingMethods: once(() => http<{ code?: string }[]>("/api/shipping").then(codes)),
  paymentMethods: once(() => http<{ code?: string }[]>("/api/payment").then(codes)),
  languages: once(() => settingValues("VirtoCommerce.Core.General.Languages").then((v) => v.map((x) => ({ id: String(x), name: String(x) })))),
  memberGroups: once(() => settingValues("Customer.MemberGroups").then((v) => v.map((x) => ({ id: String(x), name: String(x) })))),
  countries: once(() => http<Option[]>("/api/platform/common/countries").then((v) => v.map((c) => ({ id: c.id, name: c.name })))),
  // IANA ids, the same list the AngularJS editor got from moment-timezone
  timeZones: once(async () =>
    (Intl as unknown as { supportedValuesOf(key: string): string[] }).supportedValuesOf("timeZone").map((id) => ({ id, name: id })),
  ),
};
export type LookupSource = keyof typeof lookups;

// the language of localized fields, edited one at a time: en-US when the platform has it, else its first language
export function useLanguagePicker() {
  const languages = ref<Option[]>([]);
  const language = ref("en-US");
  lookups.languages().then((l) => {
    languages.value = l;
    if (l.length && !l.some((x) => x.id === language.value)) language.value = l[0].id;
  });
  return { languages, language };
}
