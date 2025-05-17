using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripleA.Utilities.Resources;

namespace TripleA.Utility.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailAddressAttribute : RegularExpressionAttribute, IClientValidatable
    {
        private const string _pattern = "^([a-zA-Z0-9_\\-\\.]+)@[a-zA-Z0-9-]+(\\.[A-Za-z0-9-]+)*(\\.[A-Za-z]{2,4})$";

        public EmailAddressAttribute()
            : base(_pattern)
        {
            ErrorMessage = AnnotationMessages.EmailAddress;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRegexRule(ErrorMessage, _pattern);
        }

        public new string ErrorMessage
        {
            get
            {
                return AnnotationMessages.EmailAddress;
            }
            set
            {
                base.ErrorMessage = AnnotationMessages.EmailAddress;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage;
        }
    }
}