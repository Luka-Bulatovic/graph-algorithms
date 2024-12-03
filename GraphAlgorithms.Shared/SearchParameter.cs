using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Xml.Linq;

namespace GraphAlgorithms.Shared
{
    public enum SearchParamType { Text = 1, NumberRange = 2, Number = 3, DateRange = 4, MultiSelectList = 5, SelectList = 6 };

    public class SearchParameter
    {
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public SearchParamType ParamType { get; set; }
        public bool AllowMultipleValues { get; set; }
        public List<string> Values { get; set; } // Can store one or more values based on FieldType and AllowMultipleValues
        public bool IsValid => ValidateParameter();

        public List<MultiSelectListItem> MultiSelectItems { get; set; } // Used when parameter is of type MultiSelectList
        public string MultiSelectListID { get; set; } // Used when parameter is of type MultiSelectList
        
        public string DisplayValues { get; set; } // Used to display initial values that are selected for Parameter (for example, names of selected MultiSelectListItems)

        public SearchParameter()
        {
            Values = new();
        }

        public SearchParameter(string key, string displayName, SearchParamType paramType, bool allowMultipleValues = false, List<MultiSelectListItem> multiSelectItems = null, string multiSelectListID = "") : this()
        {
            if (allowMultipleValues && (paramType != SearchParamType.Number && paramType != SearchParamType.Text))
                throw new InvalidDataException("Multiple values are only allowed for Search Parameters of type Number or Text.");

            Key = key;
            DisplayName = displayName;
            ParamType = paramType;
            AllowMultipleValues = allowMultipleValues;

            MultiSelectItems = multiSelectItems;
            MultiSelectListID = multiSelectListID;
        }

        private bool ValidateParameter()
        {
            if (string.IsNullOrEmpty(Key))
                return false;

            if (Values == null)
                return false;

            if(Values.Count == 0)
                return false;

            switch (ParamType)
            {
                case SearchParamType.Text:
                    if (Values.All(v => !string.IsNullOrEmpty(v)))
                        return true;
                    return false;
                case SearchParamType.NumberRange:
                    if (Values.Count == 2 && int.TryParse(Values[0], out _) && int.TryParse(Values[1], out _))
                        return true;
                    return false;
                case SearchParamType.Number:
                    if (Values.All(v => int.TryParse(v, out _)))
                        return true;
                    return false;
                case SearchParamType.DateRange:
                    if (Values.Count == 2 && DateTime.TryParse(Values[0], out _) && DateTime.TryParse(Values[1], out _))
                        return true;
                    return false;
                case SearchParamType.MultiSelectList:
                case SearchParamType.SelectList:
                    return true;
                default:
                    return false;
            }
        }
    }
}
