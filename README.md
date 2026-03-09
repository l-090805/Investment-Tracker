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

--> *The 'Forgot password' function from 'Log In' page might send the reset password email to the spam folder*

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

---

**Bellow are screenshots with the functional app, before the free trial from FlutterFlow ends and some functionality will be lost**

--> Dropdown menu for selectig assets.
<img width="1917" height="909" alt="image" src="https://github.com/user-attachments/assets/3aa8b24a-20e9-4b0f-8a81-21edf618bf3b" />

---

--> The 'Add Investment' form. User adds the asset, date when it was bought, the buying price & quantity.
<img width="1919" height="906" alt="image" src="https://github.com/user-attachments/assets/4deeb45b-2063-4829-b295-d330ac87d230" />

---

--> The form fully filled.
<img width="1919" height="908" alt="image" src="https://github.com/user-attachments/assets/79c08fe4-e312-447b-9e87-30e98c36d625" />

---

--> The 'Portfolio' page. Here users can see their real time profit and loss, shown in $usd and percentage, while also seeing the current price of the asset in real time.
<img width="1918" height="900" alt="image" src="https://github.com/user-attachments/assets/33e75aaa-3246-456d-9065-3f03827442a6" />

---

--> Bellow are the 'Sing In' & 'Sign Up' pages.
<img width="1912" height="904" alt="image" src="https://github.com/user-attachments/assets/14aea2a3-ad53-447a-8e1c-53eea6c6f053" />
<img width="1912" height="902" alt="image" src="https://github.com/user-attachments/assets/db2529eb-fe41-4826-a2b8-1952461eab02" />




