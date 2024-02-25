using Microsoft.AspNetCore.Mvc;
using NationalParkWebApp_3.Models;
using NationalParkWebApp_3.Repository.IRepository;

namespace NationalParkWebApp_3.Controllers
{
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        public NationalParkController(INationalParkRepository nationalParkRepository)
        {
            _nationalParkRepository = nationalParkRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await 
                _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _nationalParkRepository.DeleteAsync(SD.NationalParkAPIPath, id);
            if(status)
                return Json(new {success=true,message="Data Successfully Deleted !!"});
                return Json(new {success=false,message="Someting Went Wrong While Deleted Data!!"});
        }
        #endregion
        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark nationalPark = new NationalPark();
            if (id == null) return View(nationalPark);
            nationalPark = await _nationalParkRepository.GetAsync(SD.NationalParkAPIPath,id.GetValueOrDefault());
            if (nationalPark == null) return NotFound();
            return View(nationalPark);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark nationalPark)
        {
            if (ModelState.IsValid)
            {
                var file = HttpContext.Request.Form.Files;
                if (file.Count() > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = file[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    nationalPark.Picture = p1;
                }
                else
                {
                    var natonalParkInDb = await _nationalParkRepository.GetAsync(SD.NationalParkAPIPath, nationalPark.Id);
                    nationalPark.Picture = natonalParkInDb.Picture;
                }

                if (nationalPark.Id == 0)
                    await _nationalParkRepository.CreateAsync(SD.NationalParkAPIPath, nationalPark);
                else

                    await _nationalParkRepository.UpdateAsync(SD.NationalParkAPIPath, nationalPark);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(nationalPark);
            }
        }
    }
}
