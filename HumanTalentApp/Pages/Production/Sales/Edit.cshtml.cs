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

namespace HumanTalentApp.Pages.Production.Sales
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
        public ProdSale Sale { get; set; }

        [BindProperty]
        public Guid SelectedProductId { get; set; }

        [BindProperty]
        public Guid SelectedClientId { get; set; }

        public SelectList ProductList { get; set; }
        public SelectList ClientList { get; set; }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_externalApiUrl}sales/getSaleById/{Id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var saleFromApi = JsonSerializer.Deserialize<ProdSale>(data);
                Sale = saleFromApi;

                SelectedProductId = (Guid)Sale.IdeProduct;
                SelectedClientId = (Guid)Sale.IdeClient;

            }
            else
            {
                ModelState.AddModelError("", "No se pudo obtener la venta.");
            }

            ProductList = new SelectList(
                _context.TProdProducts.Select(p => new { p.IdeProduct, p.Name }).ToList(),
                "IdeProduct",
                "Name",
                SelectedProductId
            );

            ClientList = new SelectList(
                _context.TProdClients.Select(c => new { c.IdeClient, c.Name }).ToList(),
                "IdeClient",
                "Name",
                SelectedClientId
            );
        }
    }
}
