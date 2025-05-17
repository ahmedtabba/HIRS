
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
    public class DisallowDefaultValueAttribute : ValidationAttribute, IClientValidatable
    {
        public object DefaultValue { get; private set; }
        public Type DefaultValueType { get; private set; }

        public DisallowDefaultValueAttribute(Type defaultValueType, object defalutValue)
        {
            DefaultValue = defalutValue;
            DefaultValueType = defaultValueType;
        }


        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var defaultValue = Convert.ChangeType(DefaultValue, DefaultValueType);
            var propertyValue = Convert.ChangeType(value, DefaultValueType);

            if (defaultValue == null || propertyValue == null)
                throw new Exception("Something is wrong"); //or return validation result

            if (propertyValue.ToString().ToUpper() == defaultValue.ToString().ToUpper())
            {
                return new System.ComponentModel.DataAnnotations.ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
            }

            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ValidationType = "defaultvalueval",
                ErrorMessage = FormatErrorMessage("")
            };

            rule.ValidationParameters["value"] = DefaultValue;
            rule.ValidationParameters["type"] = DefaultValueType;
            rule.ValidationParameters["message"] = ErrorMessage;
            yield return rule;
        }

        public new string ErrorMessage
        {
            get
            {
                return string.Format(AnnotationMessages.DisallowEnumValue);
            }
            set
            {
                base.ErrorMessage = string.Format(AnnotationMessages.DisallowEnumValue);
            }
        }
    }
}
