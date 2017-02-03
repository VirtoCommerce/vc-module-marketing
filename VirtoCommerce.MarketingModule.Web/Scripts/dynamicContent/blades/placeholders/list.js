angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.placeholdersDynamicContentListController', ['$scope', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper', 'platformWebApp.dialogService', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.folders', 'virtoCommerce.marketingModule.dynamicContent.contentPlaces',
function ($scope, bladeUtils, uiGridHelper, dialogService, dynamicContentSearchApi, dynamicContentFoldersApi, dynamicContentPlaceholdersApi) {
    var bladeNavigationService = bladeUtils.bladeNavigationService;
    var blade = $scope.blade;
    blade.headIcon = 'fa-location-arrow';
    blade.chosenFolderId = 'ContentPlace';
    blade.currentEntity = {};

    blade.initialize = function () {
        blade.refresh();
    };

    blade.closeChildrenBlades = function () {
        angular.forEach(blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    blade.addNew = function () {
        blade.closeChildrenBlades();
        var newBlade = {
            id: 'listItemChild',
            title: 'marketing.blades.placeholders.add.title',
            subtitle: 'marketing.blades.placeholders.add.subtitle',
            chosenFolder: blade.chosenFolderId,
            controller: 'virtoCommerce.marketingModule.addPlaceholderElementController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/add.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.addNewFolder = function (data) {
        blade.closeChildrenBlades();
        var newBlade = {
            id: 'listItemChild',
            title: 'marketing.blades.placeholders.folder-details.title-new',
            subtitle: 'marketing.blades.placeholders.folder-details.subtitle-new',
            entity: data,
            isNew: true,
            controller: 'virtoCommerce.marketingModule.addFolderPlaceholderController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/folder-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.addNewPlaceholder = function (data) {
        blade.closeChildrenBlades();
        var newBlade = {
            id: 'listItemChild',
            title: 'marketing.blades.placeholders.placeholder-details.title-new',
            subtitle: 'marketing.blades.placeholders.placeholder-details.subtitle-new',
            entity: data,
            isNew: true,
            controller: 'virtoCommerce.marketingModule.addPlaceholderController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/placeholder-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.refresh = function () {
        blade.isLoading = true;
        dynamicContentSearchApi.search({
            start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            count: $scope.pageSettings.itemsPerPageCount,
            keyword: blade.searchKeyword,
            folderId: blade.chosenFolderId,
            sort: uiGridHelper.getSortExpression($scope),
            responseGroup: '20'
        }, function (data) {
            $scope.listEntries = [];
            $scope.pageSettings.totalItems = data.totalCount;
            _.each(data.contentFolders, function (contentFolder) {
                contentFolder.icon = 'fa fa-folder';
                contentFolder.isFolder = true;
                $scope.listEntries.push(contentFolder);
            });
            _.each(data.contentPlaces, function (contentPlace) {
                $scope.listEntries.push(contentPlace);
            });
            setBreadcrumbs();
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
        executeMethod: blade.addNew
    }, {
        name: 'platform.commands.delete',
        icon: 'fa fa-trash',
        canExecuteMethod: isItemsChecked,
        executeMethod: function () {
            $scope.deleteItems($scope.gridApi.selection.getSelectedRows());
        }
    }, {
        name: 'platform.commands.refresh',
        icon: 'fa fa-refresh',
        canExecuteMethod: function () {
            return true;
        },
        executeMethod: blade.refresh
    }];

    function setBreadcrumbs() {
        if (blade.breadcrumbs) {
            var breadcrumb = _.find(blade.breadcrumbs, function (b) { return b.id === blade.currentEntity.id });
            if (!breadcrumb) {
                breadCrumb = generateBreadcrumb(blade.currentEntity);
                blade.breadcrumbs.push(breadCrumb);
            } else {
                var position = blade.breadcrumbs.indexOf(breadcrumb);
                blade.breadcrumbs = blade.breadcrumbs.slice(0, position + 1);
            }
        } else {
            blade.breadcrumbs = [generateBreadcrumb({ id: 'ContentPlace', name: 'all', isFolder: true })];
        }
    }

    function generateBreadcrumb(node) {
        return {
            id: node.id,
            name: node.name,
            navigate: function () {
                $scope.selectNode(node);
            }
        }
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
    }

    $scope.clearKeyword = function () {
        blade.searchKeyword = null;
        blade.refresh();
    }

    $scope.selectNode = function (node) {
        blade.closeChildrenBlades();
        if (node.id && node.isFolder) {
            blade.currentEntity = node;
            blade.chosenFolderId = node.id;
            blade.refresh();
        } else {
            $scope.manageItem(node);
        }
    }

    $scope.manageItem = function (node) {
        var newBlade = {
            id: 'listItemChild',
            entity: node,
            isNew: false
        }
        if (node.isFolder) {
            newBlade.title = 'marketing.blades.placeholders.folder-details.title-new';
            newBlade.subtitle = 'marketing.blades.placeholders.folder-details.subtitle-new';
            newBlade.controller = 'virtoCommerce.marketingModule.addFolderPlaceholderController';
            newBlade.template = 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/folder-details.tpl.html';
        } else {
            newBlade.title = 'marketing.blades.placeholders.placeholder-details.title';
            newBlade.subtitle = 'marketing.blades.placeholders.placeholder-details.subtitle';
            newBlade.controller = 'virtoCommerce.marketingModule.addPlaceholderController';
            newBlade.template = 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/placeholder-details.tpl.html';
        }
        bladeNavigationService.showBlade(newBlade, blade);
    }

    $scope.deleteItems = function (items) {
        var dialog = {
            id: "confirmDeleteContentPlaceholdersFolder",
            title: "marketing.dialogs.placeholders-folder-delete.title",
            message: "marketing.dialogs.placeholders-folder-delete.message",
            callback: function (remove) {
                if (remove) {
                    var folderItems = _.filter(items, function (i) { return i.isFolder });
                    if (folderItems.length) {
                        dynamicContentFoldersApi.delete({
                            ids: _.map(folderItems, function (f) { return f.id })
                        }, function () {
                            blade.refresh();
                        }, function (error) {
                            bladeNavigationService.setError('Error ' + error.status, blade);
                            blade.isLoading = false();
                        });
                    }
                    var placeholderItems = _.filter(items, function (i) { return !i.isFolder });
                    if (placeholderItems.length) {
                        dynamicContentPlaceholdersApi.delete({
                            ids: _.map(placeholderItems, function (i) { return i.id })
                        }, function () {
                            blade.refresh();
                        }, function (error) {
                            bladeNavigationService.setError('Error ' + error.status, blade);
                            blade.isLoading = false();
                        });
                    }
                }
            }
        };
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
            uiGridHelper.bindRefreshOnSortChanged($scope);
        });
        bladeUtils.initializePagination($scope);
    };
}]);