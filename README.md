# Cloverpit-jumpstart
Provides a subtle early-game jumpstart to early game progression in CloverPit. The extra clover tickets and free restock smooth out the opening turns, letting players establish momentum faster without disrupting overall balance or progression.

# Modifications
 - Increase beginning clover tickets from 2 -> 4.
 - Allocate one free restock at the beginning of the game.
 - (enable isModded flag).

# Impact to gameplay
 - Accelerate early game pacing and eliminate some early shop RNG.
 - Enable aggressive build chasing at the cost of harming build variance (due to tunnel vision).
 - Only impact to late game is the snowball effect from what's essentially a free eco round at the beginning.
 - This game detects modifications by matching assembly names with keywords (MonoMod, 0Harmony, Harmony, BepInEx and MelonLoader). Since this is a direct edit, this returns false by default but has been deliberately changed to return true. This does not impact achievements or meta progression.

# Installation
Navigate to game installation (on steam, this can be done via right clicking the game in your library -> Manage -> Browse local files).
Go to CloverPit_Data/Managed/ and rename your original Assembly-CSharp.dll to something identifiable as a backup (eg. Assembly-CSharp.dll.bk).
Move the modified Assembly-CSharp.dll provided by this mod here, such that it would replace the original.
Launch the game normally, the changes should take effect immediately.

## Conflict Warning
This mod directly modifies the Assembly-CSharp.dll file, which means it'll conflict with any other mods that edit the same file.
To avoid issues:
 - Only use one modded Assembly-CSharp.dll at a time.
 - Incompatible versions or mismatched game updates may cause strange behaviour, so verify your mod matches the current game build.
