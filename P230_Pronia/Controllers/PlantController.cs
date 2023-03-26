using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P230_Pronia.DAL;
using P230_Pronia.Entities;

namespace P230_Pronia.Controllers
{
    public class PlantController : Controller
    {
        private readonly ProniaDbContext _context;

        public PlantController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Detail(int id)
        {
            if (id == 0) return NotFound();
            Plant? plant = _context.Plants
                                .Include(p=>p.PlantImages)
                                    .Include(p=>p.PlantDeliveryInformation)
                                        .Include(p=>p.PlantTags)
                                            .ThenInclude(pt=>pt.Tag)
                                                .Include(p=>p.PlantCategories)
                                                    .ThenInclude(pc=>pc.Category).FirstOrDefault(p => p.Id == id);
            
            List<int> saleProducts = _context.Plants.Select(p => p.Id).ToList();

            ViewBag.RelatedPlant = _context.Plants
                .Include(p => p.PlantImages)
                .Include (p=>p.PlantCategories)
                .ThenInclude(p => p.Category)
                .Where(p => p.PlantCategories.Any(p => saleProducts.Contains(p.Category.Id)))

                .Take(4)
                .ToList();
            if (plant is null) return NotFound();
            return View(plant);
        }
        
    }
}
