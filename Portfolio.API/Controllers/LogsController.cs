using Microsoft.AspNetCore.Mvc;

namespace Portfolio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public LogsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("latest")]
        public IActionResult GetLatestLog()
        {
            var basePath = Path.Combine(_env.ContentRootPath, "Data", "Logs");
            var today = DateTime.Today;

            // Loop through years (current and previous)
            for (int yearOffset = 0; yearOffset <= 1; yearOffset++)
            {
                var year = today.Year - yearOffset;
                var yearPath = Path.Combine(basePath, year.ToString());

                if (!Directory.Exists(yearPath)) continue;

                // Loop through months (start from current or Dec)
                for (int month = (year == today.Year ? today.Month : 12); month >= 1; month--)
                {
                    var monthName = new DateTime(year, month, 1).ToString("MMMM");
                    var monthPath = Path.Combine(yearPath, monthName);

                    if (!Directory.Exists(monthPath)) continue;

                    // Loop through days
                    int maxDay = (year == today.Year && month == today.Month) ? today.Day : DateTime.DaysInMonth(year, month);
                    for (int day = maxDay; day >= 1; day--)
                    {
                        var fileName = new DateTime(year, month, day).ToString("yyyy-MM-dd") + ".json";
                        var filePath = Path.Combine(monthPath, fileName);

                        if (System.IO.File.Exists(filePath))
                        {
                            var json = System.IO.File.ReadAllText(filePath);
                            return Content(json, "application/json");
                        }
                    }
                }
            }

            return NotFound("No dev logs found.");
        }
    }
}
