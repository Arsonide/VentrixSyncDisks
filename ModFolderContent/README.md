# Ventrix Sync Disks

## Introduction

Welcome to Ventrix Industries, the undisputed leader in air duct and vent system maintenance in our city of towering achievements and even taller monitoring masts! As our newest vent technician, you're about to embark on an exhilarating journey through the twists, turns, and occasionally, the slightly suspicious smells of our city's most intricate infrastructure. Enclosed with this orientation pamphlet, you should find your essential toolbox equipped with three cutting-edge sync disks: **Vent Mobility**, **Vent Recon**, and **Vent Mischief**. These tools will be important for the tight spaces and adventures that await you!

> A word to the wise: if you are reading this and you're not clad in a Ventrix-branded jumpsuit, chances are you've either pilfered these tools from one of our exceptionally dedicated technicians, or you've stumbled upon them in the less reputable corners of the black market. Either way, do the honorable thing and return them to Ventrix Industries immediately. Your cooperation ensures you won’t find us crawling through your ducts in search of our property!

## About Us

At Ventrix Industries, we pride ourselves on being more than just a company; we're a cornerstone of innovation and control in a world teeming with opportunities and, admittedly, a few too many competitors. Founded by the visionary yet enigmatic Dr. Arsonide in the roaring 50s, we swiftly rose to dominate the ventilation industry, ensuring every breath our citizens take is a breath of Ventrix-filtered air. Our vents and ducts form the unseen veins of our city, pulsing with secrets and potential. With presidential aspirations, and the robust backing of Kensington Indigo, we're not just powering the airflow - we're setting the stage for a Ventrix-led future where efficiency, security, and governance are as reliable as our world-class ventilation systems.

## Your Toolbox

Next up, let's dissect your toolbox. We'll dive into the nitty-gritty of each sync disk, each equipped with two unique but related upgrade paths designed to improve your vent maintenance efficiency in the field. Because a more efficient technician is our best asset in maximizing customer satisfaction (and our bottom line).

### Vent Mobility

* **Chute Scoot**: Slip through the vents as smoothly as a TV dinner after midnight. This sync disk makes you move faster through vents by secreting a sleek, friction reducing coating. ***Reminder***: Vent racing is strictly prohibited by Ventrix company policy.

* **Duct Parkour**: Sometimes the job calls for a grand entrance, or quick exit! Get in and out of vents with the flair of a circus acrobat and the grace of a ballet dancer. You will be able to enter and exit vents further away, faster than ever before, and have them automatically close behind you.

### Vent Recon

* **Acoustic Mapping**: Initially designed to sense sound waves emitted by tapping a small specialized metal rod on the vent, technicians have found that throwing money away also works! Throwing a coin against air ducts sends out an echolocation pulse (like a bat). This is visualized for you to see air ducts and vents through walls.

* **Grate Snooping**: When working near vent exits, this disk not only highlights customers and potentially hazardous security devices through walls, but also allows you to fast-forward time! Perfect for those extended repair sessions. While Ventrix officially discourages extended shifts, we understand sometimes the job knows no clock. Bring snacks.

### Vent Mischief

* **Shaft Specter**: Avoid disturbing customers with footsteps or chattering teeth during long maintenance sessions - this disk dramatically lowers the chance of you making noises as you move through vents, and makes you immune to the cold while performing vent maintenance.

* **Tunnel Terror**: This disk was designed for breaking out of vents in toxic gas emergencies. It lets you loudly burst forth from air vents in a way that will probably startle onlooking customers in private areas. It also makes you immune to toxic gas in vents. Use with caution - this disk may unleash your inner vent goblin.

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
- **Vent Mobility Enabled**: Whether or not the "Vent Mobility" sync disk is available in the game.
- **Vent Recon Enabled**: Whether or not the "Vent Recon" sync disk is available in the game.
- **Vent Mischief Enabled**: Whether or not the "Vent Mischief" sync disk is available in the game.
- **Vent Mobility Price**: The price of the "Vent Mobility" sync disk at vendors.
- **Vent Recon Price**: The price of the "Vent Recon" sync disk at vendors.
- **Vent Mischief Price**: The price of the "Vent Mischief" sync disk at vendors.
- **Available At Legit Sync Disk Clinics**: The sync disks appear in the world, but with this, they will also be purchasable at legitimate sync clinics.
- **Available At Shady Sync Disk Clinics**: The sync disks appear in the world, but with this, they will also be purchasable at black market sync clinics.
- **Available At Black Market Traders**: The sync disks appear in the world, but with this, they will also be purchasable at black market traders.
- **Vent Vertical Movement Enabled**: Whether you can now use the jump and crouch keys to move up and down in vents.

---

### 2. Scooting

- **Vent Speed Multiplier (All Upgrades)**: The multiplier on your movement speed when in vents with each upgrade.

---

### 3. Parkour

- **Added Interaction Range (All Upgrades)**: How much further you can reach vents with each upgrade.
- **Transition Speed Multiplier (All Upgrades)**: A multiplier on the speed you enter and exit vents with each upgrade.
- **Auto Close Vents (All Upgrades)**: Whether vents close behind you automatically when you enter or exit them with each upgrade.

---

### 4. Mapping

- **Echolocation Range (All Upgrades)**: How far your echolocation pulse travels down vents with each upgrade.
- **Echolocation Speed (All Upgrades)**: How quickly your echolocation pulse travels down vents with each upgrade.
- **Echolocation Duration (All Upgrades)**: How long it takes for your echolocation pulse to expire with each upgrade.
- **Coin Duration Multiplier (All Upgrades)**: A multiplier on echolocation duration while holding a coin with each upgrade.
- **Echolocation Sound Volume**: How loud the coin sounds that play during echolocation pulses are. Set to zero to turn them off.

---

### 5. Snooping

- **Can Snoop Civilians (All Upgrades)**: Whether you see civilians through walls when near vents with each upgrade.
- **Can Snoop Peek Vents (All Upgrades)**: Whether you see things through walls when near "peek" vents with each upgrade.
- **Can Snoop Security Systems (All Upgrades)**: Whether you see security systems through walls when near vents with each upgrade.
- **Can Pass Time Near Vents (All Upgrades)**: Whether you can stare at your watch to pass time when near vents with each upgrade.
- **Pass Time Warp Delay**: When you can pass time near vents, how long you must stare at your watch to pass time.
- **Pass Time Notification Delay**: When about to pass time near vents, when to notify you while staring at your watch that you are about to pass time. (Set to negative number for no notification.)

---

### 6. Specter

- **Footstep Chance (All Upgrades)**: The chance you play footstep sounds when travelling in vents, when you normally would, with each upgrade.
- **Cold Immunity (All Upgrades)**: Whether you are granted cold immunity in vents with each upgrade.

---

### 7. Terror

- **Freakout Duration (All Upgrades)**: The duration a citizen freaks out when you pop out of vents in private areas with each upgrade.
- **Toxic Immunity (All Upgrades)**: Whether you are granted toxic gas immunity in vents with each upgrade.
- **Scareable Citizens (Residence)**: How many citizens can be scared when you pop out of vents in private residence areas at one time.
- **Scareable Citizens (Workplace)**: How many citizens can be scared when you pop out of vents in workplace areas at one time.

---

### 8. Rendering

- **Central Node Size**: When rendering vent networks, how large the visualized nodes are in the center of each duct.
- **Use Directional Nodes**: When rendering vent networks, whether to render additional indicators pointing at connected air ducts.
- **Special Directional Node Colors**: When rendering vent networks, whether additional connections on vents are colored as normal ducts or as vents.
- **Directional Node Length**: When rendering vent networks, how long the indicators pointing down connected ducts are.
- **Directional Node Diameter**: When rendering vent networks, how wide the indicators pointing down connected ducts are.
- **Directional Node Offset**: When rendering vent networks, how far apart the indicators pointing down connected ducts are.
- **Node Color Normal**: When rendering vent networks, what color normal air ducts are visualized as.
- **Node Color Vent**: When rendering vent networks, what color air vents (entrances / exits) are visualized as.
- **Node Color Peek**: When rendering vent networks, what color \"peek\" ducts are visualized as, that you can see through but not exit.
- **Node Spawn Time**: When rendering vent networks, when nodes spawn in, how long it takes for them to reach full size.
- **Node Despawn Time**: When rendering vent networks, when nodes expire, how long it takes for them to shrink and disappear.

## License
All code in this project is distributed under the MIT License. Feel free to use, modify, and distribute as needed. That license can be found in **License.txt**. Attribution and licenses for all third party libraries and assets used in the creation of Ventrix Sync Disks can be found in **Attribution.txt**.