var Search = new function () {
    this.SearchParamTypes = {
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

        // Clear all Search Params button
        viewDataObj.btnClearParams.on('click', (e) => {
            Search.clearSearchParams(viewDataObj);
        });

        viewDataObj.container.on(
            'click',
            '.selected-parameters-container .btn-delete-param',
            { viewDataObj: viewDataObj },
            (e) => {
                Search.deleteSearchParam(e.data.viewDataObj, $(e.target).parent().attr("data-index"));
            }
        )

        Search.initializeSelectedSearchParams(viewDataObj);
    }

    this.onSearch = function (viewDataObj) {
        let params = {
            sortBy: viewDataObj.slSortBy.val(),
            searchParams: []
        };

        viewDataObj.currSearchParams.forEach((param, index) => {
            params.searchParams[index] = { Key: '', Values: [] };
            params.searchParams[index].Key = param.id;
            param.values.forEach((value, valueIndex) => {
                params.searchParams[index].Values.push(value);
            });
        });
        //var searchParamsString = '';

        //searchParamsString += `sortBy=${encodeURIComponent(viewDataObj.slSortBy.val())}&`;

        //viewDataObj.currSearchParams.forEach((param, index) => {
        //    searchParamsString += `searchParams[${index}].DisplayName=${encodeURIComponent(param.displayName)}&`;
        //    searchParamsString += `searchParams[${index}].Key=${encodeURIComponent(param.id)}&`;
        //    searchParamsString += `searchParams[${index}].AllowMultipleValues=${encodeURIComponent(param.allowMultipleValues)}&`;
        //    searchParamsString += `searchParams[${index}].ParamType=${encodeURIComponent(param.paramType)}&`;
        //    searchParamsString += `searchParams[${index}].DisplayValues=${encodeURIComponent(param.displayValues)}&`;
        //    param.values.forEach((value, valueIndex) => {
        //        searchParamsString += `searchParams[${index}].Values[${valueIndex}]=${encodeURIComponent(value)}&`;
        //    });
        //});

        //searchParamsString = searchParamsString.slice(0, -1);

        // Encode and make final URL
        let encoded = btoa(JSON.stringify(params));
        window.location.href = `${viewDataObj.baseUrl}?searchquery=${encodeURIComponent(encoded)}`;    
    }

    this.onAddParam = function (viewDataObj) {
        var paramID = viewDataObj.slSearchBy.val();

        if (paramID == -1) {
            return;
        }

        var searchParam = viewDataObj.searchParams.filter(f => f.Key == paramID)[0];

        var newParam = Search.createSearchParam(paramID, searchParam.DisplayName, searchParam.ParamType, searchParam.AllowMultipleValues, []);
        //{
        //    id: paramID,
        //    displayName: searchParam.DisplayName,
        //    paramType: searchParam.ParamType,
        //    allowMultipleValues: searchParam.AllowMultipleValues,
        //    values: [],
        //    displayValues: ''
        //};

        switch (searchParam.ParamType) {
            case Search.SearchParamTypes.Text:
                newParam.values.push(viewDataObj.fldText.val());
                break;
            case Search.SearchParamTypes.NumberRange:
                newParam.values.push(viewDataObj.fldNumberRangeFrom.val());
                newParam.values.push(viewDataObj.fldNumberRangeTo.val());
                break;
            case Search.SearchParamTypes.Number:
                newParam.values.push(viewDataObj.fldNumber.val());
                break;
            case Search.SearchParamTypes.DateRange:
                newParam.values.push(viewDataObj.fldDateRangeFrom.val());
                newParam.values.push(viewDataObj.fldDateRangeTo.val());
                break;
            case Search.SearchParamTypes.MultiSelectList:
                let multiSelectListObj = MultiSelectListManager.get(searchParam.MultiSelectListID);
                newParam.values = [...multiSelectListObj.selectedItems];
                newParam.displayValues = multiSelectListObj.selectedItemsNames;
                break;
            default:
                break;
        }

        if (newParam.values.filter(e => e == null || e == '').length > 0) {
            alert("Please enter values");
            return;
        }

        var existingParamIndex = viewDataObj.currSearchParams.findIndex(p => p.id == newParam.id);

        if (existingParamIndex > -1) {
            if (newParam.allowMultipleValues) {
                let existingParam = viewDataObj.currSearchParams[existingParamIndex];
                if (existingParam.values.indexOf(newParam.values[0]) == -1)
                    existingParam.values.push(newParam.values[0]);
            }
            else {
                viewDataObj.currSearchParams.splice(existingParamIndex, 1);
                viewDataObj.currSearchParams.push(newParam);
            }
        }
        else
            viewDataObj.currSearchParams.push(newParam);


        Search.redrawSelectedParameters(viewDataObj);
    }

    this.onSearchByChanged = function (viewDataObj) {
        viewDataObj.fldTextContainer.addClass("hidden");
        viewDataObj.fldNumberContainer.addClass("hidden");
        viewDataObj.fldNumberRangeContainer.addClass("hidden");
        viewDataObj.fldDateRangeContainer.addClass("hidden");
        viewDataObj.fldMultiSelectContainers.addClass("hidden");

        viewDataObj.btnAddParam.addClass("hidden");

        var paramID = viewDataObj.slSearchBy.val();

        if (paramID == -1)
            return;

        var searchParam = viewDataObj.searchParams.filter(f => f.Key == paramID)[0];

        if (searchParam == null)
            return;

        switch (searchParam.ParamType) {
            case Search.SearchParamTypes.Text:
                viewDataObj.fldText.val('');
                viewDataObj.fldTextContainer.removeClass("hidden");
                break;
            case Search.SearchParamTypes.NumberRange:
                viewDataObj.fldNumberRangeFrom.val('');
                viewDataObj.fldNumberRangeTo.val('');
                viewDataObj.fldNumberRangeContainer.removeClass("hidden");
                break;
            case Search.SearchParamTypes.Number:
                viewDataObj.fldNumber.val('');
                viewDataObj.fldNumberContainer.removeClass("hidden");
                break;
            case Search.SearchParamTypes.DateRange:
                viewDataObj.fldDateRangeFrom.val(null);
                viewDataObj.fldDateRangeTo.val(null);
                viewDataObj.fldDateRangeContainer.removeClass("hidden");
                break;
            case Search.SearchParamTypes.MultiSelectList:
                let multiSelectContainer = viewDataObj.fldMultiSelectContainers.filter(function () {
                    return $(this).attr("data-id") == paramID;
                });

                multiSelectContainer.removeClass("hidden");

                break;
            default:
                break;
        }

        viewDataObj.btnAddParam.removeClass("hidden");
    }

    this.redrawSelectedParameters = function (viewDataObj) {
        viewDataObj.selectedParamsContainer.empty();

        viewDataObj.currSearchParams.forEach((e, i) => {
            let values = '';

            if (e.displayValues != '')
                values = e.displayValues;
            else if (e.paramType == Search.SearchParamTypes.NumberRange || e.paramType == Search.SearchParamTypes.DateRange)
                values = `${e.values[0]} - ${e.values[1]}`;
            else
                values = e.values.join(', ');

            let currElement = $(`
            <div><span class="font-semibold">${i + 1}. ${e.displayName}:</span> ${values}</div>
            <div class="col-span-2 btn-delete-param" data-index="${i}"><i class="fa-solid fa-xmark text-red-500"></i></div>`);

            viewDataObj.selectedParamsContainer.append(currElement);
        });
    }

    this.initializeSelectedSearchParams = function (viewDataObj) {
        if (viewDataObj.selectedSearchParams.length == 0)
            return;

        viewDataObj.selectedSearchParams.forEach(p => {
            var currParam = Search.createSearchParam(p.Key, p.DisplayName, p.ParamType, p.AllowMultipleValues, p.Values, p.DisplayValues);
            viewDataObj.currSearchParams.push(currParam);
        });

        Search.redrawSelectedParameters(viewDataObj);
    }

    this.createSearchParam = function (id, displayName, paramType, allowMultipleValues, values, displayValues = '') {
        return {
            id: id,
            displayName: displayName,
            paramType: paramType,
            allowMultipleValues: allowMultipleValues,
            values: values,
            displayValues: displayValues == null ? '' : displayValues
        };
    }

    this.deleteSearchParam = function (viewDataObj, index) {
        viewDataObj.currSearchParams.splice(index, 1);
        Search.redrawSelectedParameters(viewDataObj);
    }

    this.clearSearchParams = function (viewDataObj) {
        viewDataObj.currSearchParams = [];
        Search.redrawSelectedParameters(viewDataObj);
    }
};