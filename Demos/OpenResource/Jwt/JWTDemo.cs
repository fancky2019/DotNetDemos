using JWT;
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
    ///官网文档： https://jwt.io/introduction/
    /// NuGet:搜索 JWT
    /// github:https://github.com/jwt-dotnet/jwt
    /// 文档：https://github.com/jwt-dotnet/jwt/blob/master/README.md
    /// 
    /// 
    /// JWT存储：cookie 或window.localStorage中。
    ///         cookie:$.cookie('the_cookie', 'the_value');读取 $.cookie('the_cookie');
    ///   localStorage:window.localStorage.setItem('key',value)、读取window.localStorage.getItem("key")。
    ///   
    /// 香关术语：CORS（ Central Authentication Service ） ;跨域资源共享
    ///           SS0 （Single Sign-On）:单点登录
    ///           CAS ：统一认证中心
    ///         重定向：如果未登录，跳转到认证中心，并将当前url作为参数传到认证中心（如：http://cas.server:8080/?service=http://a:8080/，），
    ///                 认证中心登录成功之后，返回token，跳转到之前的url。
    ///      
    /// 6.JSON Web Tokens由三部分组成，用英文句点分割(.) ，一般看起来例如：xxxxx.yyyyy.zzzzz
    ///分为：
    /// Header 头信息
    /// Payload 荷载信息, 实际数据
    ///  Signature 由头信息+荷载信息+密钥 组合之后进行加密得到
    ///　1、Header 头信息通常包含两部分，type：代表token的类型，这里使用的是JWT类型。 alg:使用的Hash算法，例如HMAC SHA256或RSA.
    ///　   {
    ///      "alg": "HS256",
    ///      "typ": "JWT"
    ///     }
    ///  2、Payload  一个token的第二个部分是荷载信息，它包含一些声明Claim(实体的描述，通常是一个User信息，还包括一些其他的元数据)
    ///  　声明分三类:
    ///  　　1)Reserved Claims, 这是一套预定义的声明，并不是必须的,这是一套易于使用、操作性强的声明。包括：iss(issuer)、exp(expiration time)、sub(subject)、aud(audience)等
    ///  　  2)Plubic Claims,
    ///  　  3)Private Claims, 交换信息的双方自定义的声明
    ///  3、Signature  使用header中指定的算法将编码后的header、编码后的payload、一个secret进行加密 
    /// </summary>
    class JWTDemo
    {
        private const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        public void Test()
        {
            var length = secret.Length;
            //调试工具：https://jwt.io/
            //默认加密算法：HMACSHA256
            //jwt 的Signature:通过加密算法（加密的key：secret）将  headerBase64Str+"."+payloadBase64Str 进行加密后的base64Str。
            //header(base64后的)
            //payload(base64后的)
            //secret 加密的key
            //eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE1Njg2MDc4MTgsInVzZXJOYW1lIjoiZmFuY2t5In0.aKR1PQdKBgK1crvd5pjXZWEzW7jp71srppV0lWTcQM4
            string encoderStr = EncoderTokenString();
            var decoderStr = DecoderTokenString(encoderStr);
            var dic = DecoderTokenToDictionary(encoderStr);
            //DateTimeOffset dto = DateTimeOffset.FromUnixTimeMilliseconds((long)dic["exp"]);
            DateTimeOffset dto = DateTimeOffset.FromUnixTimeSeconds((long)dic["exp"]);
            string str = dto.ToString("yyyy-DD-MM HH:mm:ss fff");


            JwtParts jwtParts = new JwtParts(encoderStr);
            var urlEncoder = new JwtBase64UrlEncoder();

            var decodedPayload = urlEncoder.Decode(jwtParts.Payload);
             var strPayload = Encoding.UTF8.GetString(decodedPayload);

            var decodedHeader = urlEncoder.Decode(jwtParts.Header);
            var strHeader = Encoding.UTF8.GetString(decodedHeader);

            ////得到HMACSHA256加密后的字节数组byte[32]==256位。
            /////2GLQT5eKGrY2WR8x1Myi7BQ1w92qvw5nDw-If7Ya0_Y
            //var decodedSignature = urlEncoder.Decode(jwtParts.Signature);
            //var strSignature = Encoding.UTF8.GetString(decodedSignature);

            var _algFactory = new HMACSHAAlgorithmFactory();
            var alg = _algFactory.Create("HS256");
            var secrets = new string[] { secret };
            var keys = secrets.Select(s => Encoding.UTF8.GetBytes(s)).ToArray();
    
        
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
                              //.AddHeader("","")
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
