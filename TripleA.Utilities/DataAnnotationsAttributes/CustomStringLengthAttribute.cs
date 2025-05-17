using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripleA.Utilities.Resources;

namespace TripleA.Utility.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomStringLengthAttribute : StringLengthAttribute, IClientValidatable
    {
        private int _maximumLength;
        private int _minimumLength;
        private bool _minimumLengthSet;
        private bool _minimumEqMax;

        public CustomStringLengthAttribute(int maximumLength)
            : base(maximumLength)
        {
            _maximumLength = maximumLength;
        }

        public CustomStringLengthAttribute(int minimumLength, int maximumLength)
            : base(maximumLength)
        {
            _minimumLength = minimumLength;
            _maximumLength = maximumLength;
            this.MinimumLength = _minimumLength;
            _minimumLengthSet = true;
            _minimumEqMax = _minimumLength == _maximumLength;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationStringLengthRule(ErrorMessage, MinimumLength, MaximumLength);
        }

        public new string ErrorMessage
        {
            get
            {
                if (!_minimumLengthSet)
                {
                    return string.Format(AnnotationMessages.StringMaximumLength, _maximumLength);
                }
                else
                {
                    if (_minimumEqMax)
                        return string.Format(AnnotationMessages.StringLength, _maximumLength);
                    else
                        return string.Format(AnnotationMessages.StringRange, _minimumLength, _maximumLength);
                }
            }
            set
            {
                if (!_minimumLengthSet)
                {
                    base.ErrorMessage = string.Format(AnnotationMessages.StringMaximumLength, _maximumLength);
                }
                else
                {
                    if (_minimumEqMax)
                        base.ErrorMessage = string.Format(AnnotationMessages.StringLength, _maximumLength);
                    else
                        base.ErrorMessage = string.Format(AnnotationMessages.StringRange, _minimumLength, _maximumLength);
                }

            }
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage;
        }
    }
}