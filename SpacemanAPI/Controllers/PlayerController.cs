using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Spaceman.Service.Models;
using Spaceman.Service.Services;

namespace Spaceman.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        private readonly IPlayerService _service;
        private readonly IMapper _mapper;
        private readonly Options _options;

        public PlayerController(IPlayerService service, IOptions<Spaceman.Options> options, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
            _options = options.Value;
        }

        /// <summary>
        /// Get current Player profile
        /// </summary>
        /// <returns>Player</returns>
        [HttpGet]
        public async Task<PlayerDTO> Get()
        {
            var username = User.Identity.Name;
            var player = await _service.GetByUsername(username);
            return _mapper.Map<PlayerDTO>(player);
        }

        /// <summary>
        /// Create new Player profile
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<PlayerDTO> Create([FromBody] PlayerCreateDTO value)
        {
            var player = _mapper.Map<Player>(value);
            player = await _service.Create(player, value.Password);
            return _mapper.Map<PlayerDTO>(player);
        }
        
        /// <summary>
        /// Authenticate and get new JWT token
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<PlayerAuthenticated> Authenticate([FromBody]PlayerAuth auth)
        {
            var player = await _service.Authenticate(auth.Username, auth.Password);

            if (player == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, player.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            //  return basic user info (without password) and token to store client side
            return new PlayerAuthenticated {
                Player = _mapper.Map<PlayerDTO>(player),
                Token = $"Bearer {tokenString}"
            };
        }
    }

    public class PlayerAuth
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class PlayerAuthenticated
    {
        public PlayerDTO Player { get; set; }
        public string Token { get; set; }
    }
}
