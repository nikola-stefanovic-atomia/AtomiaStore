/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko) {
    'use strict';

    function CustomerOrderAccountSelectorModel(model) {

        var self = this;

        self.newAccountType = model.newAccountType;
        self.existingAccountType = model.existingAccountType;

        self.selectorVisible = ko.observable(model.allowExistingCustomerOrders);
        self.selectedAccountType = ko.observable(model.selectedOrderAccounType);

        self.existingAccountFormVisible = ko.pureComputed(function () {
            return self.selectedAccountType() == self.existingAccountType;
        }, self);

        self.newAccountFormVisible = ko.pureComputed(function () {
            return self.selectedAccountType() == self.newAccountType;
        }, self);

    }

    _.extend(exports, {
        CustomerOrderAccountSelectorModel: CustomerOrderAccountSelectorModel
    });

})(Atomia.ViewModels, _, ko);
