using IScoreApp.Models;

namespace IScoreApp.Services
{
    public class CreditScoreService
    {
        public double Calculate(UserScore data)
        {
            double paymentScore = (data.TotalPayments == 0) ? 0 : ((double)data.OnTimePayments / data.TotalPayments) * 100;
            double debtScore = (1 - (double)(data.UsedCredit / data.CreditLimit)) * 100;
            double historyScore = Math.Min((DateTime.Now - data.AccountOpenDate).TotalDays / 3650.0 * 100, 100); // 10 years max
            double mixScore = ((double)data.UsedTypes / data.TotalTypes) * 100;

            double finalScore = 0.35 * paymentScore + 0.30 * debtScore + 0.15 * historyScore + 0.20 * mixScore;
            double scaledScore = 300 + ((finalScore / 100.0) * (850 - 300));

            data.FinalScore = Math.Round(finalScore, 2);
            data.ScaledScore = Math.Round(scaledScore, 2);

            return data.ScaledScore;
        }
    }
}
