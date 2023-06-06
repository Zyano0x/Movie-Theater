using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Movie_Theater.Models.Common;
using PagedList;

namespace Movie_Theater.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Staff, Adminstrator")]
    public class CrewsController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index(string Searchtext, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 1;
            int pageNum = page ?? 1;
            var crews = from s in _dbContext.Crews select s;
            if (!string.IsNullOrEmpty(Searchtext))
            {
                crews = crews.Where(x => x.Name.Contains(Searchtext));
            }
            crews = crews.OrderBy(m => m.Name);
            return View(crews.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrewViewModel viewModel, HttpPostedFileBase Avatar)
        {
            if (ModelState.IsValid)
            {
                var crews = new Crew
                {
                    Name = viewModel.Name,
                    DateOfBirth = viewModel.DateOfBirth,
                    Birthplace = viewModel.Birthplace,
                    Biography = viewModel.Biography,
                    AvatarPath = viewModel.AvatarPath,
                    Url = StringHelper.ConvertText(StringHelper.RemoveDiacritics(viewModel.Name))
                };

                _dbContext.Crews.Add(crews);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var crews = _dbContext.Crews.Find(id);

            if (crews == null)
            {
                return HttpNotFound();
            }

            var castViewModel = new CrewViewModel
            {
                Id = crews.Id,
                Name = crews.Name,
                DateOfBirth = crews.DateOfBirth,
                Birthplace = crews.Birthplace,
                Biography = crews.Biography,
                AvatarPath = crews.AvatarPath,
            };

            return View(castViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CrewViewModel castViewModel)
        {
            if (ModelState.IsValid)
            {
                var crews = _dbContext.Crews.Find(castViewModel.Id);

                if (crews == null)
                {
                    return HttpNotFound();
                }

                crews.Name = castViewModel.Name;
                crews.DateOfBirth = castViewModel.DateOfBirth;
                crews.Birthplace = castViewModel.Birthplace;
                crews.Biography = castViewModel.Biography;
                crews.AvatarPath = castViewModel.AvatarPath;

                // Save changes to the database
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(castViewModel);
        }

        //public ActionResult Delete(int id)
        //{
        //    var crews = _dbContext.Crews.Find(id);

        //    if (crews == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(crews);
        //}
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var crews = _dbContext.Crews.Find(id);
            if (crews == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _dbContext.Crews.Remove(crews);
            _dbContext.SaveChanges();

            return Json(new { success = true });
        }

        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _dbContext.Crews.Find(Convert.ToInt32(item));
                        _dbContext.Crews.Remove(obj);
                        _dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Areas/Admin/Content/assets/images/CrewAvatar/" + file.FileName));
            return "/Areas/Admin/Content/assets/images/CrewAvatar/" + file.FileName;
        }
    }
}