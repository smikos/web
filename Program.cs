

using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 10, Category = "Category 1" },
            new Product { Id = 2, Name = "Product 2", Price = 20, Category = "Category 2" },
            new Product { Id = 3, Name = "Product 3", Price = 30, Category = "Category 1" }
        };

        public ActionResult Index()
        {
            return View(products);
        }

        public ActionResult DeleteGroup(int groupId)
        {
            // Delete products in the specified group
            products.RemoveAll(p => p.Category == groupId.ToString());
            return RedirectToAction("Index");
        }

        public ActionResult DeleteProduct(int productId)
        {
            // Delete the product with the specified ID
            products.RemoveAll(p => p.Id == productId);
            return RedirectToAction("Index");
        }

        public ActionResult SetPrice(int productId, double price)
        {
            // Set the price for the product with the specified ID
            Product product = products.Find(p => p.Id == productId);
            if (product != null)
            {
                product.Price = price;
            }
            return RedirectToAction("Index");
        }

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
            public string Category { get; set; }
        }

        public class DeleteGroupResponseModel
        {
            public string Message { get; set; }
        }

        public class DeleteProductResponseModel
        {
            public string Message { get; set; }
        }

        public class SetPriceResponseModel
        {
            public string Message { get; set; }
        }
    }
}


// Разработка web-приложения на C# (семинары)
// Урок 1. ASP.NET база
// Доработайте контроллер, дополнив его возможностью удалять группы и продукты, а также задавать цены. Для каждого типа ответа создайте свою модель.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly List<Group> groups = new List<Group>();

        public ProductController()
        {
            // Здесь можно добавить начальные данные
            groups.Add(new Group { Id = 1, Name = "Group1" });
            groups.Add(new Group { Id = 2, Name = "Group2" });
        }

        [HttpGet]
        public IActionResult GetGroups()
        {
            return Ok(groups);
        }

        [HttpGet("{groupId}")]
        public IActionResult GetProducts(int groupId)
        {
            var group = groups.FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group.Products);
        }

        [HttpPost]
        public IActionResult AddGroup([FromBody] Group group)
        {
            groups.Add(group);
            return CreatedAtAction(nameof(GetGroups), new { id = group.Id }, group);
        }

        [HttpDelete("{groupId}")]
        public IActionResult DeleteGroup(int groupId)
        {
            var group = groups.FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }
            groups.Remove(group);
            return NoContent();
        }

        [HttpDelete("{groupId}/products/{productId}")]
        public IActionResult DeleteProduct(int groupId, int productId)
        {
            var group = groups.FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }
            var product = group.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }
            group.Products.Remove(product);
            return NoContent();
        }

        [HttpPatch("{groupId}/products/{productId}")]
        public IActionResult UpdateProductPrice(int groupId, int productId, [FromBody] decimal price)
        {
            var group = groups.FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }
            var product = group.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }
            product.Price = price;
            return Ok(product);
        }
    }

    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}