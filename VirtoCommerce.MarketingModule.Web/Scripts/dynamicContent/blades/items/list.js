angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.itemsDynamicContentListController', ['$scope', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper', 'platformWebApp.dialogService', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.folders', 'virtoCommerce.marketingModule.dynamicContent.contentItems',
function ($scope, bladeUtils, uiGridHelper, dialogService, dynamicContentSearchApi, dynamicContentFoldersApi, dynamicContentItemsApi) {
    var bladeNavigationService = bladeUtils.bladeNavigationService;
    var blade = $scope.blade;
    blade.headIcon = 'fa-inbox';
    blade.chosenFolderId = 'ContentItem';
    blade.currentEntity = {};

    blade.initializeBlade = function () {
        blade.refresh();
    };

    blade.addNew = function () {
        bladeNavigationService.closeChildrenBlades(blade, function () {
            var newBlade = {
                id: 'listItemChild',
                title: 'marketing.blades.items.add.title',
                subtitle: 'marketing.blades.items.add.subtitle',
                chosenFolder: blade.chosenFolderId,
                controller: 'virtoCommerce.marketingModule.addContentItemsElementController',
                template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/add.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        });
    };

    blade.addNewContentItem = function (data) {
        bladeNavigationService.closeChildrenBlades(blade, function () {
            var newBlade = {
                id: 'listItemChild',
                title: 'marketing.blades.items.content-item-details.title-new',
                subtitle: 'marketing.blades.items.content-item-details.subtitle-new',
                entity: data,
                isNew: true,
                controller: 'virtoCommerce.marketingModule.addContentItemsController',
                template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/content-item-details.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        });
    };

    blade.addNewFolder = function (data) {
        bladeNavigationService.closeChildrenBlades(blade, function () {
            var newBlade = {
                id: 'listItemChild',
                title: 'marketing.blades.items.folder-details.title-new',
                subtitle: 'marketing.blades.items.folder-details.subtitle-new',
                entity: data,
                isNew: true,
                controller: 'virtoCommerce.marketingModule.addFolderContentItemsController',
                template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/folder-details.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        });
    };

    blade.refresh = function () {
        blade.isLoading = true;
        return dynamicContentSearchApi.search({
            skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            take: $scope.pageSettings.itemsPerPageCount,
            keyword: blade.searchKeyword,
            folderId: blade.chosenFolderId,
            sort: uiGridHelper.getSortExpression($scope),
            responseGroup: '18'
        }, function (data) {
            $scope.listEntries = [];
            $scope.pageSettings.totalItems = data.totalCount;
            _.each(data.contentFolders, function (contentFolder) {
                contentFolder.icon = 'fa fa-folder';
                contentFolder.isFolder = true;
                $scope.listEntries.push(contentFolder);
            });
            _.each(data.contentItems, function (contentItem) {
                contentItem.icon = 'fa fa-location-arrow';
                $scope.listEntries.push(contentItem);
            });
            setBreadcrumbs();
            blade.isLoading = false;
        });
    }

    blade.toolbarCommands = [
        {
            name: 'platform.commands.refresh', icon: 'fa fa-refresh',
            canExecuteMethod: function () { return true; },
            executeMethod: blade.refresh
        }, {
            name: 'platform.commands.add', icon: 'fa fa-plus',
            canExecuteMethod: function () { return true; },
            executeMethod: blade.addNew
        }, {
            name: 'platform.commands.delete', icon: 'fa fa-trash',
            canExecuteMethod: isItemsChecked,
            executeMethod: function () {
                $scope.deleteItems($scope.gridApi.selection.getSelectedRows());
            }
        }];

    function setBreadcrumbs() {
        if (blade.breadcrumbs) {
            var breadcrumb = _.find(blade.breadcrumbs, function (b) { return b.id === blade.currentEntity.id; });
            if (!breadcrumb) {
                breadCrumb = generateBreadcrumb(blade.currentEntity);
                blade.breadcrumbs.push(breadCrumb);
            } else {
                var position = blade.breadcrumbs.indexOf(breadcrumb);
                blade.breadcrumbs = blade.breadcrumbs.slice(0, position + 1);
            }
        } else {
            blade.breadcrumbs = [generateBreadcrumb({ id: 'ContentItem', name: 'all', isFolder: true })];
        }
    }

    function generateBreadcrumb(node) {
        return {
            id: node.id,
            name: node.name,
            navigate: function () {
                $scope.selectNode(node);
            }
        };
    }

    function isItemsChecked() {
        return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
    }

    var filter = $scope.filter = {};
    filter.criteriaChanged = function () {
        if ($scope.pageSettings.currentPage > 1) {
            $scope.pageSettings.currentPage = 1;
        } else {
            blade.refresh();
        }
    };

    $scope.clearKeyword = function () {
        blade.searchKeyword = null;
        blade.refresh();
    };

    $scope.selectNode = function (node) {
        bladeNavigationService.closeChildrenBlades(blade, function () {
            if (node.id && node.isFolder) {
                blade.currentEntity = node;
                blade.chosenFolderId = node.id;
                blade.refresh();
            } else {
                $scope.manageItem(node);
            }
        });
    };

    $scope.manageItem = function (node) {
        var newBlade = {
            id: 'listItemChild',
            entity: node,
            isNew: false
        };
        if (node.isFolder) {
            newBlade.title = 'marketing.blades.items.folder-details.title';
            newBlade.subtitle = 'marketing.blades.items.folder-details.subtitle';
            newBlade.controller = 'virtoCommerce.marketingModule.addFolderContentItemsController';
            newBlade.template = 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/folder-details.tpl.html';
        } else {
            newBlade.title = 'marketing.blades.items.content-item-details.title';
            newBlade.subtitle = 'marketing.blades.items.content-item-details.subtitle';
            newBlade.controller = 'virtoCommerce.marketingModule.addContentItemsController';
            newBlade.template = 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/content-item-details.tpl.html';
        }
        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.deleteItems = function (items) {
        var dialog = {
            id: "confirmDeleteContentItemsFolder",
            title: "marketing.dialogs.content-item-folder-delete.title",
            message: "marketing.dialogs.content-item-folder-delete.message",
            callback: function (remove) {
                if (remove) {
                    var folderItems = _.filter(items, function (i) { return i.isFolder; });
                    if (folderItems.length) {
                        dynamicContentFoldersApi.delete({
                            ids: _.pluck(folderItems, 'id')
                        }, function () {
                            blade.refresh();
                        });
                    }
                    var contentItems = _.filter(items, function (i) { return !i.isFolder; });
                    if (contentItems.length) {
                        dynamicContentItemsApi.delete({
                            ids: _.pluck(contentItems, 'id')
                        }, function () {
                            blade.refresh();
                        });
                    }
                }
            }
        };
        dialogService.showConfirmationDialog(dialog);
    };

    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
            uiGridHelper.bindRefreshOnSortChanged($scope);
        });
        bladeUtils.initializePagination($scope);
    };
}]);