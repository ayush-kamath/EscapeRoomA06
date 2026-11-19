# A06 - Escape Room
- We will be importing [Principal Evil FREE – Abandoned Horror School Classroom](https://assetstore.unity.com/packages/3d/environments/principal-evil-free-abandoned-horror-school-classroom-422958) from Unity asset store for our scene setup.
- Puzzle 1: Find the exit_card in the room and try to swipe it for exit. You'll soon find that the exit card **Does Not** work!
- Puzzle 2: Solve the Anagram Puzzle. The correct answer is "English".
- Puzzle 3: Look for some hint for the password around the classroom. Pick up the glowing schedule and discover that the code is 11:30.
- Puzzle 4: Listen to the music and solve the last puzzle! This will open the door and you can access your own copy of GTA VI!!

Rubric Elements:
1) We used shader graphs to highlight the schedule and exitcard elements. We used a particle system to highlight the ID scanner.
2) We have a leaderboard displaying the 5 highest scores with an option to select your initials.
3) Multiple Senses: Visual elements, victory music, musical matching in puzzle 4, vibration haptics when pressing puzzle buttons.
4) Timer: A 10 minute timer. When it ends, the player is no longer able to open the door via puzzles, but is able to restart.

## Environment: what came from the asset pack and what we built
The classroom itself comes from the Principal Evil FREE asset pack: walls, floor, ceiling, windows, door, blackboard, desks, shelves and the other furniture and props.

Everything the player interacts with was added and set up by us on top of that room:
- Card reader, its trigger zone and label, and the exit card with our glow shader graph (`Assets/shaders/thegoldenkey.shadergraph`).
- Spark particle system on the card reader.
- Anagram tiles, the number keypad with its four digit display, and the note tiles for the melody puzzle, all placed on the blackboard.
- The class schedule prop that gives the password hint, with the same glow shader.
- Timer text, leaderboard, initials entry panel and restart button.
- The game card prize outside the door.
- The ground outside the classroom, and teleport areas on both the outside ground and the classroom floor.
- A script on the pack's door so it swings open once all four puzzles are solved.
- All game logic in `Assets/Scripts`.

## Running the game
Requirements: Unity 6000.5.9f1 with Android Build Support, and a Meta Quest headset in developer mode.

1. Clone the repository and open the folder in Unity Hub. The first import takes a few minutes.
2. Open the scene `Assets/Escaperoom.unity`. It is the only scene enabled in the build settings.
3. To play on the headset: File > Build Profiles, switch to Android, connect the Quest and choose Build And Run.
4. To try it without a headset: press Play in the editor. The scene has an XR Interaction Simulator that is only used in the editor and is left out of Quest builds.

Controls (default bindings of the XR Interaction Toolkit starter rig):
- Point the ray at a tile or button and squeeze the grip to select it.
- Squeeze the grip to pick up the exit card, the schedule and the game card.
- Push the right thumbstick forward and release to teleport.
- Use the left thumbstick to walk.

The timer length is `Timer Duration` on the `Gamemanager` object, in seconds (600 by default).

## Outside resources
- Classroom environment: [Principal Evil FREE – Abandoned Horror School Classroom](https://assetstore.unity.com/packages/3d/environments/principal-evil-free-abandoned-horror-school-classroom-422958), Unity Asset Store.
- Class schedule image (`Assets/Materials/Class Schedule Document in Pink Blue Simple Style.png`): made with a Canva template.
- End music (`Assets/music/590-EndMusic.mp3`): source to be added.
- Game card image (`Assets/Materials/game.jpg`): source to be added.
- Outside ground texture (`Assets/outsideground_material.jpg`): source to be added.
- Melody puzzle notes: generated in code, no audio files.
- Engine and packages: Unity 6000.5.9f1, Universal Render Pipeline 17.5.0, XR Interaction Toolkit 3.5.1 (including its starter assets and simulator sample), OpenXR Plugin 1.19.0, Meta XR All-in-One SDK 209.0.0, Input System 1.20.0, TextMesh Pro with the Liberation Sans font.

## AI assistance
An AI coding assistant (Claude) supported the following parts of puzzle 4 and the review before submission:
- Repetitive scene setup: duplicating the existing keypad tiles and arranging them on the board for the note tiles.
- Boilerplate code: generating the note tones, subscribing to the tile select events and adding null checks around script references.
- Bug review and testing: reading through the game flow for mistakes (for example the timer was still on a 30 second test value) and running a simulated playthrough of all four puzzles in the editor.
