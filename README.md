# Investment Tracker

A web application for tracking financial investments (stocks, crypto, indices) with real-time P&L calculations.

## Live Demo

🌐 [View Live App](https://investment-tracker-uis901.flutterflow.app/) *(hosted on FlutterFlow)*

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
- Each user sees only their own investments

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
