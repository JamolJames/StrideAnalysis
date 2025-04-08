using UnityEngine;

public class ColorDataLoader : MonoBehaviour
{
    public Texture2D colorImage; // Assign in the Inspector (e.g. RGBA32 or RGB24)
    public Color[,] ColorData { get; private set; }

    public bool LoadColorData()
    {
        if (colorImage == null)
        {
            Debug.LogError("<ColorDataLoader>: Color image not assigned!");
            return false;
        }

        int width = colorImage.width;
        int height = colorImage.height;
        ColorData = new Color[width, height];

        // GetPixels() returns each pixel as a float4 (RGBA) in 0..1 range
        Color[] pixels = colorImage.GetPixels();
        if (pixels.Length != width * height)
        {
            Debug.LogError("<ColorDataLoader>: Pixel count mismatch.");
            return false;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // The array is in row-major order: row0 [0..width-1], row1 [width..2*width-1], etc.
                // So index = y*width + x
                ColorData[x, y] = pixels[y * width + x];
            }
        }
        Debug.Log($"<ColorDataLoader>: Pixel(center) = {ColorData[width/2, height/2]}");

        Debug.Log("<ColorDataLoader>: Color data loaded successfully.");
        return true;
    }
}
