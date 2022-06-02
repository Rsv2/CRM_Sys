using Microsoft.IdentityModel.Tokens;

public interface IJwtSigningDecodingKey
{
    SecurityKey GetKey();
}
