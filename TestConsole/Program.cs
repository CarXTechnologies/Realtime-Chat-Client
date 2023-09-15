// See https://aka.ms/new-console-template for more information

using System.Net.WebSockets;
using RealtimeChat.Client;

class TestClass
{
    static void Main(string[] args)
    {
        StartClient().Wait();
    }

    private async static Task StartClient()
    {
        var token =
            "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJVc2VyREVGQVVMVDc5MzQzIiwic2NvcGVzIjpbIlJPTEVfRFIzTSIsIlJPTEVfRFIzQyIsIlJPTEVfU1RSRUVUIl0sIm9yaWciOiI2YjljYWZjZi05YmE2LTRhMjgtOGEzZS05ZTZlMGYxZGQxN2UiLCJpc3MiOiJDYXJYIElEIGF1dGgiLCJpYXQiOjE2OTMzMTk3NzksImV4cCI6NDEwMjQzNDAwMH0.yot9x132fcbAbDHhSy1hwfd0af3y97BlIhKpROc94pRhVLqyVm7qBaKWBo6swuVKcwZYb_Fex_nVzEzyynfb7Q";
        var roomId = "bd85f9a4-20ff-47e3-a2af-d06d9e0ab66f";
        var webSocketClient = new WebSocketClient("localhost", 8083,
            message =>
            {
                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime date = start.AddMilliseconds(message.Timestamp ?? DateTime.Now.Millisecond).ToLocalTime();
                Console.WriteLine("[{0}]<{1}>: {2}", date.ToLongTimeString(), message.UserName, message.Text);
            }, eventMessage =>
            {
                Console.WriteLine($"Connection are closed with status {eventMessage.CloseStatus}");
            });
        await webSocketClient.ConnectAsync(roomId, token);
        await webSocketClient.ListenAsync().WaitAsync(CancellationToken.None);
        while (true)
        {
            var readLine = Console.ReadLine();

            if (readLine == "!exit")
            {
                webSocketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnected").Wait();
                break;
            }

            webSocketClient.SendAsync(readLine).WaitAsync(CancellationToken.None);
        }
    }
}