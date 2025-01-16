using EntityModelsHumanTalentApp.Models.App;
using HumanTalentApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanTalentApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "ADMN")]
    public class EmpresarialObjetivesController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly AppDbContext _context;

        public EmpresarialObjetivesController(AppDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetObjetiveById(Guid id)
        {
            var result = _context.TAudtEmpresarialObjetives.Where(x => x.IdeEmpresarialObjetive == id);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllBrand()
        {
            var result = _context.TAudtEmpresarialObjetives.ToList();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult RegObjetive([FromBody] TAudtEmpresarialObjetive optData)
        {
            try
            {
                var data = optData ?? throw new Exception("The value is null");
                _context.TAudtEmpresarialObjetives.Add(optData);
                _context.SaveChanges();
                return StatusCode(200, new
                {
                    Message = "Registro creado",
                    Data = optData
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
        public IActionResult UpdateObjetive([FromBody] TAudtEmpresarialObjetive optData)
        {
            try
            {
                var data = optData ?? throw new Exception("The value is null");

                var branddevice = _context.TAudtEmpresarialObjetives.Where(x => x.IdeEmpresarialObjetive == optData.IdeEmpresarialObjetive).FirstOrDefault();
                if (branddevice == null)
                {
                    return StatusCode(400, new
                    {
                        success = false,
                        message = "Registro no existe",
                        result = ""
                    });
                }
                branddevice.Name = optData.Name;
                branddevice.Code = optData.Code;
                branddevice.IsPrincipal = optData.IsPrincipal;
                branddevice.Status = optData.Status;
                _context.TAudtEmpresarialObjetives.Update(branddevice);
                _context.SaveChanges();
                return StatusCode(200, new
                {
                    Message = "Registro actualizado",
                    Data = optData
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
        public IActionResult DeleteObjetive(Guid BrandId)
        {
            try
            {
                var branddevice = _context.TAudtEmpresarialObjetives.Where(x => x.IdeEmpresarialObjetive == BrandId).FirstOrDefault();
                if (branddevice == null)
                {
                    return StatusCode(400, new
                    {
                        success = false,
                        message = "Registro no existe",
                        result = ""
                    });
                }
                _context.TAudtEmpresarialObjetives.Remove(branddevice);
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
