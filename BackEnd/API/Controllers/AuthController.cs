using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        [HttpPost("register")]
        public ActionResult<User> Register(RegisterDto model)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            user.Username = model.Username;
            user.Password = passwordHash;
            return Ok(user);

        }
    }
}