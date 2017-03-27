angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.couponsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.marketingModule.promotions', function ($scope, bladeNavigationService, promotionsApi) {
    var blade = $scope.blade;

    var criteria = {
        promotionId: blade.promotionId,
        skip: 0,
        take: 1
    };
    promotionsApi.searchCoupons({ promotionId: blade.promotionId, skip: 0, take: 1 }, function (response) {
        blade.totalCouponsCount = response.totalCount;
    });

    $scope.$on("new-notification-event", function (event, notification) {
        blade.refresh();
    });

    $scope.openBlade = function () {
        var newBlade = {
            id: 'coupons',
            title: 'marketing.blades.promotion-detail.toolbar.coupons',
            promotionId: blade.currentEntity.id,
            controller: 'virtoCommerce.marketingModule.couponListController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/promotion/blades/coupon-list.tpl.html'
        }
        bladeNavigationService.showBlade(newBlade, blade);
    }
}]);