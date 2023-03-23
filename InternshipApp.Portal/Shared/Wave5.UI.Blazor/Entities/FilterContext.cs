using System;

namespace Wave5.UI.Blazor;

public class FilterContext
{
    #region [ CTor ]
    public FilterContext() {
        this.Id = Guid.NewGuid().ToString();
        Disabled = false;
        IsVisible = true;
    }
    #endregion

    #region [ Properties ]
    public string Id { get; set; }

    public string Name { get; set; }

    public string Label { get; set; }

    public string IconName { get; set; }

    public bool Disabled { get; set; }

    public bool IsVisible { get; set; }

    public Action<FilterContext> OnClick { get; set; }
    #endregion
}