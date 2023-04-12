using System.Collections.ObjectModel;
using Syncfusion.Blazor.Inputs;

namespace InternshipApp.Portal.Views;

public partial class ChatView
{
    #region [ Fields ]
    public SfTextBox? SfTextBox;
    public ObservableCollection<ChatModel> Chat { get; set; } = new ObservableCollection<ChatModel>() {
        new ChatModel {
            Name = "Jenifer",
            ChatMessage = "Hi",
            Id = "1",
            Avatar = "",
            IsSender = true
        },
        new ChatModel {
            Name = "Jenifer",
            ChatMessage = "Hi",
            Id = "1",
            Avatar = "",
            IsSender = false
        }
    };
    #endregion

        #region [ Private Methods - Event Handler ]
    public void OnSend()
    {

    }
    #endregion
}
