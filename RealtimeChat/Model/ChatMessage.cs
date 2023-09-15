namespace Reatime_Chat.model;

public class ChatMessage
{
    public ChatMessage(string text, string carxUserId = null, string userName = null, long? timestamp = null)
    {
        Text = text;
        CarxUserId = carxUserId;
        UserName = userName;
        Timestamp = timestamp;
    }

    public string Text { get; set; }
    public string CarxUserId { get; set; } 
    public string UserName { get; set; }  
    public long? Timestamp { get; set; } 
}