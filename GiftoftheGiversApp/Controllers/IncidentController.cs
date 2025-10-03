using GiftoftheGiversApp.Data;
using GiftoftheGiversApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;


namespace GiftoftheGiversApp.Controllers
{
    public class IncidentController : Controller
    {
        private readonly SqlService _sqlService;

        public IncidentController(SqlService sqlService)
        {
            _sqlService = sqlService;
        }

        public IActionResult Index()
        {
            var incidents = new List<IncidentReport>();

            using (var conn = _sqlService.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    SELECT i.IncidentId, i.UserId, i.Date, i.Incident, i.Location, i.Description, u.Name
                    FROM IncidentReports i
                    JOIN Users u ON i.UserId = u.UserId", conn);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    incidents.Add(new IncidentReport
                    {
                        IncidentId = reader.GetInt32(0),
                        UserId = reader.GetInt32(1).ToString(),

                        Date = reader.GetDateTime(2),
                        Incident = reader.GetString(3),
                        Location = reader.GetString(4),

                        Description = reader.GetString(5),

                    });
                }
            }

            return View(incidents);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IncidentReport model)
        {
            if (!ModelState.IsValid) return View(model);

            using (var conn = _sqlService.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO IncidentReports (UserId, Date, Incident, Location, Description)
                    VALUES (@UserId, @Date, @Incident, @Location, @Description)", conn);

                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@Date", model.Date);
                cmd.Parameters.AddWithValue("@Incident", model.Incident);
                cmd.Parameters.AddWithValue("@Location", model.Location);
                cmd.Parameters.AddWithValue("@Description", model.Description);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}




