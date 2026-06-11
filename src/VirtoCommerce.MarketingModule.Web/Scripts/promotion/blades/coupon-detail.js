angular.module('virtoCommerce.marketingModule')
    .controller('virtoCommerce.marketingModule.couponDetailController',
        ['$scope', "$injector",
            'platformWebApp.bladeNavigationService',
            'platformWebApp.settings',
            'virtoCommerce.marketingModule.promotions',
            function ($scope, $injector,
                bladeNavigationService,
                settings,
                promotionsApi) {
                var blade = $scope.blade;
                blade.isLoading = false;

                $scope.customerModuleInstalled = $injector.has('virtoCommerce.customerModule');

                // Coupon code pattern is configurable via the Marketing.Coupon.CodeValidationPattern setting;
                // undefined (empty setting or invalid regex) disables client-side validation, the server validates on save
                $scope.couponCodePattern = undefined;
                settings.getValues({ id: 'Marketing.Coupon.CodeValidationPattern' }, function (data) {
                    var pattern = data && data.length ? data[0] : null;
                    if (pattern) {
                        try {
                            $scope.couponCodePattern = new RegExp(pattern);
                        } catch (e) {
                            // invalid pattern configured - rely on server-side validation
                        }
                    }
                });

                blade.refresh = function (parentRefresh) {
                    if (!blade.isNew) {
                        promotionsApi.getCoupon({
                            id: blade.currentEntity.id
                        }, function (response) {
                            blade.currentEntity = response;
                            blade.parentBlade.refresh();
                        }, function (error) {
                            bladeNavigationService.setError(`Error ${error.status}`, blade);
                        });
                    }
                }

                function isDirty() {
                    return !angular.equals(blade.currentEntity, blade.originalEntity);
                }

                $scope.setForm = function (form) {
                    $scope.formCoupon = form;
                }

                $scope.datepickers = {
                    expirationDate: false
                }

                $scope.openCalendar = function ($event, calendar) {
                    $event.preventDefault();
                    $event.stopPropagation();
                    $scope.datepickers[calendar] = true;
                }

                $scope.format = 'shortDate';

                $scope.isValid = function () {
                    return isDirty()
                        && $scope.formCoupon
                        && $scope.formCoupon.$valid
                        && blade.showErrorStoreStateMessage !== true;
                };

                $scope.saveChanges = function () {
                    blade.isLoading = true;
                    promotionsApi.saveCoupon([
                        blade.currentEntity
                    ], function (response) {
                        blade.isLoading = false;
                        blade.parentBlade.refresh();
                        bladeNavigationService.closeBlade(blade);
                    }, function (error) {
                        blade.isLoading = false;
                        var message = error.data && error.data.message ? `: ${error.data.message}` : '';
                        bladeNavigationService.setError(`Error ${error.status}${message}`, blade);
                    });
                }

                blade.toolbarCommands = [{
                    name: "platform.commands.save",
                    icon: 'fas fa-save',
                    executeMethod: $scope.saveChanges,
                    canExecuteMethod: $scope.isValid
                }];

                blade.fetchMembers = function (criteria) {
                    criteria.memberTypes = [
                        "Organization", "Contact"
                    ];
                    criteria.deepSearch = true;

                    var membersApi = $injector.get('virtoCommerce.customerModule.members');

                    return membersApi.search(criteria);
                }
            }]);
