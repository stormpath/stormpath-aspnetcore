services.AddStormpath(new StormpathConfiguration()
{
    Client = new ClientConfiguration()
    {
        ApiKey = new ClientApiKeyConfiguration()
        {
            Id = "YOUR_API_KEY_ID",
            Secret = "YOUR_API_KEY_SECRET"
        }
    }
});
