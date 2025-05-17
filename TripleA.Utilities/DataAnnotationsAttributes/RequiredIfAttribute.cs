using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Text;
using TripleA.Utilities.Resources;

namespace TripleA.Utility.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfAttribute : RequiredAttribute, IClientValidatable
    {
        private RequiredAttribute _innerAttribute = new RequiredAttribute();

        public string DependentProperty { get; set; }
        public object TargetValue { get; set; }
        public object[] TargetValues { get; set; }

        public RequiredIfAttribute(string dependentProperty, object targetValue)
        {
            this.DependentProperty = dependentProperty;
            this.TargetValue = targetValue;
            ErrorMessage = AnnotationMessages.Required;
        }
        public RequiredIfAttribute(string dependentProperty, object[] targetValue)
        {
            this.DependentProperty = dependentProperty;
            this.TargetValues = targetValue;
            ErrorMessage = AnnotationMessages.Required;
        }
        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get a reference to the property this validation depends upon
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(this.DependentProperty);

            if (field != null)
            {
                // Get the value of the dependent property
                var dependentvalue = field.GetValue(validationContext.ObjectInstance, null);

                if (
                    (dependentvalue == null && this.TargetValue == null) ||
                    (dependentvalue == null && this.TargetValues == null) ||
                    (dependentvalue != null && this.TargetValue != null&& dependentvalue.Equals(this.TargetValue))||
                    (dependentvalue != null && this.TargetValues != null && containsValues(this.TargetValues, dependentvalue)) 
                    )
                {
                    // Match => means we should try validating this field
                    /*if (!base.IsValid(value))
                    {
                    }*/


                    if (!_innerAttribute.IsValid(value))
                        // Validation failed - return an error
                        return new System.ComponentModel.DataAnnotations.ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
                }
            }

            return System.ComponentModel.DataAnnotations.ValidationResult.Success;
        }

        private bool containsValues(object[] targetValues, object dependentvalue)
        {
           foreach(object target in targetValues)
            {
                if (dependentvalue.Equals(target))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "requiredif",
            };

            string depProp = BuildDependentPropertyId(metadata, context as ViewContext);

            if (this.TargetValue != null)
            {
                // find the value on the control we depend on;
                // if it's a bool, format it javascript style 
                // (the default is True or False!)
                string targetValue = (this.TargetValue ?? "").ToString();
                if (this.TargetValue.GetType() == typeof(bool))
                    targetValue = targetValue.ToLower();

                rule.ValidationParameters.Add("dependentproperty", depProp);
                rule.ValidationParameters.Add("targetvalue", targetValue);
            }
            if (this.TargetValues != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var obj in this.TargetValues)
                {
                    string targetValue = (obj ?? "").ToString();

                    if (obj.GetType() == typeof(bool))
                        targetValue = targetValue.ToLower();

                    sb.AppendFormat("|{0}", targetValue);
                }

                rule.ValidationParameters.Add("dependentproperty", depProp);
                rule.ValidationParameters.Add("targetvalue", sb.ToString().TrimStart('|'));

            }
            yield return rule;
        }

        private string BuildDependentPropertyId(ModelMetadata metadata, ViewContext viewContext)
        {
            // build the ID of the property
            string depProp = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(this.DependentProperty);
            // unfortunately this will have the name of the current field appended to the beginning,
            // because the TemplateInfo's context has had this fieldname appended to it. Instead, we
            // want to get the context as though it was one level higher (i.e. outside the current property,
            // which is the containing object (our Person), and hence the same level as the dependent property.
            var thisField = metadata.PropertyName + "_";
            if (depProp.StartsWith(thisField))
                // strip it off again
                depProp = depProp.Substring(thisField.Length);
            return depProp;
        }

        public new string ErrorMessage
        {
            get
            {
                return AnnotationMessages.Required;
            }
            set
            {
                base.ErrorMessage = AnnotationMessages.Required;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage;
        }
    }
}