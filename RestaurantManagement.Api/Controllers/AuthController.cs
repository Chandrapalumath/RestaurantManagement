using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.Api.Controllers
{
    public class AuthController : Controller
    {
        public async Task<IActionResult> Register()
        {
            return Ok();
        }
        public async Task<IActionResult> Login()
        {
            return Ok();
        }
    }
}
