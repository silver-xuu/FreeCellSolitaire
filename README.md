# FreeCellSolitaire
Free Cell Solitaire Game

Started time: Nov 21 2020 6:30pm
End time: Nov 22 2020 6:00pm

This project is a solitaire game done by Unity engine. It follows the basic rule of [FreeCell](https://en.wikipedia.org/wiki/FreeCell).
There are 52 cards dealt into 8 cascades at the bottom. On the top there is 4 free cells and foundations. 
Players needs to move all the cards into the foundation pile.

This project is done within 24 hours, the project used a lot of reference from my previous project which is also a solitaire game that's done within 24 hours: https://silverrrrr@bitbucket.org/silverrrrr/solitaire.git

## Basic Functionalities

This project follows basic FreeCell game rules and thus have following basic functionalities:

1. Cards are randomly dealt into the cascades
2. Players can drag and move a card into legal location such as open cells, empty cascades, or open foundations
3. Players can also drag and move a card or a tableau with another card or tableau to form a longer tableau
4. If all the cards is moved into foundation pile, player wins

## Additional features

The game also has some additional features:

1. There is a time counter in the game counts how long the player spends each time
2. Time is showed on the top of the game screen
3. The game also saves the time which player spend on current game into a JSON file, and record the fastest time that players spent on the game
4. The result with best record and current time spent will be showed when player finished the game
5. Player can redo/undo the actions they made
6. On the bottom right of the screen, the player can see a log of what actions they have done

## Problems/Issues fixed from previous project

1. better dragging and selecting performance
2. tableau forms before the game begins will be recognized by the system
3. cards display issue fixed

## Problems/Issues

1. Selecting and draging cards sometimes is still not perfect
2. Ending page - due to limit of time, the ending page wasn't polished and it might cause potential issue

## Future Improvement

If more time can be spent on this project, here are some changes I would love to have:


```
	1. Better Polished UI
		Right now the UI is too simple
	
	2. More Rules/Difficulties
		The game currently only follows the very basic rule of FreeCells, it will be more interesting if more rules can be implemented
    
  3. Save Progress
    Everytime the game launches, a new game will begin. It will be very helpful to have the ability to save user progress, so that players can choose to start from their previous game or start a new game.
  
  4. Restart current game
     Sometimes instead of redoing the steps, start over may be more helpful.
     
  5. Sound Effect
     Adding sound effects may increase the immersive feeling of the game and make it more fun
```
