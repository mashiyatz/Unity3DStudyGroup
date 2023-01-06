# Unity3D Study Group Source Code
Resources for the ITP 2023 Unity3D Winter Study Group, hosted by Mashi! Feel free to use and adapt any code written here for personal use.

## Getting Started
These examples were made in Unity3D 2021.3, but should work with any recent or LTS version of Unity. I recommend installing Visual Studio as well, as it comes with Unity integration out of the box. If you're completely new to Unity or scripting in Unity, try following the steps below to get started. If you're already familiar with Unity and have some experience using a game engine, you can import this project as a .unitypackage and try to decipher my code yourself - my apologies in advance for my lack of commenting.

### The Unity Interface
When you open a new Unity project, you should see something like the image below. 

![image](https://user-images.githubusercontent.com/43973044/211072560-bcc153e1-73e7-436d-b1c1-3667f660fe94.png)

On the left is the Hierarchy window, which organizes all of the game objects you currently have in your Scene. The large window in the center is the Scene window, in which you're able to manipulate different objects in space as needed. You can press W, E, R to toggle between changing a selected object's position, rotation, and scale. When an object is selected in the Hierarchy and your cursor is hovering over the Scene window, you can press the F key to zoom into that object - great for when you're trying to look for different objects in a busy Scene. The window on the right is the Inspector, which shows components attached to the selected game object - in this case, we're looking at the components attached to the Main Camera object, which includes a Camera component, Audio Listener, and Transform. 

On the bottom is the Project window, which shows all of your project's assets (including Scenes) and installed packages. Next to the tab labeled Project is Console - the Console window will display errors or warnings when you're running your project. You can also write directly to the console as needed. You can also see a tab labeled Game next to Scene on the center window. The Game window shows what the player sees when they're actually playing the game, generally determined by how the Scene's camera is positioned. Unity will switch to this window automatically when you press the play button on top. 

You can arrange these windows however you like by dragging and dropping their tabs. I personally like seeing both the Scene and Game objects at the same time, so I arrange my windows like in the image below. If there's a layout you like you can save it by pressing the dropdown (Labeled either "Default" or "Layout") on the top right corner and selecting Save Layout, then you can switch to that layout when you start a new project like I have.   

![image](https://user-images.githubusercontent.com/43973044/211072620-a3b5e8b3-a71c-49f8-aaa5-009731d1614d.png)

### Your First Script: Move a Cube

Right click on the Hierarchy window and you'll see a menu of actions as well as things you can create. Go to 3D Objects and select Cube, which will generate a cube in your Scene right away. Click on the cube and notice the Transform component in the Inspector on the right. It's most likely it has a random looking Position, as denoted by the X, Y, and Z values. You can edit these values by hand or move the cube in the Scene view. You can also select the three dots in the corner of the Transform component and select Reset, which will move the cube to (0, 0, 0). Notice that this puts the cube directly in front of the camera, which you can tell by looking at the Game window.

At the bottom of the Inspector while the cube is selected you'll find the button **Add Component**. Press that button and you'll see a huge list of components come up. Instead of selecting any one of these, write "MoveCube" into the search bar. Since no such component exists, you'll see the option **New script** under the search results. Select **New script** and you'll see a new component MoveCube added to the Inspector, as well as a new file MoveCube appear under your Assets folder in your Project window. 

While we're at it, let's make a Scripts folder inside Assets by right clicking the Assets folder > Create > Folder. Since you'll end up having a lot of different assets it's good practice to organize them into folders. Double clicking the new MoveCube script will open Visual Studio, where you'll see the following template code.

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
```

The top three lines that begin with `using` are importing essential libraries into the script. This is similar to how we use `#include` to import libraries into Arduino. 

Below that we see `public class MoveCube : MonoBehaviour`. MoveCube is of course the name of our script. In creating this script, we also created a **class**, which is essentially a group of related functions that create a blueprint for a data type. By default, our new MoveCube code will inherent the `MonoBehaviour` class, which is the base class of every Unity script. The details aren't so important - all you need to know is that by attaching this script to your cube, you've created a new **instance** of MoveCube in your scene. This will allow us to access some useful information later.

Inside the class is where we'll write our code. By default we have a `Start()` function and `Update()` function. These correspond to `setup()` and `loop()` in Arduino, or `setup()` and `draw()` in p5. Before we write anything in these, though, let's take a step back and think about what we want to do in this script: For now, let's just move the cube at some constant speed. Later, we'll try to control the cube ourselves using the WASD keys.

It sounds like we'll need to define some speed first. Above the `Start()` function, let's define a speed variable by writing the following:
```cs
public float speed;
```

A `float` represents a number that can have a fractional value, as opposed to an `int` or integer, which is always a whole number. `speed` is the name we've given the variable. What's this `public` thing? Save your script and go back to the Unity window. If you click on the cube again and check the Inspector, you'll see that the parameter `Speed` has appeared inside the Move Cube component we made a moment ago. If a variable or function inside a class is public, it basically means that it can be accessed from outside the script. In Unity, it also means that you can edit the variable from the editor. 

![image](https://user-images.githubusercontent.com/43973044/211079455-4bee7461-5805-4207-a147-0281227013ec.png)

What if you don't want a variable to be accessed from outside? Anything unlabeled, like the `Start()` and `Update()` functions, is private by default. You can also be explicit by writing `private` instead of `public`. We'll use private variables in a bit, but let's get this cube moving first. 

### Follow a Cube
