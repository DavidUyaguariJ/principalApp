using HumanTalentApp.Data;
using EntityModelsPrincipalApp.Models.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace HumanTalentApp.Pages.Production.Purchases
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public BigInteger Id { get; set; }

        [BindProperty]
        public ProdPurchase Purchase { get; set; }

        [BindProperty]
        public List<ProductViewModel> Products { get; set; } = new();

        public void OnGet()
        {
            // Cargar la compra actual
            Purchase = _context.ProdPurchases.FirstOrDefault(b => b.IdePurchase == Id);

            // Cargar los productos desde la base de datos
            var allProducts = _context.pr.ToList();

            // Crear el ViewModel con los productos
            Products = allProducts.Select(product => new ProductViewModel
            {
                IdeProduct = product.IdeProduct,
                Name = product.Name,
                IsSelected = Purchase != null && Purchase.IdeProduct == product.IdeProduct
            }).ToList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Obtener los productos seleccionados
            var selectedProducts = Products
                .Where(p => p.IsSelected)
                .Select(p => p.IdeProduct)
                .ToList();

            // Actualizar la compra en la base de datos
            var purchase = _context.ProdPurchases.FirstOrDefault(p => p.IdePurchase == Purchase.IdePurchase);
            if (purchase != null)
            {
                // Asignar el primer producto seleccionado (ejemplo: solo uno permitido)
                purchase.IdeProduct = selectedProducts.FirstOrDefault();
                _context.SaveChanges();
            }

            return RedirectToPage("/Production/Purchases/List");
        }
    }

    public class ProductViewModel
    {
        public Guid IdeProduct { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
