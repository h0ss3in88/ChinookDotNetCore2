using Chinook.Model;
using Chinook.Model.Data;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IQuery<Customer> _customer;

        public CustomerController(IQuery<Customer> customer)
        {
            _customer = customer;
        }
        // GET
        public IActionResult Index()
        {
            var result = _customer.GetAll();
            return View(result);
        }

        public IActionResult View(int id)
        {
            var result = _customer.GetById(id);
            return View(result);
        }
    }
}