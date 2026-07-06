const SIZE = 60;
const SVGNS = "http://www.w3.org/2000/svg";


let connection;
let gameId = null;
let myPlayerId = null;
let snapshot = null;
let selectedVertex = null;

const $ = (id) => document.getElementById(id);

function el(tag, attrs, children) {
  const node = document.createElementNS(SVGNS, tag);
  for (const k in attrs) node.setAttribute(k, attrs[k]);
  for (const c of children || []) node.appendChild(c);
  return node;
}

function toast(message) {
  const t = $("toast");
  t.textContent = message;
  t.classList.remove("hidden");
  clearTimeout(toast._timer);
  toast._timer = setTimeout(() => t.classList.add("hidden"), 3500);
}

function show(panelId) {
  for (const id of ["connect-panel", "lobby-panel", "game-panel"])
    $(id).classList.toggle("hidden", id !== panelId);
}

async function connect() {
  connection = new signalR.HubConnectionBuilder().withUrl("/hub").withAutomaticReconnect().build();
  connection.on("State", (s) => { snapshot = s; render(); });
  connection.onreconnecting(() => setStatus("reconnecting…", "bad"));
  connection.onreconnected(() => setStatus("connected", "ok"));

  await connection.start();
  setStatus("connected", "ok");

  const modes = await connection.invoke("GetModes");
  const select = $("mode");
  select.innerHTML = "";
  for (const m of modes) {
    const opt = document.createElement("option");
    opt.value = m; opt.textContent = m;
    select.appendChild(opt);
  }
  show("connect-panel");
}

function setStatus(text, cls) {
  const pill = $("conn-status");
  pill.textContent = text;
  pill.className = "pill" + (cls ? " " + cls : "");
}

function playerName() {
  return $("name").value.trim() || "Player";
}

async function createGame() {
  try {
    const info = await connection.invoke("CreateGame", playerName(), $("mode").value);
    gameId = info.gameId; myPlayerId = info.playerId;
  } catch (e) { toast(e.message); }
}

async function joinGame() {
  const code = $("code").value.trim().toUpperCase();
  if (!code) return toast("Enter a game code.");
  try {
    const info = await connection.invoke("JoinGame", code, playerName());
    gameId = info.gameId; myPlayerId = info.playerId;
  } catch (e) { toast(e.message); }
}

async function startGame() {
  try { await connection.invoke("StartGame", gameId); }
  catch (e) { toast(e.message); }
}

async function placeStarting(vertex, edge) {
  try {
    await connection.invoke("PlaceStarting", gameId,
      { q: vertex.q, r: vertex.r, corner: vertex.corner },
      { q: edge.q, r: edge.r, direction: edge.direction });
    selectedVertex = null;
  } catch (e) { toast(e.message); }
}

function render() {
  if (!snapshot) return;
  if (snapshot.phase === "Lobby") renderLobby();
  else renderGame();
}

function renderLobby() {
  show("lobby-panel");
  $("lobby-code").textContent = snapshot.gameId;
  const list = $("lobby-players");
  list.innerHTML = "";
  for (const p of snapshot.players) list.appendChild(playerChip(p, false));

  const enough = snapshot.players.length >= snapshot.minPlayers;
  const isHost = myPlayerId === 0;
  $("start-btn").disabled = !(enough && isHost);
  $("lobby-hint").textContent = !enough
    ? `Waiting for players (${snapshot.players.length}/${snapshot.minPlayers} needed, up to ${snapshot.maxPlayers}).`
    : isHost ? "Ready — start when everyone has joined." : "Waiting for the host to start.";
}

function playerChip(p, markCurrent) {
  const li = document.createElement("li");
  if (markCurrent && p.id === snapshot.currentPlayerId) li.classList.add("current");
  const sw = document.createElement("span");
  sw.className = "swatch"; sw.style.background = p.color;
  li.appendChild(sw);
  const name = document.createElement("span");
  name.textContent = p.name + (p.id === myPlayerId ? " (you)" : "");
  li.appendChild(name);
  if (p.hand) {
    const h = document.createElement("span");
    h.className = "hand";
    const total = Object.values(p.hand).reduce((sum, n) => sum + n, 0);
    h.textContent = total > 0 ? `🂠 ${total}` : "";
    li.appendChild(h);
  }
  return li;
}

function renderGame() {
  show("game-panel");
  const list = $("game-players");
  list.innerHTML = "";
  for (const p of snapshot.players) list.appendChild(playerChip(p, true));

  const myTurn = snapshot.currentPlayerId === myPlayerId;
  const current = snapshot.players.find((p) => p.id === snapshot.currentPlayerId);
  if (snapshot.phase === "Complete") $("status").textContent = "Setup complete! 🎉";
  else if (myTurn) $("status").textContent = selectedVertex
    ? "Your turn — pick a road touching your settlement."
    : "Your turn — pick a spot for your settlement.";
  else $("status").textContent = `Waiting for ${current ? current.name : "…"} to place.`;

  drawBoard(myTurn && snapshot.phase !== "Complete");
}

function drawBoard(interactive) {
  const b = snapshot.board;
  const svg = $("board");
  svg.setAttribute("viewBox", `${b.minX} ${b.minY} ${b.width} ${b.height}`);
  svg.innerHTML = "";

  for (const h of b.hexes) {
    svg.appendChild(el("polygon", { points: hexPoints(h.x, h.y), fill: h.color, stroke: "#0d2c40", "stroke-width": 2 }));
    if (h.token != null) drawToken(svg, h);
  }
  for (const hb of b.harbours) drawHarbour(svg, hb);
  for (const e of b.edges) svg.appendChild(edgeLine(e, "edge"));

  const roadKeys = new Set(b.roads.map((r) => key3(r.q, r.r, r.direction)));
  for (const r of b.roads)
    svg.appendChild(el("line", { x1: r.x1, y1: r.y1, x2: r.x2, y2: r.y2, stroke: r.color, "stroke-width": 8, "stroke-linecap": "round" }));

  if (b.robber) svg.appendChild(el("circle", { cx: b.robber.x, cy: b.robber.y, r: 12, fill: "#2b2b2b", stroke: "#f1e3c0", "stroke-width": 2 }));

  const occupied = new Set(b.settlements.map((s) => key3(s.q, s.r, s.corner)));

  if (interactive && selectedVertex) {
    for (const e of b.edges) {
      if (roadKeys.has(key3(e.q, e.r, e.direction))) continue;
      if (!edgeTouches(e, selectedVertex)) continue;
      const line = edgeLine(e, "edge placeable");
      line.addEventListener("click", () => placeStarting(selectedVertex, e));
      svg.appendChild(line);
    }
  }

  for (const s of b.settlements)
    svg.appendChild(el("rect", { x: s.x - 9, y: s.y - 9, width: 18, height: 18, rx: 3, fill: s.color, stroke: "#0d2c40", "stroke-width": 2 }));

  for (const v of b.vertices) {
    const vacant = !occupied.has(key3(v.q, v.r, v.corner));
    const placeable = interactive && vacant && !selectedVertex;
    const isSel = selectedVertex && sameVertex(v, selectedVertex);
    let cls = "vertex";
    if (placeable) cls += " placeable";
    if (isSel) cls += " selected";
    const dot = el("circle", { cx: v.x, cy: v.y, r: isSel ? 9 : 7, class: cls });
    if (interactive && vacant) {
      dot.classList.add("placeable");
      dot.addEventListener("click", () => { selectedVertex = { q: v.q, r: v.r, corner: v.corner, x: v.x, y: v.y }; render(); });
    }
    svg.appendChild(dot);
  }
}

function drawToken(svg, h) {
  const red = h.token === 6 || h.token === 8;
  svg.appendChild(el("circle", { cx: h.x, cy: h.y, r: 20, fill: "#f1e3c0", stroke: "#0d2c40", "stroke-width": 1.5 }));
  const t = el("text", { x: h.x, y: h.y + 6, "text-anchor": "middle", "font-size": 20, "font-weight": 700, fill: red ? "#b8312f" : "#1a1a1a" });
  t.textContent = h.token;
  svg.appendChild(t);
}

function drawHarbour(svg, hb) {
  svg.appendChild(el("circle", { cx: hb.x, cy: hb.y, r: 14, fill: hb.color, stroke: "#0d2c40", "stroke-width": 1.5 }));
  const t = el("text", { x: hb.x, y: hb.y + 4, "text-anchor": "middle", "font-size": 11, "font-weight": 700, fill: "#1a1a1a" });
  t.textContent = hb.ratio + ":1";
  svg.appendChild(t);
}

function edgeLine(e, cls) {
  return el("line", { x1: e.x1, y1: e.y1, x2: e.x2, y2: e.y2, class: cls, "stroke-linecap": "round" });
}

function hexPoints(cx, cy) {
  const pts = [];
  for (let i = 0; i < 6; i++) {
    const a = Math.PI / 180 * (60 * i - 30);
    pts.push(`${cx + SIZE * Math.cos(a)},${cy + SIZE * Math.sin(a)}`);
  }
  return pts.join(" ");
}

function key3(q, r, c) { return `${q},${r},${c}`; }
function sameVertex(a, b) { return a.q === b.q && a.r === b.r && a.corner === b.corner; }
function edgeTouches(e, v) {
  return (near(e.x1, v.x) && near(e.y1, v.y)) || (near(e.x2, v.x) && near(e.y2, v.y));
}
function near(a, b) { return Math.abs(a - b) < 0.5; }

$("create-btn").addEventListener("click", createGame);
$("join-btn").addEventListener("click", joinGame);
$("start-btn").addEventListener("click", startGame);

connect().catch((e) => { setStatus("offline", "bad"); toast("Could not connect: " + e.message); });
