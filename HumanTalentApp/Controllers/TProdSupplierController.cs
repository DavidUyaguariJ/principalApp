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
    public class TProdSupplierController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TProdSupplierController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllSuppliers()
        {
            try
            {
                var suppliers = _context.TProdSuppliers.ToList();
                return Ok(new
                {
                    Message = "Proveedores obtenidos con éxito.",
                    Data = suppliers
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetSupplierById(Guid id)
        {
            try
            {
                var supplier = _context.TProdSuppliers.FirstOrDefault(s => s.IdeSupplier == id);
                if (supplier == null)
                {
                    return NotFound(new { Message = $"Proveedor con ID {id} no encontrado." });
                }

                return Ok(new
                {
                    Message = "Proveedor encontrado con éxito.",
                    Data = supplier
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CreateSupplier([FromBody] TProdSupplier supplier)
        {
            try
            {
                if (supplier == null || string.IsNullOrEmpty(supplier.Nombre))
                {
                    return BadRequest(new { Message = "El proveedor no es válido o el nombre está vacío." });
                }

                supplier.IdeSupplier = Guid.NewGuid(); 
                _context.TProdSuppliers.Add(supplier);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.IdeSupplier }, new
                {
                    Message = "Proveedor creado con éxito.",
                    Data = supplier
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSupplier(Guid id, [FromBody] TProdSupplier supplier)
        {
            try
            {
                if (supplier == null || string.IsNullOrEmpty(supplier.Nombre))
                {
                    return BadRequest(new { Message = "El proveedor no es válido o el nombre está vacío." });
                }

                var existingSupplier = _context.TProdSuppliers.FirstOrDefault(s => s.IdeSupplier == id);
                if (existingSupplier == null)
                {
                    return NotFound(new { Message = $"Proveedor con ID {id} no encontrado." });
                }

                existingSupplier.Nombre = supplier.Nombre;
                existingSupplier.Status = supplier.Status;
                existingSupplier.ChatId = supplier.ChatId;

                _context.TProdSuppliers.Update(existingSupplier);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Proveedor actualizado con éxito.",
                    Data = existingSupplier
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(Guid id)
        {
            try
            {
                var supplier = _context.TProdSuppliers.FirstOrDefault(s => s.IdeSupplier == id);
                if (supplier == null)
                {
                    return NotFound(new { Message = $"Proveedor con ID {id} no encontrado." });
                }

                _context.TProdSuppliers.Remove(supplier);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Proveedor eliminado con éxito.",
                    Data = supplier
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
