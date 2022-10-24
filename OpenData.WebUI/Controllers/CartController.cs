using OpenData.Domain.Entities;
using OpenData.Domain.Abstract;
using OpenData.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenData.WebUI.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/

        private IODRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IODRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }

        public ViewResult Index(Cart cart, string returnUrl) 
        {
            return View(new CartIndexViewModel { Cart = cart, ReturnUrl = returnUrl });
               
        }

        public RedirectToRouteResult AddToCart(Cart cart, string ODId, string returnUrl)
        {
            OpenDataSet authority = repository.OpenData.FirstOrDefault(p => p.ODID == ODId);
            if (authority != null)
            {
                cart.AddItem(authority, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToRouteResult RemoveFromCart(Cart cart, string ODId, string returnUrl)
        {
            OpenDataSet authority = repository.OpenData.FirstOrDefault(p => p.ODID == ODId);
            if (authority != null)
            {
                cart.RemoveLine(authority);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart) 
        {
            return PartialView(cart);
        }

        //[HttpPost]
        //public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        //{ 
        //    if (cart.Lines.Count() == 0)
        //    {
        //        ModelState.AddModelError("", "Пусто");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        orderProcessor.ProcessOrder(cart, shippingDetails);
        //        cart.Clear();
        //        return View("Completed");
        //    }
        //    else
        //    {
        //        return View(shippingDetails);
        //    }
        //}

        public ViewResult Checkout() 
        {
            return View(new ShippingDetails());
        }
        //private Cart GetCart()
        //{
        //    Cart cart = (Cart)Session["Cart"];
        //    if (cart == null)
        //    {
        //        cart = new Cart();
        //        Session["Cart"] = cart;
        //    }
        //    return cart;
        //}

    }
}
