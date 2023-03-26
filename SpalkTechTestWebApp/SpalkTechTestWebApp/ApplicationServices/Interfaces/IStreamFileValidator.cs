using System;
using SpalkTechTestWebApp.ApplicationService.Models;

namespace SpalkTechTestWebApp.ApplicationService.Interfaces
{
    public interface IStreamFileValidator
    {
        ValidationResponse ValidateUploadedStream(Stream stream);
    }
}

