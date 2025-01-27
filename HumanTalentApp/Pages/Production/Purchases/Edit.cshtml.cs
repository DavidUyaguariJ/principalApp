using HumanTalentApp.Data;
using EntityModelsPrincipalApp.Models.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;
using System.Net.Http;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HumanTalentApp.Pages.Production.Purchases
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private string _externalApiUrl;

        public EditModel(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _externalApiUrl = "http://138.197.252.76:8080/";
        }

        [BindProperty(SupportsGet = true)]
        public BigInteger Id { get; set; }

        [BindProperty]
        public ProdPurchase Purchase { get; set; }

        [BindProperty]
        public Guid SelectedProductId { get; set; }

        [BindProperty]
        public Guid SelectedClientId { get; set; }

        public SelectList ProductList { get; set; }
        public SelectList ClientList { get; set; }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_externalApiUrl}purchases/getPurchaseById/{Id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var purchaseFromApi = JsonSerializer.Deserialize<ProdPurchase>(data);
                Purchase = purchaseFromApi;
                SelectedProductId = (Guid)Purchase.IdeProduct;
                SelectedClientId = (Guid)Purchase.IdeSupplier;

            }
            else
            {
                ModelState.AddModelError("", "No se pudo obtener la compra.");
            }

            ProductList = new SelectList(
                _context.TProdProducts.Select(p => new { p.IdeProduct, p.Name }).ToList(),
                "IdeProduct",
                "Name",
                Purchase.IdeProduct
            );

            ClientList = new SelectList(
                _context.TProdSuppliers.Select(c => new { c.IdeSupplier, c.Name }).ToList(),
                "IdeSupplier",
                "Name",
                Purchase.IdeSupplier
            );
        }
    }
}
