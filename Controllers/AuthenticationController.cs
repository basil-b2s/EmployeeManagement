using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.DTOS;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeMgmnt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {

            var employee = DataManager.Instance.GetEmployeeById(loginDTO.EmpId);

            if (employee != null && employee.Password == loginDTO.Password)
            {
                var claims = new List<Claim>
                {

                    new Claim(ClaimTypes.Name, employee.EmpId.ToString()),
                    new Claim(ClaimTypes.Role, employee.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Response.Cookies.Append("empId", employee.EmpId.ToString());
                HttpContext.Response.Cookies.Append("role", employee.Role);
                return Ok("Logged In successfully");
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            //User.FindFirst(ClaimTypes.Name);
            HttpContext.Response.Cookies.Delete("empId");
            HttpContext.Response.Cookies.Delete("role");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
