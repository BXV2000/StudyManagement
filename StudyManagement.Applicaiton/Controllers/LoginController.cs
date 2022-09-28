using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudyManagement.Contracts.Authentication;
using StudyManagement.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudyManagement.Applicaiton.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LoginController(IConfiguration configuration, SignInManager<Users> signInManager, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("1");
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (ModelState.IsValid)
            {
                // Check exist user
                var user = await _userManager.FindByNameAsync(login.Username);
                if(user == null)
                {
                    ModelState.AddModelError("Username", "Invalid username");
                    return BadRequest(ModelState);
                }

                // Check password
                var passwordCorrect =await _userManager.CheckPasswordAsync(user, login.Password);
                if (passwordCorrect == false)
                {
                    ModelState.AddModelError("Password", "Wrong password");
                    return BadRequest(ModelState);
                }

                // Get user role
                var userRole = await _userManager.GetRolesAsync(user);

                // Adding claims info
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, login.Username),
                    new Claim(ClaimTypes.Role,userRole[0]),
                    new Claim("UserId",user.Id)
                };

                // Generate token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryDays"]));

                var token = new JwtSecurityToken(
                    _configuration["JwtIssuer"],
                    _configuration["JwtAudience"],
                    claims,
                    expires:expiry,
                    signingCredentials:creds
                );

                return Ok(new {
                    Username = login.Username,
                    Role = userRole,
                    Token = token
                });;

            }
            return BadRequest(ModelState);
        }
    }
}
