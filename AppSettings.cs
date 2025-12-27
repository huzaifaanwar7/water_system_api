namespace GBS.Api;

public class AppSettings
{
    public string Environment { get; set; }
    public string Secret { get; set; }
    public string TwoFAIssuer { get; set; }
    public string FirebaseServerKey { get; set; }
    public string LDAPDomain { get; set; }
    public string MediaPath { get; set; }
}