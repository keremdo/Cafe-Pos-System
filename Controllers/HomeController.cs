using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DmlCafePos.Models;
using Microsoft.AspNetCore.Authorization;

namespace DmlCafePos.Controllers;
[Authorize(Roles = "admin")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
