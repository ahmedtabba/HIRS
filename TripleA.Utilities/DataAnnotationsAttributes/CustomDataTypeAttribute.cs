using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripleA.Utilities.Resources;

namespace TripleA.Utility.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomDataTypeAttribute : DataTypeAttribute, IClientValidatable
    {
        private DataType _dataType;
        public CustomDataTypeAttribute(DataType dataType)
            : base(dataType)
        {
            _dataType = dataType;
            ErrorMessage = ErrorMessage;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            if (DataType == DataType.Date || DataType == DataType.DateTime)
            {
                var rule = new ModelClientValidationRule();
                rule.ErrorMessage = ErrorMessage;
                rule.ValidationParameters.Add("datefield", metadata.PropertyName);
                rule.ValidationType = DataType.ToString().ToLower();
            }
            else
            yield return new ModelClientValidationRule { ErrorMessage = ErrorMessage, ValidationType = DataType.ToString().ToLower() };
        }

        public new string ErrorMessage
        {
            get
            {
                return AnnotationMessages.ResourceManager.GetString(string.Format("DataType_{0}", _dataType.ToString()));
            }
            set
            {
                base.ErrorMessage = AnnotationMessages.ResourceManager.GetString(string.Format("DataType_{0}", _dataType.ToString()));
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage;
        }
    }
}
