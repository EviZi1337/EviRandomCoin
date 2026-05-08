# EviZi Moneta — *EviRandomCoin*

> A LabAPI plugin that actually makes the coin flip useful (or lethal).  
> I saw someone's "fantastic" attempt at this, died of cringe, and decided to rewrite it from scratch — so your server doesn't explode.

**Requires:** [LabAPI](https://github.com/LabAPI)  
**Does NOT require:** absolute paths to some guy's `Downloads` folder on polish.

---

## 🤔 Why this and not that other junk?

Because this one was written **with a brain**. Instead of 500 `elif` statements and hardcoded garbage — actual architecture.

---

## ✨ The Good Stuff (Fully Configurable)

| Feature | Why it's better |
|---|---|
| **Weighted Random** | You can actually set probabilities. `0.01` weight for a nuke? Done. |
| **Logic Pools** | Items, Roles, Effects, Ammo, Candy — all separated. |
| **Real Teleportation** | It actually teleports you. In the previous version it was just a prank, I guess? |
| **Anti-Spam** | Cooldown added, because clicking a coin 50 times a second is unhinged behavior. |

---

## 💥 What can happen *(if you don't touch the config)*

- 🎒 **Items** — From standard Medkits to rare high-tier guns.
- ⚡ **Effects** — Speed, Regeneration, or just straight up dying.
- 🔄 **Role Swap** — Suddenly you're an SCP. Or a Spectator. Life is unfair.
- 🌪️ **Chaos** — Item rains, inventory wipes, or getting launched into the ceiling.

---

## ⚠️ Important — *Read this or don't ping me*

This plugin **works**. I've spent time making sure it doesn't leak memory or crash your server every 5 minutes.

- ❌ **If it breaks** — it's probably your config.
- ❓ **If you have questions** — don't. Read the code. It's clean enough for a toddler to understand.
- 💡 **If you want a new feature** — write it yourself. The code is modular for a reason.

> I am not your tech support. Use it, break it, fix it.  
> Just don't clutter my DMs with *"how do I install this"* — there are **4 steps below**.  
> If you can't follow them, maybe server hosting isn't for you.

---

## 🚀 Installation

```
1. Download the .dll
2. Put it in LabAPI/plugins/
3. Restart.
4. The config is massive. Don't ping me asking what "weight" means. It's math. Figure it out.
```

---

## ⚙️ Config — *The "Don't Break This" Section*

Everything is **weighted**. If you set one thing to `weight: 100` and everything else to `1` — guess what's gonna happen 99% of the time.

consume_coin_by_default: true
cooldown_seconds: 1.25
bad_luck_can_kill: false  # Set to true if you hate your players

outcomes:
  - id: "RareItem"
    weight: 5
  - id: "InventoryWipe"
    weight: 2
  - id: "Teleport"
    weight: 10
```

---

## 📄 License & Usage

I wrote this for fun and to clear Discord from trash-tier code.

| | |
|---|---|
| **Ownership** | It's mine, but now it's yours too. |
| **Modification** | Change whatever you want. Rename it, skin it, rewrite it. |
| **Credit** | Keep the author tag or don't — I know who wrote it and so do you. |
| **Support** | **ZERO.** I provide this *"as-is"*. If your server starts speaking enchantment table language after installing this, that's on you. |

---

## 👤 Author

**EviZi1337** — *The guy who actually uses SOLID principles*
```
