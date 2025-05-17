using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripleA.Utilities.Resources;

namespace TripleA.Utility.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ContactNumberAttribute : RegularExpressionAttribute, IClientValidatable
    {
        private const string _pattern = @"[\d|\+|\-|\#|\s]*";

        public ContactNumberAttribute()
            : base(_pattern)
        {
            ErrorMessage = AnnotationMessages.ContactNumber;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRegexRule(ErrorMessage, _pattern);
        }

        public new string ErrorMessage
        {
            get
            {
                return AnnotationMessages.ContactNumber;
            }
            set
            {
                base.ErrorMessage = AnnotationMessages.ContactNumber;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage;
        }
    }
}
