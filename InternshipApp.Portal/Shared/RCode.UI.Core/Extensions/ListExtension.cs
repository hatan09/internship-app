using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RCode.UI.ViewModels;

public static class ListExtension
{
    #region [ Public Methods - Convert ]
    public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list) {
        return new ObservableCollection<T>(list); 
    }
    #endregion
}