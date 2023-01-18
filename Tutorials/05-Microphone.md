# Manipulating Objects with Audio

As you might expect, Unity lets you add music and sound effects to your Scene that you can trigger upon collisions or other events. But it also lets you analyze sound clips and respond to changes in volume, adding another possible space for ~interactivity~.  

## AudioSource and AudioListener

To start, let's make sure we can identify the components we'll be working with. Create a new scene and add a cube, and arrange the camera as you wish.

//

If you click on the Main Camera in your Scene and look at the Inspector, you'll notice a component called **Audio Listener**. Click on the cube, select Add Component through the Inspector, and look for the **Audio Source**. You will see a new component with an empty slot for an **Audio Clip** object on top. Find an .mp3 file of a song you like and add it to your Assets, which will automatically create an Audio Clip file for you. You can then drag this clip to the Audio Clip slot and Play your Scene - you should hear it play immediately! 

//

Note the **Play On Awake** and **Loop** checkboxes - if both are unchecked, the only way you can play or replay the attached sound is by scripting it (or creating a button or toggle to using UI to control it). We'll see some examples of controlling the sound through scripting later. 

You can also change the clip's pitch and volume from the Inspector, as well as its **Spatial Blend**, which determines how much the sound is affected by 3D space. As an example, play your Scene and turn up the cube's Spatial Blend all the way to 3D in the Inspector, then move it around in the Scene view. You should notice that the closer you move the cube to the camera, the louder it will be. Exactly how much louder per distance moved might depend on the curve at the bottom of the Audio Source component, which shows volume picked up by the Audio Listener as a function of its distance from the source. 

//

Those are the very basics of Audio Sources and Listeners! We'll work with them a bit more below, but for now, let's turn off the Audio Source component by unchecking the box next to its label in the inspector. In this activity, we will provide our own sound! 

## Microphone and Lerp

Create a new script called `SoundAnalysis` and attach it to an empty game object in your scene. First, we'll write some code that will calculate the volume of sound picked up by our computer's microphone!

```cs
private AudioClip microphoneClip;
private int spectrumSize;
private float[] spectrum;
public static float loudness; 
```

We'll start by naming the variables above. Instead of importing an .mp3 into our Scene, we will create a new `AudioClip` using the microphone, so we'll create `microphoneClip` to store it. Our `spectrum` is basically a sample of sound that is `spectrumSize` long that we'll use to calculate our clip's volume, or `loudness`. The square brackets mean that we are creating a list of float values. Our `loudness` variable is `public` and `static` because we'll access it with our cube later. 

```cs
void Start()
{
    spectrumSize = 64;
    spectrum = new float[spectrumSize];
    microphoneClip = Microphone.Start(Microphone.devices[0], true, 1, AudioSettings.outputSampleRate);
}
```

Next, we'll initialize our `spectrum` with an empty array of 64 floats, and begin recording our `AudioClip` using `Microphone.Start()`, which needs four arguments: the microphone we're using, accessed through `Microphone.devices[0]`; whether we are continuously updating the same clip; the length of the clip (arbitrary because we're looping it); and the sample rate of the clip, for which we can just use the project's default settings. 

```cs
void Update()
{
    GetAudioDataFromMic();
    GetLoudness();
}

void GetAudioDataFromMic()
{
    int clipPosition = Microphone.GetPosition(Microphone.devices[0]);
    int startPosition = clipPosition - spectrumSize;
    if (startPosition < 0) return;
    microphoneClip.GetData(spectrum, startPosition);
}

void GetLoudness()
{
    float totalLoudness = 0;
    for (int i = 0; i < spectrum.Length; i++)
    {
        totalLoudness += Mathf.Abs(spectrum[i]);
    }

    loudness = totalLoudness / spectrumSize;
}
```

Our `Update()` function will have two parts: update the spectrum with audio data from the microphone, and then calculate the volume by taking an average of the values in the spectrum. `GetAudioDataFromMic()` gets the current time placement of the audio clip, and fills the `spectrum` with the set of most recent sound samples, with values between -1.0 and 1.0. `GetLoudness()` calculates the average value in the `spectrum`, or set of sound samples, and updates the `loudness` variable accordingly. Note the use of `Mathf.Abs()` (absolute value) in calculating the sum before dividing by the number of samples. 

Now that we have a variable to keep track of volume, we can use it to update the size and color of our cube. Let's create a new script `ScaleWithSound` and attach it to the cube. We'll start with a long list of variables to help us keep track of the cube's changes. 

```cs
public Vector3 restSize;
public Vector3 maxSize;

private Renderer rend;
public Color restColor;
public Color maxColor;

private float currentLoudness;
private float lastLoudness;
public float sensitivity;
public float threshold;

private float timeOfChange;
public float interval;



void Start()
{
    rend = GetComponent<Renderer>(); 
    rend.material.color = restColor;
    timeOfChange = Time.time;
    currentLoudness = 0;
}
```

`restSize` and `maxSize`, and `restColor` and `maxColor` are the size and color that the cube should have when the volume is at its lowest or highest. We will use `Lerp` functions later, to linearly-interpolate sizes and colors in between. `rend` is the cube's renderer, which will allow us to change its color.

`currentLoudness` and `lastLoudness` are the adjusted loudness values measured in the current and previous frame. `sensitivity` is a factor by which we'll multiply the `loudness` to produce a more prominent signal, while `threshold` is the minimum difference that this signal and `lastLoudness` must exceed to update `currentLoudness`. 

`timeOfChange` will help us keep track of how much time has elapsed as the cube changes size, while `interval` will be the duration that any such change should take. These values will help us create smoother looking transitions.

We can set the public values and colors in the editor, while we initialize `rend` by getting the `Renderer` component and set it to the cube's rest color, set `timeOfChange` to the time when the code begins running, and `currentLoudness` to 0. 

```cs
void Update()
{
        lastLoudness = currentLoudness;
        currentLoudness = SoundAnalysis.loudness * sensitivity;

        if (Mathf.Abs(currentLoudness - lastLoudness) < threshold) {
            currentLoudness = lastLoudness;
        } else {
            timeOfChange = Time.time;
        }

        rend.material.color = Color.Lerp(
            restColor, 
            maxColor, 
            Vector3.Distance(transform.localScale, restSize) / Vector3.Distance(maxSize, restSize)
        );

        Vector3 targetScale = Vector3.Lerp(restSize, maxSize, currentLoudness);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, (Time.time - timeOfChange) / interval);
}
```

Our cube's `Update()` function relies heavily on `Lerp`, or linear-interpolation, to help us transition the size and color of the cube, and also do it gradually. 

In our first few lines, we update `lastLoudness` so that it is the same value as the `currentLoudness` of the previous frame. Then we update `currentLoudness` as the `loudness` from the `SoundAnalysis` script multiplied by the sensitivity we've chosen. If the difference between `currentLoudness` and `lastLoudness` is below the threshold, we simply revert the `currentLoudness` to `lastLoudness`. This will let us ignore very small slight changes in volume. Otherwise, we reset our timer so that there is time for the cube to shift size and color using the updated value. 

Next, we update the color of the cube using `Color.Lerp()`. `Lerp` functions take three arguments: a minimum value, a maximum value, and a float from 0 to 1. The `Lerp` returns a value some percent of the way between the min and max as decided by the float. For example, `Mathf.Lerp(2, 10, 0.5f)` gives us the value that is 50% of the way between 2 and 10, which is 6. For data types like `Vector3` and `Color`, the `Lerp` is calculated for each dimension (e.g., x, y, and z, or R, G, and B).

In this case, we interpolate between `restColor` and `maxColor` using the current scale of the cube compared with its maximum size. 

Then, we use `Vector3.Lerp()` to calculate a `targetScale` between the `restSize` and `maxSize`, interpolated by the `currentLoudness`. 

Finally, we use `Vector3.Lerp()` a second time to set the cube's scale between its current scale and `targetScale`, interpolated by how much of the `interval` has passed. In other words, once at least an `interval` amount of time has passed, the cube will be at the scale corresponding to the volume of the audio input to the microphone. 

## AudioSpectrum and FFT

 We've only calculated the volume of an audio signal from the microphone, but you'll notice, if you play with your cube, that it doesn't respond to changes in pitch. 

Using FFTs (fast Fourier transform) can provide frequency (i.e., pitch) information about an audio signal. This way, we can identify sounds with more specificity, like the bass of a track. The `AudioSource` and `AudioListener`
classes have the method `GetSpectrumData()`, which takes three arguments: an array like `spectrum` where we can add the data; the audio channel from which we sample our data (we can just use 0); and the type of function to use to split our frequencies. 

We can turn the Audio Source component on our cube back on, and change our `SoundAnalysis` with the following:

```cs
private int spectrumSize;
private AudioClip microphoneClip;
private float[] spectrum;

public static float loudness;

// add an audio source
public AudioSource audioSource;

void Start()
{
    spectrumSize = 64;
    spectrum = new float[spectrumSize];
    microphoneClip = Microphone.Start(Microphone.devices[0], true, 1, AudioSettings.outputSampleRate);

    // set the audio source clip to the microphone and play it
    audioSource.clip = microphoneClip;
    audioSource.loop = true;
    audioSource.Play();
}

void Update()
{
    // fill the spectrum through FFT instead of volume 

    // GetAudioDataFromMic();
    audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
    GetLoudness();
}
```

First, we add an `AudioSource`, which we can attach to the script through the editor. In the `Start()` function, we set the clip of the audio source to the `microphoneClip`, and make sure that it's both playing and looping. In the `Update()` function, instead of getting the volume of the clip like we did in our `GetAudioDataFromMic()` function, we'll use `GetSpectrumData()` to get the relative volumes of different frequencies, which we'll average in `GetLoudness()` like before. 

What happens when we run our scene? Well, not exactly what we were hoping for. Since the `AudioSource` is always playing, it will repeat every sound you make after a second, since the `microphoneClip` we put inside it updates every second (the third argument in `Microphone.Start()` is the length of the clip).  `GetSpectrumData()` uses the sound currently playing on the `AudioSource` to calculate the spectrum, so any animation of the cube will be delayed as well.   

As far as I can tell, FFTs in Unity work best with .mp3 files already imported into the project, rather than live recording from a mic. You can check out the SoundPlayer package in the releases to see an example, while I figure out how to get the frequency of a microphone input in real time! 