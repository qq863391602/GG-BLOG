﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoreWebApp.Infrastructure;
using CoreWebApp.Infrastructure.Helper;
using CoreWebApp.Model;
using CoreWebApp.Repository.Contract;
using CoreWebApp.Service;
using CoreWebApp.Service.Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CoreWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BaseController
    {
        private UserService userService;

        public ValuesController(UserService _service)
        {
            userService = _service;
        }

        //[Produces("application/json")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult UserLogin(string userName, string pwd)
        {
            var res = userService.UserLogin(userName, pwd);
            if (res.Code == 200)
            {
                var claims = new[]
                {
                   new Claim(ClaimTypes.Name, res.Result.UserName),
                        //下边为Claim的默认配置
                    new Claim(JwtRegisteredClaimNames.Jti, res.Result.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    //这个就是过期时间，目前是过期100秒，可自定义，注意JWT有自己的缓冲过期时间
                    new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(100)).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Iss,"Blog.Core"),
                    new Claim(JwtRegisteredClaimNames.Aud,"wr"),
                    //这个Role是官方UseAuthentication要要验证的Role，我们就不用手动设置Role这个属性了
                    new Claim(ClaimTypes.Role,res.Result.Role),
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: jwtSettings.Issuer,
                    audience: jwtSettings.Audience,
                    claims: claims,
                    //expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                var encodedJwt  =  new JwtSecurityTokenHandler().WriteToken(token);
                //MemoryCacheHelper.AddMemoryCache(encodedJwt, new UserBaseViewModel {
                //    UserName = res.Result.UserName,
                //    Enabled = res.Result.Enabled,
                //    Id = res.Result.Id,
                //    LastLoginTime = res.Result.LastLoginTime
                //}, new TimeSpan(0,30,0), new TimeSpan(12,0,0));//将JWT字符串和tokenModel作为key和value存入缓存
                return Ok(new
                {
                    token = encodedJwt
                });
            }
            return BadRequest("用户名或密码错误。");
        }

        // GET api/values
        [Authorize]
        [HttpGet]
        public IEnumerable<string> Get(string userName, string pwd)
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
