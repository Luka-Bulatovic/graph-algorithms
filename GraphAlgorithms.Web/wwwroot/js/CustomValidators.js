(function ($) {
    // lessthanorequaltoproperty
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



    // evenvalue
    $.validator.addMethod('evenvalue', function (value, element, params) {
        var isValid = true;

        if (!this.optional(element)) {
            var toValidateValue = parseInt(value);

            if (toValidateValue % 2 > 0)
                isValid = false;
        }

        return isValid;
    }, '');

    $.validator.unobtrusive.adapters.add('evenvalue', [], function (options) {
        options.rules['evenvalue'] = {};
        options.messages['evenvalue'] = options.message;
    });
}(jQuery));
