# Ventrix Sync Disks

## Introduction

Welcome to Ventrix Industries, the undisputed leader in air duct and vent system maintenance in our city of towering achievements and even taller monitoring masts! As our newest vent technician, you're about to embark on an exhilarating journey through the twists, turns, and occasionally, the slightly suspicious smells of our city's most intricate infrastructure. Enclosed with this orientation pamphlet, you should find your essential toolbox equipped with three cutting-edge sync disks: **Vent Mobility**, **Vent Recon**, and **Vent Mischief**. These tools will be important for the tight spaces and adventures that await you!

> A word to the wise: if you are reading this and you're not clad in a Ventrix-branded jumpsuit, chances are you've either pilfered these tools from one of our exceptionally dedicated technicians, or you've stumbled upon them in the less reputable corners of the black market. Either way, do the honorable thing and return them to Ventrix Industries immediately. Your cooperation ensures you won’t find us crawling through your ducts in search of our property!

## About Us

At Ventrix Industries, we pride ourselves on being more than just a company; we're a cornerstone of innovation and control in a world teeming with opportunities and, admittedly, a few too many competitors. Founded by the visionary yet enigmatic Dr. Arsonide in the roaring 50s, we swiftly rose to dominate the ventilation industry, ensuring every breath our citizens take is a breath of Ventrix-filtered air. Our vents and ducts form the unseen veins of our city, pulsing with secrets and potential. With presidential aspirations, and the robust backing of Kensington Indigo, we're not just powering the airflow - we're setting the stage for a Ventrix-led future where efficiency, security, and governance are as reliable as our world-class ventilation systems.

## Installation

### r2modman or Thunderstore Mod Manager installation

If you are using [r2modman](https://thunderstore.io/c/shadows-of-doubt/p/ebkr/r2modman/) or [Thunderstore Mod Manager](https://www.overwolf.com/oneapp/Thunderstore-Thunderstore_Mod_Manager) for installation, simply download the mod from the Online tab.

### Manual installation

Follow these steps:

1. Download BepInEx from the official repository.
2. Extract the downloaded files into the same folder as the "Shadows of Doubt.exe" executable.
3. Launch the game, load the main menu, and then exit the game.
4. Download the latest version of the mod from the Releases page on either Thunderstore or Github. Unzip the files and place them in corresponding directories within "Shadows of Doubt\BepInEx...". Also, download the [SOD.Common](https://thunderstore.io/c/shadows-of-doubt/p/Venomaus/SODCommon/) dependency.
5. Start the game.

## Toolbox

Next up, let's break down your toolbox. We'll explore the specifics of each sync disk, each equipped with two unique but related upgrade paths to boost your efficiency in the field.

### Vent Mobility

* **Chute Scoot**: Slip through the vents as smoothly as a TV dinner after midnight. This sync disk makes you move faster through vents by secreting a sleek, friction reducing coating. ***Reminder***: Vent racing is strictly prohibited by Ventrix company policy.

* **Duct Parkour**: Sometimes the job calls for a grand entrance, or quick exit! Get in and out of vents with the flair of a circus acrobat and the grace of a ballet dancer. You will be able to enter and exit vents further away, faster than ever before, and have them automatically close behind you.

### Vent Recon

* **Acoustic Mapping**: Initially designed to sense sound waves emitted by tapping a small specialized metal rod on the vent, technicians have found that throwing money away also works! Throwing a coin against air ducts sends out an echolocation pulse like a bat! This is visualized for you to see air ducts and vents through walls.
* **Grate Snooping**: When working near vent exits, this disk not only highlights customers and potentially hazardous devices through walls, but also allows you to fast-forward time! Perfect for those extended repair sessions. While Ventrix officially discourages extended shifts, we understand sometimes the job knows no clock.

### Vent Mischief

* **Shaft Specter**: Avoid disturbing customers with footsteps or chattering teeth during long maintenance sessions - this disk dramatically lowers the chance of you making noises when as you move through vents, and makes you immune to the cold while performing vent maintenance.
* **Tunnel Terror**: This disk was designed for breaking out of vents in toxic gas emergencies. It lets you loudly burst forth from air vents in a way that will probably startle onlooking customers in private areas. It also makes you immune to toxic gas in vents. Use with caution - this disk may unleash your inner vent goblin.

## Configuration

In r2modman you should see "Config editor" on the left, in the "Other" section. If you click on that and then open up BepInEx\config\VentrixSyncDisks.cfg and click "Edit config", you will have many options available to you to configure Ventrix Sync Disks to your liking. Just click "Save" when you are done.

> Ventrix Sync Disks starts with default settings that are balanced for a fun vanilla gameplay experience. There are a LOT of options below, but you don't need to change them unless you want to. You can skip everything below and go play the game now if you want.

Each sync disk has extensive configuration options allowing you to change everything about the behavior of them. These changes will be reflected in the skill descriptions in-game, but be warned that you can absolutely break the game by, for example, making yourself move through vents 500x faster.

You will see many options that say (All Upgrades) below, this is just to keep this document shorter. What this means is that this option is actually three options: one reflecting the sync disk with no upgrades, one for the first upgrade, and one for the second upgrade. For many options you can adjust these for each level individually.

> If you do not see VentrixSyncDisks.cfg in your options, click "Start modded" to launch the game, then close both the game and r2modman. When you launch r2modman again, the config file will be there.

> Note that the mod caches a lot of things when it starts, so it's a good idea to modify these settings before you launch the game, not while the game is running.

---

### 1. General

- **Enabled**: Another method of enabling and disabling Ventrix Sync Disks.
- **Version**: Ventrix Sync Disks uses this to reset your configuration between major versions. Don't modify it or it will reset your configuration!
- **Vent Mobility Enabled**: Whether or not the Vent Mobility sync disk is enabled in-game.
- **Vent Recon Enabled**: Whether or not the Vent Recon sync disk is enabled in-game.
- **Vent Mischief Enabled**: Whether or not the Vent Mischief sync disk is enabled in-game.
- **Available At Black Markets**: The sync disks appear in the world, but with this, they will also be purchasable at black markets and black market sync clinics.
- **Available At Sync Disk Clinics**: The sync disks appear in the world, but with this, they will also be purchasable at legitimate sync clinics.

---

### 2. Runner

- **Vent Speed Multiplier (Base Level)**: A multiplier applied to your speed in vents with this sync disk installed and no upgrades.
- **Vent Speed Multiplier (First Upgrade)**: A multiplier applied to your speed in vents with this sync disk installed and one upgrade.
- **Vent Speed Multiplier (Second Upgrade)**: A multiplier applied to your speed in vents with this sync disk installed and two upgrades.

---

### 3. Parkour

- **Added Interaction Range (Base Level)**: This many meters are added to your vent interaction range with this sync disk installed and no upgrades.
- **Added Interaction Range (First Upgrade)**: This many meters are added to your vent interaction range with this sync disk installed and one upgrade.
- **Added Interaction Range (Second Upgrade)**: This many meters are added to your vent interaction range with this sync disk installed and two upgrades.
- **Transition Speed Multiplier (Base Level)**: A multiplier applied to your vent transition (enter / exit) time with this sync disk installed and no upgrades. Smaller is shorter.
- **Transition Speed Multiplier (First Upgrade)**: A multiplier applied to your vent transition (enter / exit) time with this sync disk installed and one upgrade. Smaller is shorter.
- **Transition Speed Multiplier (Second Upgrade)**: A multiplier applied to your vent transition (enter / exit) time with this sync disk installed and two upgrades. Smaller is shorter.
- **Auto Close Vents (Base Level)**: Whether vents automatically close after you enter / exit them with this sync disk installed and no upgrades.
- **Auto Close Vents (First Upgrade)**: Whether vents automatically close after you enter / exit them with this sync disk installed and one upgrade.
- **Auto Close Vents (Second Upgrade)**: Whether vents automatically close after you enter / exit them with this sync disk installed and two upgrades.

---

### 4. Mapping

- **Echolocation Range (Base Level)**: How many ducts your echolocation pulse travels with this sync disk installed and no upgrades.
- **Echolocation Range (First Upgrade)**: How many ducts your echolocation pulse travels with this sync disk installed and one upgrade.
- **Echolocation Range (Second Upgrade)**: How many ducts your echolocation pulse travels with this sync disk installed and two upgrades.
- **Echolocation Speed (Base Level)**: How many seconds it takes your echolocation pulse to travel one duct with this sync disk installed and no upgrades.
- **Echolocation Speed (First Upgrade)**: How many seconds it takes your echolocation pulse to travel one duct with this sync disk installed and one upgrade.
- **Echolocation Speed (Second Upgrade)**: How many seconds it takes your echolocation pulse to travel one duct with this sync disk installed and two upgrades.
- **Echolocation Duration (Base Level)**: How many seconds each "dot" of your echolocation pulse lasts before it expires with this sync disk installed and no upgrades.
- **Echolocation Duration (First Upgrade)**: How many seconds each "dot" of your echolocation pulse lasts before it expires with this sync disk installed and one upgrade.
- **Echolocation Duration (Second Upgrade)**: How many seconds each "dot" of your echolocation pulse lasts before it expires with this sync disk installed and two upgrades.
- **Coin Duration Multiplier (Base Level)**: A multiplier applied to your echolocation "dot" expiration time while you are holding your coin with this sync disk installed and no upgrades. Smaller makes them last longer.
- **Coin Duration Multiplier (First Upgrade)**: A multiplier applied to your echolocation "dot" expiration time while you are holding your coin with this sync disk installed and one upgrade. Smaller makes them last longer.
- **Coin Duration Multiplier (Second Upgrade)**: A multiplier applied to your echolocation "dot" expiration time while you are holding your coin with this sync disk installed and two upgrades. Smaller makes them last longer.

---

### 5. Snooping

- **Highlight Color Multiplier**: A hex code for what color the Snooping outline will be multiplied by.

---

### 6. Specter

- **Footstep Chance (Base Level)**: The chance you play footstep sounds when traveling in vents, when you normally would, with this sync disk installed and no upgrades.
- **Footstep Chance (First Upgrade)**: The chance you play footstep sounds when traveling in vents, when you normally would, with this sync disk installed and one upgrade.
- **Footstep Chance (Second Upgrade)**: The chance you play footstep sounds when traveling in vents, when you normally would, with this sync disk installed and two upgrades.
- **Cold Immunity (Base Level)**: Whether you are immune to cold in vents with this sync disk installed and no upgrades.
- **Cold Immunity (First Upgrade)**: Whether you are immune to cold in vents with this sync disk installed and one upgrade.
- **Cold Immunity (Second Upgrade)**: Whether you are immune to cold in vents with this sync disk installed and two upgrades.

---

### 7. Menace

- **Citizen Nerve (Base Level)**: The amount a citizen's nerve stat is damaged when you pop out of vents in private areas with this sync disk installed and no upgrades.
- **Citizen Nerve (First Upgrade)**: The amount a citizen's nerve stat is damaged when you pop out of vents in private areas with this sync disk installed and one upgrade.
- **Citizen Nerve (Second Upgrade)**: The amount a citizen's nerve stat is damaged when you pop out of vents in private areas with this sync disk installed and two upgrades.
- **Toxic Gas Immunity (Base Level)**: Whether you are immune to toxic gas in vents with this sync disk installed and no upgrades.
- **Toxic Gas Immunity (First Upgrade)**: Whether you are immune to toxic gas in vents with this sync disk installed and one upgrade.
- **Toxic Gas Immunity (Second Upgrade)**: Whether you are immune to toxic gas in vents with this sync disk installed and two upgrades.

## License
All code in this project is distributed under the MIT License. Feel free to use, modify, and distribute as needed. That license can be found in **License.txt**. Attribution and licenses for all third party libraries and assets used in the creation of Ventrix Sync Disks can be found in **Attribution.txt**.