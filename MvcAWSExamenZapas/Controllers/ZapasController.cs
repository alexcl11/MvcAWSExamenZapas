using Microsoft.AspNetCore.Mvc;
using MvcAWSExamenZapas.Models;
using MvcAWSExamenZapas.Repositories;
using MvcAWSExamenZapas.Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MvcAWSExamenZapas.Controllers
{
    public class ZapasController : Controller
    {
        private RepositoryZapas repo;
        private ServiceStorageS3 service;
        public ZapasController(RepositoryZapas repo, ServiceStorageS3 service)
        {
            this.repo = repo;
            this.service = service;
        }
        public async Task<IActionResult> Index()
        {
            List<Zapatilla> zapas = await this.repo.GetZapatillasAsync();
            return View(zapas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Zapatilla zapatilla, IFormFile file)
        {
            using (Stream stream = file.OpenReadStream())
            {
                await this.service.UploadFileAsync(file.FileName, stream);
            }
            await this.repo.CreateZapatillaAsync(zapatilla.Nombre, zapatilla.Descripcion, file.FileName);
            return RedirectToAction("Index");
        }
    }
}
