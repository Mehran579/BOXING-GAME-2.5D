# 2.5d
Simple 2.5D boxing simulator made in 3D environment
Characters from mixamo, environment by https://assetstore.unity.com/packages/3d/environments/boxing-arena-game-level-331688 
No ai-generated code used

-------Controls----------
A and D to go left and right 
SpaceBar to jump
Left mouse button to attack

--------Gameplay---------
Player - the player have character controller and player input components and mono behavior script player movement and player attack to handle movement and attack. the attack script contains the attack animation trigger as well as a coroutine to provide a cooldown between attacks, similarly the player movement script contains the boolean responsible to enable and disable walking animation
animations used - idle, walking, jump, attack, getting punched, dying(ragdoll state)

Enemy - similar to player enemy contains a character controller. the enemy behaviour is through a simple loop made using a coroutine. it's working - 
1) if player in attack range -
       attack retreat 
       retreat a few steps back (retreat time               is a random between 2 to 4 seconds)
2) else if player not in attack range -
       go up to the player
       repeat (1)
enemy animation - idle, walking, getting punched, dying (ragdoll state and other animations different from player)

UI - both characters have their own panel (canvas is set to screen overlay - camera) containing their name and health bar (which changes value through the health system script) and contains a win and lost screen. 

health system script - contains the health of the characters and the take damage function (which is called when punch collides with opponent), manages the ragdoll state through a public static bool and also manages the win and loose images

punch collider script - handles the actual hit detection and calls , enables the punch collider through animation event and similarly disabled it, calls the take damage function present in health system, triggers the "getting hit" animation as well as the taking damage soundfx 

Sound effects- attacks/punch of both players have different soundfx and in case of successful hit an "ouch" sound effect runs indicating the hit, all audio is played by the sound manager game object (containing the audio output component)