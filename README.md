# Idle Tower - Unity C# Prototype

## Concept

My idea was to make a idle game, but one that is more interactive, and te progression feels smooth. Player start with a simple tower, and a enemies spawning into the arena at a slow phase. As the tower kills more and more enemies, the player can add more towers, upgrade the existing ones, and increase the rate of which enemies spawn and the loot that is yielded

## 🎴 Cards

This is the core concept behind unlocking new towers and upgrades. As the player kills more and more enemies, they have a chance to drop cards. These cards can be upgrades, new towers, new enemy types. Once a player has unlocked a card they are able to use it to increase the rate at which they are gathering money.


![Cards](README_Images/cards.png)

Card are split up into the classic RPG rarity system:
- Common
- Uncommon
- Rare
- Epic
- Legendary

The chance of each respective rarity dropping are increasingly small. This adds some level of randomness to the progression.

![Cards](README_Images/cards2.png)
I used abstraction and scriptable objects to make new cards easy to implement. The upgradecards have a simple overridable ApplyUpgrade() function, that can be tweaked to fit new game logic. Later i plan to add more abstraction, so all cards are uniform


## Towers

![Towers](README_Images/towers.png)
This is the players way to kill enemies that spawn into the stage. All towers have their own respective stats, xp, and level. Once a tower levels up, you get a deck of 3 random upgrade cards you can choose from from the available cards that the player has unlocked. This adds another layer of randomness to the game.
Towers have different behaviors. Some target nearby enemies, some deploy traps, but they all have common stats making the upgrade system work without needing to implement new cards for every tower. 

![Towers](README_Images/towers2.png)
I also implemented these using abstraction, and defining my own enums for different behaviors. Then i used switch statements to keep it simple in the behavior part. 
Even though towers have different behaviors they can share stats like: Attack speed, Damage, Size, Range, and more. Using scriptable objects in unity, you can tweak the upgrades while still keeping the same behavior per tower.

