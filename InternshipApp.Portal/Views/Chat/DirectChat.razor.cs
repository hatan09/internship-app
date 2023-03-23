using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace InternshipApp.Portal.Views;

public partial class DirectChat
{
    private HubConnection _hub;
    private List<string> messages = new List<string>();
    private string user;
    private string message;

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _hub = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/chat"))
            .Build();

        _hub.On<string, string>("Broadcast", (user, message) =>
        {
            var encodedMsg = $"{user}: {message}";
            messages.Add(encodedMsg);
            StateHasChanged();
        });

        await _hub.StartAsync();
    }

    async Task Send()
    {
        await _hub.SendAsync("Broadcast", user, message);
        message = string.Empty;
    }
}
