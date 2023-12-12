(function ($) {
    $.validator.addMethod('lessthanorequaltoproperty', function (value, element, params) {
        var isValid = true;

        if (!this.optional(element)) {
            var otherPropertyNames = params.otherpropertynames.split('##');
            for (var i = 0; i < otherPropertyNames.length; i++) {
                var otherProperty = $('#' + otherPropertyNames[i]);
                if (parseInt(value) > parseInt(otherProperty.val())) {
                    isValid = false;
                    break;
                }
            }
        }

        return isValid;
    }, '');

    $.validator.unobtrusive.adapters.add('lessthanorequaltoproperty', ['otherpropertynames'], function (options) {
        options.rules['lessthanorequaltoproperty'] = {
            otherpropertynames: options.params.otherpropertynames
        };
        options.messages['lessthanorequaltoproperty'] = options.message;
    });
}(jQuery));
