# WolfLight
> *A soul scattered. A memory fading. Will you piece together what was lost?*

## Story Pitch

In a world that has long forgotten its savior, you discover the ruins of an ancient shrine—a stone statue of a woman, frozen mid-sacrifice. Her story has been lost to time, her name erased from history.

Scattered across the realm are fourteen runes, each holding a fragment of her shattered soul. Collect them, and her memories will flood through you—visions of her final stand against the encroaching darkness, the ultimate price she paid, and the world she saved.

But time is running out. With each passing moment, more of her essence disperses into the void. Will you gather all fourteen fragments and restore the Forgotten Keeper? Or will she fade into myth, another nameless hero lost to eternity?

Your choice determines her fate—and yours.

---

## Features

### Story & Choices
- **14 Unique Lore Messages**: Piece together the mystery one rune at a time
- **Branching Endings**: 
  - **Good Ending**: Collect all 14 runes and witness the restoration
  - **Bad Ending**: Fail to complete the collection and face the consequences
- **Emotional Journey**: From curiosity to connection to urgent determination

### Visual Design
- **Pixel Art Aesthetic**: Charming 2D sprite-based graphics
- **Parallax Backgrounds**: Multi-layered scrolling skies and landscapes
- **Atmospheric Effects**: Glowing runes, particle effects, and smooth animations
- **Cutscenes**: Text-based story sequences with typewriter effects

---

## Controls

| Action | Key |
|--------|-----|
| Move Left | `A` |
| Move Right | `D` |
| Jump | `Space` |
| Wall Jump | `Space` (while on wall) |
| Interact | `E` (near runes, signs, shrine) |
| Skip Dialogue | `Space` (during text) |

---

## How to Play

1. **Explore the Realm**: Navigate platforms, avoid death pits, and search for hidden runes
2. **Collect Runes**: Press `E` to interact with glowing runes and read their lore
3. **Piece Together the Story**: Each rune reveals more about the Forgotten Keeper
4. **Reach the Shrine**: Find the ancient statue at the end of your journey
5. **Make Your Choice**: With 14 runes, restore her. With less... face the consequences.

---

## Technical Details

### Built With
- **Engine**: Unity 2022.3+ (Universal Render Pipeline)
- **Language**: C#
- **Input System**: Unity's New Input System
- **UI**: TextMeshPro for crisp text rendering
- **Audio**: Dynamic AudioManager with scene-based music switching

### Key Systems
- **Player Controller**: Smooth physics-based movement with coyote time and jump buffering
- **Dialogue Manager**: Typewriter text effects with automatic display/hide
- **Audio Manager**: Singleton pattern with persistent audio across scenes
- **Interaction System**: Interface-based design for extensible gameplay objects
- **Scene Management**: Seamless transitions between menu, gameplay, death, and endings

### Project Structure
` bash
Assets/
├── _Scenes/ # All game scenes
├── Scripts/
│ ├── Player/ # PlayerController
│ ├── Camera/ # CameraFollow with bounds
│ ├── Environment/ # DeathPit, Rune, Shrine, Signs
│ ├── UI/ # Menu managers, dialogue system
│ ├── Managers/ # AudioManager, singleton systems
│ └── Animations/ # Animation controllers
├── Sprites/ # All visual assets
├── Audio/
│ ├── Music/ # Background tracks
│ └── SFX/ # Sound effects
├── Prefabs/ # Reusable game objects
└── Tilemaps/ # Level design tiles
`

---

## Gameplay Features Deep Dive

### Movement Mechanics
- **Variable Jump Height**: Hold space for higher jumps, tap for shorter hops
- **Coyote Time**: Grace period for jumping after leaving ledges
- **Jump Buffering**: Queue jumps before landing for responsive controls
- **Wall Mechanics**: Slide down walls and launch off at angles
- **Fast Falling**: Press down in midair for quicker descent

### Progression System
- **Rune Counter UI**: Always visible, tracks your collection progress (X/14)
- **Memory Fragments**: Each rune adds to your understanding of the story
- **Permanent Collection**: Runes stay collected even after death
- **Shrine Interaction**: Final checkpoint that judges your completion

### Death & Respawn
- **Death Animation**: Visual feedback when falling into pits
- **Death Sound**: Audio cue for failure
- **Quick Restart**: Instant level reload or game over screen
- **Persistent Progress**: Collected runes remain found

---

## Game Structure

### Scenes
1. **MainMenu**: Start screen with settings and credits
2. **MainLevel**: Primary gameplay area with all 14 runes
3. **GameOver**: Death screen with retry options
4. **GoodEnding**: Cinematic conclusion (all runes collected)
5. **BadEnding**: Alternate conclusion (incomplete collection)

### Victory Conditions
- **Good Ending**: Collect all 14 runes + interact with shrine
- **Bad Ending**: Interact with shrine without all runes
- **Death**: Fall into death pits → Game Over screen

---

## Audio Design

### Music Tracks
- **Menu Theme**: Ethereal with layered elvish singing
- **Level Theme**: Mysterious exploration music
- **Game Over Theme**: Somber, reflective
- **Good Ending Theme**: Uplifting, hopeful resolution
- **Bad Ending Theme**: Melancholic, regretful

### Sound Effects
- **Player**: Footsteps (looping), jump, land, wall slide, death
- **Environment**: Rune collection, shrine activation, ambient effects
- **UI**: Button clicks, menu navigation

---

## Story Themes

### Central Questions
- What defines a hero? 
- Is a forgotten savior still a savior?
- Do our memories of the past matter to the present?
- What price would you pay to be remembered?

### Emotional Journey
1. **Curiosity** → Who is this statue?
2. **Discovery** → What happened here?
3. **Understanding** → She sacrificed everything...
4. **Connection** → Her story matters to me
5. **Determination** → I won't let her be forgotten
6. **Resolution** → The choice is made

---

## Development Notes

### Player Experience Goals
- **10-20 minutes** for first playthrough
- **Replayability**: Try for different ending, speedrun, or perfect collection
- **Emotional Impact**: Story should resonate and motivate completion
- **Fair Challenge**: Difficult but never frustrating platforming

### Design Philosophy
- **Show, Don't Tell**: Environmental storytelling over exposition
- **Player Agency**: Choice matters - completion is earned, not given
- **Respect Player Time**: Quick restarts, no padding
- **Polish Over Features**: Core mechanics refined before expansion

---

## Future Enhancements (Potential)

- [ ] Additional levels with more lore
- [ ] Secret runes for extended lore
- [ ] Voice acting for key story moments
- [ ] Hidden areas and collectibles
- [ ] Change game font
- [ ] Pause menu
- [ ] Fix text size for different aspect ratios
- [ ] Add W for jump and arrow key movement
- [ ] Controller support
- [ ] Add controls panel to see controls (maybe in the PauseMenu)
- [ ] Mapping system in PauseMenu or mapped to a button
- [ ] Cover the sprite gaps

---

## Installation

1. Download the latest release
2. Extract the ZIP file
3. Run `WolfLight.exe`
4. Enjoy!

---

## Credits

### Development
- **Game Design & Programming**: Saf
- **Story & Writing**: Sf
- **Level Design**: Saf
### Assets
- **Music**: FreeSound and Pixabay
- **Sound Effects**: FreeSound and Pixabay
- **Sprite Art**: itch.io

---

## Tools & Frameworks

Unity Engine: Game development platform

TextMeshPro: Text rendering

Input System: Unity's new input system

---

## Play the Game

[Itch.io](https://bluekillspop.itch.io/wolflight)

---

> The Forgotten Keeper awaits. Will you restore what was lost?
