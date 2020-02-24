var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};

(function (exports, _, ko, utils, checkoutApi) {
    'use strict';

    function Recaptcha(reCaptchaPlaceholderId, siteKey) {
        var self = this;

        window.render = function () {
            grecaptcha.render(reCaptchaPlaceholderId, {
                'sitekey': siteKey,
                theme: 'light',
                type: 'image',
                size: 'normal'
            });
        };
    }

    _.extend(exports, {
        Recaptcha: Recaptcha
    });
})(Atomia.ViewModels, _, ko, Atomia.Utils, Atomia.Api.Checkout);