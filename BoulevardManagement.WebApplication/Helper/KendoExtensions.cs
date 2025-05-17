using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoulevardManagement.WebApplication.Helper
{
    public static class KendoExtensions
    {
        public static void ItemNameOrCodeFilter(this DataSourceRequest request, string name)
        {
            if (request.Filters == null)
                return;

            var filter = request.Filters.Where(f => f is FilterDescriptor).SingleOrDefault(c => (c as FilterDescriptor).Member == name);
            if (filter == null)
                return;

            var filterDescriptor = filter as FilterDescriptor;
            var itemCompositeFilterDescriptor = new CompositeFilterDescriptor { LogicalOperator = FilterCompositionLogicalOperator.Or };
            itemCompositeFilterDescriptor.FilterDescriptors.Add(new FilterDescriptor("Name", filterDescriptor.Operator, filterDescriptor.Value));
            itemCompositeFilterDescriptor.FilterDescriptors.Add(new FilterDescriptor("Code", filterDescriptor.Operator, filterDescriptor.Value));

            request.Filters.Remove(filter);
            request.Filters.Add(itemCompositeFilterDescriptor);

        }

        public static void NormalizeDateFilter(this DataSourceRequest request, string name)
        {
            if (request.Filters == null)
                return;

            var dateFilter = request.Filters.Where(f => f is FilterDescriptor).SingleOrDefault(c => (c as FilterDescriptor).Member == name);
            if (dateFilter == null)
                return;

            var normalizedFilter = TransformFilterDescriptors(dateFilter);
            request.Filters.Remove(dateFilter);
            request.Filters.Add(normalizedFilter);
        }

        public static IFilterDescriptor TransformFilterDescriptors(IFilterDescriptor filter)
        {
            if (filter is CompositeFilterDescriptor)
            {
                var compositeFilterDescriptor = filter as CompositeFilterDescriptor;
                var transformedCompositeFilterDescriptor = new CompositeFilterDescriptor { LogicalOperator = compositeFilterDescriptor.LogicalOperator };
                foreach (var filterDescriptor in compositeFilterDescriptor.FilterDescriptors)
                {
                    transformedCompositeFilterDescriptor.FilterDescriptors.Add(TransformFilterDescriptors(filterDescriptor));
                }
                return transformedCompositeFilterDescriptor;
            }
            if (filter is FilterDescriptor)
            {
                var filterDescriptor = filter as FilterDescriptor;
                if (filterDescriptor.Value is DateTime)
                {
                    var value = (DateTime)filterDescriptor.Value;
                    switch (filterDescriptor.Operator)
                    {
                        case FilterOperator.IsEqualTo:
                            //convert the "is equal to <date><time>" filter to a "is greater than or equal to <date> 00:00:00" AND "is less than <date + 1day> 00:00:00"
                            var isEqualCompositeFilterDescriptor = new CompositeFilterDescriptor { LogicalOperator = FilterCompositionLogicalOperator.And };
                            isEqualCompositeFilterDescriptor.FilterDescriptors.Add(new FilterDescriptor(filterDescriptor.Member,
                                FilterOperator.IsGreaterThanOrEqualTo, new DateTime(value.Year, value.Month, value.Day, 0, 0, 0)));
                            isEqualCompositeFilterDescriptor.FilterDescriptors.Add(new FilterDescriptor(filterDescriptor.Member,
                                FilterOperator.IsLessThan, new DateTime(value.Year, value.Month, value.Day, 0, 0, 0).AddDays(1)));
                            return isEqualCompositeFilterDescriptor;

                        case FilterOperator.IsNotEqualTo:
                            //convert the "is not equal to <date><time>" filter to a "is less than <date> 00:00:00" OR "is greater than or equal to <date + 1day> 00:00:00"
                            var notEqualCompositeFilterDescriptor = new CompositeFilterDescriptor { LogicalOperator = FilterCompositionLogicalOperator.Or };
                            notEqualCompositeFilterDescriptor.FilterDescriptors.Add(new FilterDescriptor(filterDescriptor.Member,
                                FilterOperator.IsLessThan, new DateTime(value.Year, value.Month, value.Day, 0, 0, 0)));
                            notEqualCompositeFilterDescriptor.FilterDescriptors.Add(new FilterDescriptor(filterDescriptor.Member,
                                FilterOperator.IsGreaterThanOrEqualTo, new DateTime(value.Year, value.Month, value.Day, 0, 0, 0).AddDays(1)));
                            return notEqualCompositeFilterDescriptor;

                        case FilterOperator.IsGreaterThanOrEqualTo:
                            //convert the "is greater than or equal to <date><time>" filter to a "is greater than or equal to <date> 00:00:00"
                            filterDescriptor.Value = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
                            return filterDescriptor;

                        case FilterOperator.IsGreaterThan:
                            //convert the "is greater than <date><time>" filter to a "is greater than or equal to <date + 1day> 00:00:00"
                            var greaterThanFilterDescriptor = new FilterDescriptor(filterDescriptor.Member, FilterOperator.IsGreaterThanOrEqualTo, new DateTime(value.Year, value.Month, value.Day, 0, 0, 0).AddDays(1));
                            return greaterThanFilterDescriptor;

                        case FilterOperator.IsLessThanOrEqualTo:
                            //convert the "is less than or equal to <date><time>" filter to a "is less than <date + 1day> 00:00:00"
                            var lessThanEqualToFilterDescriptor = new FilterDescriptor(filterDescriptor.Member, FilterOperator.IsLessThan, new DateTime(value.Year, value.Month, value.Day, 0, 0, 0).AddDays(1));
                            return lessThanEqualToFilterDescriptor;

                        case FilterOperator.IsLessThan:
                            //convert the "is less than <date><time>" filter to a "is less than <date> 00:00:00"
                            filterDescriptor.Value = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
                            return filterDescriptor;

                        case FilterOperator.IsNull:
                            return filterDescriptor;

                        case FilterOperator.IsNotNull:
                            return filterDescriptor;

                        default:
                            throw new Exception(string.Format("Filter operator '{0}' is not supported for DateTime member '{1}'", filterDescriptor.Operator, filterDescriptor.Member));
                    }
                }
            }
            return filter;
        }


        public static void NormalizeDateStartFilter(this DataSourceRequest request, string name)
        {
            if (request.Filters == null)
                return;

            var dateFilter = request.Filters.Where(f => f is FilterDescriptor).SingleOrDefault(c => (c as FilterDescriptor).Member == name);
            if (dateFilter == null)
                return;

            var filterDescriptor = dateFilter as FilterDescriptor;
            if (!(filterDescriptor.Value is DateTime))
                return;

            var value = (DateTime)filterDescriptor.Value;
            filterDescriptor.Value = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
        }
    }
}