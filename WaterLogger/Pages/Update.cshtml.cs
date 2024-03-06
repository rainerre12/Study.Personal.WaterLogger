using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;
using WaterLogger.Models;

namespace WaterLogger.Pages
{
    public class UpdateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public DrinkingModel DrinkingWater { get; set; }

        public UpdateModel(IConfiguration configuration)
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
                    $@"SELECT * FROM  drinking_water WHERE id ={id}";
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
                   $"UPDATE drinking_water SET date ='{DrinkingWater.Date}', quantity = {DrinkingWater.Quantity} WHERE Id = {DrinkingWater.Id}";
                tableCMD.ExecuteNonQuery();
            }
            return RedirectToPage("./Index");
        }

    }
}
