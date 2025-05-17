using System;
using System.Collections.Generic;
using System.Linq;

namespace BoulevardManagement.WebApplication.Models.DynamicGrid
{
    public class GridDynamicViewModel
    {
        public List<MyColumnSettings> GridColumns { get; set; }
    }


    public class MyColumnSettings
    {

        /// <summary>Title used in header.</summary>
        public string Title { get; set; }

        /// <summary>Property name in row viewmodel that the column is bound to.</summary>
        public string PropertyName { get; set; }


        /// <summary>True if field can be edited</summary>
        public bool Editable { get; set; }
        public bool Filterable { get; set; }

        public bool Group { get; set; }

        /// <summary>System type of the property being edited. Required for grid edtiable setting.</summary>
        public Type ColType { get; set; }

        /// <summary>Width to set the column</summary>
        public int Width { get; set; }
        public string Template { get; set; }

    }
}