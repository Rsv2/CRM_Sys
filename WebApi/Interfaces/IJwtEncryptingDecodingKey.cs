using Microsoft.IdentityModel.Tokens;

/// <summary>
///  Ключ для дешифрования данных (приватный)
/// </summary>
public interface IJwtEncryptingDecodingKey
{
    SecurityKey GetKey();
}
