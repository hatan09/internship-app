using Microsoft.AspNetCore.Components;

namespace Wave5.UI;

public partial class FormDetailsField : ComponentBase
{
    #region [ CTor ]
    public FormDetailsField() {

    }
    #endregion

    #region [ Properties - Parameters ]
    [Parameter]
    public string Label { get; set; }

    [Parameter]
    public string Value { get; set; }

    [Parameter]
    public bool AsTextArea { get; set; }
    #endregion
}
