using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Security.AccessControl;
using WaterLogger.Models;

namespace WaterLogger.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public DrinkingModel DrinkingWater { get; set; }

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet(int id)
        {
            DrinkingWater = GetById(id);
            return Page();
        }
        private DrinkingModel GetById(int id)
        {
            var drinkingWaterRecord = new DrinkingModel();
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $@"SELECT * FROM drinking_water WHERE id ={id}";
                SqliteDataReader reader = tableCmd.ExecuteReader();

                while (reader.Read())
                {
                    drinkingWaterRecord.Id = reader.GetInt32(0);
                    drinkingWaterRecord.Date = DateTime.Parse(reader.GetString(1),
                        CultureInfo.CurrentCulture.DateTimeFormat);
                    drinkingWaterRecord.Quantity = reader.GetInt32(2);
                }
                return drinkingWaterRecord;
            }
        }

        public IActionResult OnPost(int id)
        {
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = {id}";

                tableCmd.ExecuteNonQuery();
            }
            return RedirectToPage("./Index");
        }
    }
}
