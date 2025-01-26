using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EntityModelsPrincipalApp.Models.App;
using EntityModelsPrincipalApp.Models.Context;
using System;
using System.Linq;

namespace principalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TProdClientController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public TProdClientController(AppDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllClients()
        {
            try
            {
                var clients = _context.TProdClients.ToList();
                return Ok(new
                {
                    Message = "Clientes obtenidos con éxito.",
                    Data = clients
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetClientById(Guid id)
        {
            try
            {
                var client = _context.TProdClients.FirstOrDefault(c => c.IdeClient == id);
                if (client == null)
                {
                    return NotFound(new { Message = $"Cliente con ID {id} no encontrado." });
                }

                return Ok(new
                {
                    Message = "Cliente encontrado con éxito.",
                    Data = client
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CreateClient([FromBody] TProdClient client)
        {
            try
            {
                if (client == null || string.IsNullOrEmpty(client.Nombre))
                {
                    return BadRequest(new { Message = "El cliente no es válido o el nombre está vacío." });
                }

                client.IdeClient = Guid.NewGuid(); 
                _context.TProdClients.Add(client);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetClientById), new { id = client.IdeClient }, new
                {
                    Message = "Cliente creado con éxito.",
                    Data = client
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateClient(Guid id, [FromBody] TProdClient client)
        {
            try
            {
                if (client == null || string.IsNullOrEmpty(client.Nombre))
                {
                    return BadRequest(new { Message = "El cliente no es válido o el nombre está vacío." });
                }

                var existingClient = _context.TProdClients.FirstOrDefault(c => c.IdeClient == id);
                if (existingClient == null)
                {
                    return NotFound(new { Message = $"Cliente con ID {id} no encontrado." });
                }
                existingClient.Nombre = client.Nombre;
                existingClient.Status = client.Status;
                existingClient.ChatId = client.ChatId;

                _context.TProdClients.Update(existingClient);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Cliente actualizado con éxito.",
                    Data = existingClient
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(Guid id)
        {
            try
            {
                var client = _context.TProdClients.FirstOrDefault(c => c.IdeClient == id);
                if (client == null)
                {
                    return NotFound(new { Message = $"Cliente con ID {id} no encontrado." });
                }

                _context.TProdClients.Remove(client);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Cliente eliminado con éxito.",
                    Data = client
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
