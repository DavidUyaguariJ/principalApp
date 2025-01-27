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
    public class TProdProductController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly AppDbContext _context;

        public TProdProductController(AppDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetProductById(Guid id)
        {
            var result = _context.TProdProducts.Where(x => x.IdeProduct == id);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var result = _context.TProdProducts.ToList();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] TProdProduct product)
        {
            try
            {
                var data = product ?? throw new Exception("The value is null");
                _context.TProdProducts.Add(product);
                _context.SaveChanges();
                return StatusCode(200, new
                {
                    Message = "Registro creado",
                    Data = product
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
        public IActionResult UpdateProduct([FromBody] TProdProduct product)
        {
            try
            {
                var data = product ?? throw new Exception("The value is null");

                var branddevice = _context.TProdProducts.Where(x => x.IdeProduct == product.IdeProduct).FirstOrDefault();
                if (branddevice == null)
                {
                    return StatusCode(400, new
                    {
                        success = false,
                        message = "Registro no existe",
                        result = ""
                    });
                }
                branddevice.Name = product.Name;
                branddevice.Status = product.Status;
                _context.TProdProducts.Update(branddevice);
                _context.SaveChanges();
                return StatusCode(200, new
                {
                    Message = "Registro actualizado",
                    Data = product
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
        public IActionResult DeleteProduct(Guid BrandId)
        {
            try
            {
                var branddevice = _context.TProdProducts.Where(x => x.IdeProduct == BrandId).FirstOrDefault();
                if (branddevice == null)
                {
                    return StatusCode(400, new
                    {
                        success = false,
                        message = "Registro no existe",
                        result = ""
                    });
                }
                _context.TProdProducts.Remove(branddevice);
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