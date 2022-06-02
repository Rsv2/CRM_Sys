using Microsoft.IdentityModel.Tokens;


/// <summary>
/// Ключ для шифрования данных (публичный).
/// </summary>
public interface IJwtEncryptingEncodingKey
{
    string SigningAlgorithm { get; }

    string EncryptingAlgorithm { get; }

    SecurityKey GetKey();
}
