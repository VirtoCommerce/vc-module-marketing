angular.module('virtoCommerce.marketingModule')
    .controller('virtoCommerce.marketingModule.marketingMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.marketingModule.marketingMenuService', function ($scope, bladeNavigationService, marketingMenuService) {
    $scope.selectedNodeId = null;

    function initializeBlade() {
        var entities = marketingMenuService.resolve();
        $scope.blade.currentEntities = entities;
        $scope.blade.isLoading = false;

        $scope.blade.openBlade(entities[0]);
    };

    $scope.blade.openBlade = function(data) {
        $scope.selectedNodeId = data.id;

        var newBlade = {
            id: 'marketingMainListChildren',
            title: data.name,
            subtitle: 'Marketing service',
            controller: data.controller || 'virtoCommerce.marketingModule.' + data.entityName + 'ListController',
            template: data.template || 'Modules/$(VirtoCommerce.Marketing)/Scripts/' +  data.entityName +  '/blades/' + data.entityName + '-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.blade.headIcon = 'fa-flag';

    initializeBlade();
}]);
