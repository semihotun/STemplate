﻿using PaymentService.Insfrastructure.Utilities.Grid.Filter;

namespace PaymentService.Insfrastructure.Utilities.Grid.PagedList
{
    /// <summary>
    /// Pagedlist filter model
    /// </summary>
    public class PagedListFilterModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? OrderByColumnName { get; set; }
        public List<FilterModel>? FilterModelList { get; set; }
    }
}
