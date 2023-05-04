using System.Collections.ObjectModel;
using Microsoft.Fast.Components.FluentUI;

namespace InternshipApp.Portal.Views;

public static class ListOptionsFactory
{
    #region [ Public Methods - Options ]
    public static List<Option<string>> ToOptionList<TItem>(this ObservableCollection<TItem> options, Func<TItem, string> key, Func<TItem, string> value, string selectedId, string placeHolderName = null)
    {
        return options.ToList().ToOptionList(key, value, selectedId, placeHolderName);

    }

    public static List<Option<string>> ToOptionList<TItem>(this List<TItem> options, Func<TItem, string> key, Func<TItem, string> value, string selectedId, string placeHolderName = null)
    {
        var result = new List<Option<string>>();

        if (!string.IsNullOrEmpty(placeHolderName))
        {
            result.Add(new Option<string>()
            {
                // key should equal to "" in order to make this invalid value
                // so that the validation for required will be trigger
                Key = string.Empty,
                Value = placeHolderName,
                Selected = false
            });
        }

        options.ForEach(x =>
        {
            result.Add(new Option<string>()
            {
                Key = key.Invoke(x),
                Value = value.Invoke(x),
                Selected = selectedId == key.Invoke(x)
            });
        });

        return result;
    }

    #endregion
}
