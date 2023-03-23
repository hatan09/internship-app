using Microsoft.AspNetCore.Components.Web;

namespace Wave5.UI.Blazor;

public static class CommandBarFactory
{
    #region [ Public Methods - Create CommandBarItemContext ]
    public static CommandBarItemContext CreateButton(
            string name,
            string label,
            string iconName,
            Action<EventArgs> onClick)
    {

        var button = new CommandBarItemContext();
        button.Name = name;
        button.Label = label;
        button.IconName = iconName;
        button.OnClick += onClick;

        return button;
    }

    public static CommandBarItemContext CreateButton(
            string name,
            string label,
            string iconName,
            IEnumerable<IContextualMenuItemContext> contextMenu)
    {

        var button = new CommandBarItemContext();
        button.Name = name;
        button.Label = label;
        button.IconName = iconName;
        button.ContextMenu = contextMenu;

        return button;
    }

    public static CommandBarItemContext CreateBackButton(Action<EventArgs> onClick)
    {
        return CreateButton(ButtonNames.BackButton, "Back", "Back", onClick);
    }

    public static CommandBarItemContext CreateRefreshButton(Action<EventArgs> onClick)
    {
        return CreateButton(ButtonNames.RefreshButton, "Refresh", "Refresh", onClick);
    }

    public static CommandBarItemContext CreateAddButton(Action<EventArgs> onClick)
    {
        return CreateButton(ButtonNames.AddButton, "Add", "Add", onClick);
    }

    public static CommandBarItemContext CreateEditButton(Action<EventArgs> onClick, bool isVisible = false)
    {
        return CreateButton(ButtonNames.EditButton, "Edit", "SingleColumnEdit", onClick)
                    .SetProperty(nameof(CommandBarItemContext.DisableWhenNoData), true)
                    .SetProperty(nameof(CommandBarItemContext.IsVisible), isVisible);
    }

    public static CommandBarItemContext CreateDeleteButton(Action<EventArgs> onClick, bool isVisible = false)
    {
        return CreateButton(ButtonNames.DeleteButton, "Delete", "Delete", onClick)
                    .SetProperty(nameof(CommandBarItemContext.DisableWhenNoData), true)
                    .SetProperty(nameof(CommandBarItemContext.IsVisible), isVisible);
    }

    public static CommandBarItemContext CreateFilterButton(List<IContextualMenuItemContext> contextMenu)
    {
        return CreateButton(ButtonNames.FilterButton, string.Empty, "Filter", contextMenu);
    }
    #endregion


    #region [ Public Methods - Create CommandBarItemContext ]
    public static ContextualMenuItemContext CreateMenuItem(
            string id,
            string label,
            string iconName,
            Action<MouseEventArgs> onClick)
    {

        var meuItem = new ContextualMenuItemContext();
        // TODO: meuItem.Id = id;
        meuItem.Label = label;
        meuItem.IconName = iconName;
        meuItem.OnClick += onClick;

        return meuItem;
    }

    public static ContextualMenuItemContext CreateMenuItemFilterAll(Action<MouseEventArgs> onClick)
    {
        return CreateMenuItem(MenuItemNames.FilterAll, MenuItemNames.FilterAll, string.Empty, onClick);
    }

    public static ContextualMenuItemContext CreateMenuItemFilterActive(Action<MouseEventArgs> onClick)
    {
        return CreateMenuItem(MenuItemNames.FilterActive, MenuItemNames.FilterActive, string.Empty, onClick);
    }

    public static ContextualMenuItemContext CreateMenuItemFilterInActive(Action<MouseEventArgs> onClick)
    {
        return CreateMenuItem(MenuItemNames.FilterInActive, MenuItemNames.FilterInActive, string.Empty, onClick);
    }

    #endregion
}