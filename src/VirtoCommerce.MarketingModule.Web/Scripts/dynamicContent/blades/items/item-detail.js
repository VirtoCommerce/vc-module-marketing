angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.itemDetailController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.contentItems', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.dynamicProperties.dictionaryItemsApi', 'platformWebApp.settings', 'platformWebApp.dynamicProperties.api',
    function ($scope, dynamicContentItemsApi, bladeNavigationService, dialogService, dictionaryItemsApi, settings, dynamicPropertiesApi) {
        var blade = $scope.blade;
        blade.updatePermission = 'marketing:update';

        blade.refresh = function() {
            if (!blade.isNew) {
                dynamicContentItemsApi.get({id: blade.entity.id}, function (response) {
                    dynamicPropertiesApi.search({
                        objectType: blade.entity.objectType,
                        take: 10000         // It is for getting all object dynamic properties
                    }, function (dynamicPropertiesResponse) {
                        var rawDynamicProperties = dynamicPropertiesResponse.results;
                        _.each(rawDynamicProperties, function(prop) {
                            prop.values = [];
                            var filteredProperty = _.find(response.dynamicProperties, function (o) { return o.id === prop.id; });
                            if (filteredProperty) {
                                prop.values = filteredProperty.values;
                            }
                        })
                        response.dynamicProperties = rawDynamicProperties;
                        blade.entity = response;
                        blade.currentEntity = response;
                        blade.initialize();
                    });
                });
            } else {
                blade.initialize();
            }

        };

        blade.IsChanged = function () {
            // Changes check method ensures the fact of different presentation way of dynamic properties:
            // In the whole object and dynamic properties thru va-generic-value-input template directive
            // First check objects for full equivalence
            var result = angular.equals(blade.origEntity, blade.currentEntity);
            if (!result) {
                // Check possibility they are really equivalent but have a different representation of dynamic property value
                result = angular.equals(_.omit(blade.origEntity, ['dynamicProperties']), _.omit(blade.currentEntity, ['dynamicProperties']));
                if (blade.origEntity.dynamicProperties && blade.currentEntity.dynamicProperties &&
                    blade.origEntity.dynamicProperties.length > 0 && blade.currentEntity.dynamicProperties.length > 0 &&
                    blade.origEntity.dynamicProperties[0].values.length > 0 && blade.currentEntity.dynamicProperties[0].values.length > 0 ) {
                    result = result && blade.origEntity.dynamicProperties[0].values[0].valueId == blade.currentEntity.dynamicProperties[0].values[0].value.id;
                }
            }
            return !result;
        };
       
        blade.initialize = function () {
            blade.toolbarCommands = [];

            if (!blade.isNew) {
               

                blade.toolbarCommands = [
                    {
                        name: "platform.commands.save", icon: 'fas fa-save',
                        executeMethod: function () {
                            blade.saveChanges();
                        },
                        canExecuteMethod: function () {
                            return blade.IsChanged() && $scope.formScope.$valid;
                        },
                        permission: blade.updatePermission
                    },
                    {
                        name: "platform.commands.reset", icon: 'fa fa-undo',
                        executeMethod: function () {
                            angular.copy(blade.origEntity, blade.currentEntity);
                        },
                        canExecuteMethod: function () {
                            return blade.IsChanged();
                        },
                        permission: blade.updatePermission
                    },
                    {
                        name: "platform.commands.delete", icon: 'fas fa-trash-alt',
                        executeMethod: function () {
                            var dialog = {
                                id: "confirmDeleteContentItem",
                                title: "marketing.dialogs.content-item-delete.title",
                                message: "marketing.dialogs.content-item-delete.message",
                                callback: function (remove) {
                                    if (remove) {
                                        blade.isLoading = true;
                                        dynamicContentItemsApi.delete({ ids: [blade.currentEntity.id] }, function () {
                                            blade.parentBlade.initializeBlade();
                                            bladeNavigationService.closeBlade(blade);
                                        });
                                    }
                                }
                            };

                            dialogService.showConfirmationDialog(dialog);
                        },
                        canExecuteMethod: function () {
                            return true;
                        },
                        permission: blade.updatePermission
                    }
                ];
            }

            blade.toolbarCommands.push(
                {
                    name: "marketing.commands.manage-type-properties", icon: 'fa fa-edit',
                    executeMethod: function () {
                        var newBlade = {
                            id: 'dynamicPropertyList',
                            objectType: blade.currentEntity.objectType,
                            controller: 'platformWebApp.dynamicPropertyListController',
                            template: '$(Platform)/Scripts/app/dynamicProperties/blades/dynamicProperty-list.tpl.html'
                        };
                        bladeNavigationService.showBlade(newBlade, blade);
                    },
                    canExecuteMethod: function () {
                        return angular.isDefined(blade.currentEntity.objectType);
                    }
                });

            blade.origEntity = blade.entity;
            blade.currentEntity = angular.copy(blade.origEntity);
            blade.isLoading = false;
        };
        
        blade.saveChanges = function () {
            blade.isLoading = true;

            if (blade.isNew) {
                dynamicContentItemsApi.save(blade.currentEntity, function (data) {
                    blade.parentBlade.initializeBlade();
                    bladeNavigationService.closeBlade(blade);
                });
            }
            else {
                dynamicContentItemsApi.update(blade.currentEntity, function (data) {
                    blade.parentBlade.initializeBlade();
                    blade.origEntity = angular.copy(blade.currentEntity);
                    blade.isLoading = false;
                });
            }
        };

        $scope.editDictionary = function (property) {
            var newBlade = {
                id: "propertyDictionary",
                isApiSave: true,
                currentEntity: property,
                controller: 'platformWebApp.propertyDictionaryController',
                template: '$(Platform)/Scripts/app/dynamicProperties/blades/property-dictionary.tpl.html',
                onChangesConfirmedFn: function () {
                    blade.currentEntity.dynamicProperties = angular.copy(blade.currentEntity.dynamicProperties);
                }
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };

        $scope.getDictionaryValues = function (property, callback) {
            dictionaryItemsApi.query({ id: property.objectType, propertyId: property.id }, callback);
        };

        $scope.setForm = function (form) { $scope.formScope = form; };

        blade.headIcon = 'fa fa-inbox';

        settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }, function (data) {
            $scope.languages = data;
        });

        blade.refresh();
    }]);
