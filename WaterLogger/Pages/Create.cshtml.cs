using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using WaterLogger.Models;

namespace WaterLogger.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult OnGet()
        {

            return Page();
        }

        [BindProperty]
        public DrinkingModel DrinkingWater { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) // For validation Purposes
            {
                return Page();
            }
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCMD = connection.CreateCommand();
                tableCMD.CommandText =
                    $@"INSERT INTO drinking_water(date,quantity)
                        VALUES(
                                '{DrinkingWater.Date}',
                                {DrinkingWater.Quantity})";
                tableCMD.ExecuteNonQuery();
            }
            return RedirectToPage("./Index");
        }
    }
}


