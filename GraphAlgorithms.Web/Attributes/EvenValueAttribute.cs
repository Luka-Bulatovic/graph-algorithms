using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace GraphAlgorithms.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EvenValueAttribute : ValidationAttribute, IClientModelValidator
    {
        public EvenValueAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, $"data-val-evenvalue", errorMessage);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;

            try
            {
                int toValidateValue = (int)value;
                if (toValidateValue % 2 > 0)
                    validationResult = new ValidationResult(ErrorMessageString);
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
