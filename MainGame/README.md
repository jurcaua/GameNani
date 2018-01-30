View devpost link (with pictures) at: https://devpost.com/software/thacks2 

# **Game Nani**

A data-oriented dev tool for game developers, by game developers. 

We wanted to change the way user testing is done for gaming, to remove the ambiguity. So we created _Game Nani_.

## What do we keep track of?
**Main Features:**

_Player Focus Tracking_: Know every object the user looks at, with information on time periods, average and total rates, and event tracking.

_Player Key Press Tracking_: Key Press tracking on any key, touch, and auxiliary controller input, with frequency, and hold times.

## How do you see the data?
View comprehensive graphs and data logs on _every_ tagged GameObject and key press, for _every single_ play session. View both individual and aggregate reports on the collected data.

## How do I use it?
We created **two** apps to showcase the power of Game Nani.

### 1. MainGame
- In **ANY** Unity game project, drag in the UnityPackage we created.
- Drag the "CameraNani" prefab in Assets/Prefabs/, onto the main camera; configure any of the provided settings.
- Set any objects you wish to track to the "Observable" layer, or press 'Q' to set session-local observable objects.
- Play.

### 2. Analytics
- **AFTER** at least one play session with the MainGame, start the Analytics app.
- Navigate the app to discover comprehensive statistics on every play session.

## What's next?
**Features coming soon:**
- Muse support for stress/calm levels during play sessions.
- A deeper and more simple event system.
- Audio spectrum data for intense play sessions.
- More in-depth for platforming-oriented games.
- Plenty of export options, Excel, CSV, Google Drive, etc.
