using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.Jwt
{
    /// <summary>
    /// JWT (JSON Web Token)
    /// NuGet:搜索 JWT
    /// github:https://github.com/jwt-dotnet/jwt
    /// 文档：https://github.com/jwt-dotnet/jwt/blob/master/README.md
    /// </summary>
    class JWTDemo
    {
        private const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        public void Test()
        {
            string encoderStr = EncoderTokenString();
            var decoderStr = DecoderTokenString(encoderStr);
            var dic = DecoderTokenToDictionary(encoderStr);
            //DateTimeOffset dto = DateTimeOffset.FromUnixTimeMilliseconds((long)dic["exp"]);
            DateTimeOffset dto = DateTimeOffset.FromUnixTimeSeconds((long)dic["exp"]);
            string str = dto.ToString("yyyy-DD-MM HH:mm:ss fff");
        }

        private string EncoderTokenString()
        {
            //iss：jwt签发者
            //sub：jwt所面向的用户
            //aud：接收jwt的一方
            //exp：jwt的过期时间，这个过期时间必须要大于签发时间
            //nbf：定义在什么时间之前，该jwt都是不可用的.
            //iat：jwt的签发时间
            //jti：jwt的唯一身份标识，主要用来作为一次性token,从而回避重放攻击。
            var token = new JwtBuilder()
                             .WithAlgorithm(new HMACSHA256Algorithm())
                             .WithSecret(secret)
                              //.AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeMilliseconds())
                              .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                             .AddClaim("userName", "fancky")
                             .Build();
            return token;
        }

        private string DecoderTokenString(string token)
        {
            var json = new JwtBuilder()
                          .WithSecret(secret)
                          .MustVerifySignature()
                          .Decode(token);
            return json;
        }

        private Dictionary<string, object> DecoderTokenToDictionary(string token)
        {
            var dic = new JwtBuilder()
                         .WithSecret(secret)
                         .MustVerifySignature()
                         .Decode<Dictionary<string, object>>(token);
            return dic;
        }

    }
}
