var RandomGraphs = new function () {
    this.Initialize = function (viewDataObj) {
        // Event for changing Graph Class
        viewDataObj.graphClassList.on('change', (e) => {
            this.onGraphClassChanged(viewDataObj, e);
        });

        viewDataObj.form.on('submit', function (e) {
            if (!$(this).valid()) {
                // Validation failed
                e.preventDefault();
                return false;
            }

            // Validation passed, proceed with submission
            // Show spinner and disable button
            viewDataObj.btnGenerate.children(".spinner-icon").css("display", "");
            viewDataObj.btnGenerate.attr('disabled', 'disabled');

            return true;
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