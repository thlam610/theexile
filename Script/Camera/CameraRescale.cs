using UnityEngine;

public class CameraRescale : MonoBehaviour
{
    public int targetWidth = 1920;
    public int targetHeight = 1080;

    private void Start()
    {
        // Calculate the desired aspect ratio
        float targetAspect = (float)targetWidth / targetHeight;

        // Calculate the current aspect ratio
        float currentAspect = (float)Screen.width / Screen.height;

        // Calculate the ratio of the current aspect to the target aspect
        float ratio = targetAspect / currentAspect;

        // Create a new Rect with the adjusted values
        Rect rect = new Rect(0, 0, 1, 1);
        rect.width *= ratio;
        rect.x = (1 - rect.width) / 2;

        // Apply the new viewport rect to the camera
        Camera.main.rect = rect;
    }
}
