using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingProject.Models;

namespace TrainingProject.Controllers
{
    public class DeleteGasStationController : Controller
    {
        private GasStationContext _context;
        public DeleteGasStationController(GasStationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Delete(long? id)
        {
            var gasStation = _context.GasStation.Include(b => b.GasStationGasType).FirstOrDefault(b => b.GasStationId == id);
            _context.GasStation.Remove(gasStation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "GasStationList");

        }
        
    }
}