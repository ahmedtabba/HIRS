
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
    public class DisallowEnumValueAttribute : ValidationAttribute, IClientValidatable
    {
        public object DissallowedEnum { get; private set; }
        public Type EnumType { get; private set; }

        public DisallowEnumValueAttribute(Type enumType, object dissallowedEnum)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("Type must be an enum", "enumType");

            DissallowedEnum = dissallowedEnum;
            EnumType = enumType;
        }


        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var disallowed = Convert.ChangeType(DissallowedEnum, EnumType);
            var enumVal = Convert.ChangeType(value, EnumType);

            if (disallowed == null || enumVal == null)
                throw new Exception("Something is wrong"); //or return validation result

            if ((int)enumVal == (int)disallowed)
            {
                return new System.ComponentModel.DataAnnotations.ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
            }

            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ValidationType = "disallowenumvaluevalidation",
                ErrorMessage = FormatErrorMessage(ErrorMessage)
            };

            rule.ValidationParameters["dissallowedenum"] = DissallowedEnum;
            rule.ValidationParameters["enumtype"] = EnumType;
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
