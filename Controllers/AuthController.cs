using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using DbLabProject.Context;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Linq;
using DbLabProject.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DbLabProject.Controllers
{
	public class AuthController : Controller
	{
		private readonly DatabaseContext _context;

		public AuthController(DatabaseContext context)
		{
			_context = context;
		}
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login([Bind("FullName,SSN")] Student student)
		{
			if(ModelState.IsValid)
			{
				var name = student.FullName;
				var ssn = student.SSN;

				var user = _context.Students
					.Where(s => s.FullName == name && s.SSN == ssn)
					.FirstOrDefault();
			
				if(user == null)
				{
					ModelState.AddModelError((Student s) => s.SSN, "Not Correct");
					ModelState.AddModelError((Student s) => s.FullName, "Not Correct");
					return View();
				}
			
				var claims = new List<Claim>()
					{
						new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
						new Claim(ClaimTypes.Name, user.FullName),
						//new Claim(ClaimTypes.Role,result.Data.Roles),
					};

				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var principal = new ClaimsPrincipal(identity);
				// Remember me check box
				var properties = new AuthenticationProperties()
				{
					IsPersistent = false,
				};

				HttpContext.SignInAsync(principal, properties);
				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		public IActionResult UserSignOut()
		{
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return RedirectToAction("Index", "Home");
		}
	}
}
