# Mr. Trampoline's Dance Mix
> A 2D mobile arcade game fusing physics-based combo building with rhythm game risk/reward mechanics.

![gameplay gif]([ADD_GIF_HERE](https://static.wixstatic.com/media/5afcba_9c55074ff70a4015b40809b670710049~mv2.gif))

[▶ Play on itch.io](https://schokolasbotter.itch.io/mr-trampolines-dance-mix) | [📁 View Source](https://github.com/Schokolasbotter/MrTrampoline)

---

## Overview

**Platform:** iOS (Mobile) | **Engine:** Unity / C# | **Role:** Solo Developer

Mr. Trampoline's Dance Mix is a solo-developed mobile arcade title built for iOS. The player bounces a character on a trampoline to build a score multiplier — launch high enough and a rhythm phase triggers, where the player must swipe in the correct directions to score. Miss a swipe and your multiplier resets entirely. The difficulty scales continuously, rewarding mastery while punishing greed.

Developed in collaboration with a freelance artist and musician, managing full cross-discipline production as the sole programmer.

---

## Technical Contributions

- Designed and implemented a **dual-layer multiplier system** driving the core risk/reward loop
- Built a **custom swipe detection system** for precise mobile touch input
- Implemented a **10-tier dynamic difficulty system** scaling direction complexity and spawn rates with score
- Engineered a **directional camera shake system** providing physical feedback on player input
- Integrated a **third-party leaderboard REST API** for persistent high score tracking
- Developed a **coroutine-based score animation system** for satisfying score roll feedback
- Deployed a complete **iOS build pipeline** — first-time mobile development to shipped App

---

## Technical Deep Dive: The Risk/Reward Multiplier System

The core design challenge was making the player *feel* tension on every single launch — without the game feeling unfair.

The solution was a dual-layer multiplier stack. The **first layer** (`multiplier`) builds passively as the character bounces, rewarding sustained play and punishing falls. The **second layer** (`bonusCount`) is active: when the character launches above a threshold, a rhythm phase opens and the player must swipe in the indicated directions. Each correct swipe increases `danceScore` and the window for additional swipes stays open — but the swipe windows are intentionally short.

The risk: a single wrong swipe calls `resetMultiplier()`, wiping both layers instantly and forcing the state machine back to the trampoline phase. The final score calculation on a successful dance phase is:

```csharp
// Both multipliers applied only on a clean run
totalDanceScore += (int)(danceScore * multiplier * bonusCount);
```

This creates a compounding high-risk/high-reward decision on every launch cycle — attempt more swipes for a bigger multiplier, or play it safe and bank what you have.

---

## Technical Deep Dive: Dynamic Difficulty Scaling

Difficulty is driven by score thresholds mapped to a C# switch expression, unlocking new arrow directions and increasing spawn rates across 10 tiers:

```csharp
gameDifficulty = totalScore switch
{
    int x when x < scoreThreshold1 => 0,
    int x when x < scoreThreshold2 => 1,
    // ...
    int x when x >= scoreThreshold9 => 9,
    _ => 0
};
```

At difficulty 0, only cardinal directions are valid swipes. Higher tiers progressively introduce diagonal directions, increasing the cognitive load during the rhythm phase. Obstacle spawning activates at tier 4, with additional obstacles scaling unboundedly beyond tier 9.

---

## Architecture Overview

The game is managed by a central `GameManager` driving a hand-rolled state machine with five states:

```
GameState: start → trampoline ⇄ dance → end
                        ↑
                      pause
```

Input is handled via an event-driven pattern — `TouchManager` raises `SwipeDetect` and `StartGame` events that `GameManager` subscribes to, decoupling input detection from game logic.

Supporting systems: `UIManager`, `MusicManager`, `EffectPlayer`, `SpawnerScript`, `SpriteManager`, `DiscoScript`.

---

## Retrospective

The multiplier system achieved its design goal — playtests produced genuine tension and satisfying high-score chasing. The event-driven input architecture was a good decision that kept touch handling cleanly separated.

If I rebuilt this today, the main change would be breaking `GameManager` into smaller, single-responsibility systems. In its current form it handles state, scoring, camera, difficulty, spawning, and audio triggers in one class — a God Class that made late-stage changes risky and harder to reason about than they needed to be. I'd now use a proper event bus or messaging system to let decoupled systems react to state changes independently.

---

## Tech Stack

`Unity` · `C#` · `iOS` · `Mobile Touch Input` · `Coroutines` · `REST API Integration`

---

## Credits

- **Programming:** Laurent Klein  
- **Art:** [Artist Name]  
- **Music:** [Musician Name]
