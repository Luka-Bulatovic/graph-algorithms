var RandomGraphs = new function () {
    this.classes = {
        ConnectedGraph: 1,
        UnicyclicBipartiteGraph: 2,
        Tree: 3,
        UnicyclicGraph: 4,
        BipartiteGraph: 5,
        RandomAcyclicGraphWithFixedDiameter: 6
    }

    this.Initialize = function (viewDataObj) {
        // Initially all partial views should have their fields with ignored validation
        this.disableValidationForAllClasses(viewDataObj);

        // Event for changing Graph Class
        viewDataObj.graphClassList.on('change', (e) => {
            this.onGraphClassChanged(viewDataObj, e);
        });
    }

    this.onGraphClassChanged = function (viewDataObj, e) {
        viewDataObj.paramsContainers.addClass("hidden");

        // Disable validation for all classes
        this.disableValidationForAllClasses(viewDataObj);

        if (e.target.value == RandomGraphs.classes.ConnectedGraph) {
            viewDataObj.connectedGraphParamsContainer.removeClass("hidden");
        }
        else if (e.target.value == RandomGraphs.classes.UnicyclicBipartiteGraph) {
            viewDataObj.unicyclicBipartiteGraphParamsContainer.removeClass("hidden");
        }
        else if (e.target.value == RandomGraphs.classes.RandomAcyclicGraphWithFixedDiameter) {
            viewDataObj.randomAcyclicGraphWithFixedDiameterParamsContainer.removeClass("hidden");
        }


        this.toggleValidationForClass(viewDataObj, e.target.value, false);

        if (e.target.value > 0) {
            viewDataObj.btnGenerate.removeClass('hidden');
            viewDataObj.numberOfGraphsContainer.removeClass('hidden');
        }
        else {
            viewDataObj.btnGenerate.addClass('hidden');
            viewDataObj.numberOfGraphsContainer.addClass('hidden');
        }
    }

    this.disableValidationForAllClasses = function (viewDataObj) {
        this.toggleValidationForClass(viewDataObj, 1, true);
        this.toggleValidationForClass(viewDataObj, 2, true);
    }

    this.toggleValidationForClass = function (viewDataObj, graphClassID, validationDisabled) {
        var paramsContainer = null;

        if (graphClassID == RandomGraphs.classes.ConnectedGraph)
            paramsContainer = viewDataObj.connectedGraphParamsContainer;
        else if (graphClassID == RandomGraphs.classes.UnicyclicBipartiteGraph)
            paramsContainer = viewDataObj.unicyclicBipartiteGraphParamsContainer;
        else if (graphClassID == RandomGraphs.classes.RandomAcyclicGraphWithFixedDiameter)
            paramsContainer = viewDataObj.randomAcyclicGraphWithFixedDiameterParamsContainer;

        if (paramsContainer != null) {
            if (validationDisabled) {
                paramsContainer
                    .find('input')
                    .addClass('ignore-val');
            }
            else {
                paramsContainer
                    .find('input')
                    .removeClass('ignore-val');
            }
        }
    }
}