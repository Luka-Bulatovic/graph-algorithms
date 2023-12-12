using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Linq;

namespace GraphAlgorithms.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class LessThanOrEqualToPropertiesAttribute : ValidationAttribute, IClientModelValidator
    {
        string[] otherPropertyNames;

        public LessThanOrEqualToPropertiesAttribute(string[] otherPropertyNames, string errorMessage)
            : base(errorMessage)
        {
            this.otherPropertyNames = otherPropertyNames;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, $"data-val-lessthanorequaltoproperty", errorMessage);
            MergeAttribute(context.Attributes, $"data-val-lessthanorequaltoproperty-otherpropertynames", string.Join("##", otherPropertyNames));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;

            try
            {
                foreach(string otherPropertyName in otherPropertyNames)
                {
                    // Using reflection we can get a reference to the other property
                    var otherPropertyInfo = validationContext.ObjectType.GetProperty(otherPropertyName);

                    // Check that otherProperty is of type int as we expect it to be
                    if (otherPropertyInfo.PropertyType.Equals(new int().GetType()))
                    {
                        int toValidateValue = (int)value;
                        int otherPropertyValue = (int)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

                        if (toValidateValue > otherPropertyValue)
                        {
                            validationResult = new ValidationResult(ErrorMessageString);
                        }
                    }
                    else
                    {
                        validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type int");
                    }
                }
            }
            catch (Exception ex)
            {
                // Do stuff, i.e. log the exception
                // Let it go through the upper levels, something bad happened
                throw ex;
            }

            return validationResult;
        }

        private bool MergeAttribute(
            IDictionary<string, string> attributes,
            string key,
            string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }
    }
}
