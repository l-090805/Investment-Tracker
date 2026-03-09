# Investment Tracker

A full-stack mobile/web application for tracking financial investments (stocks, crypto, indices) with real-time P&L calculations.

## Live Demo

🌐 [View Live App](https://investment-tracker-uis901.flutterflow.app/) *(hosted on FlutterFlow)*

--> *Starting from 10 March 2026, some functions of the platform will stop working. The app was made using the FlutterFlow premium free trial. When it ends, some API calls will stop working due to only being able to use 2 at a time with the free plan*

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | FlutterFlow (Flutter) |
| Backend | C# / .NET 8 |
| ORM | Entity Framework Core |
| Database | SQLite |
| Hosting | Render |
| Authentication | Firebase Auth |
| Price Data | Twelve Data API |

---

## Features

- Track investments across multiple asset classes (crypto, stocks, indices)
- Real-time P&L calculation (USD and %) using live market prices
- Portfolio summary with total value and total P&L
- Add new investments with asset, quantity, buy price and buy date
- Delete investments with confirmation dialog
- User authentication (Sign In / Sign Up) via Firebase
- Per-user data isolation — each user sees only their own investments

---

## Backend API

Base URL: `https://investment-tracker-11jp.onrender.com`

> ⚠️ Hosted on Render free tier — first request may take 1-2 minutes to wake up the server.

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/assets` | GET | Returns list of available assets |
| `/api/investments` | GET | Returns all investments with P&L for authenticated user |
| `/api/investments` | POST | Add a new investment |
| `/api/investments/{id}` | DELETE | Delete an investment by ID |
| `/api/portfolio/summary` | GET | Returns total portfolio value and total P&L |

All endpoints (except `/api/assets`) require a `userId` header containing the Firebase UID of the authenticated user.

---

## Project Structure

```
Investment-Tracker/
├── Controllers/
│   ├── AssetsController.cs
│   ├── InvestmentsController.cs
│   └── PortfolioController.cs
├── Entities/
│   ├── Asset.cs
│   └── Investment.cs
├── Services/
│   ├── PortfolioService.cs
│   ├── TwelveDataPriceService.cs
│   └── AssetSymbolMapper.cs
├── Dtos/
│   ├── CreateInvestmentRequest.cs
│   ├── InvestmentResponseDto.cs
│   └── PortfolioSummaryDto.cs
├── Data/
├── Migrations/
└── Program.cs
```

---

## Running Locally

### Prerequisites
- .NET 8 SDK
- Entity Framework CLI (`dotnet tool install --global dotnet-ef`)

### Setup

```bash
# Clone the repo
git clone https://github.com/your-username/Investment-Tracker.git
cd Investment-Tracker/Investment-Tracker

# Restore dependencies
dotnet restore

# Apply migrations
dotnet ef database update

# Run the app
dotnet run
```

## Deployment

### Backend (Render)
```bash
git add .
git commit -m "your message"
git push origin main
```
Render detects the new commit and redeploys automatically (1-3 minutes).

### Frontend (FlutterFlow)
- Open project in FlutterFlow
- Click **Publish → Publish to Web**

---

## Environment Variables

| Variable | Description |
|----------|-------------|
| `ConnectionStrings__Default` | SQLite connection string |
| `TwelveData__ApiKey` | API key for Twelve Data price service |
| `PORT` | Port for the web server (set automatically by Render) |
