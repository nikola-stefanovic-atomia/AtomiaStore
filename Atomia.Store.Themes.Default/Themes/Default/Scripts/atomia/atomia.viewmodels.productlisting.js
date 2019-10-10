/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils, viewModels) {
    'use strict';

    /**
     * Creates product listing item
     * @param {Object} productData - The instance to create product listing item from.
     * @param {Object} cart - Instance of cart.
     */
    function ProductListingItem(productData, cart) {
        var self = this;

        _.extend(self, new viewModels.ProductMixin(productData, cart, viewModels));

    }


    /**
     * Create product listing.
     * @param {Object} cart - An instance of cart view model.
     */
    function ProductListingModel(cart) {
        var self = this;

        self.productIsRequired = true;
        self.singleSelection = false;
        self.products = ko.observableArray();
        self.selectedProduct = ko.observable();
        self.isPackageLabelEnabled = false;
        /** Check if product is in cart. */
        self.productIsSelected = ko.pureComputed(function() {
            return _.any(self.products(), function (product) {
                return cart.contains(product);
            });
        });

        /** Check if order flow is allowed to continue to next step. */
        self.allowContinue = ko.pureComputed(function() {
            var conditions = [];

            if (self.productIsRequired) {
                conditions.push(self.productIsSelected());
            }

            return _.every(conditions);
        });
        /** Create ProductListingItem object. */
        self.createProductListingItem = function CreateProductListingItem(productData){
            return new ProductListingItem(productData, cart);
        };

        /** Select product and update cart. */
        self.selectProduct = function SelectProduct(item) {
            if (self.singleSelection) {
                self.selectedProduct(item);

                _.each(self.products(), function (product) {
                    if (cart.contains(product)) {
                        cart.remove(product, false);
                    }
                });
            }

            cart.add(item);
        };

        /** Load view model with product listing data generated on server. */
        self.load = function load(response) {
            var products = response.data.CategoryData.Products;

            _.each(products, function (product) {
                var item = self.createProductListingItem(product);

                if (cart.contains(item)) {
                    item.initPricingVariant();
                    if (self.singleSelection) {
                        self.selectProduct(item);
                    }
                }

                self.products.push(item);
            });
        };
    }

    _.extend(exports, {
        ProductListingItem: ProductListingItem,
        ProductListingModel: ProductListingModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils, Atomia.ViewModels);
