using Microsoft.AspNetCore.Mvc;
using EntityModelsPrincipalApp.Models.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HumanTalentApp.Data;
using System;
using System.Linq;
using EntityModelsPrincipalApp.Models.Import;
using System.Configuration;

namespace principalApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "ADMN")]
    public class TProdSupplierController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly AppDbContext _context;

        public TProdSupplierController(AppDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetSupplierById(Guid id)
        {
            var result = _context.TProdSuppliers.Where(x => x.IdeSupplier == id);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllSuppliers()
        {
            var result = _context.TProdSuppliers.ToList();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateSupplier([FromBody] TProdSupplier supplier)
        {
            try
            {
                var data = supplier ?? throw new Exception("The value is null");
                _context.TProdSuppliers.Add(supplier);
                _context.SaveChanges();
                return StatusCode(200, new
                {
                    Message = "Registro creado",
                    Data = supplier
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
        public IActionResult UpdateSupplier([FromBody] TProdSupplier supplier)
        {
            try
            {
                var data = supplier ?? throw new Exception("The value is null");

                var branddevice = _context.TProdSuppliers.Where(x => x.IdeSupplier == supplier.IdeSupplier).FirstOrDefault();
                if (branddevice == null)
                {
                    return StatusCode(400, new
                    {
                        success = false,
                        message = "Registro no existe",
                        result = ""
                    });
                }
                branddevice.Name = supplier.Name;
                branddevice.Status = supplier.Status;
                branddevice.ChatId = supplier.ChatId;
                _context.TProdSuppliers.Update(branddevice);
                _context.SaveChanges();
                return StatusCode(200, new
                {
                    Message = "Registro actualizado",
                    Data = supplier
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
        public IActionResult DeleteSupplier(Guid BrandId)
        {
            try
            {
                var branddevice = _context.TProdSuppliers.Where(x => x.IdeSupplier == BrandId).FirstOrDefault();
                if (branddevice == null)
                {
                    return StatusCode(400, new
                    {
                        success = false,
                        message = "Registro no existe",
                        result = ""
                    });
                }
                _context.TProdSuppliers.Remove(branddevice);
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