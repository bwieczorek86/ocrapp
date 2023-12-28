namespace OcrPlugin.App.BlazorClient.Client.Common
{
    [Serializable]
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}