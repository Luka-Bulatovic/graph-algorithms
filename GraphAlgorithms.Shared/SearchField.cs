namespace GraphAlgorithms.Shared
{
    public enum SearchFieldType { Text = 1, NumberRange = 2, Number = 3, DateRange = 4, MultiSelectList = 5, SelectList = 6 };

    public class SearchField
    {
        public string KeyValue { get; set; }
        public string DisplayValue { get; set; }
        public SearchFieldType FieldType { get; set; }

        public SearchField(string keyValue, string displayValue, SearchFieldType fieldType)
        {
            KeyValue = keyValue;
            DisplayValue = displayValue;
            FieldType = fieldType;
        }
    }
}
