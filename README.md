# LD38
Ludum Dare 38 Compo

## Gravitational Collapse
I've decided to do a Collapse-style game with a center point of gravity. You're trying to make the world a smaller place!

## Solving center-point gravity
Adding force to the center of the screen works okay, but it causes a kind of orbital sway, the collective mass of all the terrain blobs never settles onto a single point.  

1. Determine center of mass for all bodies. Center camera over center of mass.
2. Reduce force as center point is reached.
3. Use a larger mass
4. Reduce mass as an object approaches center point.
5. CHECK Add a linear drag.  This seems to work well, but what side effects will this have?? Let's find out! WOOOOOO1

## Solving New Wobble
If you have a LOT of balls, there's a general wobble to the whole thing. Especially if you tab away and back! Things to try:

1. Apply acceleration to the force
2. Reduce mass. Although this also slows down the balls so you'll have to increase force.

## Solving contiguousness
How do I detect if two terrains are touching each other?

1. Add a second, larger collider
2. CHECK Use CollisionEnter and Exit to track who is touching what. This is what I'm currently using, however it has no room for error. Objects that may appear in contact because of overlap will not be actually contacting. Ultimately I'm going to have to resort to something like #1 to fix this issue.

## Order of terrain

-1 forest high  
-2 forest low
-3 grass high
-4 grass low
-5 savannah high
-6 svannah low
-7 desert high
-8 desert low
-9 tundra high
-10 tundra low
-11 water high
-12 water low

## TO DO
[X] Fix issue of very close but not touching contacts not counting as contiguous.
[X] SFX
[X] Main men
[X] Space bg
[X] UI
[X] Game targets: planet -> dwarf -> centaur -> plutoid -> asteroid -> space garbage
[X] Messge bar!
[X] Click to advance
[X] Tracker bar isn't always visible!
[X] Cameraman is over zooming
[X] Bomb display
[X] Random levels
[X] Clicking can sometimes result in adjacent object deleted Only happens close to edges
[X] Random adjustment to piece placement to avoid perfectly symmetrical lockups
[X] Main levels/Tutorial
[X] Music
[] Particle effects
[] Random messages when you bomb
[] Level creation effect
[] Screen shake on big collide
[] Q return to main menu
[] Drop bomb?
[] Additional mechanics
[] Creatures/add-ons
[] If restart on random level, use same random level?
[] Volcano
[] Cache instantiated gameobjects for faster replay


## SFX List
[X] Water: Splashes, drop
[X] Savannah: Rustle, wind
[X] Forest: creak, wind, leaves
[X] Tundra: woosh, ice crack
[X] Desert: Shake, granules pelting
[X] Grass: Thud, rustle
[X] Click
[X] New planet sound
[] Failure
[] Victory
