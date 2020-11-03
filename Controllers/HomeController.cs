using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPG.Web.Models;

namespace RPG.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IChangesRepository _changesRepository;

        public HomeController(IChangesRepository changesRepository)
        {
            _changesRepository = changesRepository;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _changesRepository.GetAllChanges().OrderByDescending(x => x.Id).Take(3);

            return View(model);
        }
    }
}