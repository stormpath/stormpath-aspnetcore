public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    // Logging and static file middleware (if applicable)

    app.UseStormpath();

    // MVC or other framework middleware here
}
