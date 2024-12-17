using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GraphAlgorithms.Web.Models
{
    public class GraphImportModel
    {
        [Required]
        [Display(Name = "Upload GraphML File")]
        public IFormFile UploadedFile { get; set; }
    }
}
