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
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace TrainingProject.Controllers
{
    public class GasStationListController : Controller
    {
        private GasStationContext _context;
        public GasStationListController(GasStationContext context)
        {
            _context = context;
        }
        ////IndexView////
        public async Task<IActionResult> Index(IFormCollection collection, string searchString, string searchDistrict, string searchTypeA92, string searchTypeA95, string searchTypeM83, string searchTypeE5, int? page)
        {
            string[] listGasType = new string[] { searchTypeA92, searchTypeA95, searchTypeM83, searchTypeE5 };
            ViewBag.getDistrict = _context.MDistrict.OrderBy(x => x.DistrictName).ToList();
            ViewBag.getType = _context.MType.Where(x => x.TypeType == 3).ToList();
            List<GasStation> gasStations = _context.GasStation.Include(x => x.GasStationGasType).ToList();
            List<GasStationVM> listResultGastation = new List<GasStationVM>();
            if (page == null) page = 1;
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            int lengthGasType = listGasType.Where(x => x != null).Count();
            foreach (var item in gasStations)
            {
                bool checkGasType = false;
                if (lengthGasType > 0)
                {
                    foreach (var gastype in item.GasStationGasType)
                    {
                        if (listGasType.Contains(gastype.GasType))
                        {
                            checkGasType = true;
                            break;
                        }
                    }
                }
                else
                {
                    checkGasType = true;
                }

                if (checkGasType)
                {
                    GasStationVM gasStationVM = new GasStationVM();
                    listResultGastation.OrderBy(x => x.GasStationName).ToList();
                    gasStationVM.GasStationName = item.GasStationName;
                    gasStationVM.GasStationId = item.GasStationId;
                    List<string> gasTypes = new List<string>();
                    foreach (var type in item.GasStationGasType)
                    {
                        string TypeText = getTypeTextFromMType(type.GasType, 3);
                        gasTypes.Add(TypeText);
                    }
                    gasStationVM.Types = gasTypes;
                    gasStationVM.District = _context.MDistrict.FirstOrDefault(x => x.DistrictId == item.District);
                    gasStationVM.Longitude = item.Longitude;
                    gasStationVM.Latitude = item.Latitude;
                    string rating = getTypeTextFromMType(item.Rating, 4);
                    gasStationVM.Rating = rating;
                    listResultGastation.Add(gasStationVM);
                }
            }
            //search//
            if (!String.IsNullOrEmpty(searchString))
            {
                listResultGastation = listResultGastation.Where(s => s.GasStationName.Contains(searchString)).ToList();
            }
            if (!String.IsNullOrEmpty(searchDistrict))
            {
                listResultGastation = listResultGastation.Where(s => s.District.DistrictId == Convert.ToInt64(searchDistrict)).ToList();
            }

            @ViewBag.SearchString = searchString;


            var gas = listResultGastation.OrderBy(x => x.GasStationName).ToPagedList(pageNumber, pageSize);
            return View(gas);
        }
        [HttpPost]
        public IActionResult Index(IFormCollection collection)
        {
            string a = collection["SearchString"];
            return View();
        }
        public string getTypeTextFromMType(string typeCode, int TypeType)
        {
            string result = _context.MType.FirstOrDefault(x => x.TypeCode == typeCode && x.TypeType == TypeType).TypeText;
            return result;
        }

        ////AddGasStation////
        [HttpGet]
        public IActionResult GasStationAdd()
        {
            ViewBag.getType = _context.MType.Where(x => x.TypeType == 3).ToList();
            ViewBag.getType1 = _context.MType.Where(x => x.TypeType == 4).ToList();
            ViewBag.getDistrict = _context.MDistrict.OrderBy(x => x.DistrictName).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult GasStationAdd(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                GasStation gasStation = new GasStation();
                gasStation.GasStationName = collection["GasStationName"];
                gasStation.Longitude = Convert.ToDouble(collection["Longitude"]);
                gasStation.Latitude = Convert.ToDouble(collection["Latitude"]);
                gasStation.Rating = collection["typetype"];
                gasStation.District = Convert.ToInt64(collection["searchDistrict"]);
                gasStation.Address = collection["Address"];
                gasStation.OpeningTime = collection["OpeningTime"];
                gasStation.InsertedAt = DateTime.Now;
                gasStation.UpdatedAt = DateTime.Now;
                gasStation.InsertedBy = 1;
                gasStation.UpdatedBy = 1;
                var checkExist = _context.GasStation.Where(x => x.GasStationName == collection["GasStationName"]);
                if (checkExist.Count() > 0)
                {
                    ViewBag.dupplicateName = "情報はすでに存在します";
                }
                else
                {
                    _context.GasStation.Add(gasStation);
                    _context.SaveChanges();
                    string[] arrayGastype = collection["type"];
                    foreach (var item in arrayGastype)
                    {
                        GasStationGasType gasStationGasType = new GasStationGasType();
                        gasStationGasType.GasStationId = gasStation.GasStationId;
                        gasStationGasType.GasType = item;
                        _context.GasStationGasType.Add(gasStationGasType);
                        _context.SaveChanges();
                    }
                    return RedirectToAction("Index", "GasStationList");
                }
            }
            ViewBag.getType = _context.MType.Where(x => x.TypeType == 3).ToList();
            ViewBag.getType1 = _context.MType.Where(x => x.TypeType == 4).ToList();
            ViewBag.getDistrict = _context.MDistrict.OrderBy(x => x.DistrictName).ToList();
            return View();
        }
    }
}
