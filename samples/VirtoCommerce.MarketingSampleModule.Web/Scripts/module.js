// Call this to register your module to main application
var moduleName = "virtoCommerce.marketingSample";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['virtoCommerce.coreModule.common.dynamicExpressionService', 'platformWebApp.dynamicTemplateService',
        function (dynamicExpressionService, dynamicTemplateService) {
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

            dynamicTemplateService.ensureTemplateLoaded('Modules/$(VirtoCommerce.MarketingSample)/Scripts/all-templates.html');
        }
    ]);
