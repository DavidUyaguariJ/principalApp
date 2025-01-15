using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HumanTalentApp.Data;
using EntityModelsHumanTalentApp.Models;
using EntityModelsHumanTalentApp.Models.App;
using HumanTalentApp.Models;

namespace HumanTalentApp.Pages.Users
{
	[Authorize(Roles = "ADMN")]
	public class EditModel : PageModel
	{
		private readonly AppDbContext _context;

		public EditModel(AppDbContext context)
		{
			_context = context;
		}
        
        [BindProperty(SupportsGet = true)]
		public Guid Id { get; set; }

		public new UserViewModel User { get; set; }

		public void OnGet()
		{
            User = (from u in _context.AdmnUsers
                    join r in _context.TAdmnRoles on u.IdeRole equals r.IdeRole
                    where u.IdeUser == Id
                    select new UserViewModel
                    {
                        Dni = u.Dni,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        UserName = u.UserName,
                        Email = u.Email,
                        Roles = _context.TAdmnRoles.ToList(),
                        IdeUser = u.IdeUser,
                        IdeRole= u.IdeRole
                    }).FirstOrDefault();
        }
	}
}