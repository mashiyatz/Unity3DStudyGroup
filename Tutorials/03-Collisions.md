# Rigidbodies and Collisions

While it's exciting to see our cube move around, it looks a bit lonely, doesn't it? Not to mention, it seems to have no regard for gravity as it slides on and off the plane without a care! Can we let such arrogance go unchecked?!

*Note: You may notice that the most of these screenshots refer to some kind of Enemy or EnemyGenerator. I decided spawning and destroying a bunch of "enemies" for this exercise was a bit cynical, so I renamed everything into treasure instead. Of course, you can choose any metaphor you want to represent crashing into things and making them to disappear, e.g., emotions.*

## Instantiating Objects

It's easy enough to add more objects to our Scene by adding them to the Hierarchy like we did our first cube, but we can automate this process through scripting. 

First, we'll need to create what's called a Prefab. In Unity, a prefab is essentially a template of a GameObject that includes all of its components, including scripts. You can create a prefab of any object in the Hierarchy simply by dragging and dropping it into your Assets folder in the Project window. The object won't leave the Hierarchy - instead you'll see its name highlighted blue, while a new object will appear among your assets with the same name and a blue cube icon next to it. If you click on this new object, you'll see at the top of the Inspector that it is a Prefab Asset.  

![prefab](https://user-images.githubusercontent.com/43973044/211386431-ab8f6e3c-d194-4ca0-90a6-e9440d7ecf26.png)

In your Scene, create a new cube (or any 3D object of your choice) and give it a unique name (I'll call it treasure, or treasure cube) and Material to distinguish it from our original cube, which we will now call the player cube. Drag and drop the new object from the Scene and into a Prefabs folder in your Assets, **and then delete it from the Scene**. Don't worry, it'll be back soon! 

We're going to create a new script to generate our object, but we'll need to attach this script somewhere. Right click on the Hierarchy and click **Create Empty** to create an empty game object, and call it "TreasureGenerator". Empty game objects are useful for organizing other objects as children, or adding new scripts like we will. Add a script to TreasureGenerator called GenerateTreasure, and open it on Visual Studio. 

Let's create a script that generates our treasure object every 5 seconds on a random spot. Write the following code in your GenerateTreasure script.

```cs
public GameObject treasurePrefab;
public float interval;
public float maxDistance;
private float timeSinceLastGenerate;

void Start()
{
    timeSinceLastGenerate = Time.time;
}

void Update()
{
    if (Time.time - timeSinceLastGenerate >= interval)
    {
        Vector3 pos = new(Random.Range(-maxDistance, maxDistance), 0.5f, Random.Range(-maxDistance, maxDistance));
        Instantiate(treasurePrefab, pos, transform.rotation, transform);
        timeSinceLastGenerate = Time.time;
    }
}
```

We declare a few variables in the beginning. `treasurePrefab` is of course the new prefab we made. `interval` is how long we want to leave between each generation of the treasure. `maxDistance` is the furthest from the center that we want to generate the treasure.* These are public variables because we will set them in the editor. Our only private variable, `timeSinceLastGenerate`, will be used to calculate how much time has elapsed between each treasure generation.

We initialize `timeSinceLastGenerate` as the time at which the code starts running. In the `Update()` loop, when the time elapsed (measured as the difference between the current time and `timeSinceLastGenerate`) is greater than the `interval`, we generate a random position that's at a fixed height, **instantiate** our treasure object at that position, and reset `timeSinceLastGenerate` to the current time.

Our random position is on the XZ-plane, where the plane in our scene lies. I've offset the position vertically by 0.5 because I'm generating a cube the same size as the player cube.

The `Instantiate` function's first argument is the object we want to generate, the second is the position where it will generate, the third is its orientation, and the last is the object's parent in the Hierarchy. We will generate our treasure object at the new random position. Since we're not too concerned with its rotation for now, we'll just use the rotation of our TreasureGenerator object, which will also be the parent of the treasure objects generated.  

Save your script, and in the editor, set the "Interval" and "Max Distance" values in the Inspector. To set the "Treasure Prefab", drag and drop your treasure prefab from the Inspector to the box. Press Play, and you should see some treasure appear over time! 

![instantiation](https://user-images.githubusercontent.com/43973044/211386468-2b8a137d-2156-4b5e-8320-c3679359e9fe.png)

## Adding Rigidbodies

Even with these new additions, there doesn't seem to be a lot to do in our plane. Notice that our player cube will run right through the treasure objects, and as noted, it doesn't seem to matter whether its on or off the plane. We can change that with the addition of some physics. 

Select the player cube, and then Add Component in the Inspector. Search for and add a **Rigidbody** component. The Rigidbody has a few values such as **Mass** and **Drag**, some checkboxes **Use Gravity** and **Is Kinematic**, as well as other parameters. We'll explain these a bit later, but for now, add a Rigidbody component to the treasure prefab as well.  

Now see what happens when you press Play. You should be able to push the treasure objects with the player cube, even over the edge. Once you do, you'll watch them fall towards nothingness! 

![collision](https://user-images.githubusercontent.com/43973044/211386559-c3a6a463-3325-488e-9b29-3a1571b02eef.png)

Back in the editor, uncheck **Use Gravity** in the player cube's Rigidbody, and you should see that while the treasure objects fall off the edge of the plane, the player cube will not, as expected. Next, check both **Use Gravity** and **Is Kinematic**. This time, you'll see the same thing, even though **Use Gravity** is checked! What's going on? Let's try one final test: set the player cube's Rigidbody back to its original settings, but check **Is Kinematic** for the treasure prefab. Now you'll see that the treasure objects won't budge at all!  

In Unity, the movement of objects with Rigidbodies is controlled by the physics engine, but we have some control over when and how through settings like the above. For example, an object that is kinematic will no longer be affected by forces, including collisions and gravity, and can only be controlled by its transform. However, a kinematic object can still affect other Rigidbodies. That's why when we made the player cube kinematic, it didn't fall off the edge of the plane, and why when we made the treasure objects kinematic, we couldn't push them, even though they could prevent the player cube from passing. 

For now, let's leave kinematics off for both the player cube and treasure objects. 

## Detecting Collisions and Destroying Objects

Even though we can push our treasure off the plane, it doesn't mean they disappear from the Scene. You can confirm this by keeping an eye on the Hierarchy. Recall that we made the TreasureGenerator the parent of all newly generated treasure objects. Over time, you'll see a longer and longer list of newly generated treasure objects underneath, no matter how far below the plane they've fallen!

![generation](https://user-images.githubusercontent.com/43973044/211386596-c864e6c7-f97d-4cb3-b021-6e8719988050.png)

First, let's set a timer on these newly generated treasure objects so that they self-destruct after a few seconds. This will help us prevent our Hierarchy from getting too crowded. Add a script TreasureBehavior to the treasure object prefab. Write the following code:

```cs
public float interval;
private float timeAtStart;

void Start()
{
    timeAtStart = Time.time;
}

void Update()
{
    if (Time.time - timeAtStart >= interval) Destroy(gameObject);
}
```

Just like before, we're keeping track of whether a certain amount of time has elapsed. Once that time has passed, `Destroy()` will remove the object from the Scene. Just as lowercase-t `transform` refers to the attached object's Transform component, lowercase-g `gameObject` is the object itself. Make sure to set a value for interval in the editor, or else the treasure objects will self-destruct immediately! 

Next, let's destroy the treasure objects on collision with the player cube. In the Inspector, you'll notice that the player cube and your treasure object has some kind of Collider component. As the name suggests, colliders are how Unity detects physical collisions. They come with functions that we can use to create responses to collisions. Let's add one of these methods, `OnCollisionEnter` to our TreasureBehavior script.   

```cs
private void OnCollisionEnter(Collision collision)
{
    if (collision.collider.CompareTag("Player")) Destroy(gameObject);
}
```

If you're using Visual Studio, just writing OnCollisionEnter should automatically complete the function for you - you'll just have to add the conditional. `OnCollisionEnter` runs automatically when a collision is detected. A collision will only occur when at least one of the objects involved has a non-kinematic rigidbody. 

We can identify what the object collides with using `collision.collider.gameObject`, but we can use the `CompareTag()` function instead if we are looking for a collision with an object with a specific tag. 

What is a "tag"? In the Inspector, right below the object's name, you should see the label Tag next to a dropdown menu. Most of the objects in your Scene should be untagged by default. Some tags in the dropdown include "Player", "GameCamera", and "MainController". Let's assign the player cube the "Player" tag.   

![tag](https://user-images.githubusercontent.com/43973044/211386626-d1cf1abc-e113-419c-b387-88b91b273883.png)

Now, we should see the treasure object destroyed upon touching the player cube!  
