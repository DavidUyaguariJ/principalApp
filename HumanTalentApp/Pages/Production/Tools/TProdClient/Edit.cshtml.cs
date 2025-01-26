using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EntityModelsPrincipalApp.Models.App;
using HumanTalentApp.Data;
using System;
using System.Drawing.Drawing2D;

namespace principalApp.Pages.Production.Tools.TProdClient
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

        [BindProperty]
        public TProdClient Client { get; set; }

        public void OnGet()
        {
            Client = _context.TProdClients.FirstOrDefault(b => b.IdeClient == Id);
        }
    }
}

