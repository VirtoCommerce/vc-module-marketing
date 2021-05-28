angular.module('virtoCommerce.marketingModule')
    .controller('virtoCommerce.marketingModule.itemDetailController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.contentItems', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.dynamicProperties.dictionaryItemsApi', 'platformWebApp.settings', 'platformWebApp.dynamicProperties.api',
        function ($scope, dynamicContentItemsApi, bladeNavigationService, dialogService, dictionaryItemsApi, settings, dynamicPropertiesApi) {
            var blade = $scope.blade;
            blade.updatePermission = 'marketing:update';

            blade.refresh = function () {
                if (!blade.isNew) {
                    dynamicContentItemsApi.get({ id: blade.entity.id }, function (response) {
                        dynamicPropertiesApi.search({
                            objectType: blade.entity.objectType,
                            take: 10000         // It is for getting all object dynamic properties
                        }, function (dynamicPropertiesResponse) {
                            var rawDynamicProperties = dynamicPropertiesResponse.results;
                            _.each(rawDynamicProperties, function (prop) {
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
                let equals = angular.equals(blade.origEntity, blade.currentEntity);
                if (!equals) {
                    // Check possibility they are really equivalent but have a different representation of dynamic property value
                    equals = angular.equals(_.omit(blade.origEntity, ['dynamicProperties']), _.omit(blade.currentEntity, ['dynamicProperties']));

                    if (blade.origEntity.dynamicProperties && blade.currentEntity.dynamicProperties &&
                        blade.origEntity.dynamicProperties.length && blade.currentEntity.dynamicProperties.length) {
                        for (var origEntityDynamicProperty of blade.origEntity.dynamicProperties) {
                            let currEntityDynamicProperty = blade.currentEntity.dynamicProperties.find(x => x.name == origEntityDynamicProperty.name);

                            if (origEntityDynamicProperty.isDictionary) {
                                // dictionaries comparison
                                // Case when both orig and current values are empty,
                                // dictionary field clearing (press 'Backspace') after selection is reason to currEntityDynamicProperty undefined value.
                                let currDictionaryValuesIds = currEntityDynamicProperty.values.filter(x => x.value != undefined).map(x => x.value.id);
                                // 'Every' func require length check
                                equals = origEntityDynamicProperty.values.length
                                    ? origEntityDynamicProperty.values.every(x => currDictionaryValuesIds.includes(x.valueId))
                                    : !currDictionaryValuesIds.length;
                            }
                            else if (origEntityDynamicProperty.isArray &&
                                origEntityDynamicProperty.values.length && currEntityDynamicProperty.values.length) {
                                // arrays comparison
                                // Check arrays equality (adding an element after the same element deletation)
                                let origValues = origEntityDynamicProperty.values.map(x => x.value);
                                let currValues = currEntityDynamicProperty.values.map(x => x.value);
                                equals = origValues.every(x => currValues.includes(x));
                            } else {
                                // simple types comparison
                                // !!AND EMPTY ARRAYS!!
                                equals = angular.equals(origEntityDynamicProperty.values, currEntityDynamicProperty.values);
                            }

                            if (!equals)
                                break;
                        }
                    }
                }
                return !equals;
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
