# LD38
Ludum Dare 38 Compo

## Gravitational Collapse
I've decided to do a Collapse-style game with a center point of gravity. You're trying to make the world a smaller place!

## Notes to self
Adding force to the center of the screen works okay, but it causes a kind of orbital sway, the collective mass of all the terrain blobs never settles onto a single point.  

1. Determine center of mass for all bodies. Center camera over center of mass.
2. Reduce force as center point is reached.
3. Use a larger mass
4. Reduce mass as an object approaches center point.
5. Add a linear drag.  This seems to work well, but what side effects will this have?? Let's find out! WOOOOOO1