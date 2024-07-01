# Huntly

The Definitive Final Fantasy Hunt Train Assistant for XIVLauncher.

## Supports
- Scouting Recorder
    - Automatically Scout Marks
    - [Coming Soon] Multi-Scout Capability via WebSocket Server
      - This does NOT share Marks globally. This is only used for scouting and requires a group code.
    - Imports & Exports of Marks
      - Want to Integrate? Request a Import or Export Profile via GitHub Issues.
- [Coming Soon] Conductor Tools
    - Vibe & Bullet Train Modes
    - Customizable Messages
    - Automatic Advance on Mark Death

There is even more to explore! This is still an Open Beta and there is a lot of advancements that can be made. Please use the issue tracker for problems or requested features.

There are not many choices when it comes to tools like this currently; even less that are open source and auditable. This was created as a passion project from my time in Lamia as a conductor that I want to share with all the wonderful people who pour so much time into these events.

## Instances & New Expansions (Dawntrail)

New content is, usually, instanced. There is not really a "good" way to handle this. Please scout one region at a time, export, and manually rename the Marks.

Eg: `Urqopacha : Mad Maguey : 19.3 : 14.0` would be updated to `Urqopacha : Mad Maguey (Instance 1) : 19.3 : 14.0`.

## Installation

1. Navigate to XIVLauncher Settings.
    - Protip! Do `/xlsettings` to reach this via chat.
2. Navigate to the "Experimental" Tab.
3. Scroll to 'Custom Plugin Repositories".
4. Add (below) and click the "+" to the far right.
    - `https://raw.githubusercontent.com/xCykrix/Huntly/main/repo.json`
5. Navigate to XIVLauncher Plugins.
    - Protip! Do `/xlplugins` to reach this via chat.
6. Search for "Huntly" by xCykrix.
7. Install the Plugin!

You can now do a main command in chat to open the Main Window. I recommend doing this in Alliance Chat or Tell to avoid accidental bleeds of commands.

- /scout

## Development

This is designed for Development in Linux Ubuntu. Windows Visual Studio (Code) is YMMV. Do not add changes specific to Windows.

1. Create Folder `DalamudDistribution`.
2. Copy ALL FILES from `AppData\Roaming\XIVLauncher\addon\Hooks\dev` to `DalamudDistribution`.
