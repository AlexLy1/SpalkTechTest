using Microsoft.AspNetCore.Mvc;
using SpalkTechTestWebApp.ApplicationService.Interfaces;
using SpalkTechTestWebApp.ApplicationService.Models;

namespace SpalkTechTestWebApp.Controllers;

public class HomeController : Controller
{
    private readonly IStreamFileValidator _streamFileValidator;

    public HomeController(IStreamFileValidator streamFileValidator)
    {
        // dependency injection
        _streamFileValidator = streamFileValidator;
    }

    [HttpGet]
    public IActionResult Index() {
        // return a client-facing interface where users can upload a stream file
        return View();
    }

    [HttpPost]
    public IActionResult SaveUploadStream(Stream stream)
    {
        ValidationResponse validationResponse = _streamFileValidator.ValidateUploadedStream(stream);

        if (validationResponse.IsStreamValid)
        {
            // Save valid stream or continue to further processes
            
            return Ok();
        }
        else {
            return Ok(validationResponse.ErrorMessage);
        }
    }
}

