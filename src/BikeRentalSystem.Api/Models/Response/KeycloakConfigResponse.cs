using Newtonsoft.Json;

namespace BikeRentalSystem.Api.Models.Response;

public class KeycloakConfigResponse
{
    [JsonProperty("realm")]
    public string Realm { get; set; }

    [JsonProperty("auth-server-url")]
    public string AuthServerUrl { get; set; }

    [JsonProperty("resource")]
    public string Resource { get; set; }

    [JsonProperty("ssl-required")]
    public bool SslRequired { get; set; }

    [JsonProperty("public-client")]
    public bool PublicClient { get; set; } = true;

    [JsonProperty("verify-token-audience")]
    public bool VerifyTokenAudience { get; set; } = true;

    [JsonProperty("use-resource-role-mappings")]
    public bool UseResourceRoleMappings { get; set; } = true;

    [JsonProperty("confidential-port")]
    public int ConfidentialPort { get; set; }

    [JsonProperty("credentials")]
    public KeycloakCredentials? Credentials { get; set; }

    public string Authority => $"{AuthServerUrl}/realms/{Realm}";
}

public class KeycloakCredentials
{
    [JsonProperty("secret")]
    public string? Secret { get; set; }
}
