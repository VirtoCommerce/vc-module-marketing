//Call this to register our module to main application
var moduleName = 'virtoCommerce.marketingModule';

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(
        ['$stateProvider', function ($stateProvider) {
            $stateProvider
                .state('workspace.marketing', {
                    url: '/marketing',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                            var blade = {
                                id: 'marketing',
                                title: 'marketing.blades.marketing-main.title',
                                controller: 'virtoCommerce.marketingModule.marketingMainController',
                                template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/common/marketing-main.tpl.html',
                                isClosingDisabled: true
                            };
                            bladeNavigationService.showBlade(blade);
                        }
                    ]
                });
        }])


    .factory('virtoCommerce.marketingModule.marketingMenuItemService', function () {
        return {
            items: [],
            register: function (item) {
                this.items.push(item);
            }
        };
    })

    .run(['platformWebApp.dynamicTemplateService', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', 'platformWebApp.toolbarService', 'platformWebApp.breadcrumbHistoryService', '$state', 'platformWebApp.authService', 'virtoCommerce.storeModule.stores', 'platformWebApp.permissionScopeResolver', 'platformWebApp.bladeNavigationService', 'virtoCommerce.coreModule.common.dynamicExpressionService', 'virtoCommerce.marketingModule.marketingMenuItemService', 'platformWebApp.moduleHelper'
        , function (dynamicTemplateService, mainMenuService, widgetService, toolbarService, breadcrumbHistoryService, $state, authService, stores, permissionScopeResolver, bladeNavigationService, dynamicExpressionService, marketingMenuItemService, moduleHelper) {
            //Register module in main menu
            var menuItem = {
                path: 'browse/marketing',
                icon: 'fa fa-flag',
                title: 'marketing.main-menu-title',
                priority: 40,
                action: function () { $state.go('workspace.marketing'); },
                permission: 'marketing:access'
            };
            mainMenuService.addMenuItem(menuItem);

            // Register marketing main blade items
            marketingMenuItemService.register({ id: '1', name: 'marketing.blades.marketing-main.labels.promotions', entityName: 'promotion', icon: 'fa-area-chart' });
            marketingMenuItemService.register({ id: '2', name: 'marketing.blades.marketing-main.labels.dynamic-content', entityName: 'dynamicContent', icon: 'fa-calendar-o' });

            // register back-button
            toolbarService.register(breadcrumbHistoryService.getBackButtonInstance(), 'virtoCommerce.marketingModule.itemsDynamicContentListController');
            toolbarService.register(breadcrumbHistoryService.getBackButtonInstance(), 'virtoCommerce.marketingModule.placeholdersDynamicContentListController');
            toolbarService.register(breadcrumbHistoryService.getBackButtonInstance(), 'virtoCommerce.marketingModule.addPublishingContentItemsStepController');
            toolbarService.register(breadcrumbHistoryService.getBackButtonInstance(), 'virtoCommerce.marketingModule.addPublishingPlaceholdersStepController');

            widgetService.registerWidget({
                controller: 'virtoCommerce.marketingModule.couponsWidgetController',
                template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/promotion/widgets/couponsWidget.tpl.html'
            }, 'promotionDetail');

            widgetService.registerWidget({
                controller: 'virtoCommerce.marketingModule.promotionUsageHistoryWidgetController',
                template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/promotion/widgets/promotionUsageHistoryWidget.tpl.html',
                isVisible: function (blade) {
                    return moduleHelper.isModuleInstalled('VirtoCommerce.Orders') &&
                           !blade.isNew &&
                           blade.currentEntity &&
                           blade.currentEntity.id;
                }
            }, 'promotionDetail');

            //Register permission scopes templates used for scope bounded definition in role management ui
            var marketingStoreScope = {
                type: 'MarketingStoreSelectedScope',
                title: 'Only for promotions in selected stores',
                selectFn: function (blade, callback) {
                    var newBlade = {
                        id: 'store-pick',
                        title: this.title,
                        subtitle: 'Select stores',
                        currentEntity: this,
                        onChangesConfirmedFn: callback,
                        dataService: stores,
                        controller: 'platformWebApp.security.scopeValuePickFromSimpleListController',
                        template: '$(Platform)/Scripts/app/security/blades/common/scope-value-pick-from-simple-list.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                }
            };
            permissionScopeResolver.register(marketingStoreScope);

            // PROMOTIONS
            // Customer conditions
            dynamicExpressionService.registerExpression({
                id: 'BlockCustomerCondition',
                newChildLabel: 'Add user group',
                getValidationError: function () {
                    return (this.children && this.children.length) ? undefined : 'Your promotion must have at least one eligibility criterion';
                },
            });
            dynamicExpressionService.registerExpression({
                id: 'ConditionIsEveryone',
                displayName: 'Everyone',
            });
            dynamicExpressionService.registerExpression({
                id: 'ConditionIsFirstTimeBuyer',
                displayName: 'First time customers',
            });
            dynamicExpressionService.registerExpression({
                id: 'ConditionIsRegisteredUser',
                displayName: 'Registered users',
            });

            // Exclude category or product
            dynamicExpressionService.registerExpression({
                id: 'ExcludingCategoryCondition',
                displayName: 'Items from a specific category',
            });
            dynamicExpressionService.registerExpression({
                id: 'ExcludingProductCondition',
                displayName: 'Product items',
            });
            var availableExcludings = [
                { id: 'ExcludingCategoryCondition' },
                { id: 'ExcludingProductCondition' },
            ];

            // Catalog conditions
            dynamicExpressionService.registerExpression({
                id: 'BlockCatalogCondition',
                newChildLabel: 'Add condition',
            });
            dynamicExpressionService.registerExpression({
                id: 'ConditionCategoryIs',
                displayName: 'Specific category',
                availableChildren: availableExcludings,
                newChildLabel: 'Excluding',
            });
            dynamicExpressionService.registerExpression({
                id: 'ConditionEntryIs',
                displayName: 'Specific product',
            });
            dynamicExpressionService.registerExpression({
                id: 'ConditionCodeContains',
                displayName: 'Product code contains...',
            });
            dynamicExpressionService.registerExpression({
                id: 'ConditionCurrencyIs',
                displayName: 'Currency is...',
            });
            dynamicExpressionService.registerExpression({
                id: 'ConditionInStockQuantity',
                displayName: 'In stock quantity is...',
            });
            dynamicExpressionService.registerExpression({
                id: 'ConditionHasNoSalePrice',
                displayName: 'Apply only to full price items and not sales items',
            });

            // Cart conditions
            dynamicExpressionService.registerExpression({
                id: 'BlockCartCondition',
                newChildLabel: 'Add condition',
            });
            dynamicExpressionService.registerExpression({
                id: 'ConditionAtNumItemsInCart',
                displayName: 'Number of items in the shopping cart',
                newChildLabel: 'Excluding',
                availableChildren: availableExcludings,
            });
            dynamicExpressionService.registerExpression({
                id: 'ConditionAtNumItemsInCategoryAreInCart',
                displayName: 'Number of items out of a category in the shopping cart',
                newChildLabel: 'Excluding',
                availableChildren: availableExcludings,
            });
            dynamicExpressionService.registerExpression({
                id: 'ConditionAtNumItemsOfEntryAreInCart',
                displayName: 'Number of specific product items in the shopping cart',
            });
            dynamicExpressionService.registerExpression({
                id: 'PaymentIsCondition',
                displayName: 'Payment type is...',
            });
            dynamicExpressionService.registerExpression({
                id: 'ShippingIsCondition',
                displayName: 'Shipping type is...',
            });
            dynamicExpressionService.registerExpression({
                id: 'ConditionCartSubtotalLeast',
                displayName: 'Cart subtotal is...',
                newChildLabel: 'Excluding',
                availableChildren: availableExcludings,
            });

            // Rewards
            dynamicExpressionService.registerExpression({
                id: 'BlockReward',
                newChildLabel: 'Add reward',
                getValidationError: function () {
                    return (this.children && this.children.length) ? undefined : 'Your promotion must have at least one reward';
                },
            });
            dynamicExpressionService.registerExpression({
                id: 'RewardCartGetOfAbsSubtotal',
                displayName: '$... off cart subtotal',
            });
            dynamicExpressionService.registerExpression({
                id: 'RewardCartGetOfRelSubtotal',
                displayName: '...% off cart subtotal, no more than $...',
            });
            dynamicExpressionService.registerExpression({
                id: 'RewardItemGetFreeNumItemOfProduct',
                displayName: '... free items of ... product',
            });
            dynamicExpressionService.registerExpression({
                id: 'RewardItemGetOfAbs',
                displayName: '$... off',
            });
            dynamicExpressionService.registerExpression({
                id: 'RewardItemGetOfAbsForNum',
                displayName: '$... off for ... specific product items',
            });
            dynamicExpressionService.registerExpression({
                id: 'RewardItemGetOfRel',
                displayName: '...% off for product ..., no more than $...',
            });
            dynamicExpressionService.registerExpression({
                id: 'RewardItemGetOfRelForNum',
                displayName: '...% off for ... specific product items, no more than $...',
            });
            dynamicExpressionService.registerExpression({
                id: 'RewardItemGiftNumItem',
                displayName: '... items of ... product as a gift',
            });
            dynamicExpressionService.registerExpression({
                id: 'RewardShippingGetOfAbsShippingMethod',
                displayName: '$... off for shipping at ...',
            });
            dynamicExpressionService.registerExpression({
                id: 'RewardShippingGetOfRelShippingMethod',
                displayName: '% off for shipping at ..., no more than $...',
            });
            dynamicExpressionService.registerExpression({
                id: 'RewardPaymentGetOfAbs',
                displayName: '$... off for using ... payment method',
            });
            dynamicExpressionService.registerExpression({
                id: 'RewardPaymentGetOfRel',
                displayName: '...% off for using ... payment method, no more than $...',
            });
            dynamicExpressionService.registerExpression({
                id: 'RewardItemForEveryNumInGetOfRel',
                displayName: '...% off for ... of every ... specific product items',
            });
            dynamicExpressionService.registerExpression({
                id: 'RewardItemForEveryNumOtherItemInGetOfRel',
                displayName: '...% off for ... items of a specific product per every ... items of another product',
            });

            // DYNAMIC CONTENT
            dynamicExpressionService.registerExpression({
                id: 'BlockContentCondition',
                newChildLabel: 'Add condition'
            });
            dynamicExpressionService.registerExpression({
                id: 'DynamicContentConditionCategoryIs',
                displayName: 'Specific category',
                templateURL: 'expression-ConditionCategoryIs.html',
                groupName: 'Catalog',
            });
            dynamicExpressionService.registerExpression({
                id: 'DynamicContentConditionProductIs',
                displayName: 'Specific product',
                templateURL: 'expression-ConditionEntryIs.html',
                groupName: 'Catalog',
            });

            dynamicTemplateService.ensureTemplateLoaded('Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicConditions/templates.html');
        }]);
