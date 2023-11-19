using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.Dtos;
using API.Helpers;
using API.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers;

public class UserController : BaseController
{
    // private readonly IUserService _userService;

    // public static User user = new User();
    // [HttpPost("register")]
    // public  Task<ActionResult<User>> Register(RegisterDto model)
    // {
    //     string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
    //     user.Username = model.Username;
    //     user.PasswordHash = passwordHash;
    //     return Ok(user);

    // }

    // [HttpPost("token")]
    // public async Task<IActionResult> GetTokenAsync(LoginDto model)
    // {
    //     var result = await _userService.GetTokenAsync(model);
    //     SetRefreshTokenInCookie(result.RefreshToken);
    //     return Ok(result);
    // }

    // [HttpPost("addrole")]
    // public async Task<IActionResult> AddRoleAsync(AddRoleDto model)
    // {
    //     var result = await _userService.AddRoleAsync(model);
    //     return Ok(result);
    // }

    // [HttpPost("refresh-token")]
    // public async Task<IActionResult> RefreshToken()
    // {
    //     var refreshToken = Request.Cookies["refreshToken"];
    //     var response = await _userService.RefreshTokenAsync(refreshToken);
    //     if (!string.IsNullOrEmpty(response.RefreshToken))
    //         SetRefreshTokenInCookie(response.RefreshToken);
    //     return Ok(response);
    // }


    // private void SetRefreshTokenInCookie(string refreshToken)
    // {
    //     var cookieOptions = new CookieOptions
    //     {
    //         HttpOnly = true,
    //         Expires = DateTime.UtcNow.AddDays(10),
    //     };
    //     Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    // } 
}