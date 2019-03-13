angular.module('virtoCommerce.marketingModule')
    .controller('virtoCommerce.marketingModule.catalogEntriesController', ['$scope', 'virtoCommerce.catalogModule.items', 'platformWebApp.ui-grid.extension', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper', 'platformWebApp.bladeNavigationService',
        function ($scope, items, gridOptionExtension, bladeUtils, uiGridHelper, bladeNavigationService) {
            var blade = $scope.blade;

            $scope.isEntityChanged = function () {
                return angular.equals(this.blade.productIds, this.blade.promotion.productIds);
            };

            $scope.saveChanges = function () {
                this.blade.promotion.productIds = this.blade.productIds;
                bladeNavigationService.closeBlade(this.blade);
            };

            $scope.setGridOptions = function (gridId, gridOptions) {
                gridOptionExtension.tryExtendGridOptions(gridId, gridOptions);
                initialize();
                uiGridHelper.initialize($scope, gridOptions, externalRegisterApiCallback);
                bladeUtils.initializePagination($scope);
            };

            blade.refresh = function () {
                initialize();
            };

            $scope.cancel = function () {
                initialize();
            };

            var selectedListEntries = [];
            function initialize() {
                if (!blade.promotion.productIds)
                    blade.promotion.productIds = [];

                blade.productIds = blade.promotion.productIds.slice();
                items.query({ ids: blade.productIds, respGroup: 'ItemInfo' }, function (data) {
                    blade.$scope.gridApi.grid.options.data = data;
                    selectedListEntries = angular.copy(data);
                    blade.isLoading = false;
                });
            }

            function externalRegisterApiCallback(gridApi) {
                $scope.gridApi = gridApi;
                $scope.$parent.blade.refresh();
                uiGridHelper.bindRefreshOnSortChanged($scope);
            }

            function openCatalogBlade() {
                var catalogBlade = {
                    id: "CatalogEntrySelect",
                    title: "marketing.blades.catalog-items-select.title-product",
                    controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
                    breadcrumbs: [],
                    toolbarCommands: [
                    {
                        name: "platform.commands.pick-selected", icon: 'fa fa-plus',
                        executeMethod: function () {
                            blade.$scope.gridApi.grid.options.data = _.map(selectedListEntries, function (entry) {
                                if (entry.imageUrl) {
                                    entry.imgSrc = entry.imageUrl;
                                }
                                return entry;
                            });
                            bladeNavigationService.closeBlade(catalogBlade);
                        },
                        canExecuteMethod: function () {
                            return selectedListEntries.length > 0;
                        }
                    }]
                };

                catalogBlade.options = {
                    selectedItemIds: blade.productIds,
                    showCheckingMultiple: false,
                    checkItemFn: function (listItem, isSelected) {
                        if (listItem.type == 'category') {
                            blade.error = 'Must select Product';
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
                            blade.error = undefined;
                        }
                    }
                };

                bladeNavigationService.showBlade(catalogBlade, blade);
            }

            $scope.blade.toolbarCommands = [
                {
                    name: "platform.commands.refresh", icon: 'fa fa-refresh',
                    executeMethod: function (blade) {
                        blade.refresh();
                    },
                    canExecuteMethod: function () { return true; }
                },
                {
                    name: "platform.commands.add", icon: 'fa fa-plus',
                    executeMethod: function () {
                        openCatalogBlade();
                    },
                    canExecuteMethod: function () {
                        return true;
                    }
                },
                {
                    name: "platform.commands.delete", icon: 'fa fa-trash-o',
                    executeMethod: function () {
                        var grid = blade.$scope.gridApi.grid;
                        var selectedRows = grid.api.selection.getSelectedRows();
                        for (var entry of selectedRows) {
                            grid.options.data = _.filter(grid.options.data, function (item) {
                                return item.id != entry.id;
                            });
                        }

                        selectedListEntries = angular.copy(grid.options.data);
                        blade.productIds = _.map(grid.options.data, function (item) {
                            return item.id;
                        });
                    },
                    canExecuteMethod: function () {
                        if (blade.childrenBlades.length > 0) {
                            return false;
                        }
                        if (blade.$scope.gridApi) {
                            return blade.$scope.gridApi.grid.api.selection.getSelectedRows().length > 0;
                        }
                        return false;
                    }
                }
            ];
        }]);
