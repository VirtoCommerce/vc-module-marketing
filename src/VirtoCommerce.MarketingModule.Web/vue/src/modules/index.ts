import * as blades from "./pages";
import * as locales from "./locales";
import { defineAppModule } from "@vc-shell/framework";
import "./styles.css";

export default defineAppModule({
  blades,
  locales,
  // coupon CSV import progress (ImportNotification) as a progress toast
  notifications: {
    CouponCsvImport: { toast: { mode: "progress" } },
  },
});

export * from "./pages";
