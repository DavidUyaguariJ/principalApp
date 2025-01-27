using EntityModelsPrincipalApp.Models.App;
using HumanTalentApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace principalApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "PROD")]
    public class ProdSalesController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private static string _externalApiUrl;

        public ProdSalesController(AppDbContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _externalApiUrl = _configuration.GetValue<string>("AppSettings:managementApi");
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
                var response = await client.PostAsJsonAsync($"{_externalApiUrl}sales/addSale", sale);

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
