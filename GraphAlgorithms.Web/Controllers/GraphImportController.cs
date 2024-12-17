using GraphAlgorithms.Core;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Migrations;
using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.Converters;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GraphAlgorithms.Web.Controllers
{
    [Authorize]
    public class GraphImportController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IGraphImportService graphImportService;

        public GraphImportController(
            IWebHostEnvironment webHostEnvironment,
            IGraphImportService graphImportService)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.graphImportService = graphImportService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upload(GraphImportModel model)
        {
            if (ModelState.IsValid && model.UploadedFile != null)
            {
                // Define the upload folder path
                var uploadsFolder = Path.Combine(webHostEnvironment.ContentRootPath, "Uploads");

                // Ensure the uploads folder exists
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string extension = Path.GetExtension(model.UploadedFile.FileName);

                // Check if extension is GraphML
                if (extension != ".graphml")
                {
                    ViewBag.ErrorMessage = "Please upload file with extension graphml.";
                    return View("Index");
                }

                // Generate a unique filename and save the file
                Guid guid = Guid.NewGuid();
                string fileName = guid + extension;

                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                    await model.UploadedFile.CopyToAsync(fileStream);

                try
                {
                    GraphEntity graphEntity = await graphImportService.ImportFromFile(filePath);

                    return Redirect(Url.Action("View", "GraphDrawing", new { id = graphEntity.ID }));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Please upload a valid file with GraphML representation of Graph.";
                    return View("Index");
                }
            }

            ViewBag.ErrorMessage = "Please upload a valid file.";
            return View("Index");
        }
    }
}
