angular.module('virtoCommerce.marketingModule')
    .controller('virtoCommerce.marketingModule.catalogEntriesController', ['$scope', 'virtoCommerce.catalogModule.items', 'platformWebApp.ui-grid.extension', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper', 'platformWebApp.bladeNavigationService',
        function ($scope, items, gridOptionExtension, bladeUtils, uiGridHelper, bladeNavigationService) {
            var blade = $scope.blade;

            $scope.isEntityChanged = function () {
                return angular.equals(this.blade.productIds, this.blade.currentProductIds);
            };

            $scope.saveChanges = function () {
                this.blade.promotion.productIds = this.blade.productIds.slice();
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

            function initialize() {
                blade.productIds = blade.promotion.productIds != undefined ? blade.promotion.productIds.slice() : [];
                blade.currentProductIds = angular.copy(blade.productIds);
                items.query({ ids: blade.currentProductIds, respGroup: 'ItemInfo' }, function (data) {
                    blade.$scope.gridApi.grid.options.data = data;
                    blade.selectedListEntries = data.slice();
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
                            blade.$scope.gridApi.grid.options.data = _.map(blade.selectedListEntries, function (entry) {
                                if (entry.imageUrl) {
                                    entry.imgSrc = entry.imageUrl;
                                }
                                return entry;
                            });
                            bladeNavigationService.closeBlade(catalogBlade);
                        },
                        canExecuteMethod: function () {
                            return true;
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
                                if (_.all(blade.selectedListEntries, function (x) { return x.id != listItem.id; })) {
                                    blade.selectedListEntries.push(listItem);
                                }
                            }
                            else {
                                blade.selectedListEntries = _.reject(blade.selectedListEntries, function (x) { return x.id == listItem.id; });
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

                        blade.selectedListEntries = grid.options.data.slice();
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
