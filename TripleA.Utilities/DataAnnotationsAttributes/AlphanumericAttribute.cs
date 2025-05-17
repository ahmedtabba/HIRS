using System;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripleA.Utilities.Resources;


namespace TripleA.Utility.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AlphanumericAttribute : RegularExpressionAttribute, IClientValidatable
    {
        private const string _alphanumericPatternFormat = @"[a-zA-Zء-ي\d\s{0}]*";
        private char[] _allowedChars;
        public AlphanumericAttribute(params char[] allowedChars)
            : base(BuildExpression(allowedChars))
        {
            _allowedChars = allowedChars;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRegexRule(ErrorMessage, Pattern);
        }

        protected static string BuildExpression(char[] allowedChars)
        {
            StringBuilder allowedCharsRegexBuilder = new StringBuilder();
            for (int charIndex = 0; charIndex < allowedChars.Length; charIndex++)
            {
                if (allowedChars[charIndex] == '_')
                {
                    allowedCharsRegexBuilder.Append(allowedChars[charIndex]);
                }
                else
                {
                    allowedCharsRegexBuilder.AppendFormat(@"\{0}", allowedChars[charIndex]);
                }
            }
            return string.Format(_alphanumericPatternFormat, allowedCharsRegexBuilder.ToString());
        }

        protected string GetValidationErrorMessage(char[] allowedChars)
        {
            if (allowedChars.Length == 0)
            {
                return AnnotationMessages.Alphanumeric;
            }
            else
            {
                StringBuilder allowedCharsBuilder = new StringBuilder();
                for (int charIndex = 0; charIndex < allowedChars.Length; charIndex++)
                {
                    if (charIndex != 0)
                    {
                        allowedCharsBuilder.AppendFormat(@" ,{0}", allowedChars[charIndex]);
                    }
                    else
                    {
                        allowedCharsBuilder.AppendFormat(@"{0}", allowedChars[charIndex]);
                    }
                }
                return string.Format(AnnotationMessages.AlphanumericAnd , allowedCharsBuilder.ToString());
            }
        }

        public new string ErrorMessage
        {
            get
            {
                return GetValidationErrorMessage(_allowedChars);
            }
            set
            {
                base.ErrorMessage = GetValidationErrorMessage(_allowedChars);
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage;
        }
    }
}
