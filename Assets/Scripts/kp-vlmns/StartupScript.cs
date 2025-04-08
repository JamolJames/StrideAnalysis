// using UnityEngine;

// public class StartupScript : MonoBehaviour
// {
//     public DepthDataLoader depthDataLoader; // Drag-and-drop in Inspector
//     public PointCloudGenerator pointCloudGenerator; // Drag-and-drop in Inspector
//     public PointCloudRenderer pointCloudRenderer; // Drag-and-drop in Inspector

//     private float updateInterval = 0.01f; // Time in seconds between updates
//     private float timeSinceLastUpdate = 0.0f;

//     void Start()
//     {
//         ValidateComponents();

//         // Initial setup
//         if (
//             !depthDataLoader.CombineMSBandLSB() ||
//             !depthDataLoader.LoadDepthData() 
//             || !depthDataLoader.LoadColorData() 
//             )
//         {
//             Debug.LogError("<StartupScript>: Failed to initialize data!");
//             return;
//         }

//         GenerateAndRenderPointCloud();
//     }

//     void Update()
//     {
//         // Increment the timer
//         timeSinceLastUpdate += Time.deltaTime;

//         // Check if it's time to update
//         if (timeSinceLastUpdate >= updateInterval)
//         {
//             timeSinceLastUpdate = 0.0f; // Reset the timer

//             if (!depthDataLoader.CombineMSBandLSB())
//             {
//                 Debug.LogError("<StartupScript>: Failed to combine MSB & LSB data!");
//                 return;
//             }

//             // Load depth data
//             if (!depthDataLoader.LoadDepthData())
//             {
//                 Debug.LogError("<StartupScript>: Failed to load depth data!");
//                 return;
//             }

//             if (!depthDataLoader.LoadColorData())
//             {
//                 Debug.LogError("<StartupScript>: Failed to load color data!");
//                 return;
//             }


//             // Generate and render the point cloud
//             GenerateAndRenderPointCloud();
//         }
//     }

//     private void ValidateComponents()
//     {
//         if (depthDataLoader == null) Debug.LogError("<StartupScript>: DepthDataLoader is not assigned!");
//         if (pointCloudGenerator == null) Debug.LogError("<StartupScript>: PointCloudGenerator is not assigned!");
//         if (pointCloudRenderer == null) Debug.LogError("<StartupScript>: PointCloudRenderer is not assigned!");
//     }

//     private void GenerateAndRenderPointCloud()
//     {
//         if (!pointCloudGenerator.GeneratePointCloud(depthDataLoader.DepthData, depthDataLoader.ColorData))
//         {
//             Debug.LogError("<StartupScript>: Failed to generate point cloud!");
//         }
//         else
//         {
//             pointCloudRenderer.RenderPointCloud();
//             Debug.Log("<StartupScript>: Point cloud updated successfully.");
//         }
//     }
// }


// ---  WORKING with static color ---

// using UnityEngine;

// public class StartupScript : MonoBehaviour
// {
//     public DepthDataLoader depthDataLoader; // Drag-and-drop in Inspector
//     public ColorDataLoader colorDataLoader; // Drag-and-drop in Inspector
//     public PointCloudGenerator pointCloudGenerator; // Drag-and-drop in Inspector
//     public PointCloudRenderer pointCloudRenderer; // Drag-and-drop in Inspector

//     private float updateInterval = 1.0f; // Time in seconds between updates
//     private float timeSinceLastUpdate = 0.0f;

//     void Start()
//     {
//         ValidateComponents();

//         // Initial setup - Load Depth Data
//         if (!depthDataLoader.LoadDepthData())
//         {
//             Debug.LogError("<StartupScript>: Failed to load initial depth data!");
//             return; // Early exit if depth data fails to load
//         }

//         // Initial setup - Load Color Data
//         if (!colorDataLoader.LoadColorData())
//         {
//             Debug.LogError("<StartupScript>: Failed to load initial color data!");
//             return; // Early exit if color data fails to load
//         }
//         pointCloudGenerator.ColorData = colorDataLoader.ColorData; // Set ColorData in PointCloudGenerator


//         GenerateAndRenderPointCloud();
//     }

//     void Update()
//     {
//         // Increment the timer
//         timeSinceLastUpdate += Time.deltaTime;

//         // Check if it's time to update
//         if (timeSinceLastUpdate >= updateInterval)
//         {
//             timeSinceLastUpdate = 0.0f; // Reset the timer

//             // Load depth data
//             if (!depthDataLoader.LoadDepthData())
//             {
//                 Debug.LogError("<StartupScript>: Failed to load depth data in Update!");
//                 return; // Exit Update if depth data loading fails
//             }

//             // Load color data
//             if (!colorDataLoader.LoadColorData())
//             {
//                 Debug.LogError("<StartupScript>: Failed to load color data in Update!");
//                 return; // Exit Update if color data loading fails
//             }
//             pointCloudGenerator.ColorData = colorDataLoader.ColorData; // Update ColorData in PointCloudGenerator


//             // Generate and render the point cloud
//             GenerateAndRenderPointCloud();
//         }
//     }

//     private void ValidateComponents()
//     {
//         if (depthDataLoader == null) Debug.LogError("<StartupScript>: DepthDataLoader is not assigned!");
//         if (colorDataLoader == null) Debug.LogError("<StartupScript>: ColorDataLoader is not assigned!");
//         if (pointCloudGenerator == null) Debug.LogError("<StartupScript>: PointCloudGenerator is not assigned!");
//         if (pointCloudRenderer == null) Debug.LogError("<StartupScript>: PointCloudRenderer is not assigned!");
//     }

//     private void GenerateAndRenderPointCloud()
//     {
//         if (!pointCloudGenerator.GeneratePointCloud(depthDataLoader.DepthData))
//         {
//             Debug.LogError("<StartupScript>: Failed to generate point cloud!");
//         }
//         else
//         {
//             pointCloudRenderer.RenderPointCloud();
//             Debug.Log("<StartupScript>: Point cloud updated successfully.");
//         }
//     }
// }

// //// --- tetsing with dynamic color -- integrated depth load dat aonly ---

// using UnityEngine;

// public class StartupScript : MonoBehaviour
// {
//     public DepthDataLoader depthDataLoader; // Drag-and-drop in Inspector
//     public PointCloudGenerator pointCloudGenerator; // Drag-and-drop in Inspector
//     public PointCloudRenderer pointCloudRenderer; // Drag-and-drop in Inspector

//     private float updateInterval = 1.0f; // Time in seconds between updates
//     private float timeSinceLastUpdate = 0.0f;

//     void Start()
//     {
//         ValidateComponents();

//         // Initial setup
//         if (
//             // !depthDataLoader.CombineMSBandLSB() ||
//             !depthDataLoader.LoadDepthData() ||
//             !depthDataLoader.LoadColorDataFromTexture()
//             )
//         {
//             Debug.LogError("<StartupScript>: Failed to initialize data!");
//             return;
//         }
        
//         pointCloudGenerator.ColorData = depthDataLoader.ColorData; // Set ColorData in PointCloudGenerator


//         GenerateAndRenderPointCloud();
//     }

//     void Update()
//     {
//         // Increment the timer
//         timeSinceLastUpdate += Time.deltaTime;

//         // Check if it's time to update
//         if (timeSinceLastUpdate >= updateInterval)
//         {
//             timeSinceLastUpdate = 0.0f; // Reset the timer

//             // Combine MSB and LSB into depth data
//             // if (!depthDataLoader.CombineMSBandLSB())
//             // {
//             //     Debug.LogError("<StartupScript>: Failed to combine MSB & LSB data!");
//             //     return;
//             // }

//             // Load depth data
//             if (!depthDataLoader.LoadDepthData())
//             {
//                 Debug.LogError("<StartupScript>: Failed to load depth data!");
//                 return;
//             }

//             if (!depthDataLoader.LoadColorDataFromTexture())
//             {
//                 Debug.LogError("<StartupScript>: Failed to load depth data!");
//                 return;
//             }
//             pointCloudGenerator.ColorData = depthDataLoader.ColorData; // Update ColorData in PointCloudGenerator

            

//             // Generate and render the point cloud
//             GenerateAndRenderPointCloud();
//         }
//     }

//     private void ValidateComponents()
//     {
//         if (depthDataLoader == null) Debug.LogError("<StartupScript>: DepthDataLoader is not assigned!");
//         if (pointCloudGenerator == null) Debug.LogError("<StartupScript>: PointCloudGenerator is not assigned!");
//         if (pointCloudRenderer == null) Debug.LogError("<StartupScript>: PointCloudRenderer is not assigned!");
//     }

//     private void GenerateAndRenderPointCloud()
//     {
//         if (!pointCloudGenerator.GeneratePointCloud(depthDataLoader.DepthData))
//         {
//             Debug.LogError("<StartupScript>: Failed to generate point cloud!");
//         }
//         else
//         {
//             pointCloudRenderer.RenderPointCloud();
//             Debug.Log("<StartupScript>: Point cloud updated successfully.");
//         }
//     }
// }


/// --------- Working with static color and depth ---- in data loader ----
// using UnityEngine;

// public class StartupScript : MonoBehaviour
// {
//     public DepthDataLoader depthDataLoader;
//     public PointCloudGenerator pointCloudGenerator; 
//     public PointCloudRenderer pointCloudRenderer; 

//     private float updateInterval = 0.01f; // Time in seconds between updates
//     private float timeSinceLastUpdate = 0.0f;

//     void Start()
//     {
//         ValidateComponents();

//         // Initial setup
//         if (
//             !depthDataLoader.LoadDepthData() 
//             )
//         {
//             Debug.LogError("<StartupScript>: Failed to initialize data!");
//             return;
//         }

//         GenerateAndRenderPointCloud();
//     }

//     void Update()
//     {
//         timeSinceLastUpdate += Time.deltaTime;

//         if (timeSinceLastUpdate >= updateInterval)
//         {
//             timeSinceLastUpdate = 0.0f;

//             // Load depth data
//             if (!depthDataLoader.LoadDepthData())
//             {
//                 Debug.LogError("<StartupScript>: Failed to load depth data!");
//                 return;
//             }

//             GenerateAndRenderPointCloud();
//         }
//     }

//     private void ValidateComponents()
//     {
//         if (depthDataLoader == null) Debug.LogError("<StartupScript>: DepthDataLoader is not assigned!");
//         if (pointCloudGenerator == null) Debug.LogError("<StartupScript>: PointCloudGenerator is not assigned!");
//         if (pointCloudRenderer == null) Debug.LogError("<StartupScript>: PointCloudRenderer is not assigned!");
//     }

//     private void GenerateAndRenderPointCloud()
//     {
//         if (!pointCloudGenerator.GeneratePointCloud(depthDataLoader.DepthData, depthDataLoader.ColorData))
//         {
//             Debug.LogError("<StartupScript>: Failed to generate point cloud!");
//         }
//         else
//         {
//             pointCloudRenderer.RenderPointCloud();
//             Debug.Log("<StartupScript>: Point cloud updated successfully.");
//         }
//     }
// }


/// -------------------------------------------------------------------- OPTIMIZED FINAL SCRIPTS --------------------------------------------------------------------

using UnityEngine;

public class StartupScript : MonoBehaviour
{
    public DepthDataLoader depthDataLoader;
    public PointCloudGenerator pointCloudGenerator;
    public PointCloudRenderer pointCloudRenderer;

    void Start()
    {
        ValidateComponents();

        if (
            !depthDataLoader.CombineMSBandLSB()
            || !depthDataLoader.LoadDepthData() 
            || !depthDataLoader.LoadColorData() 
            )
        {
            Debug.LogError("<StartupScript>: Failed to initialize data!");
            return;
        }

        GenerateAndRenderPointCloud();
    }

    void Update()
    {
            if (!depthDataLoader.CombineMSBandLSB())
            {
                Debug.LogError("<StartupScript>: Failed to combine MSB & LSB data!");
                return;
            }

            if (!depthDataLoader.LoadDepthData())
            {
                Debug.LogError("<StartupScript>: Failed to load depth data!");
                return;
            }

            if (!depthDataLoader.LoadColorData())
            {
                Debug.LogError("<StartupScript>: Failed to load color data!");
                return;
            }

            GenerateAndRenderPointCloud();

    }

    // Check if all required components are assigned
    private void ValidateComponents()
    {
        if (depthDataLoader == null) Debug.LogError("<StartupScript>: DepthDataLoader is not assigned!");
        if (pointCloudGenerator == null) Debug.LogError("<StartupScript>: PointCloudGenerator is not assigned!");
        if (pointCloudRenderer == null) Debug.LogError("<StartupScript>: PointCloudRenderer is not assigned!");
    }

    // Generate and render the point cloud
    private void GenerateAndRenderPointCloud()
    {
        if (pointCloudGenerator.GeneratePointCloud(depthDataLoader.DepthData, depthDataLoader.ColorData))
        {
            pointCloudRenderer.RenderPointCloud();
            // Debug.Log("<StartupScript>: Point cloud updated successfully.");
        }
        else
        {
            Debug.LogError("<StartupScript>: Failed to generate point cloud!");
        }
    }
}