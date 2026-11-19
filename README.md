# A06 - Escape Room
**Overview:**
- Puzzle 1: Find the exit_card in the room and try to swipe it for exit. You'll soon find that the exit card **Does Not** work!
- Puzzle 2: Solve the Anagram Puzzle. The correct answer is "English".
- Puzzle 3: Look for some hint for the password around the classroom. Pick up the glowing schedule and discover that the code is 11:30.
- Puzzle 4: Listen to the music and solve the last puzzle! This will open the door and you can access your own copy of GTA VI!!

**Controls:** (default bindings of the XR Interaction Toolkit starter rig):
- Point the ray at a tile or button and squeeze the grip to select it.
- Squeeze the grip to pick up the exit card, the schedule and the game card.
- Push the right thumbstick forward and release to teleport.
- Use the left thumbstick to walk (this can be dizzy, we recommend doing teleportation).
- Use the right thumbstick to adjust camera angle (or just simply turn around the head).

**Rubric Elements**:
1) We used shader graphs to highlight the schedule and exitcard elements. We used a particle system to highlight the ID scanner.
2) We have a leaderboard displaying the 5 highest scores with an option to select your initials.
3) Multiple Senses: Visual elements, victory music, musical matching in puzzle 4, vibration haptics when pressing puzzle buttons.
4) Timer: A 10 minute timer. When it ends, the player is no longer able to open the door via puzzles, but is able to restart.

**Outside resources**
- We will be importing [Principal Evil FREE – Abandoned Horror School Classroom](https://assetstore.unity.com/packages/3d/environments/principal-evil-free-abandoned-horror-school-classroom-422958) from Unity asset store for our scene setup.
- Class schedule image (`Assets/Materials/Class Schedule Document in Pink Blue Simple Style.png`): made with a Canva template.
- End music (`Assets/music/590-EndMusic.mp3`): record directly by phone from a youtube video.
- Game card image (`Assets/Materials/game.jpg`): googled picture online.
- Outside ground texture (`Assets/outsideground_material.jpg`): imported from pexels.com.
- Melody puzzle notes: generated in code, no audio files.
- Engine and packages: Unity 6000.5.9f1, Universal Render Pipeline 17.5.0, XR Interaction Toolkit 3.5.1 (including its starter assets and simulator sample), OpenXR Plugin 1.19.0, Meta XR All-in-One SDK 209.0.0, Input System 1.20.0, TextMesh Pro with the Liberation Sans font.

**AI Assistance**
- AI agents primarily helped with scene setup, such as adding teleportation functionality to all 60 tiles.
- Configured settings for different devices.
- Generated musical note tones.
- Assisted with edge-case testing to identify potential issues.