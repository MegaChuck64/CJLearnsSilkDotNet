namespace CJLearnsSilkDotNet;

public class FPSCounter
{
    public float FramesPerSecond { get; private set; }

    /// <summary>
    /// Number of times per second to internally update the FramesPerSecond value;
    /// </summary>
    public float RefreshRate { get; set; } = 1f;
    
    private float timer = 0f;
    
    private int frameCounter = 0;

    public void Update(float dt)
    {
        if (timer < RefreshRate)
        {
            frameCounter++;
            timer += dt;
        }
        else
        {
            FramesPerSecond = frameCounter / timer;
            frameCounter = 0;
            timer = 0f;
        }
    }
}