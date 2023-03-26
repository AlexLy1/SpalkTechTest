using System;
namespace SpalkTechTestWebApp.ApplicationService.Models
{
    public class ValidationResponse
    {
        public bool IsStreamValid { get; set; }
        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
    }
}

