using EntityModelsPrincipalApp.Models.App;
using HumanTalentApp.Data;
using HumanTalentApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace principalApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "ADMN,PROD")]
    public class ProdPurchasesController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private static string _externalApiUrl;
        private static string _producerApiUrl;

        public ProdPurchasesController(AppDbContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _externalApiUrl = _configuration.GetValue<string>("AppSettings:managementApi");
            _producerApiUrl = _configuration.GetValue<string>("AppSettings:producerApi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseById(long id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_externalApiUrl}purchases/getPurchaseById/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return Ok(JsonSerializer.Deserialize<object>(data));
                }
                else
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPurchases()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_externalApiUrl}purchases/getPurchases");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return Ok(JsonSerializer.Deserialize<object>(data));
                }
                else
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetAllPurchasesWithName()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_externalApiUrl}purchases/getPurchases");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var purchases = JsonSerializer.Deserialize<List<ProdPurchase>>(data);

                    // Obtener los productos y proveedores desde la base de datos
                    var productIds = purchases.Select(p => p.IdeProduct).Distinct().ToList();
                    var supplierIds = purchases.Select(p => p.IdeSupplier).Distinct().ToList();

                    var products = await _context.TProdProducts
                        .Where(p => productIds.Contains(p.IdeProduct))
                        .ToListAsync();

                    var suppliers = await _context.TProdSuppliers
                        .Where(s => supplierIds.Contains(s.IdeSupplier))
                        .ToListAsync();

                    var result = purchases.Select(p => new
                    {
                        p.IdePurchase,
                        ProductName = products.FirstOrDefault(prod => prod.IdeProduct == p.IdeProduct)?.Name ?? "Desconocido",
                        SupplierName = suppliers.FirstOrDefault(sup => sup.IdeSupplier == p.IdeSupplier)?.Name ?? "Desconocido",
                        p.Quantity,
                        isPrincipal = p.Quantity > 0
                    }).ToList();

                    return Ok(result);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPurchase([FromBody] ProdPurchase purchase)
        {
            try
            {
                if (purchase == null || purchase.IdeSupplier == null || purchase.IdeProduct == null || purchase.Quantity <= 0)
                {
                    return BadRequest("El modelo enviado tiene valores inválidos.");
                }

                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync($"{_externalApiUrl}purchases/addPurchase", purchase);

                if (response.IsSuccessStatusCode)
                {
                    TProdSupplier findSupplier = _context.TProdSuppliers.Where(x => x.IdeSupplier == purchase.IdeSupplier).FirstOrDefault();

                    TProdProduct findProduct = _context.TProdProducts.Where(x => x.IdeProduct == purchase.IdeProduct).FirstOrDefault();

                    MessageModel newMessage = new MessageModel()
                    {
                        IdClient = findSupplier.ChatId,
                        Name = findSupplier.Name,
                        ProductName = findProduct.Name,
                        ProductQuantity = purchase.Quantity,
                        Description = "agregado"
                    };
                    var message = await client.PostAsJsonAsync($"{_producerApiUrl}message/sendTopicPurchase", newMessage);
                    var data = await response.Content.ReadAsStringAsync();
                    return Ok(JsonSerializer.Deserialize<object>(data));
                }
                else
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePurchase([FromBody] ProdPurchase purchase)
        {
            try
            {
                if (purchase == null || purchase.IdeSupplier == null || purchase.IdeProduct == null || purchase.Quantity <= 0)
                {
                    return BadRequest("El modelo enviado tiene valores inválidos.");
                }

                var client = _httpClientFactory.CreateClient();
                var response = await client.PutAsJsonAsync($"{_externalApiUrl}purchases/editPurchase", purchase);

                if (response.IsSuccessStatusCode)
                {
                    TProdSupplier findSupplier = _context.TProdSuppliers.Where(x => x.IdeSupplier == purchase.IdeSupplier).FirstOrDefault();

                    TProdProduct findProduct = _context.TProdProducts.Where(x => x.IdeProduct == purchase.IdeProduct).FirstOrDefault();

                    MessageModel newMessage = new MessageModel()
                    {
                        IdClient = findSupplier.ChatId,
                        Name = findSupplier.Name,
                        ProductName = findProduct.Name,
                        ProductQuantity = purchase.Quantity,
                        Description = "actualizado"
                    };
                    var message = await client.PostAsJsonAsync($"{_producerApiUrl}message/sendTopicPurchase", newMessage);
                    var data = await response.Content.ReadAsStringAsync();
                    return Ok(JsonSerializer.Deserialize<object>(data));
                }
                else
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(long id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"{_externalApiUrl}purchases/deletePurchase/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = "Eliminado Exitosamente" });
                }
                else
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno: {ex.Message}");
            }
        }
    }
}
