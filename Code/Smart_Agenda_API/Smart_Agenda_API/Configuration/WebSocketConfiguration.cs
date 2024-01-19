namespace Smart_Agenda_API.Configuration
{
    public class WebSocketConfiguration
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public WebSocketConfiguration(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void ConfigureWebSockets(IApplicationBuilder app)
        {
            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2)
            };

            app.UseWebSockets(webSocketOptions);

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        var handler = new WebSocketHandler(_scopeFactory);
                        await handler.HandleWebSocketConnection(webSocket);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    }
                }
                else
                {
                    await next();
                }
            });
        }
    }
}
