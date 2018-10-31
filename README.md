# Pin-Games

## Summary
This project contains two games, which were created in Unity. The GameObjects in Unity incorporate C# scripts to function correctly and make the game's AI come to life. The first game is a simple game of bowling where you control a ball to knock over pins. The second game is a more complex game where you escape from pins that chase you, by navigating along platforms layed out like a maze.

## Play the Games
In order to play the games, first you need to make sure you have Unity installed on your computer. You can then navigate to:
`Pin-Games-master\Assets\Games`
and open up `1.1.unity` or `1.2.unity` (doesn't matter). When Unity is done loading, go to File -> Build And Run. Create a file (e.g. "build") and build to there. Then hit "Play!" and you're good to go! Alternatively, you could play within the Unity engine and mess around with settings if that's your desire.

## Game 1: Bowling

### Controls
Up/Left/Down/Right or W/A/S/D for moving forward, left, backwards, and right. 

Q to quit the game. 

R to restart. 

S/Z for slow mode. 

1/2 to load game 1 or game 2.

### Gameplay
You control a bowling ball on a platform leading to a bunch of pins. Your goal is to knock all the pins down within the alloted 10 seconds. Doing so will earn you a victory. Failing to do so will cause you to fail. You will want to make sure not to fall off the platform or fly off the map.

## Game 2: Pin Chase

### Controls
Left/Right or A/D for turning left and right. 

Q to quit the game. 

R to restart. 

S/Z for slow mode. 

1/2 to load game 1 or game 2.

### Gameplay
You are to traverse the path and attempt to make it to the end without getting caught by the horde. You will want to turn at paths, but need to be precise with the timing. If you turn before you reach a platform where turning is possible, or if you turn after the midpoint of a platform where turning is possible, you will suffer a speed penalty. The pins will follow you on the path, but may become erratic at any given time, flying off the path and making the game harder and more terrifying.

### Additional Info
The pins have a mode called "Woke Mode". After some time, the bunnies may become enraged and empowered, going off the path to chase you with a fiery aura. This makes the mode much more interesting and challenging. The cameras linger a bit behind the ball and readjusts its angles a lot more snappy than the implementation. The first camera feature gives the feeling of traveling fast and coming to a quick halt, and the second feature makes it easier to do sharp turns and less awkward. There are many background elements.

### Technicalities
The platforms are generated at the game's runtime through an array, but that same array is needed to determine the player's coordinates. The difficult part is getting the chasers to follow the player through a queue, while also maintaining its middle position on the platforms. Many options were explored, but I believe the hybrid option of path following AI and impulsive following AI offered the most exciting gameplay.
