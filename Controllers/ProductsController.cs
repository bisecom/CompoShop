using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CompoShop.Models;
using System.IO;
using System.Runtime.Remoting.Contexts;

namespace CompoShop.Controllers
{
    public class ProductsController : Controller
    {
        private CompoContext db = new CompoContext();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            return View(await db.Products.ToListAsync());
        }

        public async Task<ActionResult> Catalogue()
        {
            return View(await db.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("DetailsClient");
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Details/5
        public async Task<ActionResult> DetailsClient(int? id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(/*[Bind(Include = "Id,Description,Producer,Processor,RAM,HDD,Body,Picture,Price")] Product product*/ FormCollection data)
        {
            Product product = new Product();
            product.Description = data["Description"];
            product.Producer = data["Producer"];
            product.Processor = data["Processor"];
            product.RAM = data["RAM"];
            product.HDD = data["HDD"];
            product.Body = data["Body"];
            product.Price = Convert.ToInt32(data["Price"]);
            if (Request.Files["Picture"] != null)
            {
                using (var binaryReader = new BinaryReader(Request.Files["Picture"].InputStream))
                {
                    var Imagefile = binaryReader.ReadBytes(Request.Files["Picture"].ContentLength);
                    product.Picture = Imagefile;
                }
            }

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(product);

        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(/*[Bind(Include = "Id,Description,Producer,Processor,RAM,HDD,Body,Picture,Price")] Product product*/ FormCollection data)
        {
            Product product = new Product();
            product.Id = Convert.ToInt32(data["Id"]);
            product.Description = data["Description"];
            product.Producer = data["Producer"];
            product.Processor = data["Processor"];
            product.RAM = data["RAM"];
            product.HDD = data["HDD"];
            product.Body = data["Body"];
            product.Price = Convert.ToInt32(data["Price"]);
            if (Request.Files["Picture"] != null)
            {
                using (var binaryReader = new BinaryReader(Request.Files["Picture"].InputStream))
                {
                    var Imagefile = binaryReader.ReadBytes(Request.Files["Picture"].ContentLength);
                    product.Picture = Imagefile;
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Products of Basket
        [HttpGet]
        public async Task<ActionResult> Basket(int? id)
        {
            //var guid = Guid.NewGuid().ToString();
            //Session["mySession"] = guid;
            Basket myBasket = new Basket();
            if (Session["mySess"] == null)
            {
                string sessionId = System.Web.HttpContext.Current.Session.SessionID;
                Session["mySess"] = sessionId;
            }
            myBasket.Session = Session["mySess"].ToString();
            myBasket.ProductId = (int)id;
            myBasket.Quantity = 1;

            if (ModelState.IsValid)
            {
                db.Baskets.Add(myBasket);
                await db.SaveChangesAsync();
            }
            return View("Catalogue", await db.Products.ToListAsync());
        }

        // GET: Products of Basket
        [HttpPost]
        public async Task<ActionResult> Basket(Basket basket)
        {
            return View(await db.Baskets.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> BasketView()
        {
            return View(await db.Baskets.ToListAsync());
        }
        
            public async Task<ActionResult> ClearBasket()
            {
                if (Session["mySess"] != null)
                {
                    var list = await db.Baskets.ToListAsync();
                    foreach(var item in list)
                    {
                        if(item.Session == Session["mySess"].ToString())
                        db.Baskets.Remove(item);
                    }
                    await db.SaveChangesAsync();
                }
                //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Baskets]");
                return View("BasketView", await db.Baskets.ToListAsync());
            }

        [HttpGet]
        public async Task<ActionResult> DeleteProductFromBasket(int id)
        {
            Basket myBasket = await db.Baskets.FindAsync(id);
            db.Baskets.Remove(myBasket);
            await db.SaveChangesAsync();
            return View("BasketView", await db.Baskets.ToListAsync());
        }

        public ActionResult OrderPlace()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> OrderPlace(Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                await db.SaveChangesAsync();
            }
            int clientId = client.Id;
           
            IEnumerable<Basket> myBaskets = db.Baskets;
            foreach (var item in myBaskets)
            {
                if (Session["mySess"] != null && Session["mySess"].ToString() == item.Session) {
                    Order order = new Order();
                    order.Date = DateTime.Now;
                    order.ProductId = item.ProductId;
                    order.Quantity = item.Quantity;
                    order.ClientId = clientId;

                    db.Baskets.Remove(item);
                    db.Orders.Add(order);
                }
            }

            await db.SaveChangesAsync();
            ViewBag.ClientObj = client;
            return View("OrderComplete");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contacts()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
