using System;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripleA.Utilities.Resources;

namespace TripleA.Utility.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PasswordAttribute: RegularExpressionAttribute, IClientValidatable
    {
        private const string _pattern = @"(?=.*\d)(?=.*[a-zA-Zء-ي]).*";

        public PasswordAttribute()
            : base(_pattern)
        {
            ErrorMessage = AnnotationMessages.Password;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRegexRule(ErrorMessage, _pattern);
        }

        public new string ErrorMessage
        {
            get
            {
                return AnnotationMessages.Password;
            }
            set
            {
                base.ErrorMessage = AnnotationMessages.Password;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage;
        }
    }
}
