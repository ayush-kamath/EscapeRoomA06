# A06 - Escape Room
- We will be importing [Principal Evil FREE – Abandoned Horror School Classroom](https://assetstore.unity.com/packages/3d/environments/principal-evil-free-abandoned-horror-school-classroom-422958) from Unity asset store for our scene setup.
- Puzzle 1: Find the exit_card in the room and try to swipe it for exit.
- Puzzle 2: Solve the Anagraph Puzzle
- Puzzle 3: Look for some hint for the password around the classroom
- Puzzle 4: Listen to the music and solve the last puzzle!

## AI assistance
An AI coding assistant (Claude) supported the following parts of puzzle 4 and the review before submission:
- Repetitive scene setup: duplicating the existing keypad tiles and arranging them on the board for the note tiles.
- Boilerplate code: generating the note tones, subscribing to the tile select events and adding null checks around script references.
- Bug review and testing: reading through the game flow for mistakes (for example the timer was still on a 30 second test value) and running a simulated playthrough of all four puzzles in the editor.
