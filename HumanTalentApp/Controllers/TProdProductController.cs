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
    public class TProdProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TProdProductController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TProdProduct
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            try
            {
                var products = _context.TProdProducts.ToList();
                return Ok(new
                {
                    Message = "Productos obtenidos con éxito.",
                    Data = products
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(Guid id)
        {
            try
            {
                var product = _context.TProdProducts.FirstOrDefault(p => p.IdeProduct == id);
                if (product == null)
                {
                    return NotFound(new { Message = $"Producto con ID {id} no encontrado." });
                }

                return Ok(new
                {
                    Message = "Producto encontrado con éxito.",
                    Data = product
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] TProdProduct product)
        {
            try
            {
                if (product == null || string.IsNullOrEmpty(product.Nombre))
                {
                    return BadRequest(new { Message = "El producto no es válido o el nombre está vacío." });
                }

                product.IdeProduct = Guid.NewGuid(); // Generar un nuevo ID para el producto.
                _context.TProdProducts.Add(product);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetProductById), new { id = product.IdeProduct }, new
                {
                    Message = "Producto creado con éxito.",
                    Data = product
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(Guid id, [FromBody] TProdProduct product)
        {
            try
            {
                if (product == null || string.IsNullOrEmpty(product.Nombre))
                {
                    return BadRequest(new { Message = "El producto no es válido o el nombre está vacío." });
                }

                var existingProduct = _context.TProdProducts.FirstOrDefault(p => p.IdeProduct == id);
                if (existingProduct == null)
                {
                    return NotFound(new { Message = $"Producto con ID {id} no encontrado." });
                }
                existingProduct.Nombre = product.Nombre;
                existingProduct.Status = product.Status;
                _context.TProdProducts.Update(existingProduct);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Producto actualizado con éxito.",
                    Data = existingProduct
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(Guid id)
        {
            try
            {
                var product = _context.TProdProducts.FirstOrDefault(p => p.IdeProduct == id);
                if (product == null)
                {
                    return NotFound(new { Message = $"Producto con ID {id} no encontrado." });
                }

                _context.TProdProducts.Remove(product);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Producto eliminado con éxito.",
                    Data = product
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}

