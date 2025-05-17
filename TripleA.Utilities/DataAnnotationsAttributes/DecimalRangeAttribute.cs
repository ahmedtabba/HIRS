
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TripleA.Utilities.Resources;

namespace TripleA.Utility.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DecimalRangeAttribute : ValidationAttribute, IClientValidatable
    {
        private double Min { get; set; }
        private double Max { get; set; }

        public DecimalRangeAttribute(double minimumLength, double maximumLength)
        {
            Min = minimumLength;
            Max = maximumLength;
        }

        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }
            dynamic comparableValue = value;

            if (comparableValue >= Min && comparableValue <= Max)
            {
                return null; // Validation success
            }
            else
            {
                return new System.ComponentModel.DataAnnotations.ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
                // error 
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ValidationType = "rangevalidation",
                ErrorMessage = FormatErrorMessage(ErrorMessage)
            };

            rule.ValidationParameters["min"] = Min;
            rule.ValidationParameters["max"] = Max;
            yield return rule;
        }

        public new string ErrorMessage
        {
            get
            {
                return string.Format(AnnotationMessages.Range, Min, Max);
            }
            set
            {
                base.ErrorMessage = string.Format(AnnotationMessages.Range, Min, Max);
            }
        }
    }
}
