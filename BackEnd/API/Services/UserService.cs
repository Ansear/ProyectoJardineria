using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Dtos;
using API.Helpers;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JWT _jwt;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt, IPasswordHasher<User> passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _jwt = jwt.Value;
        _passwordHasher = passwordHasher;
    }
    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        var user = new User
        {
            Email = registerDto.Email,
            Username = registerDto.Username
        };
        user.Password = _passwordHasher.HashPassword(user, registerDto.Password); //Encrypt password
        var existingUser = _unitOfWork.Users
                            .Find(u => u.Username.ToLower() == registerDto.Username.ToLower() || u.Email.ToLower() == registerDto.Email.ToLower())
                            .FirstOrDefault();
        if (existingUser == null)
        {
            var rolDefault = _unitOfWork.Rols
                            .Find(u => u.RolName == Authorization.rol_default.ToString())
                            .FirstOrDefault();
            if (rolDefault != null)
            {
                try
                {
                    user.Rols.Add(rolDefault);
                    _unitOfWork.Users.Add(user);
                    await _unitOfWork.SaveAsync();
                    return $"User  {registerDto.Username} has been registered successfully";
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return $"Error: {message}";
                }
            }
            else
            {
                // Manejar el caso en el que no se encuentra el rol
                return $"Error: Default role '{Authorization.rol_default}' not found.";
            }

        }
        else
        {
            return $"This User has already been registered.";
        }
    }

    public async Task<DataUserDto> GetTokenAsync(LoginDto model)
    {
        DataUserDto dataUserDto = new DataUserDto();
        var user = await _unitOfWork.Users.GetByUserEmailAsync(model.Email);
        if (user != null)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
            if (result == PasswordVerificationResult.Success)
            {
                dataUserDto.IsAuthenticated = true;
                JwtSecurityToken jwtSecurityToken = CreateJwtToken(user);
                dataUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                dataUserDto.Email = user.Email;
                dataUserDto.UserName = user.Username;
                dataUserDto.Roles = user.Rols
                                    .Select(u => u.RolName)
                                    .ToList();
                if (user.RefreshTokens.Any(a => a.IsActive))
                {
                    var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                    dataUserDto.RefreshToken = activeRefreshToken.Token;
                    dataUserDto.RefreshTokenExpiration = activeRefreshToken.Expires;
                }
                else
                {
                    var refreshToken = CreateRefreshToken();
                    dataUserDto.RefreshToken = refreshToken.Token;
                    dataUserDto.RefreshTokenExpiration = refreshToken.Expires;
                    user.RefreshTokens.Add(refreshToken);
                    _unitOfWork.Users.Update(user);
                    await _unitOfWork.SaveAsync();
                }
                return dataUserDto;
            }
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"Incorrect user credentials {user.Username}.";
            return dataUserDto;
        }
        dataUserDto.IsAuthenticated = false;
        dataUserDto.Message = $"User does not exist with Username {model.Email}.";
        return dataUserDto;

    }

    public async Task<string> AddRoleAsync(AddRoleDto model)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(model.Username);
        if (user == null)
        {
            return $"User {model.Username} does not exists.";
        }
        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
        if (result == PasswordVerificationResult.Success)
        {
            var rolExists = _unitOfWork.Rols
                            .Find(u => u.RolName.ToLower() == model.Role.ToLower())
                            .FirstOrDefault();
            if (rolExists != null)
            {
                var userHasRol = user.Rols.Any(u => u.Id == rolExists.Id);
                if (userHasRol == false)
                {
                    user.Rols.Add(rolExists);
                    _unitOfWork.Users.Update(user);
                    await _unitOfWork.SaveAsync();
                }
                return $"Rol {model.Role} added to user {model.Username} successfully.";
            }
            return $"Rol {model.Role} was not found.";
        }
        return $"Invalid Credentials";
    }

    public async Task<DataUserDto> RefreshTokenAsync(string refreshToken)
    {
        var dataUserDto = new DataUserDto();
        var usuario = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshToken);
        if (usuario == null)
        {
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"Token is not assigned to any user.";
            return dataUserDto;
        }
        var refreshTokenBd = usuario.RefreshTokens.Single(x => x.Token == refreshToken);
        if (!refreshTokenBd.IsActive)
        {
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"Token is not active.";
            return dataUserDto;
        }
        //Revoque the current refresh token and
        refreshTokenBd.Revoked = DateTime.UtcNow;
        //generate a new refresh token and save it in the database
        var newRefreshToken = CreateRefreshToken();
        usuario.RefreshTokens.Add(newRefreshToken);
        _unitOfWork.Users.Update(usuario);
        await _unitOfWork.SaveAsync();
        //Generate a new Json Web Token
        dataUserDto.IsAuthenticated = true;
        JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
        dataUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        dataUserDto.Email = usuario.Email;
        dataUserDto.UserName = usuario.Username;
        dataUserDto.Roles = usuario.Rols
                            .Select(u => u.RolName)
                            .ToList();
        dataUserDto.RefreshToken = newRefreshToken.Token;
        dataUserDto.RefreshTokenExpiration = newRefreshToken.Expires;
        return dataUserDto;
    }

    private RefreshToken CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                Expires = DateTime.UtcNow.AddDays(10),
                Created = DateTime.UtcNow
            };
        }
    }

    private JwtSecurityToken CreateJwtToken(User usuario)
    {
        var rols = usuario.Rols;
        var rolClaims = new List<Claim>();
        foreach (var rol in rols)
        {
            rolClaims.Add(new Claim("rols", rol.RolName));
        }
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim("uid", usuario.Id.ToString())
        }
        .Union(rolClaims);
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DuracionInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }
}