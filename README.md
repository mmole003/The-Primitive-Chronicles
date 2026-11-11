Task2
The Primitive Chronicles

GAME 360 - Madelyn Molen

How to play
WASD = Up Down Left Right Space = Jump Left Click = Swing Right Click = Block

Singleton
Class: PlayerHealth

What it holds: Player health data, registers if player has been hit, registers if player is dead, calls Game Over menu

Why it's a singleton:

Observer
Event: PlayerHealth.OnHealthChanged

Listener 1: PlayerTakeDamage

Listener 2: PlayerHealthUI

Singleton 2
Class: GemManager

What it holds: Score data for number of gems player has interacted with, debug in console to check if code and interaction are working

Why it's a singleton: Every gem can access the manager instead of having individual code, that code then manages the integer number for how many gems have been collected.

Observer
Event: GemManager.OnGemsChanged

Listener 1: GEMUI

Help I used
I reused a lot of the code from within class and built upon it, mainly the gem collection using the coin collectables we made and the score code for my "gem collected" UI. I also reused code from the Unity Essentials course project that comes with unity, specifically their 3D essentials. The code borrowed was used for the player movement.

I also used a variety of tutorials

https://www.youtube.com/watch?v=aNZw588BQBo&t=112s ^Used for making my melee combat though had to be tweaked for a 3rd person view

https://www.youtube.com/playlist?list=PLGUw8UNswJEOv8c5ZcoHarbON6mIEUFBC ^Much of my code came from this series in regards to the enemy AI and behaviors they can cycle through such as following, attacking, searching, and patrolling.

https://www.youtube.com/watch?v=NY_fzd8g5MU&t=509s ^This tutorial was to better understand observer patterns and helped when making my observer pattern code for my player health.

I did use Github copilot AI since I have a github subscription, this was used both to help with syntax errors or solving mistakes such as misplacing my C# scripts in the wrong location. I did use it to help with my player block script since I couldn't get it to work on my own when translating the controls from 1st to 3rd person.

TASK 3
## State machines

Game states: Game Manager - Playing, Paused, GameOver

How it changes: Playing is the default state but will move to the Pause State when "esc" is pressed and then return back to the Playing State when either the return button in the ui is pressed or "esc" is pressed again. The game transitions to GameOver state when the player's hp hits zero. 

Character states: Player Controller - Walking, Attacking, Dead, Idle

How it changes: Walking is invoked when W or S keys are pressed then the Attacking state is transitioned to when pressing left mouse button. The Dead state is transitioned to when the player's hp reaches zero. Idle as a state is called when the player is not pressing wasd. 


EnemyStateMachine - Search, Patrol, Attack, Base <- extra state machine I had already made that controls enemy Ai

## Help I used

I used the same first person shooter tutorial linked above because he shows how to make state machines. As well as chatgpt and the microsoft visual studio ai to help me find syntax errors within my code.
