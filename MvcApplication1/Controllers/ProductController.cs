using MvcApplication1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class ProductController : Controller
    {
        private IGenericRepositry<ProductDetailsMaster> repository = null;

        public ProductController()
        {
            this.repository = new CrudRepositry<ProductDetailsMaster>();
        }

        public ProductController(IGenericRepositry<ProductDetailsMaster> repository)
        {
            this.repository = repository;
        }

       
        public ActionResult Index()
        {
          return View();
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var cate = new CrudRepositry<CategoryMaster>();
            var dt = repository.GetAll().Join(cate.GetAll(),
                        product => product.CategoryID,
                        category => category.CategoryID,
                        (product, category) => new 
                        {
                            ProductID = product.ProductID,
                            CategoryID = product.CategoryID,
                            CategoryName = category.Name,
                            Name = product.Name,
                            Description = product.Description,
                            Quantity = product.Quantity,
                            Price = product.Price,
                            Discount = product.Discount,
                            ExpirationDate = Convert.ToDateTime(product.ExpirationDate).ToString("mm/dd/yyyy")
                        }).ToList();
            if (dt != null)
            {
                if (dt.Count() > 0)
                {
                    return Json(new { draw = 0, recordsTotal = dt.Count(), recordsFiltered = dt.Count(), data = dt }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { draw = 0, recordsTotal = 0, recordsFiltered = 0, data = "" }, JsonRequestBehavior.AllowGet);

            
        }

        [HttpPost]
        public ActionResult AddProduct(ProductDetailsMaster U)
        {
            if (ModelState.IsValid)
            {
                if (U.ProductID !=0)
                {
                    repository.Update(U);
                    repository.Save();
                    return Json(new { Success = true, Message = "Record Updated sucesfully" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    repository.Insert(U);
                    repository.Save();
                    return Json(new { Success = true, Message = "Record Save sucesfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Success = false, Message = "Error Occured" }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public ActionResult delete(int id)
        {

            repository.Delete(id);
            repository.Save();
            return Json(new { Success = true, Message = "Record Deleted sucesfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}
