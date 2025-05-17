using System;
using System.Web.Mvc;
using System.Collections.Generic;
using TripleA.Utilities.Resources;

namespace TripleA.Utility.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomComparePasswordAttribute : System.ComponentModel.DataAnnotations.CompareAttribute, IClientValidatable
    {
        public CustomComparePasswordAttribute(string otherPasswordProperty)
            : base(otherPasswordProperty)
        {
            ErrorMessage = AnnotationMessages.ComparePasswords;
        }

        IEnumerable<ModelClientValidationRule> IClientValidatable.GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationEqualToRule(ErrorMessage, OtherProperty);
        }

        public new string ErrorMessage
        {
            get
            {
                return AnnotationMessages.ComparePasswords;
            }
            set
            {
                base.ErrorMessage = AnnotationMessages.ComparePasswords;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage;
        }
    }
    
}
