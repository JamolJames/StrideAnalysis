// using System.Collections.Generic;
// using System.IO;
// using UnityEngine;

// public class PointCloudGenerator : MonoBehaviour
// {
//     private float[,] depthData;         // Internal reference to depth data
//     public Color[,] ColorData;          // Assigned from ColorDataLoader
//                                         // (ensure color resolution matches depth resolution if 1:1 mapping is intended)

//     public float fx = 504.135f;         // Focal length in pixels (X-axis)
//     public float fy = 504.129f;         // Focal length in pixels (Y-axis)
//     public float cx = 324.466f;         // Principal point X-coordinate
//     public float cy = 327.652f;         // Principal point Y-coordinate

//     public List<Vector3> Points { get; private set; }      // 3D points
//     public List<Color> PointColors { get; private set; }   // Per-point colors

//     public bool GeneratePointCloud(float[,] inputData, Color[,] inputColorData, float maxDepthMM = 4500.0f)
//     {
//         if (inputData == null)
//         {
//             Debug.LogError("<PointCloudGenerator>: Input depth data is null!");
//             return false;
//         }

//         depthData = inputData;
//         ColorData = inputColorData;
//         int width = depthData.GetLength(0);
//         int height = depthData.GetLength(1);

//         Debug.Log($"<PointCloudGenerator>: Depth data dimensions: {width}x{height}");

//         // Define expected values for center point (example for pixel (320, 288))
//         int centerX = width / 2;
//         int centerY = height / 2;
//         float expectedDepthRaw = 1026f;   // Expected raw depth
//         float expectedDepthMeters = 0.07f; // Expected z position in meters
//         Color expectedColor = new Color(0.016f, 0.016f, 0.016f, 1f);

//         int zeroDepthCount = 0;
//         int warningLimit = 600;

//         Points = new List<Vector3>();
//         PointColors = new List<Color>();

//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 float rawDepth = depthData[x, y];

//                 // Log and verify center pixel values
//                 if (x == centerX && y == centerY)
//                 {
//                     Debug.Log($"<PointCloudGenerator>: Depth at ({x}, {y}) = {rawDepth}");

//                     // Verify raw depth value
//                     if (!Mathf.Approximately(rawDepth, expectedDepthRaw))
//                     {
//                      //   Debug.LogWarning($"<PointCloudGenerator>: Depth at center pixel ({x}, {y}) is unexpected! Got {rawDepth}, expected {expectedDepthRaw}.");
//                     }
//                 }

//                 if (rawDepth <= 0)
//                 {
//                     zeroDepthCount++;
//                     if (warningLimit > 0)
//                     {
//                         // Debug.LogWarning($"<PointCloudGenerator>: Skipping pixel ({x}, {y}) due to invalid depth ({rawDepth}).");
//                         warningLimit--;
//                     }
//                     continue; // Skip invalid depth values
//                 }

//                 // Scale raw depth to meters
//                 float depthMM = (rawDepth / 65535.0f) * maxDepthMM;
//                 float z = depthMM / 1000f;

//                 // Map pixel to 3D space
//                 float xPos = (x - cx) * z / fx;
//                 float yPos = (y - cy) * z / fy;

//                 Vector3 point = new Vector3(xPos, yPos, z);
//                 Points.Add(point);

//                 // Verify center pixel 3D point position
//                 if (x == centerX && y == centerY)
//                 {
//                     Debug.Log($"<PointCloudGenerator>: Point at ({x}, {y}): {point}");

//                     // Check if the Z position matches expectations
//                     if (!Mathf.Approximately(point.z, expectedDepthMeters))
//                     {
//                        // Debug.LogWarning($"<PointCloudGenerator>: Z position at center pixel ({x}, {y}) is unexpected! Got {point.z}, expected {expectedDepthMeters}.");
//                     }
//                 }

//                 // Assign color
//                 Color pointColor = ColorData != null ? ColorData[x, y] : new Color(rawDepth / 65535.0f, rawDepth / 65535.0f, rawDepth / 65535.0f);
//                 PointColors.Add(pointColor);

//                 // Verify center pixel color
//                 if (x == centerX && y == centerY)
//                 {
//                     Debug.Log($"<PointCloudGenerator>: Color at ({x}, {y}): {pointColor}");

//                     if (!ApproximatelyEqualColor(pointColor, expectedColor))
//                     {
//                       //  Debug.LogWarning($"<PointCloudGenerator>: Color at center pixel ({x}, {y}) is unexpected! Got {pointColor}, expected {expectedColor}.");
//                     }
//                 }
//             }
//         }

//         Debug.Log($"<PointCloudGenerator>: Total zero-depth pixels: {zeroDepthCount} / {width * height}");
//         Debug.Log($"<PointCloudGenerator>: Generated {Points.Count} valid points.");

//         return true;
//     }

//     // Helper method to compare colors
//     private bool ApproximatelyEqualColor(Color c1, Color c2, float tolerance = 0.01f)
//     {
//         return Mathf.Abs(c1.r - c2.r) < tolerance &&
//                Mathf.Abs(c1.g - c2.g) < tolerance &&
//                Mathf.Abs(c1.b - c2.b) < tolerance &&
//                Mathf.Abs(c1.a - c2.a) < tolerance;
//     }

// }

// -------------------------------------------------------- RANDOM COLORS --------------------------------------------------------
// using System.Collections.Generic;
// using System.IO;
// using UnityEngine;

// public class PointCloudGenerator : MonoBehaviour
// {
//     private float[,] depthData;         // Internal reference to depth data
//     public Color[,] ColorData;          // Assigned from ColorDataLoader
//                                         // (ensure color resolution matches depth resolution if 1:1 mapping is intended)

//     public float fx = 504.135f;         // Focal length in pixels (X-axis)
//     public float fy = 504.129f;         // Focal length in pixels (Y-axis)
//     public float cx = 324.466f;         // Principal point X-coordinate
//     public float cy = 327.652f;         // Principal point Y-coordinate

//     public List<Vector3> Points { get; private set; }      // 3D points
//     public List<Color> PointColors { get; private set; }   // Per-point colors

//     public bool GeneratePointCloud(float[,] inputData, float maxDepthMM = 4500.0f)
//     {
//         if (inputData == null)
//         {
//             Debug.LogError("<PointCloudGenerator>: Input depth data is null!");
//             return false;
//         }

//         depthData = inputData;
//         int width = depthData.GetLength(0);
//         int height = depthData.GetLength(1);

//         Debug.Log($"<PointCloudGenerator>: Depth data dimensions: {width}x{height}");

//         Points = new List<Vector3>();
//         PointColors = new List<Color>();

//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 float rawDepth = depthData[x, y];

//                 if (rawDepth <= 0)
//                 {
//                     continue; // Skip invalid depth values
//                 }

//                 // Scale raw depth to meters
//                 float depthMM = (rawDepth / 65535.0f) * maxDepthMM;
//                 float z = depthMM / 1000f;

//                 // Map pixel to 3D space
//                 float xPos = (x - cx) * z / fx;
//                 float yPos = (y - cy) * z / fy;

//                 Vector3 point = new Vector3(xPos, yPos, z);
//                 Points.Add(point);

//                 // Assign a random color based on depth value
//                 float normalizedDepth = rawDepth / 65535.0f;
//                 Color pointColor = new Color(Random.value, Random.value, normalizedDepth);
//                 PointColors.Add(pointColor);
//             }
//         }

//         Debug.Log($"<PointCloudGenerator>: Generated {Points.Count} valid points with random colors.");

//         return true;
//     }

//     public void ExportPointCloudAsNumpy(string filePath)
//     {
//         if (Points == null || Points.Count == 0)
//         {
//             Debug.LogError("<PointCloudGenerator>: No points available to export.");
//             return;
//         }

//         using (StreamWriter writer = new StreamWriter(filePath))
//         {
//             for (int i = 0; i < Points.Count; i++)
//             {
//                 Vector3 p = Points[i];
//                 Color c = PointColors[i];
//                 writer.WriteLine($"{p.x},{p.y},{p.z},{c.r},{c.g},{c.b}");
//             }
//         }

//         Debug.Log($"<PointCloudGenerator>: Point cloud exported to {filePath} with random colors.");
//     }
// }



// // -------------------------------------------------------- WORKING pointcloud with color for static depth and color --------------------------------------------------------


// using System.Collections.Generic;
// using System.IO;
// using UnityEngine;

// public class PointCloudGenerator : MonoBehaviour
// {
//     private float[,] depthData;         // Internal reference to depth data
//     public Color[,] ColorData;          // Assigned from ColorDataLoader (still keep this for original logic)

//     // --- NEW: Texture override for direct color input ---
//     public Texture2D colorTextureOverride; // Assign color texture directly in Inspector for testing
//     private Color[,] overrideColorData;   // Internal ColorData from the override texture
//     // --- End of new section ---

//     public float fx = 504.135f;         // Focal length in pixels (X-axis)
//     public float fy = 504.129f;         // Focal length in pixels (Y-axis)
//     public float cx = 324.466f;         // Principal point X-coordinate
//     public float cy = 327.652f;         // Principal point Y-coordinate

//     public List<Vector3> Points { get; private set; }      // 3D points
//     public List<Color> PointColors { get; private set; }   // Per-point colors

//     // Example CSV export path:
//     string numpyPath = Application.dataPath + "/pointcloud.csv";

//     public bool GeneratePointCloud(float[,] inputData, float maxDepthMM = 4500.0f)
//     {
//         if (inputData == null)
//         {
//             Debug.LogError("<PointCloudGenerator>: Input depth data is null!");
//             return false;
//         }

//         depthData = inputData;
//         int width = depthData.GetLength(0);
//         int height = depthData.GetLength(1);

//         Debug.Log($"<PointCloudGenerator>: Depth data dimensions: {width}x{height}");

//         // --- NEW: Load color data from override texture if assigned ---
//         if (colorTextureOverride != null)
//         {
//             Debug.Log("<PointCloudGenerator>: Using colorTextureOverride for color data.");
//             overrideColorData = LoadColorDataFromTexture(colorTextureOverride);
//         }
//         else
//         {
//             overrideColorData = null; // Ensure it's null if no override texture
//         }
//         // --- End of new section ---


//         // Define expected values for center point (example for pixel (320, 288))
//         int centerX = width / 2;
//         int centerY = height / 2;
//         float expectedDepthRaw = 1026f;   // Expected raw depth
//         float expectedDepthMeters = 0.07f; // Expected z position in meters
//         Color expectedColor = new Color(0.016f, 0.016f, 0.016f, 1f);

//         int zeroDepthCount = 0;
//         int warningLimit = 600;

//         Points = new List<Vector3>();
//         PointColors = new List<Color>();

//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 float rawDepth = depthData[x, y];

//                 // Log and verify center pixel values
//                 if (x == centerX && y == centerY)
//                 {
//                     Debug.Log($"<PointCloudGenerator>: Depth at ({x}, {y}) = {rawDepth}");
//                 }

//                 if (rawDepth <= 0)
//                 {
//                     zeroDepthCount++;
//                     if (warningLimit > 0)
//                     {
//                         warningLimit--;
//                     }
//                     continue; // Skip invalid depth values
//                 }

//                 // Scale raw depth to meters
//                 float depthMM = (rawDepth / 65535.0f) * maxDepthMM;
//                 float z = depthMM / 1000f;

//                 // Map pixel to 3D space
//                 float xPos = (x - cx) * z / fx;
//                 float yPos = (y - cy) * z / fy;

//                 Vector3 point = new Vector3(xPos, yPos, z);
//                 Points.Add(point);

//                 // Verify center pixel 3D point position
//                 if (x == centerX && y == centerY)
//                 {
//                     Debug.Log($"<PointCloudGenerator>: Point at ({x}, {y}): {point}");
//                 }

//                 // --- NEW: Assign color - prioritize override texture if available ---
//                 Color pointColor;
//                 if (overrideColorData != null)
//                 {
//                     pointColor = overrideColorData[x, y]; // Use color from override texture
//                 }
//                 else
//                 {
//                     pointColor = ColorData != null ? ColorData[x, y] : new Color(rawDepth / 65535.0f, rawDepth / 65535.0f, rawDepth / 65535.0f); // Fallback to original logic
//                 }
//                 PointColors.Add(pointColor);
//                 // --- End of new section ---


//                 // Verify center pixel color
//                 if (x == centerX && y == centerY)
//                 {
//                     Debug.Log($"<PointCloudGenerator>: Color at ({x}, {y}): {pointColor}");
//                 }
//             }
//         }

//         Debug.Log($"<PointCloudGenerator>: Total zero-depth pixels: {zeroDepthCount} / {width * height}");
//         Debug.Log($"<PointCloudGenerator>: Generated {Points.Count} valid points.");

//         return true;
//     }

//     // --- NEW: Helper function to load ColorData from Texture2D ---
//     private Color[,] LoadColorDataFromTexture(Texture2D texture)
//     {
//         int width = texture.width;
//         int height = texture.height;
//         Color[,] colorData = new Color[width, height];
//         Color[] pixels = texture.GetPixels();

//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 colorData[x, y] = pixels[y * width + x];
//             }
//         }
//         Debug.Log("<PointCloudGenerator>: Color data loaded from override texture.");
//         return colorData;
//     }
//     // --- End of new section ---


//     // Helper method to compare colors
//     private bool ApproximatelyEqualColor(Color c1, Color c2, float tolerance = 0.01f)
//     {
//         return Mathf.Abs(c1.r - c2.r) < tolerance &&
//                Mathf.Abs(c1.g - c2.g) < tolerance &&
//                Mathf.Abs(c1.b - c2.b) < tolerance &&
//                Mathf.Abs(c1.a - c2.a) < tolerance;
//     }

//     public void ExportPointCloudAsNumpy(string filePath)
//     {
//         if (Points == null || Points.Count == 0)
//         {
//             Debug.LogError("<PointCloudGenerator>: No points available to export.");
//             return;
//         }

//         using (StreamWriter writer = new StreamWriter(filePath))
//         {
//             for (int i = 0; i < Points.Count; i++)
//             {
//                 Vector3 p = Points[i];
//                 Color c = PointColors[i];
//                 writer.WriteLine($"{p.x},{p.y},{p.z},{(int)(c.r * 255)},{(int)(c.g * 255)},{(int)(c.b * 255)}");
//             }
//         }

//         Debug.Log($"<PointCloudGenerator>: Point cloud exported to {filePath} with colors in Open3D-compatible format.");
//     }
// }



////////////////////// -------------------- Final optimized ------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PointCloudGenerator : MonoBehaviour
{
    private float[,] depthData;
    public Color[,] ColorData;

    public float fx = 504.135f;
    public float fy = 504.129f;
    public float cx = 324.466f;
    public float cy = 327.652f;

    public Vector3[] Points { get; private set; }
    public Color[] PointColors { get; private set; }

    public bool GeneratePointCloud(float[,] inputData, Color[,] inputColorData, float maxDepthMM = 4500.0f)
    {
        if (inputData == null)
        {
            Debug.LogError("<PointCloudGenerator>: Input depth data is null!");
            return false;
        }

        depthData = inputData;
        ColorData = inputColorData;

        int width = depthData.GetLength(0);
        int height = depthData.GetLength(1);
        int totalPixels = width * height;

        // Preallocate arrays to avoid concurrency issues
        Points = new Vector3[totalPixels];
        PointColors = new Color[totalPixels];

        float maxDepthFactor = maxDepthMM / 65535.0f;
        int zeroDepthCount = 0;

        // Parallel loop over rows
        Parallel.For(0, height, y =>
        {
            int rowOffset = y * width; // Compute base index for the row to avoid conflicts

            for (int x = 0; x < width; x++)
            {
                float rawDepth = depthData[x, y];

                if (rawDepth <= 0)
                {
                    // Safe accumulation of zero-depth count
                    System.Threading.Interlocked.Increment(ref zeroDepthCount);
                    continue;
                }

                float z = (rawDepth * maxDepthFactor) / 1000f;
                float xPos = (x - cx) * z / fx;
                float yPos = (y - cy) * z / fy;

                int index = rowOffset + x;
                Points[index] = new Vector3(xPos, yPos, z);
                PointColors[index] = ColorData != null ? ColorData[x, y] : new Color(rawDepth / 65535.0f, rawDepth / 65535.0f, rawDepth / 65535.0f);
            }
        });

        Debug.Log($"<PointCloudGenerator>: Total zero-depth pixels: {zeroDepthCount} / {totalPixels}");
        Debug.Log($"<PointCloudGenerator>: Generated {totalPixels - zeroDepthCount} valid points.");

        return true;
    }
}