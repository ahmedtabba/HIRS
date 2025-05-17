using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoulevardManagement.WebApplication.Helper
{
    public static class EnumExtensions
    {
        public static IList<SelectListItem> GetSelectItemList(Type type)
        {
            var enumValues = Enum.GetValues(type).Cast<Enum>();
            var selectItems = enumValues.Select(s => new SelectListItem
            {
                Text = s.GetDescription(),
                Value = s.ToString()
            }).ToList();

            return selectItems;
        }


        public static IList<SelectListItem> GetSelectItemList(Type type, Enum value = null)
        {
            var enumValues = Enum.GetValues(type).Cast<Enum>();
            var selectItems = new List<SelectListItem>();
            if (value != null)
            {

                selectItems = enumValues.Where(c => value.HasFlag(c)).Select(s => new SelectListItem
                {
                    Text = s.GetDescription(),
                    Value = s.ToString()
                }).ToList();
            }
            return selectItems;
        }


    }
}