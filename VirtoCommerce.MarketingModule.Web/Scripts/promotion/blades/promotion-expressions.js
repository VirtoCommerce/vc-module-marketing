angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.promotionConditionCurrencyIsController', ['$scope', 'virtoCommerce.coreModule.currency.currencyUtils', function ($scope, currencyUtils) {
    $scope.currencyUtils = currencyUtils;
}])

    .controller('virtoCommerce.marketingModule.promotionExpressionsController', ['$scope', 'virtoCommerce.catalogModule.items', 'platformWebApp.authService', 'platformWebApp.bladeNavigationService', function ($scope, items, authService, bladeNavigationService) {

    $scope.openItemSelectWizard = function (parentElement, isMultiSelect) {
        if (!authService.checkPermission('marketing:update')) {
            return;
        }

        if (!isMultiSelect) {
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
        

        var openCatalogBlade = function (catalogConditionsBlade, parentBlade) {
            var catalogBlade = {
                id: "CatalogEntrySelect",
                title: "marketing.blades.catalog-items-select.title-product",
                controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
                template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
                breadcrumbs: [],
                toolbarCommands: [
                    {
                        name: "platform.commands.pick-selected", icon: 'fa fa-plus',
                        executeMethod: function (blade) {
                            blade.parentBlade.$scope.gridApi.grid.options.data = _.map(blade.parentBlade.selectedListEntries, function (entry) {
                                if (entry.imageUrl) {
                                    entry.imgSrc = entry.imageUrl;
                                }
                                return entry;
                            });
                            parentElement.productIds = parentBlade.selectedListEntries.map(function (entry) { return entry.id; });
                            catalogConditionsBlade.productIds = parentElement.productIds;
                            bladeNavigationService.closeBlade(blade);
                        },
                        canExecuteMethod: function () {
                            return true;
                        }
                    }]
            };

            catalogBlade.options = {
                selectedItemIds: catalogConditionsBlade.productIds,
                showCheckingMultiple: false,
                checkItemFn: function (listItem, isSelected) {
                    if (listItem.type == 'category') {
                        parentBlade.error = 'Must select Product';
                        listItem.selected = undefined;
                    } else {
                        if (isSelected) {
                            if (_.all(parentBlade.selectedListEntries, function (x) { return x.id != listItem.id; })) {
                                parentBlade.selectedListEntries.push(listItem);
                            }
                        }
                        else {
                            parentBlade.selectedListEntries = _.reject(parentBlade.selectedListEntries, function (x) { return x.id == listItem.id; });
                        }
                        parentBlade.error = undefined;
                    }
                }
            };

            bladeNavigationService.showBlade(catalogBlade, parentBlade);
        };

        var selectedListEntries = [];
        var newBlade = {
            id: "CatalogConditions",
            title: "marketing.blades.catalog-conditions.title-product",
            controller: 'virtoCommerce.marketingModule.catalogConditionsController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/promotion/blades/catalog-conditions.tpl.html',
            breadcrumbs: [],
            toolbarCommands: [
            {
                name: "platform.commands.refresh", icon: 'fa fa-refresh',
                executeMethod: function (blade) {
                    blade.refresh();
                },
                canExecuteMethod: function () { return true; }
            },
            {
                name: "platform.commands.add", icon: 'fa fa-plus',
                executeMethod: function (blade) {
                    openCatalogBlade(blade, newBlade);
                },
                canExecuteMethod: function () {
                    return true;
                }
            },
            {
                name: "platform.commands.delete", icon: 'fa fa-trash-o',
                executeMethod: function (blade) {
                    var grid = blade.$scope.gridApi.grid;
                    var selectedRows = grid.api.selection.getSelectedRows();
                    for (var entry of selectedRows) {
                        grid.options.data = _.filter(grid.options.data, function (item) {
                            return item.id != entry.id;
                        });
                    }

                    blade.selectedListEntries = grid.options.data.slice();
                    parentElement.productIds = _.map(grid.options.data, function (item) {
                        return item.id;
                    });
                    blade.productIds = parentElement.productIds.slice();
                },
                canExecuteMethod: function (blade) {
                    if (blade.childrenBlades.length > 0) {
                        return false;
                    }
                    if (blade.$scope.gridApi) {
                        return blade.$scope.gridApi.grid.api.selection.getSelectedRows().length > 0;
                    }
                    return false;
                }
            }]
        };

        newBlade.refresh = function () {
            newBlade.isLoading = true;
            items.query({ ids: newBlade.productIds, respGroup: 'ItemInfo' }, function (data) {
                newBlade.$scope.gridApi.grid.options.data = data;
                newBlade.selectedListEntries = newBlade.$scope.gridApi.grid.options.data.slice();
                newBlade.isLoading = false;
            });
        };

        newBlade.productIds = parentElement.productIds != undefined ? parentElement.productIds : [];
        newBlade.selectedListEntries = selectedListEntries;
        bladeNavigationService.showBlade(newBlade, $scope.blade);
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
                name: "platform.commands.pick-selected", icon: 'fa fa-plus',
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
