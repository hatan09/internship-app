namespace InternshipApp.Portal.Views;

public class ChatModel
{
	public ChatModel()
	{

	}

	#region [ Properties ]
	public string Id { get; set; }
	public string ChatMessage { get; set; }
	public string Avatar { get; set; }
	public string Name { get; set; }
	public bool IsSender { get; set; }
	#endregion
}
