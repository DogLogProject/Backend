using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ApiProject.Context;
using ApiProject.Model.UserModel;
using ApiProject.Services;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiProject.GenericRepository;
using ApiProject.Model;
using Microsoft.AspNetCore.Authorization;
using ApiProject.JWTManagerRepository;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IJWTManagerRepository _jWTManager;

        public AuthController(ApiDbContext context, IMapper mapper,
            IConfiguration configuration, IUserService userService,
            IGenericRepository<User> userRepository, IJWTManagerRepository jWTManager)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _userService = userService;
            _userRepository = userRepository;
            _jWTManager = jWTManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            try
            {
                var user = _mapper.Map<User>(request);
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);

                await _userRepository.Insert(user);
                await _context.SaveChangesAsync();

                var createdUser = await _userRepository.GetById(user.Id);

                if (createdUser == null)
                {
                    return NotFound("User was created but could not be retrieved.");
                }

                return CreatedAtAction(nameof(Register), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginUser request)
        {
            var user = await _userService.GetByName(request.Name);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest("Wrong password.");
            }
            return Ok(user.Id);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate-user")]
        public async Task<IActionResult> AuthenticateAsync(LoginUser usersdata)
        {
            var user = await _userService.GetByName(usersdata.Name);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(usersdata.Password, user.Password))
            {
                return BadRequest("Wrong password.");
            }

            var token = _jWTManager.GenerateToken(usersdata.Name, user.Id);

            if (token == null)
            {
                return Unauthorized("Invalid Attempt..");
            }

            UserRefreshTokens obj = new UserRefreshTokens
            {
                RefreshToken = token.RefreshToken,
                UserName = usersdata.Name,
                Id = user.Id
            };

            _userService.AddUserRefreshTokens(obj);
            return Ok(token);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        public IActionResult Refresh(Tokens token)
        {
            var principal = _jWTManager.GetPrincipalFromExpiredToken(token.AccessToken);
            var username = principal.Identity?.Name;
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("Invalid token!");
            }

            int userId = int.Parse(userIdClaim.Value);

            var savedRefreshToken = _userService.GetSavedRefreshTokens(userId, username, token.RefreshToken);

            if (savedRefreshToken == null || savedRefreshToken.RefreshToken != token.RefreshToken)
            {
                return Unauthorized("Invalid attempt!");
            }

            var newJwtToken = _jWTManager.GenerateRefreshToken(username, userId);

            if (newJwtToken == null)
            {
                return Unauthorized("Invalid attempt!");
            }

            UserRefreshTokens obj = new UserRefreshTokens
            {
                RefreshToken = newJwtToken.RefreshToken,
                UserName = username,
                Id = userId
            };

            _userService.DeleteUserRefreshTokens(userId, username, token.RefreshToken);
            _userService.AddUserRefreshTokens(obj);

            return Ok(newJwtToken);
        }
    }
}
