using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSalesMvc.Models;
using WebSalesMvc.Services;
using WebSalesMvc.Services.Exceptions;

namespace WebSalesMvc.Controllers {
    public class SellersController : Controller {

        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService) {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        #region Index

        //GET
        public async Task<IActionResult> Index() {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        #endregion

        #region Create
        public async Task<IActionResult> Create() {
            var Deps = await _departmentService.FindAllAsync();
            var FormView = new SellerFormViewModel { Departments = Deps };
            return View(FormView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SellerFormViewModel sellerForm) {
            if (!ModelState.IsValid) {
                var deps = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = sellerForm.Seller, Departments = deps };
                return View(viewModel);
            }
            var seller = sellerForm.Seller;
            await _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id) {
            
            if(id == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            }
            var obj = await _sellerService.FindById(id);

            if(obj == null) {
                return RedirectToAction(nameof(Error), new { message = "object not found!" });
            }

            return View(obj);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) {
            try {
                await _sellerService.Remove(id);

            }
            catch (IntegrityException e ) {

                return RedirectToAction(nameof(Error), new { message = e.Message });

            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id) {
            
            if (id == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            }
            var obj = await _sellerService.FindById(id);

            if (obj == null) {
                return RedirectToAction(nameof(Error), new { message = "object not found!" });
            }

            return View(obj);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id) {
            
            
            if(id == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            }
            
            var seller = await _sellerService.FindById(id);
            if (seller == null) {
                return RedirectToAction(nameof(Error), new { message = "object not found!" });
            }

            var deps = await _departmentService.FindAllAsync();
            var ViewForm = new SellerFormViewModel { Seller = seller, Departments = deps };
            return View(ViewForm);
            
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SellerFormViewModel sellerForm) {
            if (!ModelState.IsValid) {
                var deps = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = sellerForm.Seller, Departments = deps };
                return View(viewModel);
            }

            if (!ModelState.IsValid) {
                return View(sellerForm);
            }
            var seller = sellerForm.Seller;
            try {
                await _sellerService.Update(seller);
            }
            catch (DBConcurrencyException e ) {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (NotFoundException e) {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Error
        public IActionResult Error(string message) {
            var viewModel = new ErrorViewModel {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = message
            };
            return View(viewModel);
        }
        #endregion
    }
}
