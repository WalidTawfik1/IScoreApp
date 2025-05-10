# 💳 Credit Score Calculator (Distributed Databases)

This ASP.NET Core MVC web application calculates a user's credit score based on financial data stored across **four distributed databases**. The user enters their email, and the system fetches data, computes a weighted score, and displays a final scaled credit score with a rating.

---

## 🚀 Features

- ASP.NET Core MVC web interface
- Connects to **5 different databases** (1 for users + 4 for credit factors)
- Calculates credit score based on:
  - 🧾 Payment History (35%)
  - 💰 Outstanding Debt (30%)
  - 📆 Credit History Age (15%)
  - 🧠 Credit Mix (20%)
- Converts the score into a range between **300–850**
- Responsive UI using **Bootstrap 5**
- Uses `.env` file for secure connection string management

---

## 🧮 Credit Score Formula

```text
finalScore = 0.35 * paymentScore + 0.30 * debtScore + 0.15 * historyScore + 0.20 * mixScore
scaledScore = 300 + ((finalScore / 100) * 550)
```

## 🚀 Getting Started

1. Clone the repo
2. Add your configuration in `appsettings.json` and `.env`
3. Launch the project and test it

---

## 🔗 Links

- [Live Preview](http://creditscore.runasp.net/), Use this account for testing ahmed.hassan@example.com
- [GitHub Repository](https://github.com/WalidTawfik1/IScoreApp)
