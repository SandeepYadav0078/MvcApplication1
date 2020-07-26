using MvcApplication1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class CategoryController : Controller
    {
        private IGenericRepositry<CategoryMaster> repository = null;

        public CategoryController()
        {
            this.repository = new CrudRepositry<CategoryMaster>();
        }

        public CategoryController(IGenericRepositry<CategoryMaster> repository)
        {
            this.repository = repository;
        }

       
        public ActionResult Index()
        {
            ViewBag.userdetails = repository.GetAll().OrderBy(m => m.DisplayOrder);
           
            TempData["IsEdit"] = "0";
          
            return View();
        }
        [HttpGet]        
        public JsonResult GetAll()
        {
            IEnumerable<CategoryMaster> dt = repository.GetAll();
            if (dt != null)
            {
                if (dt.Count() > 0)
                {
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(null, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(CategoryMaster U)
        {
            if (ModelState.IsValid)
            {

                if (TempData["IsEdit"] != null && TempData["IsEdit"] == "1")
                {

                    U.CategoryID = Convert.ToInt32(TempData["Id"].ToString());
                    repository.Update(U);
                    repository.Save();
                    TempData["Message"] = "Record Update Succesfully";
                    TempData["IsEdit"] = "0";

                }
                else
                {

                    repository.Insert(U);
                    repository.Save();
                    TempData["Message"] = "Record Save Succesfully";
                    TempData["IsEdit"] = "0";

                }
            }
            else
            {
                TempData["Message"] = "All fields are required with *";
               
            }
        
           
            ViewBag.userdetails = repository.GetAll();
            return RedirectToAction("Index");
            
        }

        public ActionResult Edit(int id)
        {
            CategoryMaster Editcoat = (from u in repository.GetAll()
                                        where u.CategoryID == id
                                        select u).FirstOrDefault();
            TempData["Id"] = id;
            ViewBag.userdetails = repository.GetAll();
            TempData["IsEdit"] = "1";
            return View("Index", Editcoat);
        }

        public ActionResult delete(int id)
        {

            repository.Delete(id);
            repository.Save();
            ViewBag.userdetails = repository.GetAll();
            return RedirectToAction("Index");
        }

    }
}
