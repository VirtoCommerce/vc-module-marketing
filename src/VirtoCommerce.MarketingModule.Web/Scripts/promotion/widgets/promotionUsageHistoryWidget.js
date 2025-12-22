angular.module('virtoCommerce.marketingModule')
    .controller('virtoCommerce.marketingModule.promotionUsageHistoryWidgetController',
        ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.orderModule.order_res_customerOrders',
            function ($scope, bladeNavigationService, customerOrders) {
                var blade = $scope.widget.blade;

                function refresh() {
                    $scope.ordersCount = '...';

                    if (!blade.currentEntity || !blade.currentEntity.id || blade.isNew) {
                        $scope.ordersCount = 0;
                        return;
                    }

                    var countSearchCriteria = {
                        responseGroup: "Default",
                        take: 0,
                        promotionIds: [blade.currentEntity.id]
                    };

                    customerOrders.search(countSearchCriteria, function (data) {
                        $scope.ordersCount = data.totalCount;
                    }, function (error) {
                        $scope.ordersCount = 0;
                    });
                }

                $scope.openBlade = function () {
                    if (!blade.currentEntity || !blade.currentEntity.id || blade.isNew) {
                        return;
                    }

                    var newBlade = {
                        id: 'promotionOrders',
                        navigationGroup: blade.currentEntity.id,
                        title: 'marketing.widgets.promotion-usage-history.orders-title',
                        subtitle: blade.currentEntity.name,
                        searchCriteria: {
                            promotionIds: [blade.currentEntity.id]
                        },
                        isExpanded: true,
                        controller: 'virtoCommerce.orderModule.customerOrderListController',
                        template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/customerOrder-list.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                };

                refresh();

                // Watch for entity ID changes to refresh order count
                $scope.$watch('widget.blade.currentEntity.id', function (newId, oldId) {
                    if (newId && newId !== oldId && !blade.isNew) {
                        refresh();
                    }
                });
            }]);

