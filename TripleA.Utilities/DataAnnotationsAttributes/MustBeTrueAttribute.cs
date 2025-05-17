using System;
using System.ComponentModel.DataAnnotations;
using TripleA.Utilities.Resources;

namespace TripleA.Utility.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MustBeTrueAttribute : ValidationAttribute
    {
        public MustBeTrueAttribute()
        {
            ErrorMessage = AnnotationMessages.MustBeTrue;
        }

        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value;
        }

        public new string ErrorMessage
        {
            get
            {
                return AnnotationMessages.MustBeTrue;
            }
            set
            {
                base.ErrorMessage = AnnotationMessages.MustBeTrue;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage;
        }
    }
}