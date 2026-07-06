# Hosting a game from your own machine (free)

You can host Catan from your PC and let friends join over the internet for free,
without opening any router ports. A **Cloudflare tunnel** makes an outbound
connection from your machine to Cloudflare and gives you a public HTTPS URL that
forwards to your local server. WebSockets (which SignalR uses) work over it, and
the browser client uses a relative URL, so nothing in the app needs to change.

## One-time setup

Install `cloudflared`:

```powershell
winget install --id Cloudflare.cloudflared
```

(No Cloudflare account is needed for a quick tunnel.)

## Start a game night

From the repo root:

```powershell
./host.ps1
```

This builds the server, starts it on `http://localhost:5200`, and opens a tunnel.
It prints a URL like `https://random-words.trycloudflare.com` — **send that to your
friends**. They open it in a browser, enter the 5-letter game code you read off
your lobby, and play. Everything (the page and the `/hub` connection) goes through
the one tunnel, so there's nothing else to configure.

Press **Ctrl+C** to end the session; the tunnel closes and the server stops.

Use a different port with `./host.ps1 -Port 5300`.

## Manual alternative (two terminals)

```powershell
# terminal 1
dotnet run --project src/Catan.Server -c Release          # http://localhost:5200

# terminal 2
cloudflared tunnel --url http://localhost:5200
```

## Good to know

- **The URL changes every run.** For a stable URL, make a free Cloudflare account
  and create a *named* tunnel (`cloudflared tunnel create catan` + a config file);
  the quick tunnel above needs none of that.
- **Your machine must stay on** with the server running while people play.
- **Games live in memory** — stopping the server ends all lobbies.
- **Anyone with the URL can reach the server.** The game code controls which game
  someone joins, not whether they can connect. For friends-only that's usually
  fine; if you want, we can add a shared lobby password.
- Prefer nothing publicly reachable at all? **Tailscale** is the private
  alternative — you and your friends join one network and connect over it.
