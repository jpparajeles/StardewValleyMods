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