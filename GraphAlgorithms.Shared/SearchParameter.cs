using System.Collections.Generic;

namespace GraphAlgorithms.Shared
{
    public class SearchParameter
    {
        public string Key { get; set; }
        public SearchFieldType FieldType { get; set; }
        public List<string> Values { get; set; } // Can store one or more values based on FieldType
        public bool IsValid => ValidateParameter();

        public SearchParameter()
        {
            Values = new();
        }

        private bool ValidateParameter()
        {
            if (string.IsNullOrEmpty(Key))
                return false;

            if (Values == null)
                return false;

            if(Values.Count == 0)
                return false;

            switch (FieldType)
            {
                case SearchFieldType.Text:
                    if (Values.Count == 1 && !string.IsNullOrEmpty(Values[0]))
                        return true;
                    return false;
                case SearchFieldType.NumberRange:
                    if (Values.Count == 2 && int.TryParse(Values[0], out _) && int.TryParse(Values[1], out _))
                        return true;
                    return false;
                case SearchFieldType.Number:
                    if (Values.Count == 1 && int.TryParse(Values[0], out _))
                        return true;
                    return false;
                case SearchFieldType.DateRange:
                    if (Values.Count == 2 && DateTime.TryParse(Values[0], out _) && DateTime.TryParse(Values[1], out _))
                        return true;
                    return false;
                case SearchFieldType.MultiSelectList:
                case SearchFieldType.SelectList:
                    return true;
                default:
                    return false;
            }
        }
    }
}
