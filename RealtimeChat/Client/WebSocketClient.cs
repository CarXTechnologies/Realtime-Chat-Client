using System.Net.WebSockets;
using System.Text;
using RealtimeChat.Model;
using RealtimeChat.Utils;

namespace RealtimeChat.Client;

/// <summary>
/// WebSocketClient API
/// </summary>
public class WebSocketClient
{
    /// <summary>
    /// Event when new message are received
    /// </summary>
    public Action<ChatMessage> MessageReceivedEvent;
    
    /// <summary>
    /// Event when connection are closed remotely 
    /// </summary>
    public Action<WebSocketReceiveResult> ConnectionClosedEvent;
    
    private readonly ClientWebSocket m_client;
    private readonly string m_baseUrl;
    private readonly string m_token;

    private const string AuthHeaderName = "Authorization";

    /// <summary>
    /// WebSocketClient constructor
    /// </summary>
    /// <param name="host">host of chat server</param>
    /// <param name="port">port of chat server</param>
    /// <param name="token">player access token</param>
    public WebSocketClient(string host, int port, string token)
    {
        m_client = new ClientWebSocket();
        m_baseUrl = $"ws://{host}:{port}/chat/";
        m_token = $"Bearer {token}";
    }

    /// <summary>
    /// Start connection to chat
    /// </summary>
    /// <param name="roomId">chat room ID</param>
    public async Task ConnectAsync(string roomId)
    {
        var uri = new Uri($"{m_baseUrl}{roomId}");
        m_client.Options.SetRequestHeader(AuthHeaderName, m_token);
        await m_client.ConnectAsync(uri, CancellationToken.None);
    }

    /// <summary>
    /// Sending message
    /// </summary>
    /// <param name="message">message text</param>
    public async Task SendAsync(string message)
    {
        var messageJson = JsonUtils.ToJson(new ChatMessage(message));
        var buffer = Encoding.UTF8.GetBytes(messageJson);
        await m_client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true,
            CancellationToken.None);
    }

    /// <summary>
    /// Listening for new messages
    /// </summary>
    public async Task ListenAsync()
    {
        var buffer = new byte[1024];
        while (m_client.State == WebSocketState.Open)
        {
            var result = await m_client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                ConnectionClosedEvent(result);
                break;
            }

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var objMessage = JsonUtils.FromJson<ChatMessage>(message);

            if (objMessage == null)
            {
                continue;
            }

            MessageReceivedEvent(objMessage);
        }
    }

    /// <summary>
    /// Closing connection with chat
    /// </summary>
    /// <param name="status">close status</param>
    /// <param name="reason">close reason</param>
    public async Task CloseAsync(WebSocketCloseStatus status, string reason)
    {
        await m_client.CloseAsync(status, reason, CancellationToken.None);
    }
}