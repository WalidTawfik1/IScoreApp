namespace IScoreApp.Models
{
    public class UserScore
    {
        public int UserId { get; set; }
        public int OnTimePayments { get; set; }
        public int TotalPayments { get; set; }
        public decimal UsedCredit { get; set; }
        public decimal CreditLimit { get; set; }
        public DateTime AccountOpenDate { get; set; }
        public int UsedTypes { get; set; }
        public int TotalTypes { get; set; }
        public double FinalScore { get; set; }
        public double ScaledScore { get; set; }
    }
}
