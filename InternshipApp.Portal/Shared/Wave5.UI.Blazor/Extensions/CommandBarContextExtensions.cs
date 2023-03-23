using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;

namespace Wave5.UI.Blazor;

public static class CommandBarContextExtensions
{
    #region [ Public Methods - SetProperty ]
    public static CommandBarItemContext SetProperty<TValue>(this CommandBarItemContext context, string propertyName, TValue value) {
        context.GetType().GetProperty(propertyName).SetValue(context, value);
        return context;

    }
    #endregion

    #region [ Public Methods - Add Button ]

    public static void AddButton(this List<ICommandBarItemContext> items,
        string id,
        string label,
        string iconName,
        Action<EventArgs> onClick) {

        items.Add(CommandBarFactory.CreateButton(id, label, iconName, onClick));
    }

    public static void AddBackButton(this List<ICommandBarItemContext> items, Action<EventArgs> onClick) {
        items.Add(CommandBarFactory.CreateBackButton(onClick));
    }

    public static void AddRefreshButton(this List<ICommandBarItemContext> items, Action<EventArgs> onClick) {
        items.Add(CommandBarFactory.CreateRefreshButton(onClick));
    }

    public static void AddFilterButton(this List<ICommandBarItemContext> items, List<IContextualMenuItemContext> contextMenu) {
        items.Add(CommandBarFactory.CreateFilterButton(contextMenu));
    }

    public static void AddAddButton(this List<ICommandBarItemContext> items, Action<EventArgs> onClick) {
        items.Add(CommandBarFactory.CreateAddButton(onClick));
    }

    public static void AddEditButton(this List<ICommandBarItemContext> items, Action<EventArgs> onClick, bool isVisible = false) {
        items.Add(CommandBarFactory.CreateEditButton(onClick, isVisible));
    }

    public static void AddDeleteButton(this List<ICommandBarItemContext> items, Action<EventArgs> onClick, bool isVisible = false) {
        items.Add(CommandBarFactory.CreateDeleteButton(onClick, isVisible));
    }
    #endregion
}