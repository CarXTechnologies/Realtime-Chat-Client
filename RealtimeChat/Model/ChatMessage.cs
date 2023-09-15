namespace RealtimeChat.Model;

/// <summary>
/// Chat message model
/// </summary>
public class ChatMessage
{
    public ChatMessage(string text, string? carxUserId = null, string? userName = null, long? timestamp = null)
    {
        Text = text;
        CarxUserId = carxUserId;
        UserName = userName;
        Timestamp = timestamp;
    }

    /// <summary>
    /// Message text
    /// </summary>
    public string Text { get; }
    
    /// <summary>
    /// UUID of player
    /// </summary>
    public string? CarxUserId { get; }
    
    /// <summary>
    /// Player nickname
    /// </summary>
    public string? UserName { get; }
    
    /// <summary>
    /// Message timestamp
    /// </summary>
    public long? Timestamp { get; }
}