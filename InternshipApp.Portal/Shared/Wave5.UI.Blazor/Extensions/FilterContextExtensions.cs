using System;
using System.Collections.Generic;

namespace Wave5.UI.Blazor;

public static class FilterContextExtensions
{
    #region [ Public Methods - List<FilterContext> Extensions ]
    public static void Add(
            this List<FilterContext> items,
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

        items.Add(filter);
    }


    public static void Add(this List<FilterContext> items, FilterContext filter) {
        items.Add(filter);
    }

    public static void AddFilterAll(this List<FilterContext> items, Action<FilterContext> onClick) {
        items.Add(FilterContextFactory.CreateFilterAll(onClick));
    }

    public static void AddFilterActive(this List<FilterContext> items, Action<FilterContext> onClick) {
        items.Add(FilterContextFactory.CreateFilterActive(onClick));
    }

    public static void AddFilterInActive(this List<FilterContext> items, Action<FilterContext> onClick) {
        items.Add(FilterContextFactory.CreateFilterInActive(onClick));
    }
    #endregion
}
