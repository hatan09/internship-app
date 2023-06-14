using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class ChatPage
{
    [Parameter]
    public string ReceiverId { get; set; }
}
