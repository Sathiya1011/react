using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReactProject.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReactProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _usermanger;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly IConfiguration _config;
        public AuthenticateController(UserManager<IdentityUser> um, RoleManager<IdentityRole> rm, IConfiguration config)
        {
            _usermanger = um;
            _rolemanager = rm;
            _config = config;

        }
        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _usermanger.FindByNameAsync(model.Username);
            if (user != null && await _usermanger.CheckPasswordAsync(user, model.Password))
            {
                var userroles = await _usermanger.GetRolesAsync(user);

                var authclaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };
                foreach (var userRole in userroles)
                {
                    authclaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authclaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
        [HttpPost]
        [Route("register")]

        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userexists = await _usermanger.FindByNameAsync(model.Username);
            if (userexists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Already exists" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _usermanger.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed!pleas check the user details" });

            return Ok(new Response { Status = "Success", Message = "User Created Successfully" });
        }
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _usermanger.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _usermanger.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _rolemanager.RoleExistsAsync(UserRoles.Admin))
                await _rolemanager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _rolemanager.RoleExistsAsync(UserRoles.User))
                await _rolemanager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _rolemanager.RoleExistsAsync(UserRoles.Admin))
            {
                await _usermanger.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _rolemanager.RoleExistsAsync(UserRoles.Admin))
            {
                await _usermanger.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

    }


}

