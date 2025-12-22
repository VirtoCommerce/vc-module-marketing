angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.filterDetailController', ['$scope', '$localStorage', '$translate', 'virtoCommerce.storeModule.stores',
    function ($scope, $localStorage, $translate, stores) {
        var blade = $scope.blade;

        function initializeBlade(data) {
            blade.currentEntity = angular.copy(data);
            blade.origEntity = data;
            blade.isLoading = false;

            // Ensure status is a number (not string) for proper comparison with ui-select
            if (blade.currentEntity.status != null && blade.currentEntity.status !== undefined && blade.currentEntity.status !== '') {
                var statusValue = parseInt(blade.currentEntity.status, 10);
                blade.currentEntity.status = isNaN(statusValue) ? null : statusValue;
            } else {
                blade.currentEntity.status = null;
            }

            blade.title = blade.isNew ? 'marketing.blades.filter-detail.new-title' : data.name;
            blade.subtitle = blade.isNew ? 'marketing.blades.filter-detail.new-subtitle' : 'marketing.blades.filter-detail.subtitle';
        }

        blade.statusOptions = [
            { value: 0, label: 'marketing.blades.promotion-list.labels.all-filter' },
            { value: 1, label: 'marketing.blades.promotion-list.labels.active-filter' },
            { value: 2, label: 'marketing.blades.promotion-list.labels.upcoming-filter' },
            { value: 3, label: 'marketing.blades.promotion-list.labels.archived-filter' },
            { value: 4, label: 'marketing.blades.promotion-list.labels.deactivated-filter' }
        ];

        // Helper function to get status object by value
        blade.getStatusByValue = function(statusValue) {
            if (statusValue == null || statusValue === undefined) return null;
            return _.findWhere(blade.statusOptions, { value: statusValue });
        };

        // Helper function to get status label by value
        $scope.getStatusLabel = function(statusValue) {
            if (statusValue == null || statusValue === undefined) return '';
            var status = blade.getStatusByValue(statusValue);
            return status ? status.label : '';
        };

        blade.metaFields = [
            {
                name: 'status',
                title: "marketing.blades.filter-detail.labels.status",
                templateUrl: 'statusSelector.html'
            }, {
                name: 'storeIds',
                title: "marketing.blades.promotion-detail.labels.store",
                templateUrl: 'storeSelector.html'
            //}, {
            //    name: 'startDate',
            //    title: "marketing.blades.promotion-detail.labels.start-date",
            //    valueType: "DateTime"
            //}, {
            //    name: 'endDate',
            //    title: "marketing.blades.promotion-detail.labels.expiration-date",
            //    valueType: "DateTime"
            }
        ];

        $scope.applyCriteria = function () {
            // Normalize status to number before saving (or null if empty/invalid)
            if (blade.currentEntity.status != null && blade.currentEntity.status !== undefined && blade.currentEntity.status !== '') {
                var statusValue = parseInt(blade.currentEntity.status, 10);
                blade.currentEntity.status = isNaN(statusValue) ? null : statusValue;
            } else {
                blade.currentEntity.status = null;
            }
            
            angular.copy(blade.currentEntity, blade.origEntity);
            if (blade.isNew) {
                $localStorage.promotionSearchFilters.unshift(blade.origEntity);
                $localStorage.promotionSearchFilterId = blade.origEntity.id;
                blade.parentBlade.filter.current = blade.origEntity;
                blade.isNew = false;
            }

            initializeBlade(blade.origEntity);
            blade.parentBlade.filter.criteriaChanged();
            // $scope.bladeClose();
        };
        
        $scope.saveChanges = function () {
            $scope.applyCriteria();
        };

        var formScope;
        $scope.setForm = function (form) { formScope = form; };

        function isDirty() {
            return !angular.equals(blade.currentEntity, blade.origEntity);
        }

        blade.headIcon = 'fa fa-filter';

        blade.toolbarCommands = [
                {
                    name: "core.commands.apply-filter", icon: 'fa fa-filter',
                    executeMethod: function () {
                        $scope.saveChanges();
                    },
                    canExecuteMethod: function () {
                        return formScope && formScope.$valid;
                    }
                },
                {
                    name: "platform.commands.reset", icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy(blade.origEntity, blade.currentEntity);
                    },
                    canExecuteMethod: isDirty
                },
                {
                    name: "platform.commands.delete", icon: 'fas fa-trash-alt',
                    executeMethod: deleteEntry,
                    canExecuteMethod: function () {
                        return !blade.isNew;
                    }
                }];


        function deleteEntry() {
            blade.parentBlade.filter.current = null;
            $localStorage.promotionSearchFilters.splice($localStorage.promotionSearchFilters.indexOf(blade.origEntity), 1);
            delete $localStorage.promotionSearchFilterId;
            blade.parentBlade.refresh();
            $scope.bladeClose();
        }


        // actions on load
        if (blade.isNew) {
            $translate('marketing.blades.filter-detail.unnamed-filter').then(function (result) {
                initializeBlade({ id: new Date().getTime(), name: result });
            });
        } else {
            initializeBlade(blade.data);
        }
    }]);
