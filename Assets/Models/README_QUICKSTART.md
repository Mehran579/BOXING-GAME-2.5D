# 2.5D Unity Boxing Game — Quick Build Guide

Scripts are in `/Scripts`. Everything below is what you do *inside the Unity Editor* — this can't be scripted because it involves importing assets, building the Animator graph, and laying out UI.

**Time-box:** Day 1 = setup + characters + arena + movement working. Day 2 = combat + UI + win/lose. Day 3 = sound + bonus features + record video + build.

---

## 0. Project setup
- Unity Hub → New Project → **3D (Core)** template, Unity 2022 LTS or newer.
- **Important:** these scripts use the legacy Input Manager. Go to *Edit → Project Settings → Player → Active Input Handling* and set it to **"Input Manager (Old)"** or **"Both"**, or `Input.GetAxis`/`GetKeyDown` will throw errors.
- Folders: `Assets/Scripts`, `Assets/Models`, `Assets/Animations`, `Assets/Prefabs`, `Assets/Audio`, `Assets/Materials`, `Assets/Scenes`.

## 1. Get characters fast (Mixamo)
1. Go to mixamo.com (free Adobe login), pick any humanoid model for your Player and a different one (or same model, different color material) for the Enemy.
2. Download each character: Format **FBX for Unity**, **with Skin**.
3. With that character still selected on Mixamo, download these 5 animations **without skin** (smaller files, faster): **Idle**, **Walking**, **Jump**, **Punching** (or "Boxing"), **Hit Reaction**.
4. Drag everything into `Assets/Models`. Select each FBX → **Rig** tab → Animation Type = **Humanoid** → Apply.

## 2. Build the arena (10 minutes)
- Quick version: a scaled-up Plane for the floor + 4 thin Cubes as ring posts at the corners + a couple of stretched Cubes/Cylinders between them as ropes. Color the floor differently so it reads as a ring.
- Faster: grab a free boxing ring model from Sketchfab or the Asset Store and drag it in.
- Add a Directional Light and a simple skybox so it doesn't look flat.

## 3. Set up the Player
1. Drag the Player model into the scene.
2. Add components: **CharacterController** (adjust Center/Height/Radius to roughly fit the model), **Health.cs**, **PlayerController.cs**.
3. Tag the GameObject **"Player"** (Inspector → Tag dropdown → Add Tag if needed).
4. In the model's hierarchy, find the right-hand bone (e.g. `mixamorig:RightHand`). Add an empty child GameObject there named "PunchHitbox", give it a small **BoxCollider**, and add **PunchHitbox.cs**. Set `targetTag = "Enemy"`.
5. On PlayerController, drag the PunchHitbox object into the `punchHitbox` field, and the Animator into `animator`.

## 4. Set up the Enemy
Same as above but: **EnemyAI.cs** instead of PlayerController, tag = **"Enemy"**, its PunchHitbox's `targetTag = "Player"`. Drag the Player's transform into EnemyAI's `player` field.

## 5. Animator Controller (the fiddly part)
1. Project window → right-click → *Create → Animator Controller* → name it `FighterAnimator`.
2. Open the Animator window, drag in your 5 clips as states: **Idle** (set as default), **Walk**, **Jump**, **Punch**, **Hit**.
3. Add parameters: `Speed` (Float), `Jump` (Trigger), `Grounded` (Bool), `Punch` (Trigger), `Hit` (Trigger).
4. Transitions:
   - Idle ↔ Walk: Speed > 0.1 → Walk, Speed < 0.1 → Idle.
   - Any State → Jump on `Jump` trigger (uncheck "Has Exit Time" for instant response); Jump → Idle when `Grounded` = true.
   - Any State → Punch on `Punch` trigger; Punch → Idle with Exit Time = 1 (no condition needed, just let the clip finish).
   - Any State → Hit on `Hit` trigger; Hit → Idle with Exit Time ≈ 0.8.
5. Drag this **same controller** onto both the Player's and Enemy's Animator component — they share parameter names so one controller works for both.
6. Open the **Punch** clip in the Animation window, scrub to the moment the fist extends, and add an **Animation Event** calling `ActivatePunchHitbox()`. A few frames later, add another calling `DeactivatePunchHitbox()`. This is what makes hit detection only count during the actual swing instead of the whole animation.

## 6. Camera
Add **CameraFollow2_5D.cs** to the Main Camera. Drag in Player and Enemy transforms. It auto-frames both fighters from the side and zooms based on distance — no manual keyframing needed.

## 7. Canvas UI
1. *GameObject → UI → Canvas*.
2. Add two **Sliders** (*UI → Slider*): "PlayerHealthBar" top-left, "EnemyHealthBar" top-right. Set Min Value = 0, Max Value = 1. If you don't want them draggable, delete each Slider's "Handle Slide Area" child.
3. Add two full-screen **Panels**: "WinPanel" ("YOU WIN") and "LosePanel" ("YOU LOSE"), each with a big Text/TextMeshPro. Leave both inactive in the hierarchy (the script activates them).
4. Create an empty GameObject "UIManager", add **UIManager.cs**, drag in the two sliders and two panels.
5. Create an empty GameObject "GameManager", add **GameManager.cs**, drag in the Player's Health, Enemy's Health, and the UIManager object.

## 8. Sound (fast pass)
- Add an **AudioSource** to each fighter. On the Health component, under the `OnHit` event list, click `+`, drag in that AudioSource, choose `AudioSource.PlayOneShot`, and assign a free punch SFX clip (freesound.org or Asset Store). No code needed — `OnHit` is already a public UnityEvent.
- For music: empty GameObject "Music" with an AudioSource, a looping track, "Play On Awake" checked.

## 9. Build & submit
1. *File → Build Settings* → add your scene → pick platform (PC/Mac/Linux Standalone for `.exe`, or Android for `.apk`) → **Build**.
2. Record 1–2 minutes of gameplay: movement, jump, punches landing, health bars dropping, and a win/lose screen.
3. Zip `Assets`, `Packages`, and `ProjectSettings` (skip `Library` — not needed and huge), plus your Source Code, build output, and video. Email to both addresses in the assessment PDF.

---

## 10. Bonus features (cheap add-ons, once core loop works)

**Combo system** — in `PlayerController`, track punch timestamps and add a damage multiplier on rapid hits:
```csharp
private int comboCount = 0;
private float comboWindow = 0.8f;
// in HandlePunch(), after a successful punch press:
if (Time.time - lastPunchTime < comboWindow) comboCount++; else comboCount = 1;
// pass comboCount to PunchHitbox to scale damage, reset on Idle re-entry
```

**Knockout** — in `Health.OnDeath`, instead of just freezing time, trigger a "KO" animation trigger and disable the loser's controller script (`GetComponent<PlayerController>().enabled = false`) before freezing.

**Hit VFX** — in `PunchHitbox.OnTriggerEnter`, after `TakeDamage`, do `Instantiate(hitVfxPrefab, transform.position, Quaternion.identity)` with a small spark/particle prefab.

**Camera shake** — add a coroutine to `CameraFollow2_5D` that nudges `transform.position` randomly for ~0.15s, triggered from `Health.OnHit`:
```csharp
public void Shake(float duration = 0.15f, float magnitude = 0.15f) => StartCoroutine(DoShake(duration, magnitude));
private System.Collections.IEnumerator DoShake(float duration, float magnitude) {
    Vector3 original = transform.localPosition;
    float t = 0f;
    while (t < duration) {
        transform.localPosition = original + (Vector3)(Random.insideUnitCircle * magnitude);
        t += Time.deltaTime;
        yield return null;
    }
    transform.localPosition = original;
}
```

**Improved UI** — add a round timer (`Text` + countdown coroutine in GameManager) and a "Fight!" intro text that fades out at match start.
