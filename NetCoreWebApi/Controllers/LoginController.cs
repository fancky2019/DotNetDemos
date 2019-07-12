using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NetCoreWebApi.Model.Entity;
using NetCoreWebApi.Model.ViewModel;

namespace NetCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class LoginController : Controller
    {
        private const string _securityKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        //http://localhost:8464/api/Product/GetProductList?id=4
        [HttpGet("Login")]
        public IActionResult Login([FromQuery]User user)
        {
            //  var user = await _userManager.GetUserByUserNameAndPwd(request.Username, request.Password);//根据用户密码获取用户信息

            if (user == null)
            {
                return Json(new MessageResult<String> { Message = "用户名或密码错误", Success = false });
            }
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()) };//创建声明
            var now = DateTime.Now;
            //var ex = now + TimeSpan.FromMinutes(30);//过期时间设置为30分钟
            var ex = now + TimeSpan.FromSeconds(180);//过期时间设置为3分钟
                                                    //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));//获取密钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));//获取密钥
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//加密方式
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "yourdomain.com",
                Audience = "yourdomain.com",
                Expires = ex,
                IssuedAt = now,
                SigningCredentials = creds,
                Subject = new ClaimsIdentity(claims)
            };
            //每次都会生成一个新的token
            var token = new JwtSecurityTokenHandler().CreateEncodedJwt(securityTokenDescriptor);
            return Json(token);
        }

        //https://github.com/jwt-dotnet/jwt
        //PostMan:http 请求头(Headers)添加
        //Key           Value
        //Authorization:Bearer token。注意：Bearer 后一个空格
        [HttpGet("Test")]
        [Authorize]
        public IActionResult Test([FromQuery]User user)
        {
            return Json("Authorize");
        }

        [HttpGet("UnAuthorize")]
        public IActionResult UnAuthorize([FromQuery]User user)
        {
            return Json("UnAuthorize");
        }

    }
}