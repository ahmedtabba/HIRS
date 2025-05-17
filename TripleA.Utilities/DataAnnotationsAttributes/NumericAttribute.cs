using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripleA.Utilities.Resources;

namespace TripleA.Utility.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NumericAttribute : RegularExpressionAttribute, IClientValidatable
    {
        private const string _pattern = @"\d*";

        public NumericAttribute()
            : base(_pattern)
        {
            ErrorMessage = AnnotationMessages.Number;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRegexRule(ErrorMessage, _pattern);
        }

        public new string ErrorMessage
        {
            get
            {
                return AnnotationMessages.Number;
            }
            set
            {
                base.ErrorMessage = AnnotationMessages.Number;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage;
        }
    }
}
