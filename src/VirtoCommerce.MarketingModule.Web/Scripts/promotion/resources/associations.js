angular.module('virtoCommerce.marketingModule')
    .factory('virtoCommerce.marketingModule.associations', ['$resource', function ($resource) {
        return $resource('api/marketing/associations/:id', null, {
            search: { url: 'api/marketing/associations/search', method: 'POST' }
        });
    }]);
