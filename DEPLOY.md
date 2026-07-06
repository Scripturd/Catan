# Deploy Catan as a free website (Render)

This hosts the game as a real website (e.g. `https://catan-xxxx.onrender.com`) that
your friends can open any time — no need to run anything on your own machine.

The app is packaged as a Docker image (see [`Dockerfile`](Dockerfile)) and Render
builds and runs it straight from your GitHub repo. The browser client is served by
the same server, and the SignalR client uses a relative URL, so it automatically
uses secure WebSockets over HTTPS — nothing in the app needs to change.

## One-time deploy

1. Make sure the repo is pushed to GitHub (it is).
2. Go to <https://render.com>, sign up (free), and connect your GitHub account.
3. **New + → Blueprint**, pick this repo. Render reads [`render.yaml`](render.yaml)
   and creates a free web service that builds the `Dockerfile`.
   - (Or **New + → Web Service → Docker** and accept the defaults — same result.)
4. Click **Deploy**. First build takes a few minutes.
5. Render gives you a URL like `https://catan-xxxx.onrender.com`. That's your game —
   share it. Every push to `main` redeploys automatically.

Render sets a `PORT` environment variable; the server reads it and listens on
`0.0.0.0:$PORT` (see [`Program.cs`](src/Catan.Server/Program.cs)). Locally it still
defaults to `localhost:5200`.

## What to expect on the free plan

- **Cold starts.** A free service spins down after ~15 minutes of no traffic. The
  first visitor after that waits ~30–60s while it wakes up, then it's fast.
- **Games are ephemeral.** A spin-down or redeploy wipes all lobbies (state lives in
  memory). Fine for game nights; surviving restarts would need a database (later).
- **Single instance.** The free plan runs one instance, which is exactly what the
  in-memory SignalR design needs. Don't turn on autoscaling without adding a
  Redis / Azure SignalR backplane first.

## Testing the image locally (optional, needs Docker)

```bash
docker build -t catan .
docker run -e PORT=8080 -p 8080:8080 catan   # then open http://localhost:8080
```

## Still want the from-your-machine option?

It's still there — see [`HOSTING.md`](HOSTING.md) for the Cloudflare tunnel path.
