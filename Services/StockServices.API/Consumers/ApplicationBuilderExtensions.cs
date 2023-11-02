namespace StockServices.API;

    public static class ApplicationBuilderExtentions
    {

    private static ProductCreatedEventConsumer Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<ProductCreatedEventConsumer>();

            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);
            return app;
        }

        private static void OnStarted()
        {
            Listener.Register();
        }
        
    }
