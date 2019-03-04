angular.module('virtoCommerce.marketingModule')
    .controller('virtoCommerce.marketingModule.catalogConditionsController', ['$scope', 'virtoCommerce.catalogModule.items', 'platformWebApp.ui-grid.extension', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper',
        function ($scope, items, gridOptionExtension, bladeUtils, uiGridHelper) {

            $scope.setGridOptions = function (gridId, gridOptions) {
                gridOptions.enableSorting = true;
                gridOptionExtension.tryExtendGridOptions(gridId, gridOptions);
                uiGridHelper.initialize($scope, gridOptions, externalRegisterApiCallback);
                bladeUtils.initializePagination($scope);
            };

            function externalRegisterApiCallback(gridApi) {
                $scope.gridApi = gridApi;
                $scope.$parent.blade.refresh();
                uiGridHelper.bindRefreshOnSortChanged($scope);
            }
        }]);
