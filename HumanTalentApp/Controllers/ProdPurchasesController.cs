using EntityModelsPrincipalApp.Models.App;
using HumanTalentApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace principalApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "ADMN")]
    public class ProdPurchasesController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private static string _externalApiUrl;

        public ProdPurchasesController(AppDbContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _externalApiUrl = _configuration.GetValue<string>("AppSettings:managementApi");
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
    }
}
