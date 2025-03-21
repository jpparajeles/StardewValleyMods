# 3.3.1
- *Bugfix*: Fixes the double mod behavior

# 3.3.0
- *Bugfix*: Add more check for invalid flowers to prevent a null search on seed map initialization
- *New feature*: Probabilities generator preset. A new option to generate a custom probability set based on vanilla vs modded flowers and an algorithm to scale the probability based on Equality, Randomness or Price
    - Vanilla vs Modded
      - No modded, Always Vanilla: Sets the modded flowers to Disabled
      - Less modded: Will prefer vanilla flowers over modded flowers
      - Normal: No behavior change based on Modded vs vanilla
      - Less vanilla: Will prefer modded flowers over vanilla flowers
      - No vanilla, Always modded: Sets the vanilla flowers to Disabled
    - Algorithm
      - Equal: Flowers will have the same probability to each other
      - Price: The more expensive the flower, the less likely that it happen, specially if it's very expensive
      - Random: Each flower will get a random probability
- *Bugfix* Fix some typos

# 3.2.0
- *Mod author feature* *New Feature*: Adds the Asset `Mods/jpp.WildFlowersReimagined/IgnoreList` that allows mod authors to prevent flowers from spawning.

# 3.1.2
- *Bugfix*: Fix NullReferenceException on the debug code, also disable the debug code paths

# 3.1.1
- *Note*: Swap the RNG from the global one to a local one to not affect any random event from the game directly

# 3.1.0
- *New feature*: Added the KeepRegrowFlower option to either preserve or remove flowers that bloom multiple times (ex. Cornucopia Trellises flowers)

# 3.0.0
- *New feature*: Multiplayer support: Online multiplayer should be functional using Spacecore synchronization
- *New Dependency* SpaceCore
- *Minor fix* Set MinimumApiVersion to `4.0.0` to ensure it loads with Stardew 1.6

# 2.3.0
- *BUGFIX*: Fixes an issue where multiple flower ids have the same item data

# 2.2.0 
- *BUGFIX*: Fixes flower getting destroyed accidentally when inventory was full on harvest by hand

# 2.1.0
- *New Feature*: Adds option to try to spawn flowers in all locations, useful for mods like SVE
- *BUGFIX*: Prevents multiplayer from crashing by disabling the mod outside the main client. Multiplayer support is been look at it, but it will take more time

# 2.0.2
- *BUGFIX*: Correct bug fix for 2.0.1, invalid saved data should be skipped with a warning
- *BUGFIX*: Clear the patch map state when loading a save game in case of loading from title
- *BUGFIX*: Fix a logic issue when creating the saved data where invalid data was been saved.

# 2.0.1
- *BUGFIX*: Additional null checks and handling on save loading

# 2.0.0
- *New Feature*: Flower probability configuration. Adds a new set of options that allow the individual configuration of the flowers.
    - Disabled: They will get 0x multiplier to the configuration making them not spawn
    - Rarely: Adds only 1 (1/3x normal) copy of the seed
	- Occasionally: Adds 2 (2/3x normal) copies of the seed
    - Normal: Adds 3 (1x) copies of the seed
    - Often: Adds 4 (4/3x) copies of the seed
    - Abundant: Adds 5 (5/3x) copies of the seed

This adds as well an option to preserve flowers already planted even if their config drops to 0

# 1.3.0
- *BUGFIX* Fix scythe harvests

# 1.2.0
- *BUGFIX* Removes invalid / missing FlowerGrass from any location
- *Improvement*: Better description for `HarvestByScythe`

# 1.1.0
- *BUGFIX* Allows flowers with multiple seeds to work

# 1.0.0
- Initial mod release