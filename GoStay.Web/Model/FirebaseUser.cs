using Newtonsoft.Json;

namespace GoStay.Web.Model
{
    public class credential
    {
        [JsonProperty("accessToken")]
        public string? AccessToken { get; set; }
    }

    public class user
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("photoURL")]
        public string? PhotoURL { get; set; }

        [JsonProperty("displayName")]
        public string? DisplayName { get; set; }
    }

    public class FirebaseUser
    {
        [JsonProperty("user")]
        public user User { get; set; }

        [JsonProperty("credential")]
        public credential? Credential { get; set; }
    }
}
