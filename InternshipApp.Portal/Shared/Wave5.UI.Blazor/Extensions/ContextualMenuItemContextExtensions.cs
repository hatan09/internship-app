using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;

namespace Wave5.UI.Blazor;

public static class ContextualMenuItemContextExtensions
{
    #region [ Public Methods - Add Button ]

    public static void AddButton(this List<IContextualMenuItemContext> items,
        string id,
        string label,
        string iconName,
        Action<MouseEventArgs> onClick) {

        items.Add(CommandBarFactory.CreateMenuItem(id, label, iconName, onClick));
    }


    public static void AddMenuItemFilterAll(this List<IContextualMenuItemContext> items, Action<MouseEventArgs> onClick) {
        items.Add(CommandBarFactory.CreateMenuItemFilterAll(onClick));
    }

    public static void AddMenuItemFilterActive(this List<IContextualMenuItemContext> items, Action<MouseEventArgs> onClick) {
        items.Add(CommandBarFactory.CreateMenuItemFilterActive(onClick));
    }

    public static void AddMenuItemFilterInActive(this List<IContextualMenuItemContext> items, Action<MouseEventArgs> onClick) {
        items.Add(CommandBarFactory.CreateMenuItemFilterInActive(onClick));
    }
    #endregion
}