# Unity3D Study Group Source Code
Resources for the ITP 2023 Unity3D Winter Study Group, hosted by Mashi! Feel free to use and adapt any code written here for personal use.

## Getting Started
These examples were made in Unity3D 2021.3, but should work with any recent or LTS version of Unity. I recommend installing Visual Studio as well, as it comes with Unity integration out of the box. If you're completely new to Unity or scripting in Unity, try following the steps below to get started. If you're already familiar with Unity and have some experience using a game engine, you can import this project as a .unitypackage and try to decipher my code yourself - my apologies in advance for my lack of commenting.

### The Unity Interface
When you open a new Unity project, you should see something like the image below. 

![image](https://user-images.githubusercontent.com/43973044/211072560-bcc153e1-73e7-436d-b1c1-3667f660fe94.png)

On the left is the Hierarchy window, which organizes all of the game objects you currently have in your Scene. The large window in the center is the Scene window, in which you're able to manipulate different objects in space as needed. You can press W, E, R to toggle between changing a selected object's position, rotation, and scale. When an object is selected in the Hierarchy and your cursor is hovering over the Scene window, you can press the F key to zoom into that object - great for when you're trying to look for different objects in a busy Scene. The window on the right is the Inspector, which shows components attached to the selected game object - in this case, we're looking at the components attached to the Main Camera object, which includes a Camera component, Audio Listener, and Transform. 

On the bottom is the Project window, which shows all of your project's assets (including Scenes) and installed packages. Next to the tab labeled Project is Console - the Console window will display errors or warnings when you're running your project. You can also write directly to the console as needed. You can also see a tab labeled Game next to Scene on the center window. The Game window shows what the player sees when they're actually playing the game. Unity will switch to this window automatically when you press the play button on top. 

You can arrange these windows however you like by dragging and dropping their tabs. I personally like seeing both the Scene and Game objects at the same time, so I arrange my windows like in the image below. If there's a layout you like you can save it by pressing the dropdown (Labeled either "Default" or "Layout") on the top right corner and selecting Save Layout, then you can switch to that layout when you start a new project like I have.   

![image](https://user-images.githubusercontent.com/43973044/211072620-a3b5e8b3-a71c-49f8-aaa5-009731d1614d.png)

