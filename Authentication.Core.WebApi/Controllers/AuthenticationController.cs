using Authentication.Core.WebApi.Dto;
using Authentication.Core.WebApi.Handlers;
using Authentication.Core.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Authentication.Core.WebApi.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly JWTSettings _jWTSettings;
        private readonly Auth_DBContext _context;
        public AuthenticationController(Auth_DBContext context, IOptions<JWTSettings> options)
        {
            this._context = context;
            this._jWTSettings = options.Value;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] UserDto user)
        {
            //var result = _context.UserInfos.FirstOrDefault(o => o.Username == user.username && o.Password == user.password);
            var result = (from usr in _context.UserInfos
                          where usr.Username == user.username &&
                          usr.Password == user.password
                          select usr).FirstOrDefault();
            if(result == null)
            {
                return Unauthorized("UnAuthorized");
            }
            var tokenhadler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(_jWTSettings.SecretKey);
            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity
                (
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, result.Username),
                    }
                ),
                Expires = DateTime.Now.AddSeconds(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey),SecurityAlgorithms.HmacSha256)
            };
            var token = tokenhadler.CreateToken(tokenDescripter);
            string finaltoken = tokenhadler.WriteToken(token);

            return Ok(finaltoken);
        }
    }
}