using Microsoft.IdentityModel.Tokens;

public interface IJwtSigningEncodingKey
{
    string SigningAlgorithm { get; }

    SecurityKey GetKey();
}