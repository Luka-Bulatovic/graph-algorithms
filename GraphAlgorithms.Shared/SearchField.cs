namespace GraphAlgorithms.Shared
{
    public enum SearchFieldType { Text = 1, NumberRange = 2, Number = 3, DateRange = 4, MultiSelectList = 5, SelectList = 6 };

    public class SearchField
    {
        public string KeyValue { get; set; }
        public string DisplayValue { get; set; }
        public SearchFieldType FieldType { get; set; }

        /// <summary>
        /// Can be used for field types such as Text and Number,
        /// that allow entry of a single value at a time,
        /// so that user can select multiple values which will be treated with OR operator.
        /// </summary>
        public bool AllowMultipleValues { get; set; }

        public SearchField(string keyValue, string displayValue, SearchFieldType fieldType, bool allowMultipleValues = false)
        {
            if (allowMultipleValues && (fieldType != SearchFieldType.Number && fieldType != SearchFieldType.Text))
                throw new InvalidDataException("Multiple values are only allowed for Search Parameters of type Number or Text.");

            KeyValue = keyValue;
            DisplayValue = displayValue;
            FieldType = fieldType;
            AllowMultipleValues = allowMultipleValues;
        }
    }
}
