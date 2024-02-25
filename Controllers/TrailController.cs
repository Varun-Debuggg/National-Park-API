using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NationalParkWebApp_3.Models;
using NationalParkWebApp_3.Models.ViewModels;
using NationalParkWebApp_3.Repository.IRepository;

namespace NationalParkWebApp_3.Controllers
{
    public class TrailController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly INationalParkRepository _nationalParkRepository;
        public TrailController(ITrailRepository trailRepository, INationalParkRepository nationalParkRepository)
        {
            _trailRepository = trailRepository;
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
            return Json(new { data =await _trailRepository.GetAllAsync(SD.TrailAPIPath) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status =await _trailRepository.DeleteAsync(SD.TrailAPIPath, id);
            if (status)
                return Json(new { success = true, message = "Data Deleted Successfully |" });
            return Json(new { success = false, message = "Something Went Wrong !" });
        }
        #endregion
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> nationalParks = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath);
            TrailVM trailVM = new TrailVM()
            {
                nationalParkList = nationalParks.Select(np=> new SelectListItem()
                {
                    Text= np.Name,
                    Value = np.Id.ToString()
                }),
                Trail = new Trail()
            };
            if(id == null) return View(trailVM);
            trailVM.Trail = await _trailRepository.GetAsync(SD.TrailAPIPath,id.GetValueOrDefault());
            if (trailVM.Trail == null) return NotFound();
            return View(trailVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailVM trailVM)
        {
            if (ModelState.IsValid)
            {
                if (trailVM.Trail.Id == 0)

                    await _trailRepository.CreateAsync(SD.TrailAPIPath, trailVM.Trail);
                else
                    await _trailRepository.UpdateAsync(SD.TrailAPIPath, trailVM.Trail);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> nationalParks = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath);
                trailVM = new TrailVM()
                {
                    nationalParkList = nationalParks.Select(np=> new SelectListItem()
                    {
                        Text = np.Name,
                        Value = np.Id.ToString()
                    }),
                    Trail = new Trail()
                };
                return View(trailVM);
            }
        }
    }
}
