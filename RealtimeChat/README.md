### Библиотека для использования чата

#### Установка библиотеки:

Добавить RealimeChat.dll как ассет в редакторе. Из зависимостей используется только [Newtonsoft.Json](https://www.newtonsoft.com/json), который включен в
пакет Unity

#### Использование библиотеки

Библиотека предоставляет класс **WebSocketClient**, который позволяет подключатся к комнате чата и получать/отправлять
сообшения.
Библиотека асинхронная, работа идёт с классом *
*[Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=netcore-2.1)**.

Пример создания экземпляра клиента:

```csharp
using System.Net.WebSockets;
using RealtimeChat.Client;

...
    
var client = new WebSocketClient("localhost", 8083, token);
```

где указывается:

1. Хост чата
2. Порт чата
3. Токен доступа, используемый для вызова серверных эндпоинтов

Далее, можно подписаться на события сокета: получения сообщения и сигнала о закрытие соединения.
```csharp
client.MessageReceivedEvent += OnMessageReceived();
client.ConnectionClosedEvent += OnConnectionClosed();

private static Action<WebSocketReceiveResult> OnConnectionClosed()
{
    return closeEvent => { Debug.Log($"Closed event: {closeEvent.CloseStatusDescription}"); };
}

private static Action<ChatMessage> OnMessageReceived()
{
    return msg => { Debug.Log($"Message: {msg.Text}"); };
}
```

Созданным клиентом можно подключиться к определенной комнате чата по её ID и начать "слушать" сокет:
```csharp
 client.ConnectAsync(roomId).GetAwaiter().OnCompleted(() =>
 {
     client.ListenAsync().Start();
     Debug.Log($"Connected to chat room <{roomId}>");
 });
```
Пример отправки сообщений:
```csharp
client.SendAsync("Hello mates!").Start();
```
Пример закрытия соединиения
```csharp
client.CloseAsync().GetAwaiter().OnCompleted(() =>
{
    Debug.Log("Connection are closed");
});
```
Сообщения чата (ChatMessage) содержат в себе:
1. Text - сообщение чата
2. CarxUserId - ID игрока
3. UserName - ник игрока
4. Timestamp - временная метка сообщения~~~~