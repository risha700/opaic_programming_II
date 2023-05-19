
# OPAIC Programing 2 - Check point 3


---
## Problem Domain
    A simple slot machine game
---
Assignment As Problem: 
   
    TASK
    Your game should provide a Spin button. The user is given $100 at the start of the game.
    It costs $10 for each click of the Spin button. When the user clicks this Spin button,
    a random sequence of 20 images is displayed in each of the three locations.
    Each image should be a tree, a coyote, or a sheep (these .bmp files are available on the Moodle Resources Tab).
    If all three images are the same, the player wins $50. Your game should keep track of the player’s current total money.
    Your game should also tell the player, at each spin, if he has won or lost.
---

# Class Diagram

![](./Models/generatedPuml/Spinner\ Diagram.png)

---
# Build and run

```
   dotnet restore

   dotnet run 

```

### Mentor: Prof. Barry Dowdeswell