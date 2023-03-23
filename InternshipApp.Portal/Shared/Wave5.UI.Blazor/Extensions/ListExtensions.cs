using Microsoft.Fast.Components.FluentUI;
using System.Collections.Generic;

namespace Wave5.UI.Blazor.FluentUI;

public static class ListExtensions
{
    #region [ Public Methods - List<string> ]
    public static List<Option<string>> ToOptionList<TValue>(this List<TValue> list, TValue selectedItem, string placeHolderName = "") {

        var result = new List<Option<string>>();
        if (!string.IsNullOrEmpty(placeHolderName)) {

            result.Add(new Option<string>() {
                Key = default,
                Value = placeHolderName,
                Selected = false
            });

        }

        list.ForEach(x => result.Add(new Option<string>() {
            Key = x.ToString(),
            Value = x.ToString(),
            Selected = x.Equals(selectedItem)
        }));

        return result;
    }
    #endregion
}