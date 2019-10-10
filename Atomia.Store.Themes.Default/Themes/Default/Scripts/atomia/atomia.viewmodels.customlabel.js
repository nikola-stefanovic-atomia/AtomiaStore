/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    /** Create custom label view model.
     * @param {Object} cart - Instance of cart to operate on.
     * @param {Object} selectedItem - Item to to set label to. Can be an observable.
     */
    function CustomLabelModel(cart, selectedItem) {
        var self = this;

        self.selectedItem = _.isFunction(selectedItem) ? selectedItem : function () { return selectedItem; };
        self.selectedItemLabel = ko.observable();
        self.uniqueId = _.uniqueId('label-');

        /** Set label from cart to 'SelectedItem'. */
        self.setInitialLabel = function setInitialLabel() {
            var selectedItem = self.selectedItem(),
                itemInCart;

            if (selectedItem !== undefined) {
                itemInCart = selectedItem.getItemInCart();
            }

            if (itemInCart && itemInCart.attrs.label) {
                self.selectedItemLabel(itemInCart.attrs.label);
            }
        };

        self._handleLabelChanged = function handleLabelChanged(newLabelValue) {
            var selectedItem = self.selectedItem();

            if (selectedItem === undefined || !selectedItem.isInCart()) {
                return;
            }

            if (newLabelValue !== undefined && newLabelValue.length > 0) {
                cart.addLabel(selectedItem, newLabelValue);
            }
            else {
                cart.removeLabel(selectedItem());
            }
        }

        self.preventSubmit = function () {
            return false;
        };

        /** Update hosting label when an hosting item ('addedItem') is added to cart. */
        utils.subscribe('cart:add', function(addedItem) {
            var selectedItem = self.selectedItem();

            if (selectedItem.equals(addedItem)) {
                cart.addLabel(addedItem, self.selectedItemLabel());
            }
        });

        /** Update hosting label when an hosting item ('removedItem') is removed from cart. */
        utils.subscribe('cart:remove', function (removedItem) {
            var selectedItem = self.selectedItem();
            if (selectedItem.isInCart() && selectedItem.equals(removedItem)) {
                cart.removeLabel(removedItem);
            }
        });

        self.setInitialLabel();
        self.selectedItemLabel.subscribe(self._handleLabelChanged);
    }

    /* Module exports */
    _.extend(exports, {
        CustomLabelModel: CustomLabelModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
