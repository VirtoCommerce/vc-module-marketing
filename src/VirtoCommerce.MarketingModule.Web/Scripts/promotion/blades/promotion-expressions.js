angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.promotionConditionCurrencyIsController', ['$scope', 'virtoCommerce.coreModule.currency.currencyUtils', function ($scope, currencyUtils) {
    $scope.currencyUtils = currencyUtils;
}])

    .controller('virtoCommerce.marketingModule.promotionExpressionsController', ['$scope', 'virtoCommerce.catalogModule.items', 'platformWebApp.authService', 'platformWebApp.bladeNavigationService', function ($scope, items, authService, bladeNavigationService) {

    $scope.openItemSelectWizard = function (parentElement, isMultiSelect) {
        if (!authService.checkPermission('marketing:update')) {
            return;
        }

        if (isMultiSelect) {
            var newBlade = {
                id: "CatalogEntries",
                title: "marketing.blades.catalog-entries.title-product",
                controller: 'virtoCommerce.marketingModule.catalogEntriesController',
                template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/promotion/blades/catalog-entry-list.tpl.html',
                breadcrumbs: [],
                promotion: parentElement
            };
            bladeNavigationService.showBlade(newBlade, $scope.blade);
            return;
        }

        if (parentElement.productId) {
            let itemDetailBlade = {
                id: "listItemDetail",
                itemId: parentElement.productId,
                title: parentElement.productName,
                controller: 'virtoCommerce.catalogModule.itemDetailController',
                template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
            };
            bladeNavigationService.showBlade(itemDetailBlade, $scope.blade);
            return;
        }
        
        var selectedListEntries = [];
        var catalogBlade = {
            id: "CatalogEntrySelect",
            title: "marketing.blades.catalog-items-select.title-product",
            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            breadcrumbs: [],
            toolbarCommands: [
            {
                name: "platform.commands.pick-selected", icon: 'fas fa-plus',
                executeMethod: function (blade) {
                    parentElement.productId = selectedListEntries[0].id;
                    parentElement.productName = selectedListEntries[0].name;
                    parentElement.productCode = selectedListEntries[0].code;
                    bladeNavigationService.closeBlade(blade);
                },
                canExecuteMethod: function () {
                    return selectedListEntries.length == 1;
                }
            }]
        };

        catalogBlade.options = {
            showCheckingMultiple: false,
            checkItemFn: function (listItem, isSelected) {
                if (listItem.type == 'category') {
                    catalogBlade.error = 'Must select Product';
                    listItem.selected = undefined;
                } else {
                    if (isSelected) {
                        if (_.all(selectedListEntries, function (x) { return x.id != listItem.id; })) {
                            selectedListEntries.push(listItem);
                        }
                    }
                    else {
                        selectedListEntries = _.reject(selectedListEntries, function (x) { return x.id == listItem.id; });
                    }
                    catalogBlade.error = undefined;
                }
            }
        };

        bladeNavigationService.showBlade(catalogBlade, $scope.blade);
    };

    $scope.openCategorySelectWizard = function (parentElement) {
        if (!authService.checkPermission('marketing:update')) {
            return;
        }

        var selectedListEntries = [];
        var newBlade = {
            id: "CatalogCategorySelect",
            title: "marketing.blades.catalog-items-select.title-category",
            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            breadcrumbs: [],
            toolbarCommands: [
            {
                name: "platform.commands.pick-selected", icon: 'fas fa-plus',
                executeMethod: function (blade) {
                    parentElement.categoryId = selectedListEntries[0].id;
                    parentElement.categoryName = selectedListEntries[0].name;
                    bladeNavigationService.closeBlade(blade);
                },
                canExecuteMethod: function () {
                    return selectedListEntries.length == 1;
                }
            }]
        };

        newBlade.options = {
            showCheckingMultiple: false,
            allowCheckingItem: false,
            allowCheckingCategory: true,
            checkItemFn: function (listItem, isSelected) {
                if (listItem.type != 'category') {
                    newBlade.error = 'Must select Category';
                    listItem.selected = undefined;
                } else {
                    if (isSelected) {
                        if (_.all(selectedListEntries, function (x) { return x.id != listItem.id; })) {
                            selectedListEntries.push(listItem);
                        }
                    }
                    else {
                        selectedListEntries = _.reject(selectedListEntries, function (x) { return x.id == listItem.id; });
                    }
                    newBlade.error = undefined;
                }
            }
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
}]);
