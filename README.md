## Developer & Contributions
Peace Anya (Game Designer) </br>
Jevon (Game Programmer) </br>
Jason (Game Programmer) </br>
Sambalcapcay (Game Artist)

## About
**[Game Title]** is a 2D narrative delivery game where players take on the role of a mail carrier. Walk through the world, pick up letters, manage what you carry in your backpack, and deliver each piece of mail to the right mailbox. Return to the office when the day is over and see how you did. Every letter has its own details, so paying attention to who it's for matters.

<br>

## Key Features

**Mail Delivery Loop**: Pick up letters around the world, carry them in your backpack, and drop them off at the correct mailboxes before the day ends.

**Backpack & Inventory System**: Manage collected items and letters in a slot-based backpack, with item descriptions you can inspect to understand what you're carrying.

**Interaction System**: A flexible interaction system lets the player interact with mailboxes, mail pickups, and the office through one shared interface.

**Day Structure & Feedback**: Notifications keep you informed during play, the office marks the end of your route, and an end-of-day screen wraps up each shift. A tutorial helps new players learn the ropes.

**Atmospheric Visuals**: Parallax scrolling backgrounds and smooth scene fades give the world depth and polish.

<table>
<tr>
<td align="center" width="50%">
<strong>Mail Pickup & Backpack</strong><br>
Collect letters and manage your backpack
<img width="100%" alt="gif1" src="PATH_TO_GIF_1">
</td>
<td align="center" width="50%">
<strong>Mailbox Delivery</strong><br>
Deliver letters to the right mailbox
<img width="100%" alt="gif2" src="PATH_TO_GIF_2">
</td>
</tr>
</table>

## Scene Flow
```mermaid
flowchart LR
  mm[Main Menu]
  tut[Tutorial]
  gp[Gameplay]
  office[Office]
  eod[End of Day]

  mm -- "Start Game" --> tut
  tut -- "Finish Tutorial" --> gp
  gp -- "Enter Office" --> office
  office -- "Finish Route" --> eod
  eod -- "Next Day" --> gp
  eod -- "Exit" --> mm
```

## Layer / Module Design
```mermaid
---
config:
  theme: neutral
  look: neo
---
graph TD
    subgraph "Core Systems"
        SL[Scene Loader]
        SF[Scene Fader]
        EG[Exit Game]
    end

    subgraph "Player Systems"
        PM[Player Movement]
        PI[Player Interactor]
    end

    subgraph "Interaction"
        II[IInteractable]
        Mailbox[Mailbox Interact]
        Pickup[Mail Pick Up]
        Office[Office Trigger]
    end

    subgraph "Inventory & Data"
        Backpack[Backpack Manager]
        Item[Item Data]
        Letter[Letter Data]
    end

    subgraph "UI Systems"
        Slot[Slot UI]
        ItemDesc[Item Description UI]
        Notif[Notification UI]
        Tutorial[Tutorial UI]
        EOD[End Of Day UI]
    end

    subgraph "Environment"
        Parallax[Parallax Layer]
    end

    PM --> PI
    PI --> II
    II --> Mailbox
    II --> Pickup
    II --> Office

    Pickup --> Backpack
    Mailbox --> Backpack
    Backpack --> Item
    Backpack --> Letter
    Backpack --> Slot
    Slot --> ItemDesc

    Pickup --> Notif
    Mailbox --> Notif
    Office --> EOD
    EOD --> SL
    Office --> SF
    SF --> SL
    Tutorial --> SL
    EG --> SL

    classDef coreStyle fill:#e1f5fe,stroke:#01579b,stroke-width:2px
    classDef playerStyle fill:#e8f5e8,stroke:#1b5e20,stroke-width:2px
    classDef interactStyle fill:#fff3e0,stroke:#e65100,stroke-width:2px
    classDef dataStyle fill:#ffebee,stroke:#b71c1c,stroke-width:2px
    classDef uiStyle fill:#f3e5f5,stroke:#4a148c,stroke-width:2px
    classDef envStyle fill:#eceff1,stroke:#37474f,stroke-width:2px

    class SL,SF,EG coreStyle
    class PM,PI playerStyle
    class II,Mailbox,Pickup,Office interactStyle
    class Backpack,Item,Letter dataStyle
    class Slot,ItemDesc,Notif,Tutorial,EOD uiStyle
    class Parallax envStyle
```

## Modules and Features

The delivery gameplay, backpack inventory, interaction system, and UI are powered by a modular Unity C# scripting system.

| 📂 Name | 🎬 Scene | 📋 Responsibility |
|---------|----------|-------------------|
| **PlayerMovement** | **Gameplay** | - Handle player movement input<br/>- Control speed and facing direction<br/>- Support animation states |
| **PlayerInteractor** | **Gameplay** | - Detect nearby interactable objects<br/>- Trigger interaction on player input<br/>- Work with any object that implements IInteractable |
| **IInteractable** | **Gameplay** | - Interface for interactable objects<br/>- Let mailboxes, pickups, and the office share one interaction flow |
| **MailPickUp** | **Gameplay** | - Let the player collect letters from the world<br/>- Add picked up mail to the backpack<br/>- Trigger pickup notifications |
| **MailboxInteract** | **Gameplay** | - Handle delivering letters to mailboxes<br/>- Check the letter against the mailbox<br/>- Remove delivered letters from the backpack |
| **OfficeTrigger** | **Gameplay** | - Detect when the player reaches the office<br/>- Start the end of day sequence |
| **BackpackManager** | **Gameplay** | - Store and manage collected items and letters<br/>- Add and remove items<br/>- Keep the backpack UI in sync |
| **ItemData** | **Data** | - Define item properties such as name, icon, and description |
| **LetterData** | **Data** | - Define letter properties such as sender, recipient, and contents |
| **SlotUI** | **Gameplay** | - Display a single backpack slot<br/>- Show item icon and handle selection |
| **ItemDescriptionUI** | **Gameplay** | - Show details of the selected item or letter |
| **NotificationUI** | **Gameplay** | - Display short messages to the player<br/>- Give feedback on pickups and deliveries |
| **TutorialUI** | **Tutorial** | - Guide new players through the controls and delivery loop |
| **EndOfDayUI** | **End of Day** | - Show the end of day summary<br/>- Handle continue and exit options |
| **SceneLoader** | **All Scenes** | - Load and switch between scenes |
| **SceneFader** | **All Scenes** | - Fade the screen in and out<br/>- Provide smooth scene transitions |
| **ExitGame** | **Main Menu** | - Quit the application |
| **ParallaxLayer** | **Gameplay** | - Scroll background layers at different speeds<br/>- Create depth in the scene |

<br>

## Game Flow Chart
```mermaid
---
config:
  theme: redux
  look: neo
---
flowchart TD
  start([Game Start])
  start --> tutorial[Tutorial]
  tutorial --> move{Player Input}

  move -->|A / D| walk[Move Character]
  move -->|Interact Key| interact{Interactable Nearby?}
  move -->|Backpack Key| bag[Open Backpack]

  interact -->|Mail Pickup| pickup[Pick Up Letter]
  interact -->|Mailbox| deliver{Correct Mailbox?}
  interact -->|Office| office[Enter Office]
  interact -->|None| move

  pickup --> addbag[Add to Backpack]
  addbag --> notifyPick[Show Notification]
  notifyPick --> move

  deliver -->|Yes| remove[Remove Letter from Backpack]
  deliver -->|No| wrong[Show Notification]
  remove --> notifyDeliver[Show Notification]
  notifyDeliver --> move
  wrong --> move

  bag --> slot[Select Slot]
  slot --> desc[Show Item Description]
  desc --> move

  office --> fade[Scene Fade Out]
  fade --> eod[End of Day Summary]
  eod --> next{Continue?}
  next -->|Yes| start
  next -->|No| exit[Exit to Main Menu]
```

<br>

## Play The Game
<a href="https://exaltedartix.itch.io/wordbearer">Play Now</a>

<br>

## Installation & Setup
1. Clone this repository
2. Open the project in Unity (version used in your project recommended)
3. Open the main menu scene
4. Press Play to start testing

## Controls
- **A / D or Arrow Keys** - Movement
- **E** - Interact (pick up mail, deliver to mailbox, enter office)
- **Backpack Key** - Open or close the backpack
- **Escape** - Pause / Exit

## Credits
Game developed as a personal project by team.

- Engine : Unity
- Assets : Clip Studio Paint & Free assets itch.io
