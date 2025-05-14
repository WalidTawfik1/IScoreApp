using DotNetEnv;
using IScoreApp.Models;
using IScoreApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IScoreApp.Controllers
{
    public class CreditController : Controller
    {
        private readonly CreditScoreService _scoreService;

        public CreditController()
        {
            Env.Load();
            _scoreService = new CreditScoreService();
        }

        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index(string email)
        {
            int? userId = GetUserIdByEmail(email);
            if (userId == null)
            {
                ViewBag.Error = "User not found.";
                return View();
            }

            var scoreData = GetDataFromDatabases(userId.Value);
            _scoreService.Calculate(scoreData);
            return View("Result", scoreData);
        }

        private int? GetUserIdByEmail(string email)
        {
            string userConnStr = Environment.GetEnvironmentVariable("Users");

            using var conn = new SqlConnection(userConnStr);
            conn.Open();
            var cmd = new SqlCommand("SELECT user_id FROM Users WHERE email = @email", conn);
            cmd.Parameters.AddWithValue("@email", email);
            var result = cmd.ExecuteScalar();
            return result != null ? (int?)result : null;
        }

        private UserScore GetDataFromDatabases(int userId)
        {
            var score = new UserScore { UserId = userId };

            using (var conn = new SqlConnection(Environment.GetEnvironmentVariable("PaymentHistory")))
            {
                conn.Open();
                var cmd1 = new SqlCommand("SELECT COUNT(*) FROM Payments WHERE user_id = @uid", conn);
                cmd1.Parameters.AddWithValue("@uid", userId);
                score.TotalPayments = (int)cmd1.ExecuteScalar();

                var cmd2 = new SqlCommand("SELECT COUNT(*) FROM Payments WHERE user_id = @uid AND paid_on_time = 1", conn);
                cmd2.Parameters.AddWithValue("@uid", userId);
                score.OnTimePayments = (int)cmd2.ExecuteScalar();
            }

            using (var conn = new SqlConnection(Environment.GetEnvironmentVariable("Debt")))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT used_credit, credit_limit FROM Debt WHERE user_id = @uid", conn);
                cmd.Parameters.AddWithValue("@uid", userId);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    score.UsedCredit = reader.GetDecimal(0);
                    score.CreditLimit = reader.GetDecimal(1);
                }
            }

            using (var conn = new SqlConnection(Environment.GetEnvironmentVariable("History")))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT account_open_date FROM History WHERE user_id = @uid", conn);
                cmd.Parameters.AddWithValue("@uid", userId);
                score.AccountOpenDate = (DateTime)cmd.ExecuteScalar();
            }

            using (var conn = new SqlConnection(Environment.GetEnvironmentVariable("CreditMix")))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT used_types, total_types FROM CreditMix WHERE user_id = @uid", conn);
                cmd.Parameters.AddWithValue("@uid", userId);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    score.UsedTypes = reader.GetInt32(0);
                    score.TotalTypes = reader.GetInt32(1);
                }
            }

            return score;
        }
    }
}