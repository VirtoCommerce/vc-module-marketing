// Call this to register your module to main application
var moduleName = "virtoCommerce.marketingSample";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['virtoCommerce.coreModule.common.dynamicExpressionService', '$http', '$compile',
        function (dynamicExpressionService, $http, $compile) {
            //Register Sample expressions
            dynamicExpressionService.registerExpression({
                id: 'BlockSampleCondition',
                newChildLabel: '+ add sample condition',
                getValidationError: function () {
                    return (this.children && this.children.length) ? undefined : 'Promotion requires at least one eligibility';
                }
            });
            dynamicExpressionService.registerExpression({
                id: 'SampleCondition',
                displayName: 'Sample condition is []'
            });

            $http.get('Modules/$(VirtoCommerce.MarketingSample)/Scripts/all-templates.html').then(function (response) {
                // compile the response, which will put stuff into the cache
                $compile(response.data);
            });
        }
    ]);
