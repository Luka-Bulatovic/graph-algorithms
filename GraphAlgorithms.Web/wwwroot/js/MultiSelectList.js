var MultiSelectList = new function () {
    this.Initialize = function (viewDataObj) {
        // Show/hide menu on button click
        viewDataObj.btnDropdown.on('click', () => {
            viewDataObj.dropdownMenu.toggleClass('hidden');
        });

        // Hide dropdown menu when clicking outside of MultiSelectList
        $(document).on('click', function (e) {
            if (!viewDataObj.btnDropdown.has(e.target).length
                && !viewDataObj.dropdownMenu.has(e.target).length > 0
                && !viewDataObj.btnDropdown.is($(e.target))
                && !viewDataObj.dropdownMenu.is($(e.target))
            ) {
                viewDataObj.dropdownMenu.addClass('hidden');
            }
        });

        // Handle changing checkboxes in dropdown
        viewDataObj.dropdownItems.find('input[type="checkbox"]').on('change', function (e) {
            MultiSelectList.onDropdownItemChanged(viewDataObj, e);
        });

        // Handle requests for selected items
        EventBus.on('getMultiSelectSelectedItems', (e) => MultiSelectList.onGetMultiSelectSelectedItems(viewDataObj, e));
    }

    this.onDropdownItemChanged = function (viewDataObj, e) {
        var itemObj = $(e.target);
        var value = $(e.target).val();

        // Add/remove item to selected items
        if (itemObj.is(':checked'))
            viewDataObj.selectedItems.push(value);
        else {
            let idx = viewDataObj.selectedItems.indexOf(value);
            if (idx > -1)
                viewDataObj.selectedItems.splice(idx, 1);
        }

        // Construct string of all selected item names
        var selectedItemsNames = '';
        for (let i = 0; i < viewDataObj.selectedItems.length; i++) {
            let filteredItems = viewDataObj.items.filter((it) => it.key == viewDataObj.selectedItems[i]);
            let item = null;

            if (filteredItems.length > 0)
                item = filteredItems[0];

            if (item != null)
                selectedItemsNames += (selectedItemsNames.length > 0 ? ', ' : '') + item.value;
        }

        viewDataObj.selectedItemsNames = selectedItemsNames;
    }

    this.onGetMultiSelectSelectedItems = function (viewDataObj, e) {
        if (e.detail.id != viewDataObj.multiSelectID)
            return;

        EventBus.emit('getMultiSelectSelectedItemsResponse', {
            id: viewDataObj.multiSelectID,
            selectedItemIDs: viewDataObj.selectedItems.join('$'),
            selectedItemsNames: viewDataObj.selectedItemsNames
        });
    }
}