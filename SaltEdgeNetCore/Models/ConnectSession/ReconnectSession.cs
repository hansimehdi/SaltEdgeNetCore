using Newtonsoft.Json;
using SaltEdgeNetCore.Models.Consents;

namespace SaltEdgeNetCore.Models.ConnectSession
{
    public class ReconnectSession: SeSession
    {
        [JsonProperty("connection_id")]
        public string ConnectionId { get; set; }
        
        [JsonProperty("consent")]
        public SeConsent Consent { get; set; }
        
        [JsonProperty("show_consent_confirmation")]
        public bool? ShowConsentConfirmation { get; set; }
        
        [JsonProperty("credentials_strategy")]
        public string CredentialsStrategy { get; set; }
        
        [JsonProperty("override_credentials_strategy")]
        public string OverrideCredentialsStrategy { get; set; }
    }
}