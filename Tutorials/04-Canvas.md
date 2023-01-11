# Adding UI Elements

Our cube is an outlaw to be reckoned with - with just one touch, it will snatch away treasure before your very eyes. But how do we keep track of its riches? Well, what if we could keep track of the number of treasures it has collected? 

## Resetting the Scene

First, the elephant in the room - what happens when our cube leaps off the edge of the plane, falling forever into the void? Though a lovely metaphor for how I often view the world, it forces us to restart the game through the editor. 

Instead, we could restart the Scene when it's obvious that the player cube isn't coming back - let's say after we detect that its downward velocity is over 20 meters per second. 

On top of our MoveCube script, where we have our libraries imported, let's add another which will allow us to restart our Scene. 

```cs
using UnityEngine.SceneManagement;
```

In order to access the player cube's velocity, we need access to its Rigidbody component. Declare a new variable for a rigidbody, and initialize it as the player cube's Rigidbody component. 

```cs
public float speed;
public float rotationSpeed;
private Rigidbody rb;

void Start()
{
    rb = GetComponent<Rigidbody>();
}
```
The method `GetComponent<>()` returns the component in the attached game object with the name inside the brackets. 

Finally, inside the `Update()` loop, add the following conditional.

```cs
if (rb.velocity.y <= -20) {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
```

When the player cube's velocity along the Y-axis is less than -20, i.e., its downward velocity is at least 20, we reload the current Scene using the `SceneManager` from library we imported above. Now when you play the game and the player cube falls off the edge, it will reappear at its starting position after a few moments of free fall.

This is great, but you the player should feel ashamed for driving the cube over edge of the plane in the first place! Instead of automatically restarting the Scene, let's display a Game Over screen and prompt the player to restart. 

## Creating a Canvas

Right click on the Hierarchy and select UI > Panel. A semi-transparent white film will appear over your Game view, and you'll see a Panel object appear in the Hierarchy as a child of an object called **Canvas**. The Canvas basically holds all of the Scene's UI elements. At the bottom of the Hierarchy you'll also see a new object called **EventSystem**. You don't need to pay much attention to that, except know that it helps manage interactions with the UI. 

Change the panel to any color you want through its Image component in the Inspector. Next, add two more UI elements as children of the panel, Text and Button. You can find these under UI > Legacy in newer versions of Unity. 

*An aside: Unity seems to recommend using the TextMeshPro package for adding text to the UI, but it requires downloading some assets and importing a different library in your script. I haven't noticed any particular benefit of using TMP myself, so I prefer the Legacy tools for simplicity's sake.*

Rename these new game objects appropriately, and resize and rearrange them on screen as needed by arranging them in the Scene view (where they'll appear massive relative to the rest of the objects in the Scene), or by changing values in their **Rect Transform** components. You can also change text size, color, font, and more. For now, I'll make my textbox read "Game Over" and place it above a button that says "Retry".

When positioning these components, you might want to work with a fixed size Game view as well. Note that you can choose an aspect ratio in the Game window. If you click on the Canvas, you will also see a **Canvas Scaler** component in the Inspector. Change the UI Scale Mode to "Scale With Screen Size", and choose a familiar Reference Resolution, such as 1920 by 1080. Make sure to edit font and window sizes accordingly! 

![canvas](https://user-images.githubusercontent.com/43973044/211419861-08becad4-9e0f-4057-bb3d-b973450ebcfb.png)

Now we have a proper Game Over screen, but if you Play now, you'll see that it's displayed from the beginning. Not to mention, clicking on the Retry button doesn't do anything. 

Let's go back to our MoveCube script. At the moment, when we detect that the cube is in freefall we will restart the game, but instead, let's bring up the Game Over panel. 

```cs
public GameObject gameOverPanel;
```

First we'll make a new public variable for the panel. We can drag and drop the panel object into the Move Cube component of our player cube in the Hierarchy.

```cs
void Start()
{
    rb = GetComponent<Rigidbody>();
    gameOverPanel.SetActive(false);
}
```

Inside our `Start()` function, we'll set the game over panel inactive, so that it's not visible in the beginning of the game. We can also do this in the Inspector by unchecking the box next to the object's name.  

```cs
if (rb.velocity.y <= -20) gameOverPanel.SetActive(true); 
```

Let's replace the conditional we wrote above with another that sets the game over panel active when downward velocity reaches a certain point. But don't forget about the old script just yet - we still need it to restart the game.

```cs
public void ReloadScene()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
```

Create a new **public** function that contains our Scene reload script. We'll use it in a moment. 

Back in the editor, click on the Retry button in the Hierarchy window and find its Button component in the Inspector. At the bottom of the component, you'll see "On Click()" over what seems like an empty menu. These are functions that Unity will run upon clicking the button. Click on the plus sign, and you'll see three small windows appear. Drag and drop your player cube object from the Hierarchy to where it says "None". On the right where it says No Function, you'll see that you can now search the dropdown menu. In the menu, select "MoveCube", and then "ReloadScene()". Now, when you press the Retry button, it should reload the Scene!

![button1](https://user-images.githubusercontent.com/43973044/211419893-c8e963f5-0cec-4e33-9a93-1efa789fc4e1.png)

![button2](https://user-images.githubusercontent.com/43973044/211419911-2c8c5f24-2bbc-4729-8ca9-38b9d229ffb4.png)

## Updating UI Text

Let's revisit our original goal - display the number of treasures the cube has acquired. We can start by creating a new Text object. This one should not be a child of the panel, but placed below it within the Canvas hierarchy so that it will appear in front of the panel. Give it an appropriate name and format it as needed. 

We'll need some way to keep track of the score. We might want to add it to the MoveCube script, **but let's keep in mind that we should try to limit each script to a clearly defined set of functions**. It's looking like our MoveCube script is about to get a little crowded, so let's make sure we know how we've organized our code by commenting as needed. 

I'll add a script to the score textbox itself called UpdateScore. Wherever you choose to keep track of the score, add the following line with the rest of your variable declarations.

```cs
public static int score; 
```

And in the `Start()` function, initialize its value as 0. This is a new type of variable we haven't encountered yet called a `static` variable. A static variable means that only one instance of the variable exists for all instances of the class. They're useful because they can be retrieved without creating a new instance of the class. Thus, if we want to get the score from the script, we only have to call `UpdateScore.score`, or whatever script you made `.score`. 

Let's go to our TreasureBehavior script next. At the moment, our treasure objects will simply be destroyed upon collision with the player cube. But let's increment the score by 1 right before that. 

```cs
private void OnCollisionEnter(Collision collision)
{
    if (collision.collider.CompareTag("Player"))
    {
        UpdateScore.score += 1;
        Destroy(gameObject);
    }
}
```

Finally, let's update the text itself. We'll have to add another library to access the UI components. In UpdateScore, let's add the following.

```cs
using UnityEngine.UI;
```

Let's create a new `Text` variable and initialize it as the Text component of our attached object. (Remember that if you're updating the text from a different script attached to an object without a Text component, you will have to make a public variable and attach the Text object through the editor.) Then we can set the text inside textbox as the score value!  

```cs
public static int score;
private Text textbox;

void Start()
{
    score = 0;
    textbox = GetComponent<Text>();
}

void Update()
{
    textbox.text = score.ToString();
}
```

Play the game and you should see the score update on screen upon each collision with a treasure object! If the text is a little hard to see because of the white plane, try using a new Material for the plane to add some contrast.

![score](https://user-images.githubusercontent.com/43973044/211419933-709e78fc-598c-4792-95b2-bd50218b6770.png)
