# Operation Lighthouse

An arcade-inspired, high-energy tactical tower defense and resource-management game built in Unity 6 using the Universal Render Pipeline (URP). Take on the role of an outpost supplier, scavenge wreckage on the beach, manage your inventory limits under pressure, and automate a high-powered defense network to hold back waves of aggressive slimes!

---

## 🕹️ Core Gameplay Loop

1. **Scavenge:** Sprint across the beach territory to gather raw metallic scrap dropped by fallen enemies.
2. **Manage Pockets:** Track your inventory weight tightly! If your pockets hit maximum carrying capacity, scrap drops will safely rest on the sand until you clear space.
3. **Deposit & Automate:** Step onto the central flattened sphere **Deposit Zone** to seamlessly empty your backpack into the main lighthouse vault.
4. **Defend:** Use your gathered resources to unlock, scale, and reinforce your automated turret systems to shred invading swarms.

---

## ✨ Features & Polish (The "Juice")

* **HDR Bloom Glow Effects:** High Dynamic Range (HDR) emission textures coupled with URP Bloom post-processing layers give weapon projectiles a bright, vibrant, arcade neon visual style.
* **Cinematic Asynchronous Loading:** Smooth, modern title menu screen featuring an ambient bokeh dust particle simulation that transitions into a 5-second asynchronous asset-deployment loading screen.
* **Smart UI Architecture:** Dedicated decoupled menu frameworks split cleanly between Data Models (Health tracking, capacity ceilings) and User Interfaces (Fading Game Over canvas nodes).
* **Audio Feedback Network:** Complete dynamic soundscape including looping ambient title themes, deployment sirens, crisp turret firing feedback, and localized "play-and-forget" 3D audio effects when popping slimes.
* **Map Clamping Boundaries:** Optimized programmatic position clamping using `Mathf.Clamp()` to lock the player smoothly inside the active game space without requiring heavy invisible 3D wall colliders.

---

## 🛠️ Technical Implementation Details

* **Game Engine:** Unity 6
* **Render Pipeline:** Universal Render Pipeline (URP)
* **Input Architecture:** Unity New Input System (`PlayerInput` actions workflow)
* **Audio Handling:** Optimized spatial audio mixing utilizing `AudioSource.PlayClipAtPoint()` to bypass object destruction limits.
* **Performance Control:** Clean `.gitignore` profiling to isolate heavy local compilation tracks (`/Library/`, `/Temp/`, `*.csproj`) from the GitHub repository source code.

---

## 🎮 How To Play (Controls)

* **Movement:** Use WASD / Arrow Keys to sprint around the beach grid.
* **Collection:** Walk directly over metallic scrap to pick it up.
* **Depositing:** Walk directly over the flat sphere zone next to the central tower to dump your inventory.

---

## 📁 Project Directory Structure Highlight

* `Assets/Scripts/PlayerController.cs` - Manages bounded movement, movement animations, and enemy collision hazards.
* `Assets/Scripts/PlayerInventory.cs` - Tracks inventory capacity limits and triggers dynamic links to the tower vault upon step-in.
* `Assets/Scripts/TowerWeapon.cs` - Handles automatic nearest-target radar scanning, turret tracking interpolation, and firing sfx.
* `Assets/Scripts/GameOver.cs` - Manages the multi-second real-time countdown delay before rendering canvas execution profiles.
* `Assets/Scripts/MainMenuManager.cs` - Drives the corner loading slider calculations and scene index jumps.

---
*Developed as a standalone prototype deployment for the final project review.*
