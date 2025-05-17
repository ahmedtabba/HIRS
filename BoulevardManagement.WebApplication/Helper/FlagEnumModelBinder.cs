using System;
using System.Web.Mvc;

namespace BoulevardManagement.WebApplication.Helper
{
    public class FlagEnumModelBinder<T> : DefaultModelBinder where T : struct
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (value != null)
            {
                var rawValues = value.RawValue as string[];
                if (rawValues != null)
                {
                    T result;
                    if (Enum.TryParse<T>(string.Join(",", rawValues), out result))
                    {
                        return result;
                    }
                }
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}