using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        //POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTo registerRequestDTo)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTo.Username,
                Email = registerRequestDTo.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDTo.Password);
            if (identityResult.Succeeded)
            {
                // add roles to the user
                if (registerRequestDTo.Roles != null && registerRequestDTo.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDTo.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! Please login.");
                    }
                }
            }

            return BadRequest("Something went wrong");
        }

        // POST: /api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTo loginRequestDTo)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDTo.Username);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDTo.Password);

                if (checkPasswordResult)
                {
                    // get roles for this user
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        // create token
                       var jwtToken= tokenRepository.CreateJWTToken(user,roles.ToList());

                        var response = new LoginResponseDTo
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                   
                }
            }

            return BadRequest("Username or password is incorrect");
        }
    }
}
