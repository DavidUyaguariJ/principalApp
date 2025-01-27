using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EntityModelsPrincipalApp.Models.App;
using HumanTalentApp.Data;
using System;
using System.Drawing.Drawing2D;

namespace principalApp.Pages.Production.Tools.TProdProducts
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
        public TProdProduct Product { get; set; }

        public void OnGet()
        {
            Product = _context.TProdProducts.FirstOrDefault(b => b.IdeProduct == Id);
        }
    }
}

