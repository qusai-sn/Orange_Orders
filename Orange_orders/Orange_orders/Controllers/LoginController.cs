using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orange_orders.Models; // Adjust this namespace to match your project
using System;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly OrangeOrdersContext _context;

    public LoginController(OrangeOrdersContext context)
    {
        _context = context;
    }

    // POST: api/login
    [HttpPost]
    public async Task<ActionResult<User>> Login([FromBody] LoginModel loginModel)
    {
         var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginModel.Username && u.PasswordHash == loginModel.Password);

        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

         return Ok(new
        {
            user.UserId,
            user.Username,
            user.Email
        });
    }
}
