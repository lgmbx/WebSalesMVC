using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSalesMvc.Models;
using WebSalesMvc.Services;

namespace WebSalesMvc.Controllers {
    public class SellersController : Controller {

        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService) {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        //GET
        public async Task<IActionResult> Index() {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        //GET
        public async Task<IActionResult> Create() {
            var Deps = await _departmentService.FindAllAsync();
            var FormView = new SellerFormViewModel { Departments = Deps };
            return View(FormView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SellerFormViewModel sellerForm) {
            var seller = sellerForm.Seller;
            await _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id) {
            
            if(id == null) {
                return NotFound();
            }
            var obj = await _sellerService.FindById(id);

            if(obj == null) {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) {
            await _sellerService.Remove(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id) {
            
            if (id == null) {
                return NotFound();
            }
            var obj = await _sellerService.FindById(id);

            if (obj == null) {
                return NotFound();
            }

            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            
            var seller = await _sellerService.FindById(id);
            var deps = await _departmentService.FindAllAsync();
            
            var ViewForm = new SellerFormViewModel { Seller = seller, Departments = deps };
            return View(ViewForm);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SellerFormViewModel sellerForm) {
            var seller = sellerForm.Seller;
            await _sellerService.Update(seller);
            return RedirectToAction(nameof(Index));
        }

    }
}
