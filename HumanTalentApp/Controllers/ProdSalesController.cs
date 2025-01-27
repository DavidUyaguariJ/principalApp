using EntityModelsPrincipalApp.Models.App;
using HumanTalentApp.Data;
using HumanTalentApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace principalApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "ADMN,PROD")]
    public class ProdSalesController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private static string _externalApiUrl;
        private static string _producerApiUrl;

        public ProdSalesController(AppDbContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _externalApiUrl = _configuration.GetValue<string>("AppSettings:managementApi");
            _producerApiUrl = _configuration.GetValue<string>("AppSettings:producerApi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleById(long id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_externalApiUrl}sales/getSaleById/{id}");

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
        public async Task<IActionResult> GetAllSales()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_externalApiUrl}sales/getSales");

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

        public async Task<IActionResult> GetAllSalesWithNames()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_externalApiUrl}sales/getSales");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var sales = JsonSerializer.Deserialize<List<ProdSale>>(data);

                    var productIds = sales.Select(s => s.IdeProduct).Distinct().ToList();
                    var products = await _context.TProdProducts
                        .Where(p => productIds.Contains(p.IdeProduct))
                        .ToListAsync();

                    var clientIds = sales.Select(s => s.IdeClient).Distinct().ToList();
                    var clients = await _context.TProdClients
                        .Where(c => clientIds.Contains(c.IdeClient))
                        .ToListAsync();

                    var result = sales.Select(s => new
                    {
                        s.IdeSale,
                        ProductName = products.FirstOrDefault(prod => prod.IdeProduct == s.IdeProduct)?.Name ?? "Desconocido",
                        ClientName = clients.FirstOrDefault(c => c.IdeClient == s.IdeClient)?.Name ?? "Desconocido",
                        s.Quantity,
                        IsPrincipal = s.Quantity > 0
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
        public async Task<IActionResult> AddSale([FromBody] ProdSale sale)
        {
            try
            {
                if (sale == null || sale.IdeClient == null || sale.IdeProduct == null || sale.Quantity <= 0)
                {
                    return BadRequest("El modelo enviado tiene valores inválidos.");
                }
                var client = _httpClientFactory.CreateClient();
                sale.IdeSale = null;
                var response = await client.PostAsJsonAsync($"{_externalApiUrl}sales/addSale", sale);

                if (response.IsSuccessStatusCode)
                {   
                    TProdClient findClient =  _context.TProdClients.Where(x => x.IdeClient == sale.IdeClient).FirstOrDefault();

                    TProdProduct findProduct = _context.TProdProducts.Where(x => x.IdeProduct == sale.IdeProduct).FirstOrDefault();

                    MessageModel newMessage = new MessageModel() {
                        IdClient= findClient.ChatId,
                        Name = findClient.Name,
                        ProductName = findProduct.Name,
                        ProductQuantity= sale.Quantity,
                        Description= "Agregado"
                    };
                    var message = await client.PostAsJsonAsync($"{_producerApiUrl}message/sendTopicSale", newMessage);
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
        public async Task<IActionResult> UpdateSale([FromBody] ProdSale sale)
        {
            try
            {
                if (sale == null || sale.IdeClient == null || sale.IdeProduct == null || sale.Quantity <= 0)
                {
                    return BadRequest("El modelo enviado tiene valores inválidos.");
                }

                var client = _httpClientFactory.CreateClient();
                var response = await client.PutAsJsonAsync($"{_externalApiUrl}sales/editSale", sale);

                if (response.IsSuccessStatusCode)
                {
                    TProdClient findClient = _context.TProdClients.Where(x => x.IdeClient == sale.IdeClient).FirstOrDefault();

                    TProdProduct findProduct = _context.TProdProducts.Where(x => x.IdeProduct == sale.IdeProduct).FirstOrDefault();

                    MessageModel newMessage = new MessageModel()
                    {
                        IdClient = findClient.ChatId,
                        Name = findClient.Name,
                        ProductName = findProduct.Name,
                        ProductQuantity = sale.Quantity,
                        Description = "Actualizado"
                    };
                    var message = await client.PostAsJsonAsync($"{_producerApiUrl}message/sendTopicSale", newMessage);
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
        public async Task<IActionResult> DeleteSale(long id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"{_externalApiUrl}sales/deleteSale/{id}");

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
