using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Dto;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;



namespace api.Controllers
{
    [Route("api/users")]
    [ApiController]
    //[Authorize(Roles = "Admin")]

    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(RegisterDto dto)
        {
            var user = new AppUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                Nume = dto.Nume,
                Prenume = dto.Prenume,
                CNP = dto.CNP,
                CodBluetooth = dto.CodBluetooth
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "User");
            return Ok(new { message = "User creat." });
        }
    }
}