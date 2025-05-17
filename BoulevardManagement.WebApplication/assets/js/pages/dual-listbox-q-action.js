'use strict';

// Class definition
var KTDualListbox = function () {

    // Private functions
    var initDualListbox = function () {
        // Dual Listbox
        var listBoxes = $('.kt-dual-listbox-q-action');

        listBoxes.each(function () {
            var $this = $(this);
            // get titles
            var availableTitle = ($this.attr('data-available-title') != null) ? $this.attr('data-available-title') : 'Available actions';
            var selectedTitle = ($this.attr('data-selected-title') != null) ? $this.attr('data-selected-title') : 'Selected actions';

            // get button labels
            var addLabel = ($this.attr('data-add') != null) ? $this.attr('data-add') : 'Add';
            var removeLabel = ($this.attr('data-remove') != null) ? $this.attr('data-remove') : 'Remove';
            var addAllLabel = ($this.attr('data-add-all') != null) ? $this.attr('data-add-all') : 'Add first Eight';
            var removeAllLabel = ($this.attr('data-remove-all') != null) ? $this.attr('data-remove-all') : 'Remove All';

            // get options
            var options = [];
            $this.children('option').each(function () {
                var value = $(this).val();
                var label = $(this).text();
                var selected = !!($(this).is(':selected'));
                options.push({ text: label, value: value, selected: selected });
            });

            // get search option
            var search = ($this.attr('data-search') != null) ? $this.attr('data-search') : '';

            // clear duplicates
            $this.empty();

            var selecte
            // init dual listbox
            var dualListBox = new DualListbox($this.get(0), {
                addEvent: function (value) {

                    selectedActions = []
                    for (var i = 0; i < this.selected.length; i++) {
                        selectedActions.push(parseInt(this.selected[i].dataset["id"]));
                    }



                },
                removeEvent: function (value) {

                    selectedActions = []
                    for (var i = 0; i < this.selected.length; i++) {
                        selectedActions.push(parseInt(this.selected[i].dataset["id"]));
                    }

                },
                availableTitle: availableTitle,
                selectedTitle: selectedTitle,
                addButtonText: addLabel,
                removeButtonText: removeLabel,
                addAllButtonText: addAllLabel,
                removeAllButtonText: removeAllLabel,
                options: options,
            });

            if (search == 'false') {
                dualListBox.search.classList.add('dual-listbox__search--hidden');
            }
        });
    };

    return {
        // public functions
        init: function () {
            initDualListbox();
        },
    };
}();

KTUtil.ready(function () {
    KTDualListbox.init();
});
