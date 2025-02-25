
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MegwayParcel.Admin.Models.LandMarkViewModels
{
    public class ImportViewModel
    {
        [Required(ErrorMessage = "Please select an Excel file.")]
        [Display(Name = "Excel File")]
        public IFormFile ExcelFile { get; set; }
    }
}
