using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RPG.Web.Models;
using RPG.Web.ViewModels;

namespace RPG.Web.Controllers
{
    public class ChangelogController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IChangesRepository _changesRepository;
        private readonly ICommentRepository _commentRepository;

        public ChangelogController(UserManager<ApplicationUser> userManager,
            IChangesRepository changesRepository, ICommentRepository commentRepository)
        {
            _userManager = userManager;
            _changesRepository = changesRepository;
            _commentRepository = commentRepository;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _changesRepository.GetAllChanges().OrderByDescending(x => x.Id);

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ViewResult Details(int id)
        {
            var changes = _changesRepository.GetChanges(id);
            if(changes == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
            changes.Comments = _commentRepository.GetComments(id);

            var model = new ChangelogDetailsViewModel()
            {
                Changes = changes
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ViewResult> Details(int id, Comment comment)
        {
            var changes = _changesRepository.GetChanges(id);
            if(changes == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }

            if(ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                comment.UserId = user.Id;
                comment.ChangesId = id;
                _commentRepository.Add(comment);
            }

            changes.Comments = _commentRepository.GetComments(id);

            var model = new ChangelogDetailsViewModel()
            {
                Changes = changes
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = _commentRepository.GetComment(id);
            if(comment == null)
            {
                ViewBag.ErrorMessage = $"Nie można znaleźć komentarza o Id: '{id}'.";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            var user = await _userManager.GetUserAsync(User);
            var usersInRole = await _userManager.GetUsersInRoleAsync("Admin");
            var commentUser = await _userManager.FindByIdAsync(comment.UserId);

            if(user.Id == comment.UserId || usersInRole.FirstOrDefault(x => x.Id == user.Id) != null)
            {
                _commentRepository.Delete(id);
            }

            return RedirectToAction("Details", new { id = comment.ChangesId });
        } 

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Changes model)
        {
            if(ModelState.IsValid)
            {
                var changes =_changesRepository.Add(model);
                return RedirectToAction("Details", new { id = changes.Id });
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ViewResult Edit(int id)
        {
            var model = _changesRepository.GetChanges(id);

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Changes model)
        {
            if(ModelState.IsValid)
            {
                var changes = _changesRepository.GetChanges(model.Id);
                changes.Content = model.Content;
                changes.Version = model.Version;

                _changesRepository.Update(changes);
                return RedirectToAction("Details", new { id = changes.Id });
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public RedirectToActionResult Delete(int id)
        {
            _changesRepository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}