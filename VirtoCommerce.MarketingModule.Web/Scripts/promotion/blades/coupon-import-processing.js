angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.couponImportProcessingController', ['$rootscope', '$scope', function ($rootscope, $scope) {
    var blade = $scope.blade;
    blade.isLoading = false;

    $scope.$on("new-notification-event", function (event, notification) {
        if (blade.notification && notification.id == blade.notification.id) {
            angular.copy(notification, blade.notification);
            if (notification.finished) {
                blade.parentBlade.refresh();
            }
        }
    });
}]);