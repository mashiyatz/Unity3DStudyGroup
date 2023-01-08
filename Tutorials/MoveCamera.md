# Moving the Camera
Great! We can now move our cube around! But if you take a look at the Game view, the cube doesn't have to go very far before it's completely off screen. We'll have to update its position so that we can follow the cube. 

## Decorate the Scene

Before that, though, let's make a couple small changes to our Scene. Right click on the Hierarchy, select 3D Object, and add a Plane. Make sure to reset its position - you'll notice that your cube, if you also placed it at (0, 0, 0), is submerged. Change the cube's Y position to 0.5 and it should be resting right on top of the new plane. Game objects in the scene have their pivot at the center by default. 

Next, let's set the position of the camera so that it gives us a good view of the cube. By default, the camera's position is (0, 1, -10), and its rotation is (0, 0, 0), which makes it hard to see the cube when it moves along the z-axis. I chose (0, 5, -5) for the position, and (50, 0, 0) for the rotation. 

While we're at it, let's also change the Scene's background using the Camera component. Unity's default Skybox turns everything in the background below the horizon a dull gray. Change the `Clear Flags` setting to `Solid Color` and choose a color you like. In the Game view, you should see the background change accordingly.  

You'll notice that the cube is still hard to see because it's the same color as the plane. We can fix that by changing its Material. Materials in Unity change the appearance of surfaces using Shaders. To create a new material, right click on the assets folder > Create > Material. You can name the new material whatever you want, but just as we did with our script, put it inside a new Materials folder under Assets. 

Click on your new material, and you'll see a bunch of settings available in the inspector. For now, let's just change the color by clicking on the color box next to **Albedo**. Choose a color you like, and then drag and drop the Material onto the cube in the Scene view, or onto its name in the Hierarchy. You should see the color change immediately! 

## Parents and Children

There's an easy way to get the camera to follow the player. In the Hierarchy, drag and drop your Camera object on the cube. You'll notice in the Hierarchy the Main Camera is indented beneath the cube - this means the Main Camera is a child of the cube. Click on the camera and look at the Inspector - if you've set the cube at (0, 0.5, 0), you should see that the camera's Y position is offset 0.5 from what you set earlier. In the Hierarchy, a child's transform is relative to its parent. Try moving the cube in the Scene view, you should see that the camera's position stays the same. Press Play, and the cube shouldn't move relative to the camera. 

## Write a Script to Follow the Player

Another way of getting the camera to follow the player would be to write a script. Remove the camera as a child of the cube by dragging it out. Right click on the Scripts folder under Assets in the Project window and select Create > C# Script. Name it "FollowPlayer", and then drag and drop the script from the folder to the camera in the Hierarchy. 

Open the script in Visual Studio, and write the following lines inside the `FollowPlayer` class and `Start()` and `Update()` functions.

```cs
public Transform cube;
private Vector3 offset;

void Start()
{
    offset = transform.position;
}

void Update()
{
    transform.position = cube.position + offset;
}
```

Like in our first script, we start by declaring variables. This time, we create a public `Transform` variable called `cube` and a private `Vector3` named `offset`. Recall that an object's Transform component contains information about its position, rotation, and scale. We want the cube's Transform so that we can use its position to update the camera's own. But how do we get the Transform of another object?

Go back to Unity, click on the camera in the Hierarchy, and check the newly added Follow Player component in the Inspector. Since the variable `cube` is public, you should see a new parameter **Cube** added with a box that says "None (Transform)" right next to it. Drag and drop the cube object from the Hierarchy and into this box, and you'll see it fill immediately with "Cube (Transform)". In doing so, we have set the Transform variable in our script to the Transform of our cube!

The variable `offset` is private because we only plan on using it within this script. Inside `Start()`, we set `offset` as the camera's position. Since `Start()` only executes once, and before `Update()`, `offset` will be the position you set the camera in the Scene view.

Finally, we update the camera's position (remember that lowercase-t `transform` corresponds to the transform of the object to which the script is attached) by adding `offset` to the cube's position. 

When you Play, you should be able to follow the cube on screen just like before.

## Why Bother Scripting It?

Our FollowPlayer script might seem like a few extra steps we don't need, and indeed it might be, depending on what you're making. But try this: make the camera a child of the cube again, and now rotate the cube in the Scene window or by changing its rotation values in the Inspector. A child's transform - that is, not just its position - is relative to its parent, so whenever the cube rotates (or stretches), so will the camera.

This might not be a problem if you're just translating the cube, but if you implement rotations or collisions that could potentially knock the cube over (and we will!), then you're better off scripting the camera's position. 