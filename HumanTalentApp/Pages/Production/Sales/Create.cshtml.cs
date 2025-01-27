using EntityModelsPrincipalApp.Models.App;
using HumanTalentApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanTalentApp.Pages.Production.Sales
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<TProdProduct> TProdProducts { get; set; }
        [BindProperty]
        public List<TProdClient> TProdClients { get; set; }

        public void OnGet()
        {
            TProdProducts = _context.TProdProducts.ToList();
            TProdClients = _context.TProdClients.ToList();
        }
    }
}
