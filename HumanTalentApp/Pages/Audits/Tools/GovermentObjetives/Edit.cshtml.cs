using HumanTalentApp.Data;
using EntityModelsHumanTalentApp.Models.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanTalentApp.Pages.Audits.Tools.GovermentObjetives
{
    public class EditModel : PageModel
    {
		private readonly AppDbContext _context;
        public EditModel(AppDbContext context)
        {
			_context = context;
		}

		[BindProperty(SupportsGet = true)]
		public Guid Id { get; set; }

        public new TAudtGovermentObjetive Brand { get; set; }
		public void OnGet()
        {
            Brand = _context.TAudtGovermentObjetives.FirstOrDefault(b => b.IdeGuvermentObjetive == Id);
        }
    }
}
