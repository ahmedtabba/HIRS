
using System;
using System.Collections;
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
    public class EnsureMinimumElementsAttribute : ValidationAttribute, IClientValidatable
    {

        private readonly int _minElements;
        public EnsureMinimumElementsAttribute(int minElements)
        {
            _minElements = minElements;
        }


        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as IList;

            if (list != null)
            {
                if (list.Count < _minElements)
                {
                    return new System.ComponentModel.DataAnnotations.ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
                }
            }
            return null;

        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ValidationType = "ensureminimumelementsvalidation",
                ErrorMessage = FormatErrorMessage("")
            };

            yield return rule;
        }


        public new string ErrorMessage
        {
            get
            {
                return string.Format(AnnotationMessages.DisallowEnumValue, _minElements, AnnotationMessages.Item);

            }
            set
            {
                base.ErrorMessage = string.Format(AnnotationMessages.DisallowEnumValue, _minElements, AnnotationMessages.Item);
            }
        }
    }
}
