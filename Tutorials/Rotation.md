# Rotation and Different Kinds of Movement

So far we've just translated our cube - that is, shift its position up, down, left, right, or diagonally without being so concerned about where it's facing. Depending on your game, however, you might want to allow finer control over the angle the cube is oriented. Let's consider another way of controlling the cube. 

```cs
public float speed;
public float rotationSpeed;

void Update()
{
    Vector3 linearDirection = Vector3.zero;
    Vector3 angularDirection = Vector3.zero;

    if (Input.GetKey(KeyCode.W)) linearDirection += transform.forward;
    if (Input.GetKey(KeyCode.S)) linearDirection -= transform.forward;

    if (Input.GetKey(KeyCode.A)) angularDirection += Vector3.down;
    if (Input.GetKey(KeyCode.D)) angularDirection += Vector3.up;

    transform.position += speed * Time.deltaTime * linearDirection;
    transform.eulerAngles += rotationSpeed * Time.deltaTime * angularDirection;
}
```

Just like before, we've made a `speed` variable, but this time we're declaring a `rotationSpeed` as well. We can set both values in the editor, but keep in mind that while they're both floats, their units are different - `speed` is meters per second, while `rotationSpeed` is angles per second. 

We start our `Update()` loop by creating two direction variables corresponding to linear (i.e., forward and backward) and angular motion. We'll use the W and S keys to update the `linearDirection` and move the cube forward and backward, and the A and D keys to update `angularDirection`, rotating the cube counterclockwise or clockwise respectively.

Instead of using `Vector3.forward` and `Vector3.backward` to update `linearDirection`, we're using positive and negative `transform.forward`. This corresponds to the direction of the local Z-axis, that is, the Z-axis of the cube itself. `Vector3.forward` always has the value (0, 0, 1), while `transform.forward` will change depending on which way the cube is facing. Since we're rotating the cube, its forward vector will change.

`Vector3.down` and `Vector3.up` are shorthands for (0, -1, 0) and (0, 1, 0), respectively. In Unity, the Y-axis measures height, and is perpendicular to the surface. Thus, rotations *around* the Y-axis are parallel to the surface. 

We update the cube's position as before, this time without normalization because linear direction can only be (0, 0, 1), (0, 0, 0), or (0, 0, -1). In other words, its magnitude is always either 1 or 0. 

We update its rotation similarly, but note that we are accessing something called `transform.eulerAngles` instead of the more obvious-sounding `transform.rotation`. Unity uses something called quaternions to represent orientation. Quaternions are four-dimensional vectors that use complex numbers, and as such aren't so intuitive, but are used internally by Unity. Meanwhile, euler angles represent orientation with respect to a fixed coordinate system, like in the editor. Thus, we should edit a transform's euler angles whenever we rotate an object in our script!