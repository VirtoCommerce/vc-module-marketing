angular.module('virtoCommerce.marketingModule')
    .controller('virtoCommerce.marketingModule.promotionUsageHistoryWidgetController',
        ['$scope', '$injector', 'platformWebApp.bladeNavigationService',
            function ($scope, $injector, bladeNavigationService) {
                var blade = $scope.widget.blade;
                    var customerOrders = null;

                    // Orders module is an optional dependency. Avoid hard DI to prevent injection errors
                    // when VirtoCommerce.Orders isn't installed.
                    if ($injector.has('virtoCommerce.orderModule.order_res_customerOrders')) {
                        customerOrders = $injector.get('virtoCommerce.orderModule.order_res_customerOrders');
                    }

                function refresh() {
                    $scope.ordersCount = '...';

                    if (!blade.currentEntity || !blade.currentEntity.id || blade.isNew) {
                        $scope.ordersCount = 0;
                        return;
                    }

                        if (!customerOrders) {
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

                        if (!customerOrders) {
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
                        moduleName: "vc-order",
                        isExpanded: true,
                        controller: 'virtoCommerce.orderModule.customerOrderListController',
                        template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/customerOrder-list.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                    $scope.moduleName = "vc-order";
                };

                refresh();
            }]);

