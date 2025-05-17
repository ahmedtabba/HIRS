using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripleA.Utilities.Resources;

namespace TripleA.Utility.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DecimalNumberAttribute : RegularExpressionAttribute, IClientValidatable
    {
        //private const string _pattern = @"[\d]*[\.]?[\d]+";
        private const string _pattern = @"^-?\d*\.{0,1}\d+$";

        public DecimalNumberAttribute()
            : base(_pattern)
        {
            ErrorMessage = AnnotationMessages.DecimalNumber;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRegexRule(ErrorMessage, _pattern);
        }

        public new string ErrorMessage
        {
            get
            {
                return AnnotationMessages.DecimalNumber;
            }
            set
            {
                base.ErrorMessage = AnnotationMessages.DecimalNumber;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage;
        }
    }
}
