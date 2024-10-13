using System;

namespace GraphAlgorithms.Web
{
    public class FieldMetadata
    {
        public string FieldName;
        public Func<object> Getter;
        public Action<object> Setter;
        public Type Type;

        public FieldMetadata(string fieldName, Func<object> getter, Action<object> setter, Type type)
        {
            FieldName = fieldName;
            Getter = getter;
            Setter = setter;
            Type = type;
        }
    }
}
