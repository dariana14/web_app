using System.Text.Json.Serialization;

namespace App.DTO.v1_0.Identity;

public class JWTResponse
{
    public string Jwt { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    
    [JsonPropertyName("userId")]
    public String UserId { get; set; }  = default!;
}