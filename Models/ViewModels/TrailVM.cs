using Microsoft.AspNetCore.Mvc.Rendering;

namespace NationalParkWebApp_3.Models.ViewModels
{
    public class TrailVM
    {
        public Trail Trail { get; set; }
        public IEnumerable<SelectListItem> nationalParkList { get; set; }
    }
}
