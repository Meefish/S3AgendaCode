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
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var messageData = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                if (messageData.TryGetValue("calendarId", out var calendarIdString) && int.TryParse(calendarIdString, out var calendarId))
                {
                    RegisterCalendarIdSocket(calendarId.ToString(), webSocket);
                    await SendPushNotification(calendarId, webSocket);
                }
            }
        }

        private void RegisterCalendarIdSocket(string calendarIdString, WebSocket webSocket)
        {
            WebSocket? existingSocket;
            if (!_sockets.TryGetValue(calendarIdString, out existingSocket))
            {
                _sockets.Add(calendarIdString, webSocket);
            }
            else
            {

                if (_sockets[calendarIdString] != webSocket)
                {
                    _sockets[calendarIdString] = webSocket;
                }


            }
        }
        private static async Task SendMessageAsync(WebSocket webSocket, string message)
        {
            if (webSocket.State == WebSocketState.Open)
            {
                var responseBuffer = Encoding.UTF8.GetBytes(message);
                await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }


        private async Task SendPushNotification(int calendarId, WebSocket webSocket)
        {
            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var calendarDAL = scope.ServiceProvider.GetRequiredService<ICalendarDAL>();
                    var tasks = await calendarDAL.CheckTaskEvent(calendarId);
                    if (tasks.Any())
                    {
                        var response = JsonConvert.SerializeObject(tasks);
                        await SendMessageAsync(webSocket, response);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(10));        //If polling would be allowed
                }
            }
            finally
            {
                await CloseWebSocketAsync(calendarId.ToString(), webSocket);
            }
        }
        private async Task CloseWebSocketAsync(string calendarIdString, WebSocket webSocket)
        {
            _sockets.Remove(calendarIdString);

            if (webSocket.State != WebSocketState.Closed && webSocket.State != WebSocketState.Aborted)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }
        }
    }
}