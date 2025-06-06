namespace Labs.UI.Middleware
{
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class FileLoggerExtensions
    {
        public static IApplicationBuilder UseFileLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FileLogger>();
        }
    }
}
