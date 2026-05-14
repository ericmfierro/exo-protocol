<h1>Exo-Protocol</h1>

<p>
Exo-Protocol is a high-intensity sci-fi arena FPS where players remotely pilot a cyborg combat unit through escalating military combat simulations. 
Chain kills to activate bullet time, push your neural link to its limits, and decide whether to revive at the cost of your sanity or restart from scratch. 
Every death is a decision. Every kill chain is a rush.
</p>

<h2>Group Members</h2>
<ul>
  <li>Michelle Reyes</li>
  <li>Brian Khuu</li>
  <li>Eric Fierro</li>
</ul>

<h2>Links</h2>
<p>
<b>Gameplay Demo:</b><br>
<a href="https://www.youtube.com/watch?v=vV9sd3yLf2w">
https://www.youtube.com/watch?v=vV9sd3yLf2w
</a>
</p>

<p>
<b>Download Build:</b><br>
<a href="https://michellereyes04.itch.io/exo-protocol">
https://michellereyes04.itch.io/exo-protocol
</a>
</p>

<h2>Project Setup</h2>
<ul>
  <li><b>Unity Version:</b> 6000.4.4f1</li>
  <li><b>Main Playable Scene:</b> Assets/Scenes/Main.unity</li>
  <li><b>Menu Scene:</b> Assets/Scenes/Menu.unity</li>
  <li><b>Game Over Scene:</b> Assets/Scenes/GameOver.unity</li>
  <li><b>Win Scene:</b> Assets/Scenes/WonGame.unity</li>
</ul>

<h2>Build Settings Scene Order</h2>
<table border="1" cellpadding="6">
  <tr>
    <th>Index</th>
    <th>Scene</th>
  </tr>
  <tr>
    <td>0</td>
    <td>Menu</td>
  </tr>
  <tr>
    <td>1</td>
    <td>Main</td>
  </tr>
  <tr>
    <td>2</td>
    <td>GameOver</td>
  </tr>
  <tr>
    <td>3</td>
    <td>WonGame</td>
  </tr>
</table>

<h2>Controls</h2>
<table border="1" cellpadding="6">
  <tr>
    <th>Input</th>
    <th>Action</th>
  </tr>
  <tr>
    <td>WASD</td>
    <td>Move</td>
  </tr>
  <tr>
    <td>Mouse Movement</td>
    <td>Look / Aim</td>
  </tr>
  <tr>
    <td>Left Mouse Button</td>
    <td>Fire Weapon</td>
  </tr>
  <tr>
    <td>Space</td>
    <td>Jump</td>
  </tr>
  <tr>
    <td>Shift</td>
    <td>Sprint</td>
  </tr>
  <tr>
    <td>Walk into Pickups</td>
    <td>Collect Health / Ammo Pickups</td>
  </tr>
</table>

<p>
The game uses the Unity Starter Assets First Person Controller input system. 
Cursor lock is enabled during gameplay and unlocked on the menu, win, and game-over scenes.
</p>

<h2>Gameplay Rules</h2>
<ul>
  <li>Player health starts at 2000.</li>
  <li>Player ammo starts at 100.</li>
  <li>Each player shot consumes 1 ammo.</li>
  <li>Enemy ranged shots deal 2 damage per hit.</li>
  <li>Enemy contact damage deals 10 damage per hit with a 1-second cooldown.</li>
  <li>The Game Over scene loads when player health reaches 0 or ammo reaches 0.</li>
  <li>The Win scene loads when the player kills 10 soldiers.</li>
  <li>Kill chains reward health, ammo, score, and score multipliers.</li>
  <li>Sanity drains over time and triggers escalating audio and visual psychosis effects.</li>
</ul>

<h2>Play Instructions</h2>
<p>
Networking is not currently implemented in the playable build. 
There is no active host, join, lobby, relay, or networked player-spawn system yet.
</p>

<ol>
  <li>Open <b>Assets/Scenes/Menu.unity</b>.</li>
  <li>Press <b>Play</b> in the Unity Editor.</li>
  <li>Click <b>Start</b> to load the main scene.</li>
  <li>Play locally as a single-player experience.</li>
</ol>
