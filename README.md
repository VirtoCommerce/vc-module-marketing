# Virto Commerce Marketing Module

[![CI status](https://github.com/VirtoCommerce/vc-module-marketing/workflows/Module%20CI/badge.svg?branch=dev)](https://github.com/VirtoCommerce/vc-module-marketing/actions?query=workflow%3A"Module+CI") [![Quality gate](https://sonarcloud.io/api/project_badges/measure?project=VirtoCommerce_vc-module-marketing&metric=alert_status&branch=dev)](https://sonarcloud.io/dashboard?id=VirtoCommerce_vc-module-marketing) [![Reliability rating](https://sonarcloud.io/api/project_badges/measure?project=VirtoCommerce_vc-module-marketing&metric=reliability_rating&branch=dev)](https://sonarcloud.io/dashboard?id=VirtoCommerce_vc-module-marketing) [![Security rating](https://sonarcloud.io/api/project_badges/measure?project=VirtoCommerce_vc-module-marketing&metric=security_rating&branch=dev)](https://sonarcloud.io/dashboard?id=VirtoCommerce_vc-module-marketing) [![Sqale rating](https://sonarcloud.io/api/project_badges/measure?project=VirtoCommerce_vc-module-marketing&metric=sqale_rating&branch=dev)](https://sonarcloud.io/dashboard?id=VirtoCommerce_vc-module-marketing)

## Overview

The Virto Commerce Marketing Module provides **promotions** and **dynamic content** capabilities for the Virto Commerce Platform.

### Architecture

The module implements a domain-driven structure where:
- **Promotions** define marketing rules (conditions + rewards) evaluated during cart/order processing.
- **Dynamic content** provides content items, placeholders, and publications for storefront personalization.
- **Admin UI** enables day-to-day operations (create/edit/search/filter).
- **Optional integrations** are enabled when dependent modules are installed (e.g., Orders module).

### Core principles

- **Rule-based promotions**: conditions and rewards are modeled as expression trees to support extensibility.
- **Store scope**: promotions can be targeted to specific store(s) (or globally when no stores are assigned).
- **Optional dependencies**: some features light up only when related modules are present (e.g., Orders).

## Key Features

### Promotions

* **Promotion lifecycle**: create, edit, clone, delete, enable/disable
* **Coupons support**: manage promotion coupons and coupon usages
* **Search and filtering**:
  - keyword search
  - saved filters (stored in client storage)
  - predefined status filters: **All Promotions**, **Active**, **Upcoming**, **Archived**, **Deactivated**
* **Promotion usage visibility (optional Orders integration)**:
  - “Usage History” widget shows the number of orders that used the promotion
  - opens an Orders blade filtered by the current promotion
  - shown only when `VirtoCommerce.Orders` module is installed

### Dynamic content

* **Dynamic content items**: create and manage content items and folders
* **Placeholders**: manage placeholders that define content slots in storefront pages
* **Publications**: publish content to placeholders with time windows and conditions

### Security and permissions

* **Role-based access control**:
  - `marketing:access`, `marketing:read`, `marketing:create`, `marketing:update`, `marketing:delete`
* **Scope-based access**: permission scope supports restricting access by Store

## Configuration

Navigate to **Settings → Marketing** in the Admin Portal (or manage settings via platform settings API).

### General

| Setting | Default | Description |
|---------|---------|-------------|
| `Marketing.Promotion.CombinePolicy` | `BestReward` | Promotion evaluation policy: `BestReward` or `CombineStackable` |

### Optional dependencies

The module declares optional dependencies in `src/VirtoCommerce.MarketingModule.Web/module.manifest`, including:

* `VirtoCommerce.Orders` (optional): enables coupon usage recording from order events and the “Usage History” widget in promotion detail

## Architecture

### Project Structure

```
vc-module-marketing/
├── src/
│   ├── VirtoCommerce.MarketingModule.Core/         # Domain models, interfaces, constants
│   ├── VirtoCommerce.MarketingModule.Data/         # Data access, services, search, export/import
│   ├── VirtoCommerce.MarketingModule.Data.SqlServer/   # SQL Server migrations
│   ├── VirtoCommerce.MarketingModule.Data.PostgreSql/  # PostgreSQL migrations
│   ├── VirtoCommerce.MarketingModule.Data.MySql/       # MySQL migrations
│   └── VirtoCommerce.MarketingModule.Web/          # Web API, module init, Admin UI assets
├── tests/
│   └── VirtoCommerce.MarketingModule.Test/         # Unit and integration tests
└── samples/
    └── VirtoCommerce.MarketingSampleModule.Web/    # Sample implementations
```

### Key components

- **PromotionSearchService / PromotionService**: promotion CRUD and search/filtering
- **Promotion evaluation policies**: `BestReward` and `CombineStackable` (controlled by `Marketing.Promotion.CombinePolicy`)
- **Dynamic content services**: folders, items, places (placeholders), publications
- **Export/Import**: module export/import implementation for marketing entities

### Domain Model

```
Promotion
├── Conditions (expression tree)
├── Rewards (expression tree)
├── Stores[] (optional scope)
├── Coupons[] (optional)
└── Usages[] (optional; tracked when Orders module is installed)

Dynamic content
├── DynamicContentFolder[]
├── DynamicContentItem[]
├── DynamicContentPlace[] (placeholders)
└── DynamicContentPublication[]
    ├── ContentPlaces[]
    └── ContentItems[]
```

### Key user scenarios

#### Promotions: create and manage

* Open **Marketing → Promotions**
* Create a new promotion (define conditions/rewards, enable/disable, assign stores, optionally add coupons)
* Use the context menu to **Manage / Copy ID / Clone / Delete**

#### Promotions: quickly filter by status

* Use predefined filters:
  * **Active**: active right now
  * **Upcoming**: starts in the future
  * **Archived**: ended in the past
  * **Deactivated**: disabled promotions
  * **All Promotions**: no status restriction
* Save custom filters (for example: “Active in Store A/B”)

#### Promotions: view usage history (requires Orders module)

* Open a promotion detail blade
* If **Orders** is installed, the **Usage History** widget shows how many orders used this promotion
* Click it to open Orders list filtered by the promotion, with paging/sorting

#### Dynamic content: publish marketing content

* Use placeholders to control where content renders on the storefront
* Create dynamic content items and organize them in folders
* Create publications and configure publishing windows

## Documentation

* [Marketing module user documentation](https://docs.virtocommerce.org/platform/user-guide/marketing/overview/)
* [REST API](https://virtostart-demo-admin.govirto.com/docs/index.html?urls.primaryName=VirtoCommerce.Marketing)
* [View on GitHub](https://github.com/VirtoCommerce/vc-module-marketing)
* [Developer guide](https://docs.virtocommerce.org/platform/developer-guide/Extensibility/extending-dynamic-expression-tree/#defining-new-class-for-expression-tree-prototype)

## Available resources

* Module related service implementations as a <a href="https://www.nuget.org/packages/VirtoCommerce.MarketingModule.Data" target="_blank">NuGet package</a>
* API client as a <a href="https://www.nuget.org/packages/VirtoCommerce.MarketingModule.Client" target="_blank">NuGet package</a>

## References

* [Deployment](https://docs.virtocommerce.org/platform/developer-guide/Tutorials-and-How-tos/Tutorials/deploy-module-from-source-code/)
* [Installation](https://docs.virtocommerce.org/platform/user-guide/modules-installation/)
* [Home](https://virtocommerce.com)
* [Community](https://www.virtocommerce.org)
* [Download latest release](https://github.com/VirtoCommerce/vc-module-marketing/releases/latest)

## License

Copyright (c) Virto Solutions LTD. All rights reserved.

Licensed under the Virto Commerce Open Software License (the "License"); you
may not use this file except in compliance with the License. You may
obtain a copy of the License at

http://virtocommerce.com/opensourcelicense

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
implied.
