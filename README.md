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

Below that we see `public class MoveCube : MonoBehaviour`. MoveCube is of course the name of our script. In creating this script, we also created a **class**, which is essentially a group of related functions that form a blueprint for a data type. By default, our new MoveCube code will inherent the `MonoBehaviour` class, which is the base class of every Unity script. The details aren't so important - all you need to know is that by attaching this script to your cube, you've created a new **instance** of MoveCube in your scene. This will allow us to access some useful information later.

Inside the class is where we'll write our code. By default we have a `Start()` function and `Update()` function. These correspond to `setup()` and `loop()` in Arduino, or `setup()` and `draw()` in p5. Before we write anything in these, though, let's take a step back and think about what we want to do in this script: For now, let's just move the cube at some constant speed. Later, we'll try to control the cube ourselves using the WASD keys.

It sounds like we'll need to define some speed first. Above the `Start()` function, let's define a speed variable by writing the following:
```cs
public float speed;
```

A `float` represents a number that can have a fractional value, as opposed to an `int` or integer, which is always a whole number. `speed` is the name we've given the variable. What's this `public` thing? Save your script and go back to the Unity window. If you click on the cube again and check the Inspector, you'll see that the parameter `Speed` has appeared inside the Move Cube component we made a moment ago. If a variable or function inside a class is public, it basically means that it can be accessed from outside the script. In Unity, it also means that you can edit the variable from the editor. 

![image](https://user-images.githubusercontent.com/43973044/211079455-4bee7461-5805-4207-a147-0281227013ec.png)

What if you don't want a variable to be accessed from outside? Anything unlabeled, like the `Start()` and `Update()` functions, is private by default. You can also be explicit by writing `private` instead of `public`. We'll use private variables in a bit, but let's get this cube moving first. 

There are several ways of moving things in Unity, but one is by simply incrementing the position of the object in each `Update()` loop. In `Update()`, write the following three lines.

```cs
Vector3 deltaPosition = Vector3.zero;
deltaPosition.z = speed * Time.deltaTime;
transform.position += deltaPosition;
```

We're introducing a few new things here. First, we are creating a new variable `deltaPosition` that is a `Vector3`. In Unity, a Vector3 is a data type that holds three floats corresponding to the x, y, and z axes - in other words, it's a useful data type for describing things in 3D space. We initialize `deltaPosition` as (0, 0, 0) using the shorthand `Vector3.zero`. Since we define this within `Update()`, this means that every frame, `deltaPosition` will reset. 

In the next line, we update just the `z` value of `deltaPosition` by setting it equal to the speed we created above times `Time.deltaTime`, which is the amount of time that passes in a single frame. Recall that distance is speed times time, so `deltaPosition` is simply a tiny distance. `Time.deltaTime` is used instead of explicit time values to account for fluctuations in framerate that may affect the frequency of the `Update()` loop. Think of it this way: imagine your game normally runs at 60fps, but on a different device it has to run at 30fps. If you use `Time.deltaTime`, two players playing on different devices will get to experience the game the same way, since the `deltaPosition`, for example, will scale accordingly. 

In the last line, we add our tiny distance to the cube's position, which is `transform.position`. Recall how in Unity, the Transform component includes values for position, rotation, and scale. You can retrieve these values from the object a script is attached to using `transform` (with a lowercase t). We use `+=`, which is the same as writing `transform.position = transform.position + deltaPosition`. 

If you've followed these directions, and then save your code and press Play, you might see...a completely motionless cube! That's because we haven't updated our `speed` value in the Inspector yet. Choose a value that makes sense to you. In general, scales in Unity are in meters, so a value of 5 would mean moving 5 meters per second (at least in our code). If you press Play again, you should see the cube move. If it's hard to tell from the Game view, find the cube in the Scene view. Nice! But in the (paraphrased) words of Tom Igoe, "It needs more interaction." So let's add input from the player. 

Essentially, we want the cube to move forward when we press W, backward when we press S, and left or right if we press A or D, respectively. We can wait for the player to press a specific button by adding the following conditional (i.e., if-statement) to the `Update()` loop.

```cs
if (Input.GetKey(KeyCode.W)) {
    // do stuff
    ;
}
```

If you'd like, try using a combination of this conditional and the above to write a script for moving the cube with player input before moving on. Feel free to create new variables as needed. The more you practice, the more you'll notice other approaches you can take. Here's how I would script it, with a brief explanation below. 

```cs
Vector3 direction = Vector3.zero;

if (Input.GetKey(KeyCode.W)) direction += Vector3.forward;
if (Input.GetKey(KeyCode.S)) direction += Vector3.back;
if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
if (Input.GetKey(KeyCode.D)) direction += Vector3.right;

direction.Normalize();

Vector3 deltaPosition = speed * Time.deltaTime * direction;
transform.position += deltaPosition;
```

You may recall from physics class from back in the day the difference between speed and velocity, which we use interchangeably in everyday life. Speed is a scalar, or just a magnitude, a number. Velocity is a vector, which has magnitude and direction. This is the difference between saying the car travels at 30km/hour and that it's traveling 30km/hour due north. If we're going to make an object move, we need both the speed at which it will move and the direction of its movement to determine its displacement, or where it ends up, after a certain amount of time. Separating these pieces, magnitude and direction, will help us organize our code as well. 

Anyway, I started with a new `Vector3` called `direction`, which I update using the WASD inputs from the player. Just like before, I'm using Unity's built-in shorthand. In Unity, **forward** corresponds to the positive-z direction, **right** corresponds to the positive-x, and **up** corresponds to the positive-y direction, so `Vector3.forward` represents the vector (0, 0, 1), `Vector3.back` represents the vector (0, 0, -1), and so on. 

After updating the direction with the player's input, I **normalize** the direction vector. This means that I change its magnitude to 1. Without getting too much into the details, the reason I want to do this is that without it, the cube would move more quickly diagonally (e.g., if I pressed both W and A at the same time), than it would when pressing just W, S, A or D. You can test this out yourself by commenting out `direction.Normalize()`. 

After that, I create a `deltaPosition` variable that is the product of speed, time, and direction, and add it to the cube's current position.

How does this compare with how you would move a cube? Note that there isn't a "wrong" way, but different solutions that may meet different needs. For example, maybe you don't want to move the cube diagonally, in which case, you might not need a direction vector at all. Or maybe you'd like to make smaller, slighter turns, in which case you would need to use access the Transform's rotation values (which we will do next!). Once you can write your own scripts, there are plenty of ways you can solve a problem! Don't be afraid to explore! 

## Tutorials
If you'd like to follow along what we're working on in our study group, check out these tutorials.
- [Move Camera](/Tutorials/MoveCamera.md)
- [Move Camera](/Tutorials/Rotation.md)
