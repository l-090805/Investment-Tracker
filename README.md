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

---

**Bellow are screenshots with the app**

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




