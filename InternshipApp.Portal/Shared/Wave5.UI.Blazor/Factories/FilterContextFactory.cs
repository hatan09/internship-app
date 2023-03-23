using System;

namespace Wave5.UI.Blazor;

public static class FilterContextFactory
{
    #region [ Public Methods - Create ]
    public static FilterContext CreateFilter(
            string name,
            string label,
            string iconName,
            Action<FilterContext> onClick) {

        var filter = new FilterContext();
        filter.Name = name;
        filter.Label = label;
        filter.IconName = iconName;
        filter.OnClick = (x) => {
            onClick.Invoke(filter);
        };

        return filter;
    }


    public static FilterContext CreateFilterAll(Action<FilterContext> onClick) {
        return CreateFilter(FilterNames.FilterAll, "All", string.Empty, onClick);
    }

    public static FilterContext CreateFilterActive(Action<FilterContext> onClick) {
        return CreateFilter(FilterNames.FilterActive, "Active", string.Empty, onClick);
    }

    public static FilterContext CreateFilterInActive(Action<FilterContext> onClick) {
        return CreateFilter(FilterNames.FilterInActive, "Inactive", string.Empty, onClick);
    }
    #endregion
}