using BoulevardManagement.DTO;
using BoulevardManagement.WebApplication.Resources;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BoulevardManagement.WebApplication.Helper
{
    public static class Extensions
    {


        //new
        public class LocalizedDescriptionAttribute : DescriptionAttribute
        {
            private readonly string _resourceKey;
            private readonly ResourceManager _resource;
            public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
            {
                _resource = new ResourceManager(resourceType);
                _resourceKey = resourceKey;
            }

            public override string Description
            {
                get
                {
                    string displayName = _resource.GetString(_resourceKey);

                    return string.IsNullOrEmpty(displayName)
                        ? string.Format("[[{0}]]", _resourceKey)
                        : displayName;
                }
            }
        }

       
            public static string GetDescription(this Enum enumValue)
            {
                FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return enumValue.ToString();
            }
       
        //new


        public static GridBuilder<T> KendoGrid<T>(this HtmlHelper html)
            where T : class
        {
            return KendoGrid<T>(html, null);
        }

        public static GridBuilder<T> KendoGrid<T>(this HtmlHelper html, IEnumerable<T> dataSource)
           where T : class
        {
            GridBuilder<T> builder;

            if (dataSource != null)
                builder = html.Kendo().Grid<T>(dataSource);
            else
                builder = html.Kendo().Grid<T>();


            builder
           .Scrollable()
           .Resizable(resize => resize.Columns(true))

           .Events(e=> {
               if (System.Globalization.CultureInfo.CurrentCulture.TextInfo.IsRightToLeft)
               {
                   e.DataBound("gridDataBound");
               }
           }
           )

           .Pageable()
           .Filterable(f => f
           .Mode(GridFilterMode.Row)
                                                                                  .Enabled(true)
                                                                                  .Operators(g =>
                                                                                      g.ForString(h =>
                                                                                      {
                                                                                          h.Clear();
                                                                                          h.Contains(GridOptionViewResource.Contains);
                                                                                          h.DoesNotContain(GridOptionViewResource.DoesNotContain);
                                                                                          h.StartsWith(GridOptionViewResource.StartsWith);
                                                                                          h.EndsWith(GridOptionViewResource.EndsWith);

                                                                                          h.IsEqualTo(GridOptionViewResource.IsEqualTo);
                                                                                          h.IsNotEqualTo(GridOptionViewResource.IsNotEqualTo);
                                                                                          h.IsEmpty(GridOptionViewResource.IsEmpty);
                                                                                          h.IsNotEmpty(GridOptionViewResource.IsNotEmpty);

                                                                                          h.IsNull(GridOptionViewResource.IsNull);
                                                                                          h.IsNotNull(GridOptionViewResource.IsNotNull);



                                                                                      }

                                                                                      ).ForDate(c => {

                                                                                          c.Clear();

                                                                                          c.IsEqualTo(GridOptionViewResource.IsEqualTo);
                                                                                          c.IsNotEqualTo(GridOptionViewResource.IsNotEqualTo);
                                                                                          c.IsGreaterThan(GridOptionViewResource.IsGreaterThan);
                                                                                          c.IsGreaterThanOrEqualTo(GridOptionViewResource.IsGreaterThanOrEqualTo);
                                                                                          c.IsLessThan(GridOptionViewResource.IsLessThan);
                                                                                          c.IsLessThanOrEqualTo(GridOptionViewResource.IsLessThanOrEqualTo);

                                                                                          c.IsNull(GridOptionViewResource.IsNull);
                                                                                          c.IsNotNull(GridOptionViewResource.IsNotNull);


                                                                                      })
                                                                                       .ForEnums(c => {

                                                                                           c.Clear();

                                                                                           c.IsEqualTo(GridOptionViewResource.IsEqualTo);
                                                                                           c.IsNotEqualTo(GridOptionViewResource.IsNotEqualTo);
                                                                                           //  c.IsGreaterThan(GridOptionViewResource.IsGreaterThan);

                                                                                           c.IsNull(GridOptionViewResource.IsNull);
                                                                                           c.IsNotNull(GridOptionViewResource.IsNotNull);


                                                                                       })


                                                                                      )
                                                                                      .Messages(ms => ms
                                                                                      .And(GridOptionViewResource.And)
                                                                                      .Cancel(GridOptionViewResource.Cancel)
                                                                                      .CheckAll(GridOptionViewResource.CheckAll)
                                                                                      .Clear(GridOptionViewResource.Clear)
                                                                                      .Filter(GridOptionViewResource.Filter)
                                                                                      .Or(GridOptionViewResource.Or)
                                                                                      .Info(GridOptionViewResource.Filter)
                                                                                      .Search(GridOptionViewResource.Search)
                                                                                      .Value(GridOptionViewResource.Value)
                                                                                      .IsTrue(GridOptionViewResource.True)
                                                                                      .IsFalse(GridOptionViewResource.False)
                                                                                      
                                                                                      .Equals(GridOptionViewResource.Equal)))
                                                                                        
                                                                                       .ColumnMenu(c => c.
                                                                                       Columns(true)
                                                                                       .Messages(m => m
                                                                                       .Filter(GridOptionViewResource.Filter)
                                                                                       .Columns(GridOptionViewResource.Columns)
                                                                                       .SortAscending(GridOptionViewResource.SortAscending)
                                                                                       .SortDescending(GridOptionViewResource.SortDescending)))
                                                                                      
           .Pageable(pageable => pageable.Refresh(true).Messages(ms=>ms.ItemsPerPage(GridOptionViewResource.ItemsPerPage)).PageSizes(new List<object> { 5, 10, 20, 50, 100 }).ButtonCount(5))
           .DataSource(c => c.Ajax().PageSize(50))
           .Sortable();

            return builder;
        }

        public static string GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }

        //old
        //public static string GetDescription(this Enum value)
        //{
        //    var fieldInfo = value.GetType().GetField(value.GetName());
        //    var descriptionAttribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
        //    return descriptionAttribute == null ? value.GetName() : descriptionAttribute.Description;
        //}

        public static MvcHtmlString AdvancedEnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> enumAccessor, object htmlAttributes)
        {
            var propertyInfo = enumAccessor.ToPropertyInfo();
            var enumType = propertyInfo.PropertyType;
            var enumValues = Enum.GetValues(enumType).Cast<Enum>();
            var selectItems = enumValues.Select(s => new SelectListItem
            {
                Text = s.GetDescription(),
                Value = s.ToString()
            });

            return htmlHelper.DropDownListFor(enumAccessor, selectItems, htmlAttributes);
        }


        public static MvcHtmlString AdvancedEnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> enumAccessor,string optionLabel="")
        {
            var propertyInfo = enumAccessor.ToPropertyInfo();
            var enumType = propertyInfo.PropertyType;
            var enumValues = Enum.GetValues(enumType).Cast<Enum>();
            var selectItems = enumValues.Select(s => new SelectListItem
            {
                Text = s.GetDescription(),
                Value = s.ToString()
            });

            return htmlHelper.DropDownListFor(enumAccessor, selectItems,  optionLabel);
        }

        public static PropertyInfo ToPropertyInfo(this LambdaExpression expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            return (memberExpression == null) ? null : memberExpression.Member as PropertyInfo;
        }
    }

    public static class IdentityExtensions
    {
        public static JobRole GetJobRole(this IIdentity identity)
        {
            return (JobRole)Enum.Parse(typeof(JobRole), ((ClaimsIdentity)identity).FindFirst("JobRole").Value);
        }

        //public static string GetEmail(this IIdentity identity)
        //{
        //    return ((ClaimsIdentity)identity).FindFirst("Email").Value;
        //}

        public static bool IsIT(this IIdentity identity)
        {
            return identity.GetJobRole() == JobRole.IT;
        }

        public static bool IsServiceProvider(this IIdentity identity)
        {
            return identity.GetJobRole() == JobRole.ServiceProvider;
        }

        public static bool IsConsultant(this IIdentity identity)
        {
            return identity.GetJobRole() == JobRole.Consultant;
        }

       

        public static string GetFullName(this IIdentity identity)
        {
            return ((ClaimsIdentity)identity).FindFirst("FullName").Value;
        }
        //public static string GetColor(this IIdentity identity)
        //{
        //    return ((ClaimsIdentity)identity).FindFirst("SpecifiedColor")?.Value;
        //}
        public static string GetId(this IIdentity identity)
        {
            return ((ClaimsIdentity)identity).FindFirst("Id").Value;
        }
        public static string GetGender(this IIdentity identity)
        {
            return ((ClaimsIdentity)identity).FindFirst("Gender")?.Value;
        }

        public static bool HasPhoto(this IIdentity identity)
        {
            return identity.IsAuthenticated ? Convert.ToBoolean(((ClaimsIdentity)identity).FindFirst("HasPhoto").Value) : false;
        }



    }
}