/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko) {
	'use strict';

    /** Create a Knockout view model for validating login details of existing customer. */
    function ExistingCustomerModel() {
        var self = this;
            
        self.username = ko.observable();
        self.password = ko.observable();
	}

	/* Module exports */
	_.extend(exports, {
	    ExistingCustomerModel: ExistingCustomerModel
	});

})(Atomia.ViewModels, _, ko);
