var Search = new function () {
    this.SearchFieldTypes = {
        Text: 1,
        NumberRange: 2,
        Number: 3,
        DateRange: 4,
        MultiSelectList: 5,
        SelectList: 6
    };

    this.Initialize = function (viewDataObj) {
        // Search button
        viewDataObj.btnSearch.on('click', (e) => {
            Search.onSearch(viewDataObj);
        });

        // Add Param button
        viewDataObj.btnAddParam.on('click', (e) => {
            Search.onAddParam(viewDataObj);
        });

        // Search By changed
        viewDataObj.slSearchBy.on('change', (e) => {
            Search.onSearchByChanged(viewDataObj);
        });
    }

    this.onSearch = function (viewDataObj) {
        var searchParamsString = '';

        viewDataObj.currSearchParams.forEach((param, index) => {
            searchParamsString += `searchParams[${index}].Key=${encodeURIComponent(param.id)}&`;
            searchParamsString += `searchParams[${index}].FieldType=${encodeURIComponent(param.paramType)}&`;
            param.values.forEach((value, valueIndex) => {
                searchParamsString += `searchParams[${index}].Values[${valueIndex}]=${encodeURIComponent(value)}&`;
            });
        });

        searchParamsString = searchParamsString.slice(0, -1);

        window.location.href = `/GraphLibrary?${searchParamsString}`;
    }

    this.onAddParam = function (viewDataObj) {
        var paramID = viewDataObj.slSearchBy.val();

        if (paramID == -1) {
            return;
        }

        var searchField = viewDataObj.searchFields.filter(f => f.KeyValue == paramID)[0];

        var newParam = {
            id: paramID,
            displayName: searchField.DisplayValue,
            paramType: searchField.FieldType,
            values: []
        };

        switch (searchField.FieldType) {
            case Search.SearchFieldTypes.Text:
                newParam.values.push(viewDataObj.fldText.val());
                break;
            case Search.SearchFieldTypes.NumberRange:
                newParam.values.push(viewDataObj.fldNumberRangeFrom.val());
                newParam.values.push(viewDataObj.fldNumberRangeTo.val());
                break;
            case Search.SearchFieldTypes.Number:
                newParam.values.push(viewDataObj.fldNumber.val());
                break;
            case Search.SearchFieldTypes.DateRange:
                newParam.values.push(viewDataObj.fldDateRangeFrom.val());
                newParam.values.push(viewDataObj.fldDateRangeTo.val());
                break;
            default:
                break;
        }

        var existingParamIndex = viewDataObj.currSearchParams.findIndex(p => p.id == newParam.id);

        if (existingParamIndex > -1)
            viewDataObj.currSearchParams.splice(existingParamIndex, 1);

        viewDataObj.currSearchParams.push(newParam);

        Search.redrawSelectedParameters(viewDataObj);
    }

    this.onSearchByChanged = function (viewDataObj) {
        viewDataObj.fldTextContainer.addClass("hidden");
        viewDataObj.fldNumberContainer.addClass("hidden");
        viewDataObj.fldNumberRangeContainer.addClass("hidden");
        viewDataObj.fldDateRangeContainer.addClass("hidden");

        var paramID = viewDataObj.slSearchBy.val();

        if (paramID == -1)
            return;

        var searchField = viewDataObj.searchFields.filter(f => f.KeyValue == paramID)[0];

        if (searchField == null)
            return;

        switch (searchField.FieldType) {
            case Search.SearchFieldTypes.Text:
                viewDataObj.fldText.val('');
                viewDataObj.fldTextContainer.removeClass("hidden");
                break;
            case Search.SearchFieldTypes.NumberRange:
                viewDataObj.fldNumberRangeFrom.val('');
                viewDataObj.fldNumberRangeTo.val('');
                viewDataObj.fldNumberRangeContainer.removeClass("hidden");
                break;
            case Search.SearchFieldTypes.Number:
                viewDataObj.fldNumber.val('');
                viewDataObj.fldNumberContainer.removeClass("hidden");
                break;
            case Search.SearchFieldTypes.DateRange:
                viewDataObj.fldDateRangeFrom.val(null);
                viewDataObj.fldDateRangeTo.val(null);
                viewDataObj.fldDateRangeContainer.removeClass("hidden");
                break;
            default:
                break;
        }
    }

    this.redrawSelectedParameters = function (viewDataObj) {
        viewDataObj.selectedParamsContainer.empty();

        viewDataObj.currSearchParams.forEach((e, i) => {
            let currElement = $(`
            <div><span class="font-semibold">${i + 1}. ${e.displayName}:</span> ${e.values[0]} ${e.values.length > 1 ? " - " + e.values[1] : ""}</div>
            <div class="col-span-2"><i class="fa-solid fa-xmark text-red-500"></i></div>`);

            viewDataObj.selectedParamsContainer.append(currElement);
        });
    }
};