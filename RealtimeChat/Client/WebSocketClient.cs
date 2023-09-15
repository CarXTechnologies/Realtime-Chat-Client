using System.Net.WebSockets;
using System.Text;
using Reatime_Chat.model;
using Reatime_Chat.utils;

namespace Reatime_Chat.websocket;

/// <summary>
/// WebSocketClient API
/// </summary>
public class WebSocketClient
{
    private readonly ClientWebSocket _client;
    private readonly string _baseUrl;
    private readonly Action<ChatMessage> _doOnMessage;
    private readonly Action<WebSocketReceiveResult> _doOnCloseStatus;
    
    private const string AuthHeaderName = "Authorization";


    /// <summary>
    /// WebSocketClient constructor
    /// </summary>
    /// <param name="host">host of chat server</param>
    /// <param name="port">port of chat server</param>
    /// <param name="doOnMessage">action on receiving new message</param>
    /// <param name="doOnCloseStatus">action on receiving close signal from server</param>
    public WebSocketClient(string host, int port, Action<ChatMessage> doOnMessage,
        Action<WebSocketReceiveResult> doOnCloseStatus)
    {
        _client = new ClientWebSocket();
        _baseUrl = $"ws://{host}:{port}/chat/";
        _doOnMessage = doOnMessage;
        _doOnCloseStatus = doOnCloseStatus;
    }

    /// <summary>
    /// Start connection to chat
    /// </summary>
    /// <param name="roomId">chat room ID</param>
    /// <param name="token">player access token</param>
    public async Task ConnectAsync(string roomId, string token)
    {
        var uri = new Uri($"{_baseUrl}{roomId}");
        _client.Options.SetRequestHeader(AuthHeaderName, $"Bearer {token}");
        await _client.ConnectAsync(uri, CancellationToken.None);
    }
    
    public async Task SendAsync(string message)
    {
        var messageJson = JsonUtils.ToJson(new ChatMessage(message));
        var buffer = Encoding.UTF8.GetBytes(messageJson);
        await _client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text,
            WebSocketMessageFlags.EndOfMessage,
            CancellationToken.None);
        Console.WriteLine("Sent: {0}", message);
    }

    public async Task ListenAsync()
    {
        var buffer = new byte[1024];
        while (true)
        {
            var result = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                _doOnCloseStatus(result);
                break;
            }

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var objMessage = JsonUtils.FromJson<ChatMessage>(message);

            _doOnMessage(objMessage);
        }
    }

    public async Task CloseAsync(WebSocketCloseStatus status, string reason)
    {
        await _client.CloseAsync(status, reason, CancellationToken.None);
    }
}