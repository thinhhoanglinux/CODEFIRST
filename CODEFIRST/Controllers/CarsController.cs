using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CODEFIRST.Models;
using CODEFIRST.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace CODEFIRST.Controllers
{
    public class CarsController : Controller
    {
        private ApplicationDbContext _dbContext;
        public CarsController()
        {
            _dbContext = new ApplicationDbContext();
        }
        [Authorize]
        public ActionResult Index()
        {
            if (HttpContext.User.IsInRole("ADMIN"))
            {
                var showCar = _dbContext.Cars
                            .Include(c => c.Category)
                            .Include(c => c.Employee);
                return View(showCar);
            }
            return RedirectToAction("Index","Home");
        }
        // GET: Cars
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CarViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Heading = "Create"
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarViewModel viewModel)
        {
            var car = new Car
            {
                EmployeeId = User.Identity.GetUserId(),
                CategoryId = viewModel.Category,
                Name = viewModel.Name,
                StartDate = viewModel.StartDate,
                Price = viewModel.Price,
                Quantity = viewModel.Quantity
            };
            _dbContext.Cars.Add(car);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Cars");
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            try
            {
                var car = _dbContext.Cars.Single(m => m.Id == id && m.EmployeeId == userId);
                var viewModel = new CarViewModel
                {
                    Categories = _dbContext.Categories.ToList(),
                    Name = car.Name,
                    Heading = "Edit",
                    Price = car.Price,
                    Quantity = car.Quantity,
                    Category = car.CategoryId,
                    Id = car.Id
                };
                return View("Create", viewModel);
            }
            catch (Exception e)
            {
                return HttpNotFound();
            }
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CarViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("Create", viewModel);
            }
            var userId = User.Identity.GetUserId();
            var car = _dbContext.Cars.Single(m => m.Id == viewModel.Id && m.EmployeeId == userId);

            car.Name = viewModel.Name;
            car.Price = viewModel.Price;
            car.Quantity = viewModel.Quantity;
            car.CategoryId = viewModel.Category;
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Cars");
        }
        [Authorize]
        public ActionResult Details(int id)
        {
            var userId = User.Identity.GetUserId();
            var car = _dbContext.Cars.Include(c => c.Category).Include(c => c.Employee).Single(m => m.Id == id && m.EmployeeId == userId);
            return View(car);
        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            var car = _dbContext.Cars.Include(c => c.Category).Include(c => c.Employee).Single(m => m.Id == id && m.EmployeeId == userId);
            return View(car);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id, Car car)
        {
            var userId = User.Identity.GetUserId();
            car = _dbContext.Cars.Include(c => c.Employee).Include(c => c.Category).Single(s => s.Id == id && s.EmployeeId == userId);
            _dbContext.Cars.Remove(car);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Cars");
        }
    }
}