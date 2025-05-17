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
    public class CustomRequiredAttribute : RequiredAttribute, IClientValidatable
    {
        private bool _isCompact = false;

        public CustomRequiredAttribute()
        {
            ErrorMessage = AnnotationMessages.Required;
        }

        public CustomRequiredAttribute(bool isCompact)
        {
            _isCompact = isCompact;
            ErrorMessage = GetErrorMessage();
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRequiredRule(ErrorMessage);
        }

        public new string ErrorMessage
        {
            get
            {
                return GetErrorMessage();
            }
            set
            {
                base.ErrorMessage = GetErrorMessage();
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage;
        }

        private string GetErrorMessage()
        {
            if (_isCompact)
            {
                return "*";
            }
            return AnnotationMessages.Required;
        }
    }
}
