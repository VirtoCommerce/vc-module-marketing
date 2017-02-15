angular.module('virtoCommerce.marketingModule')
.factory('virtoCommerce.marketingModule.promotions', ['$resource', function ($resource) {
    return $resource('api/marketing/promotions/:id', null, {
        search: { url: 'api/marketing/promotions/search', method: 'POST' },
        getNew: { url: 'api/marketing/promotions/new' },
        update: { method: 'PUT' }
    });
}]);