using Newtonsoft.Json;
using Smart_Agenda_Logic.Interfaces;
using System.Net.WebSockets;
using System.Text;

namespace Smart_Agenda_API
{
    public class WebSocketHandler
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private Dictionary<string, WebSocket> _sockets = new Dictionary<string, WebSocket>();

        public WebSocketHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task HandleWebSocketConnection(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var messageData = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                    if (messageData.TryGetValue("calendarId", out var calendarIdString) && int.TryParse(calendarIdString, out var calendarId))
                    {
                        if (!_sockets.ContainsKey(calendarIdString))
                        {
                            _sockets.Add(calendarIdString, webSocket);
                        }

                        using var scope = _scopeFactory.CreateScope();
                        var calendarDAL = scope.ServiceProvider.GetRequiredService<ICalendarDAL>();
                        var tasks = await calendarDAL.CheckTaskEvent(calendarId);

                        var response = JsonConvert.SerializeObject(tasks);
                        var responseBuffer = Encoding.UTF8.GetBytes(response);
                        await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }

        }

        public void SendPushNotification(string calendarIdString, string message)
        {
            if (_sockets.TryGetValue(calendarIdString, out var webSocket))
            {
                var responseBuffer = Encoding.UTF8.GetBytes(message);
                webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}