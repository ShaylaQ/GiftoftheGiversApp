using GiftoftheGiversApp.Data;
using GiftoftheGiversApp.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GiftoftheGiversApp.Controllers
{
    public class DonationController : Controller
    {
        private readonly SqlService _sqlService;

        public DonationController(SqlService sqlService)
        {
            _sqlService = sqlService;
        }

        public IActionResult Index()
        {
            var donations = new List<Donation>();

            using (var conn = _sqlService.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    SELECT d.DonationId, d.UserId, d.Date, d.Type, d.AidQuantity, u.Name
                    FROM Donations d
                    JOIN Users u ON d.UserId = u.UserId", conn);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    donations.Add(new Donation
                    {
                        DonationId = reader.GetInt32(0),
                        UserId = reader.GetInt32(1).ToString(),

                        Date = reader.GetDateTime(2),
                        Type = reader.GetString(3),
                        AidQuantity = reader.GetInt32(4),

                    });
                }
            }

            return View(donations);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Donation model)
        {
            if (!ModelState.IsValid) return View(model);

            using (var conn = _sqlService.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO Donations (UserId, Date, Type, AidQuantity)
                    VALUES (@UserId, @Date, @Type, @AidQuantity)", conn);

                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@Date", model.Date);
                cmd.Parameters.AddWithValue("@Type", model.Type);
                cmd.Parameters.AddWithValue("@AidQuantity", model.AidQuantity);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
