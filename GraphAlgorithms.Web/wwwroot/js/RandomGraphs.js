var RandomGraphs = new function () {
    this.Initialize = function (viewDataObj) {
        // Event for changing Graph Class
        viewDataObj.graphClassList.on('change', (e) => {
            this.onGraphClassChanged(viewDataObj, e);
        });
    }

    this.onGraphClassChanged = function (viewDataObj, e) {
        var selectedValue = e.target.value;

        if (selectedValue != "0") {
            window.location.href = viewDataObj.indexAction + '?id=' + selectedValue;
        }
        //if (e.target.value > 0) {
        //    viewDataObj.btnGenerate.removeClass('hidden');
        //    viewDataObj.numberOfGraphsContainer.removeClass('hidden');
        //}
        //else {
        //    viewDataObj.btnGenerate.addClass('hidden');
        //    viewDataObj.numberOfGraphsContainer.addClass('hidden');
        //}
    }
}