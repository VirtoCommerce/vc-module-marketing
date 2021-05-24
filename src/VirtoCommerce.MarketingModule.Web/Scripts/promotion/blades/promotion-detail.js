angular.module('virtoCommerce.marketingModule')
    .controller('virtoCommerce.marketingModule.promotionDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.marketingModule.promotions', 'virtoCommerce.storeModule.stores', 'virtoCommerce.coreModule.common.dynamicExpressionService', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.items', '$q', 'platformWebApp.settings', 'platformWebApp.metaFormsService', 'virtoCommerce.shippingModule.shippingMethods', 'virtoCommerce.paymentModule.paymentMethods', function ($scope, bladeNavigationService, marketing_res_promotions, stores, dynamicExpressionService, categories, items, $q, settings, metaFormsService, shippingMethods, paymentMethods) {
        var blade = $scope.blade;
        blade.updatePermission = 'marketing:update';

        blade.metaFields = metaFormsService.getMetaFields("promotionDetail");

        blade.expressionTreeTemplateUrl = dynamicExpressionService.expressionTreeTemplateUrl;

        blade.showPriority = false;
        settings.get({ id: 'Marketing.Promotion.CombinePolicy' }, function (data) {
            blade.showPriority = data.value === 'CombineStackable';
        });
        blade.refresh = function (parentRefresh) {

            var shippingMethodsPromise = !blade.shippingMethods
                ? shippingMethods.getAllRegistered(function (methods) {
                    blade.shippingMethods = _.uniq(methods, method => method.code);
                    // VP-5647: Need to fill `method.id` for proper work of methods selectors UI template from core module (e.g. expression-RewardShippingGetOfAbsShippingMethod.html)
                    _.each(blade.shippingMethods, function (method) {
                        method.id = method.id ? method.id : method.code;
                    });
                })
                    .$promise
                : $q.when();

            var paymentMethodsPromise = !blade.paymentMethods
                ? paymentMethods.getAllRegistered(function (methods) {
                    blade.paymentMethods = _.uniq(methods, method => method.code);
                    // VP-5647: Need to fill `method.id` for proper work of methods selectors UI template from core module (e.g. expression-RewardShippingGetOfAbsShippingMethod.html)
                    _.each(blade.paymentMethods, function (method) {
                        method.id = method.id ? method.id : method.code;
                    });
                })
                    .$promise
                : $q.when();

            // VP-5647: Wait for payment/shipment method loading before requesting stores and initialize dynamic tree
            $q.all(shippingMethodsPromise, paymentMethodsPromise).then(() => {
                if (blade.isNew) {
                    if (blade.isCloning) {
                        blade.data.id = null;
                        blade.data.name = null;
                        initializeBlade(blade.data);
                    } else {
                        marketing_res_promotions.getNew(initializeBlade);
                    }
                } else {
                    marketing_res_promotions.get({ id: blade.currentEntity.id, code: (blade.currentEntityCode === undefined) ? '' : blade.currentEntityCode }, function (data) {
                        initializeBlade(data);
                        if (parentRefresh) {
                            blade.parentBlade.refresh();
                        }
                    });
                }
            });
        };

        function initializeBlade(data) {
            if (!blade.isNew) {
                blade.title = data.name;
            }

            // transform simple string to complex object. Simple string isn't editable.
            data.coupons = _.map(data.coupons, function (x) { return { text: x }; });

            blade.expressionPromises = [];
            if (data.dynamicExpression) {
                _.each(data.dynamicExpression.children, extendElementBlock);
            }

            $q.all(blade.expressionPromises).then(function (allData) {
                blade.currentEntity = angular.copy(data);
                blade.origEntity = data;
            }).finally(function () {
                blade.isLoading = false;
            });
        }

        function isDirty() {
            return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
        }

        $scope.cancelChanges = function () {
            //angular.copy(blade.origEntity, blade.currentEntity);
            $scope.bladeClose();
        };

        $scope.saveChanges = function () {
            bladeNavigationService.setError(null, blade);
            blade.isLoading = true;

            blade.currentEntity.coupons = _.pluck(blade.currentEntity.coupons, 'text');

            if (blade.currentEntity.dynamicExpression) {
                _.each(blade.currentEntity.dynamicExpression.children, stripOffUiInformation);
            }

            if (blade.isNew) {
                marketing_res_promotions.save({}, blade.currentEntity, function (data) {
                    blade.isNew = undefined;
                    blade.currentEntity = data;
                    initializeToolbar();
                    blade.refresh(true);
                });
            } else {
                marketing_res_promotions.update({}, blade.currentEntity, function (data) {
                    blade.refresh(true);
                });
            }
        };

        $scope.setForm = function (form) { $scope.formScope = form; };

        $scope.isValid = function () {
            return isDirty()
                && $scope.formScope
                && $scope.formScope.$valid;
            //&& (!blade.currentEntity.dynamicExpression;
            //    || (blade.currentEntity.dynamicExpression.children[0].children.length > 0
            //        && blade.currentEntity.dynamicExpression.children[3].children.length > 0));
        };

        blade.onClose = function (closeCallback) {
            bladeNavigationService.showConfirmationIfNeeded(isDirty() && !blade.isNew, $scope.isValid(), blade, $scope.saveChanges, closeCallback, "marketing.dialogs.promotion-save.title", "marketing.dialogs.promotion-save.message");
        };

        blade.headIcon = 'fa fa-area-chart';

        function initializeToolbar() {
            if (!blade.isNew) {
                blade.toolbarCommands = [
                    {
                        name: "platform.commands.save",
                        icon: 'fas fa-save',
                        executeMethod: $scope.saveChanges,
                        canExecuteMethod: $scope.isValid,
                        permission: blade.updatePermission
                    },
                    {
                        name: "platform.commands.reset",
                        icon: 'fa fa-undo',
                        executeMethod: function () {
                            angular.copy(blade.origEntity, blade.currentEntity);
                        },
                        canExecuteMethod: isDirty,
                        permission: blade.updatePermission
                    },
                    {
                        name: "platform.commands.clone", icon: 'fas fa-clone',
                        executeMethod: function () {
                            var newBlade = {
                                id: 'promotionClone',
                                title: 'marketing.blades.promotion-detail.title-new',
                                isNew: true,
                                isCloning: true,
                                data: blade.currentEntity,
                                controller: 'virtoCommerce.marketingModule.promotionDetailController',
                                template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/promotion/blades/promotion-detail.tpl.html'
                            };
                            bladeNavigationService.showBlade(newBlade, blade.parentBlade);
                        },
                        canExecuteMethod: function () { return !isDirty(); },
                        permission: 'marketing:create'
                    }
                ];
            }
        }

        // datepicker
        $scope.datepickers = {
            str: false,
            end: false
        };

        $scope.open = function ($event, which) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepickers[which] = true;
        };

        // Dynamic ExpressionBlock
        function extendElementBlock(expressionBlock) {
            var retVal = dynamicExpressionService.expressions[expressionBlock.id];
            if (!retVal) {
                retVal = { displayName: 'unknown element: ' + expressionBlock.id };
            }

            _.extend(expressionBlock, retVal);

            if (!expressionBlock.children) {
                expressionBlock.children = [];
            }
            _.each(expressionBlock.excludingCategoryIds, function (id) {
                // load each category to display its name in UI
                var excludingItem = { id: 'ExcludingCategoryCondition', categoryId: id };
                expressionBlock.children.push(excludingItem);
                var promise = categories.query({ ids: [id], respGroup: 'Info' }, function (data) {
                    if (_.any(data))
                        excludingItem.categoryName = data[0].name;
                }).$promise;
                blade.expressionPromises.push(promise);
            });

            _.each(expressionBlock.excludingProductIds, function (id) {
                // load each product to display its name and code in UI
                var excludingItem = { id: 'ExcludingProductCondition', productId: id };
                expressionBlock.children.push(excludingItem); // categoryName
                var promise = items.query({ ids: [id], respGroup: 'ItemInfo' }, function (data) {
                    if (_.any(data)) {
                        excludingItem.productName = data[0].name;
                        excludingItem.productCode = data[0].code;
                    }
                }).$promise;
                blade.expressionPromises.push(promise);
            });

            _.each(expressionBlock.children, extendElementBlock);
            _.each(expressionBlock.availableChildren, extendElementBlock);
        }

        function stripOffUiInformation(expressionElement) {
            expressionElement.availableChildren = undefined;
            expressionElement.displayName = undefined;
            expressionElement.getValidationError = undefined;
            expressionElement.newChildLabel = undefined;
            expressionElement.templateURL = undefined;

            var selectedCategories = _.where(expressionElement.children, { id: 'ExcludingCategoryCondition' });
            expressionElement.excludingCategoryIds = _.pluck(selectedCategories, 'categoryId');
            expressionElement.children = _.difference(expressionElement.children, selectedCategories);

            var selectedProducts = _.where(expressionElement.children, { id: 'ExcludingProductCondition' });
            expressionElement.excludingProductIds = _.pluck(selectedProducts, 'productId');
            expressionElement.children = _.difference(expressionElement.children, selectedProducts);

            _.each(expressionElement.children, stripOffUiInformation);
        }


        //$scope.$watch('blade.currentEntity.endDate',
        //    function (newValue, oldValue, scope) {
        //        if (newValue != undefined && (newValue instanceof Date)) {
        //            debugger;
        //            //var date = angular.copy(blade.currentEntity.endDate)
        //            //date = moment(date).add({ hours: 23, minutes: 59, seconds: 59 }).toDate();
        //            //newValue = moment(newValue).add({ hours: 23, minutes: 57, seconds: 59 }).toDate();
        //            newValue.setHours(23, 59, 59);

        //        }
        //    }
        //);

        initializeToolbar();

        /// TODO: refactor to new va-marketing-stores-selector directive.
        // PageSize amount must be enough to show scrollbar in dropdown list container.
        // If scrollbar doesn't appear auto loading won't work.
        $scope.pageSize = 25;
        $scope.stores = [];
        var lastSearchPhrase = '';
        var totalCount = 0;

        $scope.fetchStores = function ($select) {
            $q.all([loadPromotionStores(), $scope.fetchNextStores($select)]);
        };

        function loadPromotionStores() {
            if (_.any(blade.currentEntity.storeIds) && !_.any($scope.stores)) {
                return stores.search({
                    storeIds: blade.currentEntity.storeIds,
                    take: blade.currentEntity.storeIds.length,
                    responseGroup: 'none'
                }, (data) => {
                    joinStores(data.results);
                }).$promise;
            }

            return $q.resolve();
        }

        $scope.fetchNextStores = ($select) => {
            $select.page = $select.page || 0;

            if (lastSearchPhrase !== $select.search) {
                lastSearchPhrase = $select.search;
                $select.page = 0;
            }

            if ($select.page === 0 || totalCount > $scope.stores.length) {
                return stores.search(
                    {
                        searchPhrase: $select.search,
                        take: $scope.pageSize,
                        skip: $select.page * $scope.pageSize,
                        responseGroup: 'none'
                    }, (data) => {
                        joinStores(data.results);
                        $select.page++;

                        if ($select.page * $scope.pageSize < data.totalCount) {
                            $scope.$broadcast('scrollCompleted');
                        }

                        totalCount = Math.max(totalCount, data.totalCount);
                    }).$promise;
            }

            return $q.resolve();
        };

        function joinStores(newItems) {
            newItems = _.reject(newItems, x => _.any($scope.stores, y => y.id === x.id));

            _.each(newItems, function (store) {
                // VP-5647: Left for backward compatibility, as virtoCommerce.dynamicExpressions.shippingMethodRewardController in Core module expects these properties to be filled in the store
                store.shippingMethods = blade.shippingMethods || [];
                store.paymentMethods = blade.paymentMethods || [];
            });

            $scope.stores = $scope.stores.concat(newItems);
        }

        blade.refresh(false);

        $scope.exclusivityTypes = [{ name: 'marketing.blades.promotion-detail.labels.combined-with-others', value: false }, { name: 'marketing.blades.promotion-detail.labels.exclusive', value: true }];
    }]);
