/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils, cartApi) {
    'use strict';

    function toCartApiData(cart) {
        var apiCart = {
            CampaignCode: cart.campaignCode(),
            CartItems: [],
            CustomAttributes: []
        };

        _.each(cart.attrs(), function (attr) {
            var name,
                value = attr.value,
                key = attr.name;

            if (value != null) {
                name = cart._origAttrNames[key] || key[0].toUpperCase() + key.slice(1);

                apiCart.CustomAttributes.push({
                    Name: name,
                    Value: value
                });
            }
        });

        _.each(cart.cartItems(), function (cartItem) {
            var apiItem = {
                ArticleNumber: ko.unwrap(cartItem.articleNumber),
                Quantity: ko.unwrap(cartItem.quantity) || 1,
                CustomAttributes: []
            };

            var renewalPeriod = ko.unwrap(cartItem.renewalPeriod);

            if (renewalPeriod) {
                apiItem.RenewalPeriod = {
                    Period: renewalPeriod.period,
                    Unit: renewalPeriod.unit
                };
            }
            else {
                apiItem.RenewalPeriod = null;
            }

            _.each(cartItem.attrs, function (value, key) {
                var name;

                if (value != null) {
                    // Original casing attribute name, or PascalCase.
                    name = cartItem._origAttrNames[key] || key[0].toUpperCase() + key.slice(1);

                    apiItem.CustomAttributes.push({
                        Name: name,
                        Value: value
                    });
                }
            });

            apiCart.CartItems.push(apiItem);
        });

        return apiCart;
    }

    /** Update 'cart' with 'cartData'. */
    function updateCart(cart, cartData) {
        var cartItems = [], cartTaxes = [], cartAttrs = [];

        _.each(cartData.CartItems, function (cartItemData) {
            var item, cartItem;
            var itemData = {
                articleNumber: cartItemData.ArticleNumber,
                name: punycode.toUnicode(cartItemData.Name),
                description: cartItemData.Description,
                categories: _.pluck(cartItemData.Categories, 'Name'),
                categoryDescription: _.pluck(cartItemData.Categories, 'Description').join(', '),
                quantity: cartItemData.Quantity,
                price: cartItemData.Price,
                taxes: [],
                discount: cartItemData.Discount,
                total: cartItemData.Total,
                renewalPeriod: null,
                attrs: {},
                _origAttrNames: {}
            };

            if (cartItemData.RenewalPeriod) {
                itemData.renewalPeriod = {
                    period: cartItemData.RenewalPeriod.Period,
                    unit: cartItemData.RenewalPeriod.Unit,
                    toString: function () {
                        return cartItemData.RenewalPeriod.Display;
                    }
                };
            }

            _.each(cartItemData.Taxes, function (tax) {
                itemData.taxes.push({
                    name: tax.Name,
                    amount: tax.Amount,
                    percentage: tax.Percentage
                });
            });

            _.each(cartItemData.CustomAttributes, function (attr) {
                var name = attr.Name[0].toLowerCase() + attr.Name.slice(1);

                itemData.attrs[name] = attr.Value;

                // Save the attribute name with original casing, to be able to send it back to api that way.
                itemData._origAttrNames[name] = attr.Name;
            });

            item = cart.createCartItem(itemData);
            cartItem = addCartItemExtensions(cart, item);

            cartItems.push(cartItem);
        });

        _.each(cartData.Taxes, function (tax) {
            cartTaxes.push({
                name: tax.Name,
                amount: tax.Amount,
                percentage: tax.Percentage
            });
        });

        _.each(cartData.CustomAttributes, function (attr) {
            var name = attr.Name[0].toLowerCase() + attr.Name.slice(1);

            cartAttrs.push({name: name, value: attr.Value});

            // Save the attribute name with original casing, to be able to send it back to api that way.
            cart._origAttrNames[name] = attr.Name;
        });

        cart.attrs(cartAttrs);
        cart.cartItems(cartItems);
        cart.subTotal(cartData.SubTotal);
        cart.total(cartData.Total);
        cart.taxes(cartTaxes);
        cart.campaignCode(cartData.CampaignCode);
        cart.pricesIncludeVat(cartData.PricesIncludeVat);

        utils.publish('cart:update');
    }

    /**
     * Create a cart item view model.
     * @param {Object} item - The item to create cart item from.
     * @returns The created cart item.
     */
    function CartItem(item) {
        var self = this;

        self.id = _.uniqueId('tmp-cartitem-');
        
        /** 
         * Equality comparer between this item and 'other'. Defaults to comparing 'Id' properties. 
         * @param {Object} other - The item to compare to.
         */
        self.equals = function equals(other) {
            return self.id === other.id;
        };

        /** 
         * Collects custom options (renewal period and domain name) of item.
         * @returns {string[]} Array of custom options.
         */
        self.customOptions = function customOptions() {
            var options = [];

            if (self.renewalPeriod) {
                options.push(self.renewalPeriod);
            }

            if (!self.isDomainItem() && self.attrs.domainName !== undefined) {
                options.push(self.attrs.domainName);
            }

            return options;
        };

        _.extend(self, item);
    }

    
    /** Create view model for cart. */
    function CartModel(notification) {
        var self = this;
        
        self.domainCategories = [];
        self.cartItems = ko.observableArray();
        self.subTotal = ko.observable(0);
        self.total = ko.observable(0);
        self.taxes = ko.observableArray();
        self.campaignCode = ko.observable('');
        self.pricesIncludeVat = ko.observable(false);
        self.attrs = ko.observableArray();
        self._origAttrNames = {};
        
        self.numberOfItems = ko.pureComputed(function numberOfItems() {
            return self.cartItems().length;
        });
        
        self.isEmpty = ko.pureComputed(function isEmpty() {
            return self.numberOfItems() <= 0;
        });

        self.createCartItem = function createCartItem(item) {
            return new CartItem(item);
        };

        /** Items in cart that are domain items, like registration or transfer */
        self.domainItems = function domainItems() {
            return _.filter(self.cartItems(), function (item) {
                var i = _.intersection(self.domainCategories, item.categories);

                return i.length > 0;
            });
        };

        /** Distinct article numbers of items in cart. */
        self.articleNumbers = function articleNumbers() {
            return _.uniq(_.map(self.cartItems(), function (item) {
                return item.articleNumber;
            }));
        };

        /** Distinct categories of items in cart. */
        self.categories = function categories() {
            return _.union.apply(_, _.pluck(self.cartItems(), 'categories'));
        };

        /** Check if cart contains item that 'equals' 'item'. */
        self.contains = function contains(item) {
            var isInCart = _.any(self.cartItems(), function (cartItem) {
                return item.equals(cartItem);
            });

            return isInCart;
        };

        /** Get first item in cart that 'equals' 'item'. */
        self.getExisting = function getExisting(item) {
            var itemInCart = _.find(self.cartItems(), function (cartItem) {
                return item.equals(cartItem);
            });

            return itemInCart;
        };

        /** Add 'item' to cart and recalculate. Optionally set 'recalculate' to false. */
        self.add = function add(item, recalculate) {
            var cartItem;

            if (recalculate === undefined) {
                recalculate = true;
            }

            // Placeholder values if item is added in view where template of cart use bindings on any
            // of these properties. They will be quickly replaced when the real CartItem from recalculate
            // replaces the temporary local value.
            item.categories = item.categories || [];
            item.discount = item.discount || '';
            item.total = item.total || '';
            item.quantity = item.quantity || 1;
            item.taxes = item.taxes || [];

            if (!self.contains(item)) {
                cartItem = self.createCartItem(item);
                self.cartItems.push(cartItem);

                if (recalculate === true) {
                    cartApi.recalculateCart(toCartApiData(self), function (result) {
                        updateCart(self, result.Cart);

                        utils.publish('cart:add', cartItem);
                    });
                }
            }
        };

        /** Remove 'item' from cart and recalculate. Optionally set 'recalculate' to false. */
        self.remove = function remove(item, recalculate) {
            var itemInCart;

            if (recalculate === undefined) {
                recalculate = true;
            }

            if (self.contains(item)) {
                itemInCart = _.find(self.cartItems(), function (cartItem) {
                    return item.equals(cartItem);
                });

                if (itemInCart !== undefined) {
                    self.cartItems.remove(itemInCart);

                    if (recalculate === true) {
                        cartApi.recalculateCart(toCartApiData(self), function (result) {
                            updateCart(self, result.Cart);

                            utils.publish('cart:remove', itemInCart);

                            if (itemInCart.isDomainItem()) {
                                self.clearDomainItem(itemInCart);
                            }
                        });
                    }
                }
            }
        };

        /** Add 'item' to or remove 'item' from cart and recalculate. Optionally set 'recalculate' to false. */
        self.addOrRemove = function addOrRemove(item, recalculate) {
            self.contains(item) ? self.remove(item, recalculate) : self.add(item, recalculate);
        };

        /** Associate 'domainName' to 'mainItem'. Optionally set 'recalculate' to false. */
        self.addDomainName = function addDomainName(mainItem, domainName, recalculate) {
            var mainInCart = self.getExisting(mainItem);

            if (recalculate === undefined) {
                recalculate = true;
            }

            if (mainInCart === undefined) {
                throw new Error('mainItem is not in cart.');
            }

            if (mainInCart.attrs.domainName !== domainName) {
                mainInCart.attrs.domainName = domainName;

                if (recalculate === true) {
                    cartApi.recalculateCart(toCartApiData(self), function (result) {
                        updateCart(self, result.Cart);
                    });
                }
            }
        };


        self.hasHostingPackage = function hasHostingPackage() {
            var itemInCart = _.find(self.cartItems(), function (cartItem) {
                return cartItem.categories.includes("HostingPackage");
            });

            if (itemInCart != undefined) {
                return true;
            }
            else {
                utils.publish("uiSetNotification", {
                    title: notification.title,
                    message: notification.message,
                    messageType: 'warning'
                });
                return false;
            }
        }

        /** Remove any domain name associations from 'mainItem'. Optionally set 'recalculate' to false.*/
        self.removeDomainName = function removeDomainName(mainItem, recalculate) {
            var mainInCart = self.getExisting(mainItem);

            if (recalculate === undefined) {
                recalculate = true;
            }

            if (mainInCart === undefined) {
                throw new Error('mainItem is not in cart.');
            }

            if (mainInCart.attrs.domainName !== undefined) {
                mainInCart.attrs.domainName = undefined;

                if (recalculate === true) {
                    cartApi.recalculateCart(toCartApiData(self), function (result) {
                        updateCart(self, result.Cart);
                    });
                }
            }
        };

        /** Remove any associations to domain name tied to 'domainItem' from any other items in cart. */
        self.clearDomainItem = function ClearDomainItem(domainItem) {
            var domainNameToRemove = domainItem.attrs.domainName;

            _.each(self.cartItems(), function (item) {
                if (!domainItem.equals(item) && item.attrs.domainName === domainNameToRemove) {
                    self.removeDomainName(item);
                }
            });
        };

        /** Load cart with JSON data from 'getCartResponse'. */
        self.load = function load(getCartResponse) {
            updateCart(self, getCartResponse.data.Cart);

            if (getCartResponse.data.DomainCategories !== undefined) {
                self.domainCategories = getCartResponse.data.DomainCategories;
            }
        };

        /** Add 'campaignCode' to cart. Replaces any existing campaign code and recalculates cart. */
        self.addCampaignCode = function addCampaignCode(campaignCode) {
            if (!_.isString(campaignCode) || campaignCode === '') {
                throw new Error('campaignCode must be a non-empty string.');
            }

            self.campaignCode(campaignCode);

            cartApi.recalculateCart(toCartApiData(self), function (result) {
                updateCart(self, result.Cart);

                utils.publish('cart:addCampaignCode', campaignCode);
            });
        };

        /** Remove campaign code from cart and recalculate. */
        self.removeCampaignCode = function removeCampaignCode() {
            self.campaignCode('');

            cartApi.recalculateCart(toCartApiData(self), function (result) {
                updateCart(self, result.Cart);

                utils.publish('cart:removeCampaignCode');
            });
        };

        /** Manually trigger recalculate */
        self.recalculate = function recalculate() {
            cartApi.recalculateCart(toCartApiData(self), function (result) {
                updateCart(self, result.Cart);
            });
        };

        self.addAttr = function addAttr(name, value, recalculate) {
            if (!_.isString(name) || name === '') {
                throw new Error('name must be a non-empty string.');
            }

            if (!_.isString(value) || value === '') {
                throw new Error('value must be a non-empty string.');
            }

            if (recalculate === undefined) {
                recalculate = true;
            }

            self.attrs.push({ name: name, value: value });

            if (recalculate === true) {
                cartApi.recalculateCart(toCartApiData(self), function (result) {
                    updateCart(self, result.Cart);

                    utils.publish('cart:addAttr', { name: name, value: value });
                });
            }
        };

        self.removeAttr = function removeAttr(name, value, recalculate) {
            if (!_.isString(name) || name === '') {
                throw new Error('name must be a non-empty string.');
            }

            if (!_.isString(value) || value === '') {
                throw new Error('value must be a non-empty string.');
            }

            if (recalculate === undefined) {
                recalculate = true;
            }

            self.attrs.remove(function (attr) {
                return name === self._origAttrNames[attr.name] && value === attr.value;
            });

            if (recalculate === true) {
                cartApi.recalculateCart(toCartApiData(self), function (result) {
                    updateCart(self, result.Cart);

                    utils.publish('cart:removeAttr', { name: name, value: value });
                });
            }
        };

        self.addUpdateAttr = function (name, value, recalculate) {
            if (!_.isString(name) || name === '') {
                throw new Error('name must be a non-empty string.');
            }

            if (!_.isString(value) || value === '') {
                throw new Error('value must be a non-empty string.');
            }

            if (recalculate === undefined) {
                recalculate = true;
            }

            self.attrs.remove(function (attr) {
                return name === self._origAttrNames[attr.name];
            });
            self.attrs.push({ name: name, value: value });

            if (recalculate === true) {
                cartApi.recalculateCart(toCartApiData(self), function (result) {
                    updateCart(self, result.Cart);
                    utils.publish('cart:addUpdateAttr', { name: name, value: value });
                });
            }
        };
    }

    /** 
     * Extends 'item' with cart helper methods.
     * @param {Object} cart - The cart to apply helpers to.
     * @param {Object} item - The item to extend with helper methods.
     * @returns {Object} The extended item.
     */
    function addCartItemExtensions(cart, item) {
        var cartExtensions = {
            isInCart: ko.computed(function isInCart() {
                return cart.contains(item);
            }).extend({ notify: 'always' }),
            
            getItemInCart: function getItemInCart() {
                return cart.getExisting(item);
            },

            addToCart: function addToCart() {
                return cart.add(item);
            },

            removeFromCart: function removeFromCart() {
                return cart.remove(item);
            },

            toggleInCart: function toggleInCart() {
                cart.addOrRemove(item);

                // Since this is expected to be used with checkboxes, 
                // returning true allows the click to continue and check the checkbox
                return true;
            },

            isDomainItem: function isDomainItem() {
                return _.intersection(cart.domainCategories, item.categories).length > 0;
            }
        };

        return _.extend(item, cartExtensions);
    }

    
    /* Module exports */
    _.extend(exports, {
        CartItem: CartItem,
        CartModel: CartModel,
        addCartItemExtensions: addCartItemExtensions
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils, Atomia.Api.Cart);
