using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainingProject.Models;
using TrainingProject.ViewModels;
using System.Web;
using System.Collections;
using PagedList;
using Microsoft.AspNetCore.Http;
using X.PagedList;
namespace TrainingProject.Controllers
{
    public class FeedBackViewController : Controller
    {
        private GasStationContext _context;
        public FeedBackViewController(GasStationContext context)
        {
            _context = context;
        }
        public  IActionResult Feedback(int id, int  page)
        {
            if(id.ToString() == null)
            {
                return NotFound(); 
            }
            GasStation gasStations = _context.GasStation.Include(x => x.GasStationGasType).Include(x=>x.GasStationFeedback).FirstOrDefault(x=>x.GasStationId == id);
            GasStationVM resultGastation = new GasStationVM();
            resultGastation.Types = new List<string>();
            foreach (var type in gasStations.GasStationGasType)
            {
                string TypeText = getTypeTextFromMType(type.GasType, 3);
                resultGastation.Types.Add(TypeText);
            }
            resultGastation.GasStationFeedback = gasStations.GasStationFeedback.Take(10).Skip(page).ToList();
            resultGastation.GasStationName = gasStations.GasStationName;
            resultGastation.Address = gasStations.Address;
            resultGastation.OpeningTime = gasStations.OpeningTime;
            string rating = getTypeTextFromMType(gasStations.Rating, 4);
            resultGastation.Rating = rating;
            ViewBag.getType = _context.MType.Where(x => x.TypeType == 3).ToList();
            ViewBag.getFB = _context.GasStationFeedback.Include(x => x.Feedback).Include(x => x.FeedbackAt);
            return View(resultGastation);
        }
        public string getTypeTextFromMType(string typeCode, int TypeType)
        {
            string result = _context.MType.FirstOrDefault(x => x.TypeCode == typeCode && x.TypeType == TypeType).TypeText;
            return result;
        }
    }
}