using HumanTalentApp.Data;
using EntityModelsHumanTalentApp.Models.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace principalApp.Pages.Audits.Tools.AlignmentObjetives
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

        public new TAudtAlignmentObjetive Brand { get; set; }
		public void OnGet()
        {
            Brand = _context.TAudtAlignmentObjetives.FirstOrDefault(b => b.IdeAlignmentObjetive == Id);
        }
    }
}
