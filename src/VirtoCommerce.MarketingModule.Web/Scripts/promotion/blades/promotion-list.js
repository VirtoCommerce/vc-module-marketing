angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.promotionListController', ['$scope', '$localStorage', 'virtoCommerce.marketingModule.promotions', 'platformWebApp.dialogService', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper',
    function ($scope, $localStorage, promotions, dialogService, bladeUtils, uiGridHelper) {
        var blade = $scope.blade;
        var bladeNavigationService = bladeUtils.bladeNavigationService;

        $scope.getGridOptions = () => {
            return {
                useExternalSorting: true,
                data: 'blade.currentEntities',
                rowTemplate: 'promotion-list.row.html',
                columnDefs: [
                    { name: 'actions', displayName: '', enableColumnResizing: false, enableSorting: false, width: 30, cellTemplate: 'list-actions.cell.html', pinnedLeft: true},
                    { name: 'name', displayName: 'marketing.blades.promotion-list.labels.name', width: 350, cellTemplate: 'promotion-list-name.cell.html'},
                    { name: 'isActive', displayName: 'marketing.blades.promotion-list.labels.isActive', width: 120, cellTemplate: 'promotion-list-bool.cell.html'},
                    { name: 'hasCoupons', displayName: 'marketing.blades.promotion-list.labels.hasCoupons', width: 120, cellTemplate: 'promotion-list-bool.cell.html'},
                    { name: 'startDate', displayName: 'marketing.blades.promotion-list.labels.startDate', width: 150, cellFilter: 'date' },
                    { name: 'endDate', displayName: 'marketing.blades.promotion-list.labels.endDate', width: 150, cellFilter: 'date', cellTemplate: '$(Platform)/Scripts/common/templates/ui-grid/am-time-ago.cell.html' },
                    { name: 'description', displayName: 'marketing.blades.promotion-list.labels.description', width: 350 },
                ]
            };
        };

        blade.refresh = function () {
            blade.isLoading = true;

            var criteria = {
                responseGroup: 'none',
                keyword: filter.keyword,
                sort: uiGridHelper.getSortExpression($scope),
                skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                take: $scope.pageSettings.itemsPerPageCount
            };
            if (filter.current) {
                angular.extend(criteria, filter.current);
            }

            promotions.search(criteria, function (data) {
                blade.isLoading = false;

                $scope.pageSettings.totalItems = data.totalCount;
                blade.currentEntities = data.results;
            });
        };

        $scope.selectNode = function (node) {
            $scope.selectedNodeId = node.id;

            var newBlade = {
                id: 'listItemChild',
                currentEntity: node,
                title: node.name,
                subtitle: blade.subtitle,
                controller: 'virtoCommerce.marketingModule.promotionDetailController',
                template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/promotion/blades/promotion-detail.tpl.html'
            };

            bladeNavigationService.showBlade(newBlade, blade);
        };

        $scope.copy = function (text) {
            var copyElement = document.createElement("span");
            copyElement.appendChild(document.createTextNode(text));
            copyElement.id = 'tempCopyToClipboard';
            angular.element(document.body.append(copyElement));

            var range = document.createRange();
            range.selectNode(copyElement);
            window.getSelection().removeAllRanges();
            window.getSelection().addRange(range);

            document.execCommand('copy');
            window.getSelection().removeAllRanges();
            copyElement.remove();
        };

        $scope.clonePromotion = function (promotion) {
            bladeNavigationService.closeChildrenBlades(blade, function () {
                var newBlade = {
                    id: 'promotionClone',
                    title: 'marketing.blades.promotion-detail.title-new',
                    subtitle: blade.subtitle,
                    isNew: true,
                    isCloning: true,
                    data: angular.copy(promotion),
                    controller: 'virtoCommerce.marketingModule.promotionDetailController',
                    template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/promotion/blades/promotion-detail.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            });
        };

        $scope.deleteList = function (list) {
            var dialog = {
                id: "confirmDeleteItem",
                title: "marketing.dialogs.promotions-delete.title",
                message: "marketing.dialogs.promotions-delete.message",
                callback: function (remove) {
                    if (remove) {
                        bladeNavigationService.closeChildrenBlades(blade, function () {
                            blade.isLoading = true;

                            var itemIds = _.pluck(list, 'id');
                            promotions.remove({ ids: itemIds }, function () {
                                blade.refresh();
                            });
                        });
                    }
                }
            };
            dialogService.showConfirmationDialog(dialog);
        };

        blade.headIcon = 'fa fa-area-chart';

        blade.toolbarCommands = [
            {
                name: "platform.commands.refresh", icon: 'fa fa-refresh',
                executeMethod: blade.refresh,
                canExecuteMethod: function () { return true; }
            },
            {
                name: "platform.commands.add", icon: 'fas fa-plus',
                executeMethod: function () {
                    bladeNavigationService.closeChildrenBlades(blade, function () {
                        var newBlade = {
                            id: 'listItemChild',
                            title: 'marketing.blades.promotion-detail.title-new',
                            subtitle: blade.subtitle,
                            isNew: true,
                            currentEntity: {},
                            controller: 'virtoCommerce.marketingModule.promotionDetailController',
                            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/promotion/blades/promotion-detail.tpl.html'
                        };
                        bladeNavigationService.showBlade(newBlade, blade);
                    });
                },
                canExecuteMethod: function () { return true; },
                permission: 'marketing:create'
            },
            {
                name: "platform.commands.delete", icon: 'fas fa-trash-alt',
                executeMethod: function () {
                    $scope.deleteList($scope.gridApi.selection.getSelectedRows());
                },
                canExecuteMethod: function () {
                    return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
                },
                permission: 'marketing:delete'
            }
        ];

        // simple and advanced filtering
        var filter = blade.filter = $scope.filter = {};
        $scope.$localStorage = $localStorage;
        if (!$localStorage.promotionSearchFilters) {
            $localStorage.promotionSearchFilters = [{ name: 'marketing.blades.promotion-list.new-filter' }];
        }

        addPredefinedFilters($localStorage.promotionSearchFilters);

        if ($localStorage.promotionSearchFilterId) {
            filter.current = _.findWhere($localStorage.promotionSearchFilters, { id: $localStorage.promotionSearchFilterId });
        }

        filter.change = function () {
            $localStorage.promotionSearchFilterId = filter.current ? filter.current.id : null;
            if (filter.current && !filter.current.id) {
                filter.current = null;
                filter.keyword = '';
                showFilterDetailBlade({ isNew: true });
            } else {
                filter.keyword = filter.current ? filter.keyword : '';
                bladeNavigationService.closeBlade({ id: 'filterDetail' });
                filter.criteriaChanged();
            }
        };

        filter.edit = function () {
            if (filter.current) {
                showFilterDetailBlade({ data: filter.current });
            }
        };

        function showFilterDetailBlade(bladeData) {
            var newBlade = {
                id: 'filterDetail',
                controller: 'virtoCommerce.marketingModule.filterDetailController',
                template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/promotion/blades/filter-detail.tpl.html'
            };
            angular.extend(newBlade, bladeData);
            bladeNavigationService.showBlade(newBlade, blade);
        }

        filter.filterByKeyword = function () {
            filter.criteriaChanged();
        };

        filter.criteriaChanged = function () {
            if ($scope.pageSettings.currentPage > 1) {
                $scope.pageSettings.currentPage = 1;
            } else {
                blade.refresh();
            }
        };

        function buildPredefinedFilters() {
            return [
                {
                    name: 'marketing.blades.promotion-list.labels.active-filter',
                    id: 'active',
                    status: 1 // PromotionStatus.Active
                },
                {
                    name: 'marketing.blades.promotion-list.labels.upcoming-filter',
                    id: 'upcoming',
                    status: 2 // PromotionStatus.Upcoming
                },
                {
                    name: 'marketing.blades.promotion-list.labels.archived-filter',
                    id: 'archived',
                    status: 3 // PromotionStatus.Archived
                },
                {
                    name: 'marketing.blades.promotion-list.labels.deactivated-filter',
                    id: 'deactivated',
                    status: 4 // PromotionStatus.Deactivated
                },
                {
                    name: 'marketing.blades.promotion-list.labels.all-filter',
                    id: 'all',
                    status: 0 // PromotionStatus.All
                }
            ];
        }

        function addPredefinedFilters(baseFilters) {
            var predefinedFilters = buildPredefinedFilters();
            var filterWithoutId = null;

            // Remove the filter without an id from baseFilters if it exists
            for (var j = 0; j < baseFilters.length; j++) {
                if (!baseFilters[j].id) {
                    filterWithoutId = baseFilters.splice(j, 1)[0];
                    break;
                }
            }

            // Add or replace predefined filters in baseFilters
            for (var i = 0; i < predefinedFilters.length; i++) {
                var predefinedFilter = _.findWhere(baseFilters, { id: predefinedFilters[i].id });
                if (predefinedFilter) {
                    angular.extend(predefinedFilter, predefinedFilters[i]);
                } else {
                    baseFilters.push(predefinedFilters[i]);
                }
            }

            // Push the filter without an id back to the end of baseFilters if it exists
            if (filterWithoutId) {
                baseFilters.push(filterWithoutId);
            }
        }

        // ui-grid
        $scope.setGridOptions = function (gridId, gridOptions) {
            $scope.gridOptions = gridOptions;
            
            uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
                uiGridHelper.bindRefreshOnSortChanged($scope);
            });

            bladeUtils.initializePagination($scope);

            return gridOptions;
        };

        // actions on load
        //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
        //blade.refresh();
    }]);
