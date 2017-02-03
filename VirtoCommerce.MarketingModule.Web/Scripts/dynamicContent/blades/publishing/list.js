angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.publishingDynamicContentListController', ['$scope', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper', 'platformWebApp.dialogService', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.contentPublications',
function ($scope, bladeUtils, uiGridHelper, dialogService, dynamicContentSearchApi, dynamicContentPublicationsApi) {
    var bladeNavigationService = bladeUtils.bladeNavigationService;
    var blade = $scope.blade;
    blade.headIcon = 'fa-paperclip';
    blade.isLoading = false;

    blade.initialize = function () {
        blade.refresh();
    }

    blade.addNewPublishing = function () {
        bladeNavigationService.closeChildrenBlades(blade, function () {
            var newBlade = {
                id: 'add_publishing_element',
                title: 'marketing.blades.publishing.publishing-main-step.title-new',
                subtitle: 'marketing.blades.publishing.publishing-main-step.subtitle-new',
                isNew: true,
                controller: 'virtoCommerce.marketingModule.addPublishingFirstStepController',
                template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/publishing/publishing-main-step.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, $scope.blade);
        });
    }

    blade.removePublishings = function (selectedRows) {
        bladeNavigationService.closeChildrenBlades(blade, function () {
            var dialog = {
                id: 'confirmDeletePublishings',
                title: 'marketing.dialogs.publication-delete.title',
                message: 'marketing.dialogs.publication-delete.message',
                callback: function (confirm) {
                    if (confirm) {
                        var publishingIds = _.pluck(selectedRows, 'id');
                        dynamicContentPublicationsApi.remove({ ids: publishingIds }, blade.refresh);
                    }
                }
            }
            dialogService.showConfirmationDialog(dialog);
        });
    }

    blade.refresh = function () {
        blade.isLoading = true;
        dynamicContentSearchApi.search({
            skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            take: $scope.pageSettings.itemsPerPageCount,
            sort: uiGridHelper.getSortExpression($scope),
            responseGroup: '8',
            keyword: blade.searchKeyword
        }, function (data) {
            $scope.listEntries = data.contentPublications;
            $scope.pageSettings.totalItems = data.totalCount;
            blade.isLoading = false;
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
            blade.isLoading = false;
        });
    }

    blade.toolbarCommands = [{
        name: 'platform.commands.add',
        icon: 'fa fa-plus',
        canExecuteMethod: function () {
            return true;
        },
        executeMethod: blade.addNewPublishing
    }, {
        name: 'platform.commands.delete',
        icon: 'fa fa-trash',
        canExecuteMethod: isItemsChecked,
        executeMethod: function () {
            return blade.removePublishings($scope.gridApi.selection.getSelectedRows());
        }
    }, {
        name: 'platform.commands.refresh',
        icon: 'fa fa-refresh',
        canExecuteMethod: function () {
            return true;
        },
        executeMethod: blade.refresh
    }];

    function isItemsChecked() {
        return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
    }

    $scope.clearKeyword = function () {
        blade.searchKeyword = null;
        blade.refresh();
    }

    $scope.manageItem = function (item) {
        var newBlade = {
            id: 'edit_publishing_element',
            title: 'marketing.blades.publishing.publishing-main-step.title',
            subtitle: 'marketing.blades.publishing.publishing-main-step.subtitle',
            entity: item,
            isNew: false,
            controller: 'virtoCommerce.marketingModule.addPublishingFirstStepController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/publishing/publishing-main-step.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.deleteItems = function (items) {
        var dialog = {
            id: "confirmDeleteContentPublications",
            title: "marketing.dialogs.content-item-folder-delete.title",
            message: "marketing.dialogs.content-item-folder-delete.message",
            callback: function (remove) {
                if (remove) {
                    dynamicContentPublicationsApi.remove({
                        ids: _.map(items, function (i) { return i.id })
                    }, function () {
                        blade.refresh();
                    }, function (error) {
                        bladeNavigationService.setError('Error ' + error.status, blade);
                        blade.isLoading = false();
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
            uiGridHelper.bindRefreshOnSortChanged($scope);
        });
        bladeUtils.initializePagination($scope);
    };
}]);