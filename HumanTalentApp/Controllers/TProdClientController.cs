using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EntityModelsPrincipalApp.Models.App;
using HumanTalentApp.Data;
using System;
using System.Linq;

namespace principalApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "ADMN")]
    public class TProdClientController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly AppDbContext _context;

        public TProdClientController(AppDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetClientById(Guid id)
        {
            var result = _context.TProdClients.Where(x => x.IdeClient == id);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllClients()
        {
            var result = _context.TProdClients.ToList();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateClient([FromBody] TProdClient client)
        {
            try
            {
                var data = client ?? throw new Exception("The value is null");
                _context.TProdClients.Add(client);
                _context.SaveChanges();
                return StatusCode(200, new
                {
                    Message = "Registro creado",
                    Data = client
                });
            }
            catch (Exception e)
            {
                return StatusCode(400, new
                {
                    error = e.Message
                });
            }
        }

        [HttpPut]
        public IActionResult UpdateClient([FromBody] TProdClient client)
        {
            try
            {
                var data = client ?? throw new Exception("The value is null");

                var branddevice = _context.TProdClients.Where(x => x.IdeClient == client.IdeClient).FirstOrDefault();
                if (branddevice == null)
                {
                    return StatusCode(400, new
                    {
                        success = false,
                        message = "Registro no existe",
                        result = ""
                    });
                }
                branddevice.Name = client.Name;
                branddevice.Status = client.Status;
                branddevice.ChatId = client.ChatId;
                _context.TProdClients.Update(branddevice);
                _context.SaveChanges();
                return StatusCode(200, new
                {
                    Message = "Registro actualizado",
                    Data = client
                });
            }
            catch (Exception e)
            {
                return StatusCode(400, new
                {
                    error = e.Message
                });
            }
        }

        [HttpDelete("{BrandId?}")]
        public IActionResult DeleteClient(Guid BrandId)
        {
            try
            {
                var branddevice = _context.TProdClients.Where(x => x.IdeClient == BrandId).FirstOrDefault();
                if (branddevice == null)
                {
                    return StatusCode(400, new
                    {
                        success = false,
                        message = "Registro no existe",
                        result = ""
                    });
                }
                _context.TProdClients.Remove(branddevice);
                _context.SaveChanges();
                return StatusCode(200, new
                {
                    Message = "Registro eliminado",
                    Data = branddevice
                });
            }
            catch (Exception e)
            {
                return StatusCode(400, new
                {
                    error = e.Message
                });
            }
        }
    }
}