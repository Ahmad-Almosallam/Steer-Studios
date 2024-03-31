using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steer.Api.Data;
using Steer.Api.Entities;
using Steer.Api.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Steer.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly IRepository<SteerUser> _userRepository;
        private readonly IRepository<Token> _tokenRepository;
        private readonly IConfiguration _configuration;
        private readonly ApplicationUserHelper _applicationUserHelper;
        public AuthorizationController(IRepository<SteerUser> userRepository, IConfiguration configuration, IRepository<Token> tokenRepository, ApplicationUserHelper applicationUserHelper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _tokenRepository = tokenRepository;
            _applicationUserHelper = applicationUserHelper;
        }



        [HttpPost("login"), Produces("application/json")]
        public async Task<IActionResult> Exchange(string userName)
        {
            var user = await _userRepository.GetAsync(x => x.UserName == userName);

            if (user == null)
            {
                return BadRequest();
            }

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                };

            // check if same user has token
            var token = await _tokenRepository.GetAsync(x => x.SteerUserId == user.Id && x.IsValid);
            if (token != null)
            {
                token.IsValid = false;
                await _tokenRepository.UpdateAsync(token);
            }
            var jwtSecurityToken = JWTHelper.GetToken(authClaims, _configuration);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            var collection = await _tokenRepository.GetCollectionAsync();
            await collection.InsertOneAsync(new Token()
            {
                AccessToken = accessToken,
                IsValid = true,
                SteerUserId = user.Id,
            });

            return Ok(accessToken);
        }

        [HttpGet("me"), Produces("application/json"), Authorize]
        public async Task<IActionResult> Me()
        {
            var user = await _userRepository.GetAsync(x => x.Id == _applicationUserHelper.GetUserId());
            return Ok(user);
        }
    }

}
