using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views; 
public partial class SendEmailView {
    #region [ Properties - Parameters ]
    [Parameter]
    public int CompanyId { get; set; }


    #endregion

    public string Subject { get; set; }
    public string Body { get; set; }
}
