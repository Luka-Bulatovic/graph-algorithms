using System;

namespace GraphAlgorithms.Web
{
    public class FieldMetadata
    {
        public string FieldName;
        public string DisplayName;
        public Func<object> Getter;
        public Action<object> Setter;
        public Type Type;

        // Validation properties
        public bool IsRequired { get; set; } = false;
        public string RequiredErrorMessage { get; set; } = "This field is required.";

        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public string RangeErrorMessage { get; set; } = "Value is out of range.";

        public bool IsEven { get; set; } = false;
        public string EvenErrorMessage { get; set; } = "Value must be even.";

        public string[] LessThanOrEqualToPropertyNames { get; set; }
        public string LessThanOrEqualToErrorMessage { get; set; } = "";

        public FieldMetadata(
            string fieldName, Func<object> getter, Action<object> setter, Type type,

            // Validation params
            bool isRequired = false,
            int? minValue = null, int? maxValue = null,
            bool isEven = false,
            string[] lessThanOrEqualToPropertyNames = null
        )
        {
            FieldName = fieldName;
            DisplayName = fieldName;
            Getter = getter;
            Setter = setter;
            Type = type;

            // Validation properties
            IsRequired = isRequired;
            MinValue = minValue;
            MaxValue = maxValue;
            IsEven = isEven;
            LessThanOrEqualToPropertyNames = lessThanOrEqualToPropertyNames ?? new string[] { };
            SetValidationMessages();
        }

        private void SetValidationMessages()
        {
            // Range
            if (MinValue.HasValue && MaxValue.HasValue)
                RangeErrorMessage = string.Format("Value must be in range {0} to {1}.", MinValue.Value, MaxValue.Value);
            else if(MinValue.HasValue)
                RangeErrorMessage = string.Format("Value must be greater than or equal to {0}.", MinValue.Value);
            else if (MaxValue.HasValue)
                RangeErrorMessage = string.Format("Value must be less than or equal to {0}.", MaxValue.Value);

            // Less than or equal to properties
            LessThanOrEqualToErrorMessage = string.Format("Value must be less than or equal to properties: {0}.", string.Join(",", LessThanOrEqualToPropertyNames));
        }
    }
}
