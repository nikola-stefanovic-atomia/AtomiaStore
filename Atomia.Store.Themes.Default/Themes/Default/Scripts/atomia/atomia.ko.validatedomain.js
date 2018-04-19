(function ($, ko) {
    'use strict';

    /** Show or hide element with a jQuery slide animation. Binding option 'slideDuration' sets animation time in ms. */
    ko.bindingHandlers.validateDomain = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var name = 'ValidateDomain';
            var message = Atomia.RESX.validateDomainMessage;
            var validate = function (value, element, params) {
                var re = new RegExp(Atomia.RESX.validateDomainRegex);

                var idnDecoded = punycode.toUnicode(value);
                if (idnDecoded !== '' && !re.test(idnDecoded)) {
                    return false;
                }

                return true;
            };

            setTimeout(function () {
                $(element).parents('form:first').validate();
                var $formrow = $(element).parents('.Formrow:first');

                $.validator.addMethod(name, function (value, element, params) {
                    return validate(value, element, params);
                });

                var ruleOpts = {
                    messages: {}
                };

                ruleOpts[name] = true; // Activates the rule.
                ruleOpts.messages[name] = message;

                $(element).rules('add', ruleOpts);

            }, 250);
        }
    };

}(jQuery, ko));