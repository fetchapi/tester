using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainingProject.Models;

namespace TrainingProject.Controllers
{
    public class EditGasStationController : Controller
    {
        private GasStationContext _context;
        public EditGasStationController(GasStationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {

            if (id.ToString() == null)
            {
                return NotFound();
            }
            var gasStation = await _context.GasStation.FindAsync(id);
            ViewBag.District = new SelectList(_context.MDistrict, "DistrictId", "DistrictName", gasStation.District);
            ViewBag.Rating = new SelectList(_context.MType.Where(x => x.TypeType == 4), "TypeCode", "TypeText", gasStation.Rating);
            ViewBag.Type = new SelectList(_context.MType.Where(x => x.TypeType == 3), "TypeCode", "TypeText", gasStation.Rating);
            if (gasStation == null)
            {
                return NotFound();
            }
            return View(gasStation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var lstGasStation = _context.GasStation.ToList();
                    var gasStation = _context.GasStation.Include(x => x.GasStationGasType).FirstOrDefault(x => x.GasStationId == id);
                    gasStation.GasStationName = collection["GasStationName"];
                    gasStation.Longitude = Convert.ToDouble(collection["Longitude"]);
                    gasStation.Latitude = Convert.ToDouble(collection["Latitude"]);
                    gasStation.Rating = collection["Rating"];
                    gasStation.District = Convert.ToInt64(collection["District"]);
                    gasStation.Address = collection["Address"];
                    gasStation.OpeningTime = collection["OpeningTime"];
                    gasStation.InsertedAt = DateTime.Now;
                    gasStation.UpdatedAt = DateTime.Now;
                    gasStation.InsertedBy = 1;
                    gasStation.UpdatedBy = 1;

                    var checkExist = _context.GasStation.Where(x => x.GasStationName == collection["GasStationName"] && x.GasStationId == id);
                    if (checkExist.Count() > 0)
                    {
                        TempData["checkDupplicate"] = "情報はすでに存在します";
                        return RedirectToAction("Edit", new { id = id });
                    }
                    else
                    {
                        _context.GasStationGasType.RemoveRange(gasStation.GasStationGasType);
                        _context.Update(gasStation);
                        _context.SaveChanges();
                        string[] arrayGastype = collection["type"];
                        foreach (var item in arrayGastype)
                        {
                            GasStationGasType gasStationGasType = new GasStationGasType();
                            gasStationGasType.GasStationId = gasStation.GasStationId;
                            gasStationGasType.GasType = item;
                            _context.Update(gasStationGasType);
                            _context.SaveChanges();
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GasStationExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "GasStationList");
            }
            return View();
        }
        private bool GasStationExists(long id)
        {
            return _context.GasStation.Any(e => e.GasStationId == id);
        }
    }

}