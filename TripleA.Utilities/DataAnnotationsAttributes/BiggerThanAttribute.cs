using System;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace TripleA.Utility.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BiggerThanAttribute : ValidationAttribute
    {
        private bool _orEqual;

        public string OtherProperty
        {
            get;
            private set;
        }

        public BiggerThanAttribute(string otherProperty, bool orEqual = false)
        {
            OtherProperty = otherProperty;
            _orEqual = orEqual;
        }

        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo propertyInfo = validationContext.ObjectType.GetProperty(this.OtherProperty);

            if (propertyInfo == null)
            {
                throw new InvalidOperationException("Property type is invalid");
            }

            dynamic comparableValue = value;
            dynamic comparableValue2 = propertyInfo.GetValue(validationContext.ObjectInstance, null);

            bool biggerThan = false;

            try
            {
                biggerThan = comparableValue > comparableValue2;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not compare two items", ex);
            }

            if (comparableValue == null ||
                comparableValue2 == null || 
                biggerThan ||
                (_orEqual && comparableValue == comparableValue2))
            {
                return null;
            }
            return new System.ComponentModel.DataAnnotations.ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
