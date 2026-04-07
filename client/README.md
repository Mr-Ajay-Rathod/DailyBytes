DailyBytes Angular client

Commands:

1. Install dependencies:
   npm install

2. Run dev server:
   npm start

The dev server will use proxy.conf.json to forward `/api` calls to the backend at https://localhost:5001.
Ensure the API is running (`dotnet run` in `DailyBytes.API`) before starting the client.
