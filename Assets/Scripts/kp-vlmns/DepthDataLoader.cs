// ------------------------- Working with depth data texture -------------------------
// using UnityEngine;

// public class DepthDataLoader : MonoBehaviour
// {
//     public Texture2D depthImage; // Assign the depth image in the Inspector
//     public float[,] DepthData { get; private set; } // Public property for loaded data

//     public bool LoadDepthData()
//     {
//         if (depthImage == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Depth image not assigned!");
//             return false;
//         }

//         int width = depthImage.width;
//         int height = depthImage.height;
//         DepthData = new float[width, height];

//         // Read raw 16-bit depth data from the texture
//         Texture2D depthTexture = depthImage;

//         Color[] pixels = depthTexture.GetPixels();

//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 // Scale raw 16-bit depth values to millimeters (Python logic)
//                 DepthData[x, y] = depthTexture.GetPixel(x, y).r * 65535.0f;
//             }
//         }

//         Debug.Log($"<DepthDataLoader>: Pixel(center) = {DepthData[width/2, height/2]}");

//         Debug.Log("<DepthDataLoader>: Depth data loaded successfully.");
//         return true;
//     }
// }



//// ---------- Working with color and depth but with layered artificats like jiqings renderer ----------
// using UnityEngine;

// public class DepthDataLoader : MonoBehaviour
// {
//     public RenderTexture depthRenderTexture; // Assign the depth RenderTexture in the Inspector

//     private Texture2D depthImage;
//     public float[,] DepthData { get; private set; } // Public property for loaded data

//     public bool LoadDepthData()
//     {
//         if (depthRenderTexture == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Depth RenderTexture not assigned!");
//             return false;
//         }

//         // Convert RenderTexture to Texture2D
//         RenderTexture.active = depthRenderTexture;
//         if (depthImage == null || depthImage.width != depthRenderTexture.width || depthImage.height != depthRenderTexture.height)
//         {
//             depthImage = new Texture2D(depthRenderTexture.width, depthRenderTexture.height, TextureFormat.RGBA16, false);
//         }
//         depthImage.ReadPixels(new Rect(0, 0, depthRenderTexture.width, depthRenderTexture.height), 0, 0);
//         depthImage.Apply();
//         RenderTexture.active = null;

//         int width = depthImage.width;
//         int height = depthImage.height;
//         DepthData = new float[width, height];

//         // Read 16-bit depth data from R and G channels
//         Color[] pixels = depthImage.GetPixels();
//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 Color pixel = pixels[y * width + x];
//                 int depthLSB = Mathf.RoundToInt(pixel.r * 255); // Extract R channel as LSB
//                 int depthMSB = Mathf.RoundToInt(pixel.g * 255); // Extract G channel as MSB

//                 int depthValue = (depthMSB << 8) | depthLSB; // Combine MSB and LSB
//                 DepthData[x, y] = depthValue; // Store in depth data array
//             }
//         }

//         Debug.Log($"<DepthDataLoader>: Pixel(0,0) = {DepthData[0,0]}");
//         Debug.Log($"<DepthDataLoader>: Pixel(center) = {DepthData[width/2, height/2]}");

//         Debug.Log("<DepthDataLoader>: Depth data loaded successfully.");
//         return true;
//     }
// }

// // ---------- WORKING  renderer with manual 8 bit unorm MSB and 8 bit unorm LSB images > LoadData ----------
// using UnityEngine;

// public class DepthDataLoader : MonoBehaviour
// {
//     public Texture2D msbImage; // Assign the MSB image in the Inspector
//     public Texture2D lsbImage; // Assign the LSB image in the Inspector
//     private Texture2D depthImage; // Combined 16-bit depth texture

//     public float[,] DepthData { get; private set; } // Public property for loaded data

//     public bool CombineMSBandLSB()
//     {
//         if (msbImage == null || lsbImage == null)
//         {
//             Debug.LogError("<DepthDataLoader>: MSB or LSB image not assigned!");
//             return false;
//         }

//         if (msbImage.width != lsbImage.width || msbImage.height != lsbImage.height)
//         {
//             Debug.LogError("<DepthDataLoader>: MSB and LSB image dimensions do not match!");
//             return false;
//         }

//         int width = msbImage.width;
//         int height = msbImage.height;

//         // IMP use R16 format for depth
//         depthImage = new Texture2D(width, height, TextureFormat.R16, false);


//         // Combine MSB and LSB into a single 16-bit depth image
//         Color[] msbPixels = msbImage.GetPixels();
//         Color[] lsbPixels = lsbImage.GetPixels();
//         Color[] depthPixels = new Color[width * height];

//         for (int i = 0; i < depthPixels.Length; i++)
//         {
//             // Convert MSB and LSB to 8-bit integers
//             byte msb = (byte)Mathf.RoundToInt(msbPixels[i].r * 255); // Extract MSB
//             byte lsb = (byte)Mathf.RoundToInt(lsbPixels[i].r * 255); // Extract LSB

//             // Combine into a 16-bit depth value
//             ushort depthValue = (ushort)((msb << 8) | lsb);

//             // Normalize the 16-bit depth value to [0, 1] for RGBA32 storage
//            float normalizedDepth = depthValue / 65535.0f;
//             // float normalizedDepth = depthValue;  ---  This makes pointcloud flat
//             depthPixels[i] = new Color(normalizedDepth, 0, 0, 1); // Store in red channel
//         }

//         depthImage.SetPixels(depthPixels);
//         depthImage.Apply();

//         Debug.Log("<DepthDataLoader>: Combined MSB and LSB into depth image successfully.");
//         return true;
//     }



//     public bool LoadDepthData()
//     {
//         if (depthImage == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Depth image not assigned or created!");
//             return false;
//         }

//         int width = depthImage.width;
//         int height = depthImage.height;
//         DepthData = new float[width, height];

//         // Read raw 16-bit depth data from the texture
//         Color[] pixels = depthImage.GetPixels();

//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 // Scale normalized depth values back to millimeters
//                 DepthData[x, y] = pixels[y * width + x].r * 65535.0f;
//             }
//         }

//         Debug.Log($"<DepthDataLoader>: Pixel(0,0) = {DepthData[0, 0]}");
//         Debug.Log($"<DepthDataLoader>: Pixel(center) = {DepthData[width / 2, height / 2]}");

//         Debug.Log("<DepthDataLoader>: Depth data loaded successfully.");
//         return true;
//     }
// }

// ---------- Wokring BUT has convex artifacts  renderer with live 8 bit unorm MSB and 8 bit unorm LSB images > LoadData ----------
// using UnityEngine;
// using System.IO;
// using UnityEngine;


// public class DepthDataLoader : MonoBehaviour
// {
//     public Texture2D depthImage; // Assign the LSB image in the Inspector


//     private Texture2D msbImage; // Assign the LSB image in the Inspector
//     private Texture2D lsbImage; // Assign the LSB image in the Inspector

//     public RenderTexture incomingRenderTexture; // RenderTexture dynamically updated
//     public float[,] DepthData { get; private set; } // Public property for loaded data


//     public bool ExtractMSBandLSBFromRenderTexture()
//     {
//         if (incomingRenderTexture == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Incoming RenderTexture is not assigned!");
//             return false;
//         }

//         // Ensure MSB and LSB textures are initialized and match the RenderTexture dimensions
//         if (msbImage == null || msbImage.width != incomingRenderTexture.width || msbImage.height != incomingRenderTexture.height)
//         {
//             msbImage = new Texture2D(incomingRenderTexture.width, incomingRenderTexture.height, TextureFormat.R8, false);
//         }

//         if (lsbImage == null || lsbImage.width != incomingRenderTexture.width || lsbImage.height != incomingRenderTexture.height)
//         {
//             lsbImage = new Texture2D(incomingRenderTexture.width, incomingRenderTexture.height, TextureFormat.R8, false);
//         }

//         // Read the RenderTexture into a temporary Texture2D
//         Texture2D tempTexture = new Texture2D(incomingRenderTexture.width, incomingRenderTexture.height, TextureFormat.RGBA32, false);
//         RenderTexture.active = incomingRenderTexture;
//         tempTexture.ReadPixels(new Rect(0, 0, incomingRenderTexture.width, incomingRenderTexture.height), 0, 0);
//         tempTexture.Apply();
//         RenderTexture.active = null;

//         // Extract MSB and LSB from the temporary Texture2D
//         Color[] tempPixels = tempTexture.GetPixels();
//         Color[] msbPixels = new Color[tempPixels.Length];
//         Color[] lsbPixels = new Color[tempPixels.Length];

//         for (int i = 0; i < tempPixels.Length; i++)
//         {
//             msbPixels[i] = new Color(tempPixels[i].g, 0, 0, 1); // MSB in Green channel
//             lsbPixels[i] = new Color(tempPixels[i].r, 0, 0, 1); // LSB in Red channel
//         }

//         msbImage.SetPixels(msbPixels);
//         msbImage.Apply();

//         lsbImage.SetPixels(lsbPixels);
//         lsbImage.Apply();


//         Debug.Log("<DepthDataLoader>: Extracted MSB and LSB from RenderTexture.");
//         return true;
//     }


// public bool CombineMSBandLSB()
//     {
//         if (msbImage == null || lsbImage == null)
//         {
//             Debug.LogError("<DepthDataLoader>: MSB or LSB image not assigned!");
//             return false;
//         }

//         if (msbImage.width != lsbImage.width || msbImage.height != lsbImage.height)
//         {
//             Debug.LogError("<DepthDataLoader>: MSB and LSB image dimensions do not match!");
//             return false;
//         }

//         int width = msbImage.width;
//         int height = msbImage.height;

//         // IMP use R16 format for depth
//         depthImage = new Texture2D(width, height, TextureFormat.R16, false);


//         // Combine MSB and LSB into a single 16-bit depth image
//         Color[] msbPixels = msbImage.GetPixels();
//         Color[] lsbPixels = lsbImage.GetPixels();
//         Color[] depthPixels = new Color[width * height];

//         for (int i = 0; i < depthPixels.Length; i++)
//         {
//             // Convert MSB and LSB to 8-bit integers
//             byte msb = (byte)Mathf.RoundToInt(msbPixels[i].r * 255); // Extract MSB
//             byte lsb = (byte)Mathf.RoundToInt(lsbPixels[i].r * 255); // Extract LSB

//             // Combine into a 16-bit depth value
//             ushort depthValue = (ushort)((msb << 8) | lsb);

//             // Normalize the 16-bit depth value to [0, 1] for RGBA32 storage
//             float normalizedDepth = depthValue / 65535.0f;
//             depthPixels[i] = new Color(normalizedDepth, 0, 0, 1); // Store in red channel
//         }

//         depthImage.SetPixels(depthPixels);
//         depthImage.Apply();

//         Debug.Log("<DepthDataLoader>: Combined MSB and LSB into depth image successfully.");
//         return true;
//     }


//     public bool LoadDepthData()
//     {
//         if (depthImage == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Depth image not assigned or created!");
//             return false;
//         }

//         int width = depthImage.width;
//         int height = depthImage.height;
//         DepthData = new float[width, height];

//         // Read raw 16-bit depth data from the texture
//         Color[] pixels = depthImage.GetPixels();

//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 // Scale normalized depth values back to millimeters
//                 DepthData[x, y] = pixels[y * width + x].r * 65535.0f;
//             }
//         }

//         Debug.Log($"<DepthDataLoader>: Pixel(0,0) = {DepthData[0, 0]}");
//         Debug.Log($"<DepthDataLoader>: Pixel(center) = {DepthData[width / 2, height / 2]}");

//         Debug.Log("<DepthDataLoader>: Depth data loaded successfully.");
//         return true;
//     }
// }


// ---------- Working renderer similar to manual with live 8 bit unorm MSB and 8 bit unorm LSB images > LoadData ----------
// using UnityEngine;
// using System.IO;

// public class DepthDataLoader : MonoBehaviour
// {
//     public Texture2D depthImage; // Assign the LSB image in the Inspector

//     public RenderTexture incomingRenderTexture; // RenderTexture dynamically updated
//     public float[,] DepthData { get; private set; } // Public property for loaded data


//     public bool CombineMSBandLSB()
//     {
//         if (incomingRenderTexture == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Incoming RenderTexture is not assigned!");
//             return false;
//         }

//         if (depthImage == null || depthImage.width != incomingRenderTexture.width || depthImage.height != incomingRenderTexture.height)
//         {
//             depthImage = new Texture2D(incomingRenderTexture.width, incomingRenderTexture.height, TextureFormat.R16, false);
//         }


//         // Read the RenderTexture into a temporary Texture2D
//         Texture2D tempTexture = new Texture2D(incomingRenderTexture.width, incomingRenderTexture.height, TextureFormat.RGBA32, false);
//         RenderTexture.active = incomingRenderTexture;
//         tempTexture.ReadPixels(new Rect(0, 0, incomingRenderTexture.width, incomingRenderTexture.height), 0, 0);
//         tempTexture.Apply();
//         RenderTexture.active = null;

//         // Extract MSB and LSB from the temporary Texture2D
//         Color[] tempPixels = tempTexture.GetPixels();
//         Color[] depthPixels = new Color[640 * 576];
//         ushort[] depthData = new ushort[tempPixels.Length];


//         for (int i = 0; i < tempPixels.Length; i++)
//         {
//             // Convert MSB and LSB to 8-bit integers
//             byte msb = (byte)Mathf.RoundToInt(tempPixels[i].g * 255); // Extract MSB
//             byte lsb = (byte)Mathf.RoundToInt(tempPixels[i].r * 255); // Extract LSB

//             // Combine into a 16-bit depth value
//             ushort depthValue = (ushort)((msb << 8) | lsb);

//             // Normalize the 16-bit depth value to [0, 1]
//             float normalizedDepth = depthValue / 65535.0f;
//             depthPixels[i] = new Color(normalizedDepth, 0, 0, 1); // Store in red channel
//         }

//         depthImage.SetPixels(depthPixels);
//         depthImage.Apply();

//         Debug.Log("<DepthDataLoader>: Combined MSB and LSB into depth image successfully.");
//         return true;
//     }


//     public bool LoadDepthData()
//     {
//         if (depthImage == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Depth image not assigned or created!");
//             return false;
//         }

//         int width = depthImage.width;
//         int height = depthImage.height;
//         DepthData = new float[width, height];

//         // Read raw 16-bit depth data from the texture
//         Color[] pixels = depthImage.GetPixels();

//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 // Scale normalized depth values back to millimeters
//                 DepthData[x, y] = pixels[y * width + x].r * 65535.0f;
//             }
//         }

//         Debug.Log($"<DepthDataLoader>: Pixel(0,0) = {DepthData[0, 0]}");
//         Debug.Log($"<DepthDataLoader>: Pixel(center) = {DepthData[width / 2, height / 2]}");

//         Debug.Log("<DepthDataLoader>: Depth data loaded successfully.");
//         return true;
//     }
// }

// ---------- Working renderer with correct quality but with test Spout Megaframe ----------

// using UnityEngine;
// using System.IO;

// public class DepthDataLoader : MonoBehaviour
// {
//     public Texture2D depthImage; // Assign the LSB image in the Inspector

//     public RenderTexture incomingRenderTexture; // RenderTexture dynamically updated
//     public float[,] DepthData { get; private set; } // Public property for loaded data


//     public bool CombineMSBandLSB()
//     {
//         if (incomingRenderTexture == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Incoming RenderTexture is not assigned!");
//             return false;
//         }

//         if (depthImage == null || depthImage.width != incomingRenderTexture.width || depthImage.height != incomingRenderTexture.height)
//         {
//             depthImage = new Texture2D(incomingRenderTexture.width, incomingRenderTexture.height, TextureFormat.R16, false);
//         }


//         // Read the RenderTexture into a temporary Texture2D
//         Texture2D tempTexture = new Texture2D(incomingRenderTexture.width, incomingRenderTexture.height, TextureFormat.RGBA32, false);
//         RenderTexture.active = incomingRenderTexture;
//         tempTexture.ReadPixels(new Rect(0, 0, incomingRenderTexture.width, incomingRenderTexture.height), 0, 0);
//         tempTexture.Apply();
//         RenderTexture.active = null;

//         // Extract MSB and LSB from the temporary Texture2D
//         Color[] tempPixels = tempTexture.GetPixels();
//         Color[] depthPixels = new Color[640 * 576];
//         ushort[] depthData = new ushort[tempPixels.Length];


//         for (int i = 0; i < tempPixels.Length; i++)
//         {
//             // Convert MSB and LSB to 8-bit integers
//             byte msb = (byte)Mathf.RoundToInt(tempPixels[i].g * 255); // Extract MSB
//             byte lsb = (byte)Mathf.RoundToInt(tempPixels[i].r * 255); // Extract LSB

//             // Combine into a 16-bit depth value
//             ushort depthValue = (ushort)((msb << 8) | lsb);

//             // Normalize the 16-bit depth value to [0, 1]
//             float normalizedDepth = depthValue / 65535.0f;
//             depthPixels[i] = new Color(normalizedDepth, 0, 0, 1); // Store in red channel
//         }

//         depthImage.SetPixels(depthPixels);
//         depthImage.Apply();

//         Debug.Log("<DepthDataLoader>: Combined MSB and LSB into depth image successfully.");
//         return true;
//     }


//     public bool LoadDepthData()
//     {
//         if (depthImage == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Depth image not assigned or created!");
//             return false;
//         }

//         int width = depthImage.width;
//         int height = depthImage.height;
//         DepthData = new float[width, height];

//         // Read raw 16-bit depth data from the texture
//         Color[] pixels = depthImage.GetPixels();

//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 // Scale normalized depth values back to millimeters
//                 DepthData[x, y] = pixels[y * width + x].r * 65535.0f;
//             }
//         }

//         Debug.Log($"<DepthDataLoader>: Pixel(0,0) = {DepthData[0, 0]}");
//         Debug.Log($"<DepthDataLoader>: Pixel(center) = {DepthData[width / 2, height / 2]}");

//         Debug.Log("<DepthDataLoader>: Depth data loaded successfully.");
//         return true;
//     }
// }

// ---------- Testing renderer with correct quality but with Actual Spout Megaframe ----------

// using UnityEngine;
// using System.IO;

// public class DepthDataLoader : MonoBehaviour
// {
//     public Texture2D depthImage; // Final combined depth image (640x576)
//     public RenderTexture incomingRenderTexture; // RenderTexture dynamically updated
//     public float[,] DepthData { get; private set; } // Public property for loaded data


//     // Updated CombineMSBandLSB method
// public bool CombineMSBandLSB()
// {
//     if (incomingRenderTexture == null)
//     {
//         Debug.LogError("<DepthDataLoader>: Incoming RenderTexture is not assigned!");
//         return false;
//     }

//     // Validate depthImage (640x576)
//     if (depthImage == null || depthImage.width != 640 || depthImage.height != 576)
//     {
//         depthImage = new Texture2D(640, 576, TextureFormat.R16, false);
//     }

//     // Crop and unpack LSB and MSB regions
//     RenderTexture lsbUnpacked = new RenderTexture(640, 576, 0, RenderTextureFormat.R8);
//     RenderTexture msbUnpacked = new RenderTexture(640, 576, 0, RenderTextureFormat.R8);

//     // Crop LSB and MSB regions
//     CropAndUnpackRegion(incomingRenderTexture, 720, 288, lsbUnpacked); // LSB
//     CropAndUnpackRegion(incomingRenderTexture, 1008, 288, msbUnpacked); // MSB

//     // Combine LSB and MSB into depthPixels
//     Texture2D lsbTexture = new Texture2D(640, 576, TextureFormat.R8, false);
//     Texture2D msbTexture = new Texture2D(640, 576, TextureFormat.R8, false);

//     ReadRenderTexture(lsbUnpacked, lsbTexture);
//     ReadRenderTexture(msbUnpacked, msbTexture);

//     Color[] depthPixels = new Color[640 * 576];
//     for (int i = 0; i < depthPixels.Length; i++)
//     {
//         byte lsb = (byte)(lsbTexture.GetPixel(i % 640, i / 640).r * 255);
//         byte msb = (byte)(msbTexture.GetPixel(i % 640, i / 640).r * 255);

//         // Combine into 16-bit depth value
//         ushort depthValue = (ushort)((msb << 8) | lsb);
//         float normalizedDepth = depthValue / 65535.0f;

//         depthPixels[i] = new Color(normalizedDepth, 0, 0, 1);
//     }

//     // Update depthImage with the processed data
//     depthImage.SetPixels(depthPixels);
//     depthImage.Apply();

//     Debug.Log("<DepthDataLoader>: Processed incoming frame successfully.");
//     return true;
// }

// private void CropAndUnpackRegion(RenderTexture source, int startY, int height, RenderTexture target)
// {
//     // Create a temporary texture for cropping
//     Texture2D packedTexture = new Texture2D(1280, height, TextureFormat.RGBA32, false);
//     Rect cropRect = new Rect(0, source.height - startY - height, 1280, height);

//     RenderTexture.active = source;
//     packedTexture.ReadPixels(cropRect, 0, 0);
//     packedTexture.Apply();
//     RenderTexture.active = null;

//     // Unpack the left and right halves into a 640x576 texture
//     Texture2D unpackedTexture = new Texture2D(640, 576, TextureFormat.R8, false);
//     for (int y = 0; y < height; y++)
//     {
//         for (int x = 0; x < 1280; x++)
//         {
//             int destX = x % 640; // Column index in the 640-wide output
//             int destY;

//             if (x < 640)
//             {
//                 // Map the left half (top) to the bottom half of the output
//                 destY = y + 288;
//             }
//             else
//             {
//                 // Map the right half (bottom) to the top half of the output
//                 destY = y;
//             }

//             // Get the pixel value and set it in the output texture
//             Color pixel = packedTexture.GetPixel(x, y);
//             unpackedTexture.SetPixel(destX, destY, new Color(pixel.r, pixel.r, pixel.r, 1));
//         }
//     }
//     unpackedTexture.Apply();

//     // Copy the unpacked texture to the target RenderTexture
//     Graphics.CopyTexture(unpackedTexture, target);

//     // Clean up temporary textures
//     Destroy(packedTexture);
//     Destroy(unpackedTexture);
// }

// private void ReadRenderTexture(RenderTexture renderTexture, Texture2D texture)
// {
//     RenderTexture.active = renderTexture;
//     texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
//     texture.Apply();
//     RenderTexture.active = null;
// }

//     public bool LoadDepthData()
//     {
//         if (depthImage == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Depth image not assigned or created!");
//             return false;
//         }

//         int width = depthImage.width;
//         int height = depthImage.height;

//         DepthData = new float[width, height];

//         // Read depth data from the processed depthImage
//         Color[] pixels = depthImage.GetPixels();
//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 DepthData[x, y] = pixels[y * width + x].r * 65535.0f;
//             }
//         }

//         Debug.Log("<DepthDataLoader>: Depth data loaded successfully.");
//         return true;
//     }
// }

// ------------- testing optimized  ---------------- //

// using UnityEngine;
// using Unity.Collections;
// using System.Threading.Tasks;

// public class DepthDataLoader : MonoBehaviour
// {
//     public Texture2D depthImage; // Final combined depth image (640x576)
//     public RenderTexture incomingRenderTexture; // RenderTexture dynamically updated
//     public float[,] DepthData { get; private set; } // Public property for loaded data

//     private const int Width = 640;
//     private const int Height = 576;

//     // Preallocated reusable resources
//     private Texture2D tempTexture;
//     private float[] depthPixels; // Reusing as a single channel array

//     private void Start()
//     {
//         // Allocate the final output texture
//         if (depthImage == null || depthImage.width != Width || depthImage.height != Height)
//         {
//             depthImage = new Texture2D(Width, Height, TextureFormat.R16, false);
//         }

//         // Allocate reusable temporary texture and buffers
//         tempTexture = new Texture2D(1280, 288, TextureFormat.RGBA32, false);
//         depthPixels = new float[Width * Height];
//     }

//     private void OnDestroy()
//     {
//         Destroy(tempTexture);
//         tempTexture = null;
//     }

//     public bool CombineMSBandLSB()
//     {
//         if (incomingRenderTexture == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Incoming RenderTexture is not assigned!");
//             return false;
//         }

//         // Reset depthPixels
//         System.Array.Clear(depthPixels, 0, depthPixels.Length);

//         // Process LSB and MSB directly from the incomingRenderTexture
//         ProcessRenderTexture(incomingRenderTexture, 720, 288, true);  // Process LSB
//         ProcessRenderTexture(incomingRenderTexture, 1008, 288, false); // Process MSB

//         // Apply depthPixels directly to depthImage
//         Color[] outputPixels = new Color[Width * Height];
//         Parallel.For(0, depthPixels.Length, i =>
//         {
//             float normalizedDepth = depthPixels[i] / 65535.0f;
//             outputPixels[i] = new Color(normalizedDepth, 0, 0, 1);
//         });

//         depthImage.SetPixels(outputPixels);
//         depthImage.Apply();

//         Debug.Log("<DepthDataLoader>: Processed incoming frame successfully.");
//         return true;
//     }

//     private void ProcessRenderTexture(RenderTexture source, int startY, int height, bool isLSB)
//     {
//         // Crop and read pixel data directly from the RenderTexture
//         Rect cropRect = new Rect(0, source.height - startY - height, 1280, height);

//         RenderTexture.active = source;
//         tempTexture.ReadPixels(cropRect, 0, 0);
//         tempTexture.Apply();
//         RenderTexture.active = null;

//         // Process the cropped pixel data and update depthPixels
//         Color[] croppedPixels = tempTexture.GetPixels();

//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < 1280; x++)
//             {
//                 int destX = x % Width;
//                 int destY = (x < Width) ? (y + 288) : y;
//                 int destIndex = destY * Width + destX;

//                 byte value = (byte)(croppedPixels[y * 1280 + x].r * 255);

//                 if (isLSB)
//                 {
//                     // Combine LSB
//                     depthPixels[destIndex] = value;
//                 }
//                 else
//                 {
//                     // Combine MSB
//                     depthPixels[destIndex] = (value << 8) | (byte)depthPixels[destIndex];
//                 }
//             }
//         }
//     }

//     public bool LoadDepthData()
//     {
//         if (depthImage == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Depth image not assigned or created!");
//             return false;
//         }

//         int width = depthImage.width;
//         int height = depthImage.height;

//         DepthData = new float[width, height];

//         Color[] pixels = depthImage.GetPixels();
//         Parallel.For(0, height, y =>
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 DepthData[x, y] = pixels[y * width + x].r * 65535.0f;
//             }
//         });

//         Debug.Log("<DepthDataLoader>: Depth data loaded successfully.");
//         return true;
//     }
// }

// ---------- TESTING WITH COLOR + DEPTH DATA LOADER IN 1 for working spout ----------
// using UnityEngine;
// using System.IO;

// public class DepthDataLoader : MonoBehaviour
// {
//     public Texture2D depthImage; // Final combined depth image (640x576)
//     public RenderTexture incomingRenderTexture; // RenderTexture dynamically updated
//     public float[,] DepthData { get; private set; } // Public property for loaded depth data

//     public Texture2D colorImage; // Public Texture2D for Color Image (1280x720) - Added for color
//     public Color[,] ColorData { get; private set; } // Public property for loaded color data - Added for color


//     // Updated CombineMSBandLSB method (DEPTH - NO CHANGES HERE)
//     public bool CombineMSBandLSB()
//     {
//         if (incomingRenderTexture == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Incoming RenderTexture is not assigned!");
//             return false;
//         }

//         // Validate depthImage (640x576)
//         if (depthImage == null || depthImage.width != 640 || depthImage.height != 576)
//         {
//             depthImage = new Texture2D(640, 576, TextureFormat.R16, false);
//         }

//         // Crop and unpack LSB and MSB regions
//         RenderTexture lsbUnpacked = new RenderTexture(640, 576, 0, RenderTextureFormat.R8);
//         RenderTexture msbUnpacked = new RenderTexture(640, 576, 0, RenderTextureFormat.R8);

//         // Crop LSB and MSB regions
//         CropAndUnpackRegion(incomingRenderTexture, 720, 288, lsbUnpacked); // LSB
//         CropAndUnpackRegion(incomingRenderTexture, 1008, 288, msbUnpacked); // MSB

//         // Combine LSB and MSB into depthPixels
//         Texture2D lsbTexture = new Texture2D(640, 576, TextureFormat.R8, false);
//         Texture2D msbTexture = new Texture2D(640, 576, TextureFormat.R8, false);

//         ReadRenderTexture(lsbUnpacked, lsbTexture);
//         ReadRenderTexture(msbUnpacked, msbTexture);

//         Color[] depthPixels = new Color[640 * 576];
//         for (int i = 0; i < depthPixels.Length; i++)
//         {
//             byte lsb = (byte)(lsbTexture.GetPixel(i % 640, i / 640).r * 255);
//             byte msb = (byte)(msbTexture.GetPixel(i % 640, i / 640).r * 255);

//             // Combine into 16-bit depth value
//             ushort depthValue = (ushort)((msb << 8) | lsb);
//             float normalizedDepth = depthValue / 65535.0f;

//             depthPixels[i] = new Color(normalizedDepth, 0, 0, 1);
//         }

//         // Update depthImage with the processed data
//         depthImage.SetPixels(depthPixels);
//         depthImage.Apply();

//         Debug.Log("<DepthDataLoader>: Processed depth frame successfully.");
//         return true;
//     }

//     // New function to load Color Data from top 1280x720 region - ADDED COLOR FUNCTIONALITY
//     public bool LoadColorDataFromTexture()
//     {
//         if (incomingRenderTexture == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Incoming RenderTexture is not assigned for color data!");
//             return false;
//         }

//         // Initialize colorImage if it's null or not the correct size
//         if (colorImage == null || colorImage.width != 1280 || colorImage.height != 720)
//         {
//             colorImage = new Texture2D(1280, 720, TextureFormat.RGBA32, false); // Or RGB24, depending on your color format
//         }

//         // Create a temporary Texture2D to read pixels from the RenderTexture region
//         Texture2D colorTextureRegion = new Texture2D(1280, 720, TextureFormat.RGBA32, false); // Match colorImage format
//         Rect cropRect = new Rect(0, 0, 1280, 720); // Crop from the top 1280x720

//         RenderTexture.active = incomingRenderTexture;
//         colorTextureRegion.ReadPixels(cropRect, 0, incomingRenderTexture.height - 720); // Read from top-left corner of the region
//         colorTextureRegion.Apply();
//         RenderTexture.active = null;

//         // Load Color Data Array
//         int width = colorTextureRegion.width;
//         int height = colorTextureRegion.height;
//         ColorData = new Color[width, height];
//         Color[] pixels = colorTextureRegion.GetPixels(); // Get pixels from the temporary texture

//         if (pixels.Length != width * height)
//         {
//             Debug.LogError("<DepthDataLoader>: Color Pixel count mismatch after loading from RenderTexture region.");
//             return false;
//         }

//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 ColorData[x, y] = pixels[y * width + x];
//             }
//         }
//         Destroy(colorTextureRegion); // Clean up temporary texture

//         Debug.Log("<DepthDataLoader>: Color data loaded from incoming texture successfully.");
//         return true;
//     }


//     private void CropAndUnpackRegion(RenderTexture source, int startY, int height, RenderTexture target) // DEPTH - NO CHANGES HERE
//     {
//         // Create a temporary texture for cropping
//         Texture2D packedTexture = new Texture2D(1280, height, TextureFormat.RGBA32, false);
//         Rect cropRect = new Rect(0, source.height - startY - height, 1280, height);

//         RenderTexture.active = source;
//         packedTexture.ReadPixels(cropRect, 0, 0);
//         packedTexture.Apply();
//         RenderTexture.active = null;

//         // Unpack the left and right halves into a 640x576 texture
//         Texture2D unpackedTexture = new Texture2D(640, 576, TextureFormat.R8, false);
//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < 1280; x++)
//             {
//                 int destX = x % 640; // Column index in the 640-wide output
//                 int destY;

//                 if (x < 640)
//                 {
//                     // Map the left half (top) to the bottom half of the output
//                     destY = y + 288;
//                 }
//                 else
//                 {
//                     // Map the right half (bottom) to the top half of the output
//                     destY = y;
//                 }

//                 // Get the pixel value and set it in the output texture
//                 Color pixel = packedTexture.GetPixel(x, y);
//                 unpackedTexture.SetPixel(destX, destY, new Color(pixel.r, pixel.r, pixel.r, 1));
//             }
//         }
//         unpackedTexture.Apply();

//         // Copy the unpacked texture to the target RenderTexture
//         Graphics.CopyTexture(unpackedTexture, target);

//         // Clean up temporary textures
//         Destroy(packedTexture);
//         Destroy(unpackedTexture);
//     }

//     private void ReadRenderTexture(RenderTexture renderTexture, Texture2D texture) // DEPTH - NO CHANGES HERE
//     {
//         RenderTexture.active = renderTexture;
//         texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
//         texture.Apply();
//         RenderTexture.active = null;
//     }

//     public bool LoadDepthData() // DEPTH - NO CHANGES HERE
//     {
//         if (depthImage == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Depth image not assigned or created!");
//             return false;
//         }

//         int width = depthImage.width;
//         int height = depthImage.height;

//         DepthData = new float[width, height];

//         // Read depth data from the processed depthImage
//         Color[] pixels = depthImage.GetPixels();
//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 DepthData[x, y] = pixels[y * width + x].r * 65535.0f;
//             }
//         }

//         Debug.Log("<DepthDataLoader>: Depth data loaded successfully.");
//         return true;
//     }
// }

///// ------- Working color mapped to depth - static------- /////
///
// using UnityEngine;

// public class DepthDataLoader : MonoBehaviour
// {
//     // Depth texture (e.g. 640×576)
//     public Texture2D depthImage;
//     // Color texture (e.g. 1280×720)
//     public Texture2D colorTexture;

//     public float[,] DepthData { get; private set; }     // [width, height] in mm
//     public Color[,] ColorData { get; private set; }     // [width, height], aligned to depth

//     // --------------------------------------------------------------------------------
//     //  1) HARD-CODED INTRINSICS, DISTORTION COEFFICIENTS, EXTRINSICS
//     //     (directly from your Python snippet)
//     // --------------------------------------------------------------------------------

//     // Depth Intrinsics
//     public const float fx_d = 503.359406f;
//     public const float fy_d = 503.437744f;
//     public const float cx_d = 321.307129f;
//     public const float cy_d = 344.940552f;

//     // Depth Distortion (Brown–Conrady)
//     public const float k1_d = 0.593356f;
//     public const float k2_d = 0.018897f;
//     public const float k3_d = -0.000569f;
//     public const float k4_d = 0.934931f;
//     public const float k5_d = 0.142730f;
//     public const float k6_d = -0.002843f;
//     public const float p1_d = 0.000027f;
//     public const float p2_d = 0.000019f;

//     // Color Intrinsics
//     public const float fx_c = 610.220215f;
//     public const float fy_c = 610.198914f;
//     public const float cx_c = 638.855225f;
//     public const float cy_c = 367.438721f;

//     // Color Distortion
//     public const float k1_c = 0.189565f;
//     public const float k2_c = -2.505723f;
//     public const float k3_c = 1.585458f;
//     public const float k4_c = 0.072184f;
//     public const float k5_c = -2.314136f;
//     public const float k6_c = 1.498891f;
//     public const float p1_c = 0.000567f;
//     public const float p2_c = 0.000051f;

//     // Extrinsics (Depth -> Color). Translation in mm
//     // Rotation stored row-major in a 2D array
//     public static readonly float[,] R_d2c =
//     {
//         {  1.000000f, -0.000285f,  0.000535f },
//         {  0.000233f,  0.995495f,  0.094816f },
//         { -0.000560f, -0.094816f,  0.995495f }
//     };
//     public static readonly Vector3 t_d2c = new Vector3(
//         -31.918030f,
//         -2.101260f,
//          3.968107f
//     );

//     // --------------------------------------------------------------------------------
//     //  2) EXISTING DEPTH-LOADING CODE (UNCHANGED!)
//     // --------------------------------------------------------------------------------
//     public bool LoadDepthData()
//     {
//         if (depthImage == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Depth image not assigned!");
//             return false;
//         }

//         int width = depthImage.width;
//         int height = depthImage.height;
//         DepthData = new float[width, height];

//         // Read raw 16-bit depth data from the texture
//         Texture2D depthTexture = depthImage;

//         Color[] pixels = depthTexture.GetPixels();

//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 // Scale raw 16-bit depth values to millimeters (Python logic)
//                 DepthData[x, y] = depthTexture.GetPixel(x, y).r * 65535.0f;
//             }
//         }

//         Debug.Log($"<DepthDataLoader>: Pixel(center) = {DepthData[width/2, height/2]}");
//         Debug.Log("<DepthDataLoader>: Depth data loaded successfully.");

//         // --------------------------------------------------------------------------------
//         //  3) ADD NEW COLOR ALIGNMENT LOGIC *HERE* (no changes above this line)
//         // --------------------------------------------------------------------------------
//         if (colorTexture == null)
//         {
//             Debug.LogWarning("<DepthDataLoader>: No colorTexture assigned—skipping ColorData creation.");
//             return true;
//         }

//         // Create the ColorData array same size as DepthData
//         ColorData = new Color[width, height];

//         // Read the entire color texture into an array (for fast CPU indexing)
//         int colorW = colorTexture.width;   // e.g. 1280
//         int colorH = colorTexture.height;  // e.g. 720
//         Color[] colorPixels = colorTexture.GetPixels(); // row-major: colorPixels[y * colorW + x]

//         // For each depth pixel, map to color space
//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 float Z_d = DepthData[x, y]; // depth in mm
//                 if (Z_d <= 1e-3f)
//                 {
//                     // Invalid/zero depth
//                     ColorData[x, y] = Color.black;
//                     continue;
//                 }

//                 // (A) Distorted normalized coords in depth: (x_d, y_d)
//                 float x_d = (x - cx_d) / fx_d;
//                 float y_d = (y - cy_d) / fy_d;

//                 // (B) Undistort (x_d, y_d) -> (x_undist, y_undist)
//                 float x_ud, y_ud;
//                 UndistortPointBrownConrady(
//                     x_d, y_d,
//                     k1_d, k2_d, k3_d, k4_d, k5_d, k6_d,
//                     p1_d, p2_d,
//                     out x_ud, out y_ud,
//                     20
//                 );

//                 // (C) Back-project to 3D in depth camera coords
//                 float X_d = x_ud * Z_d;
//                 float Y_d = y_ud * Z_d;

//                 // (D) Transform depth->color: p_c = R_d2c * p_d + t_d2c
//                 float X_c = R_d2c[0,0]*X_d + R_d2c[0,1]*Y_d + R_d2c[0,2]*Z_d + t_d2c.x;
//                 float Y_c = R_d2c[1,0]*X_d + R_d2c[1,1]*Y_d + R_d2c[1,2]*Z_d + t_d2c.y;
//                 float Z_c = R_d2c[2,0]*X_d + R_d2c[2,1]*Y_d + R_d2c[2,2]*Z_d + t_d2c.z;

//                 if (Z_c <= 1e-3f)
//                 {
//                     // behind color camera or invalid
//                     ColorData[x, y] = Color.black;
//                     continue;
//                 }

//                 // (E) Normalize in color camera
//                 float x_c_norm = X_c / Z_c;
//                 float y_c_norm = Y_c / Z_c;

//                 // (F) Forward-distort those normalized coords for the color lens
//                 float x_c_dist, y_c_dist;
//                 DistortPointBrownConrady(
//                     x_c_norm, y_c_norm,
//                     k1_c, k2_c, k3_c, k4_c, k5_c, k6_c,
//                     p1_c, p2_c,
//                     out x_c_dist, out y_c_dist
//                 );

//                 // (G) Convert to pixel coords in the color image
//                 float u_c = fx_c * x_c_dist + cx_c;
//                 float v_c = fy_c * y_c_dist + cy_c;
//                 int u_ci = Mathf.RoundToInt(u_c);
//                 int v_ci = Mathf.RoundToInt(v_c);

//                 // (H) If valid, sample from color texture
//                 if (u_ci >= 0 && u_ci < colorW && v_ci >= 0 && v_ci < colorH)
//                 {
//                     int idx = v_ci * colorW + u_ci;
//                     Color c = colorPixels[idx];
//                     ColorData[x, y] = c;
//                 }
//                 else
//                 {
//                     // out of bounds => black or some fallback
//                     ColorData[x, y] = Color.black;
//                 }
//             }
//         }

//         Debug.Log("<DepthDataLoader>: ColorData aligned to depth successfully.");
//         return true;
//     }

//     // --------------------------------------------------------------------------------
//     //  4) BROWN–CONRADY DISTORT/UNDISTORT HELPERS
//     // --------------------------------------------------------------------------------

//     /// <summary>
//     /// Given an undistorted normalized point (xu, yu), apply Brown–Conrady forward distortion.
//     /// </summary>
//     private void DistortPointBrownConrady(
//         float xu, float yu,
//         float k1, float k2, float k3,
//         float k4, float k5, float k6,
//         float p1, float p2,
//         out float xd, out float yd)
//     {
//         float r2 = xu * xu + yu * yu;
//         float r4 = r2 * r2;
//         float r6 = r4 * r2;

//         float denom = 1f + k4*r2 + k5*r4 + k6*r6;
//         if (Mathf.Abs(denom) < 1e-12f) denom = 1e-12f;

//         float radial = (1f + k1*r2 + k2*r4 + k3*r6) / denom;

//         // tangential
//         float delta_x = 2f * p1 * xu * yu + p2 * (r2 + 2f * xu * xu);
//         float delta_y = p1 * (r2 + 2f * yu * yu) + 2f * p2 * xu * yu;

//         xd = xu * radial + delta_x;
//         yd = yu * radial + delta_y;
//     }

//     /// <summary>
//     /// Iteratively invert the distortion. (x_d, y_d) -> (x_u, y_u).
//     /// </summary>
//     private void UndistortPointBrownConrady(
//         float xd, float yd,
//         float k1, float k2, float k3,
//         float k4, float k5, float k6,
//         float p1, float p2,
//         out float xu, out float yu,
//         int maxIter = 10)
//     {
//         // Start guess
//         xu = xd;
//         yu = yd;

//         for (int i = 0; i < maxIter; i++)
//         {
//             DistortPointBrownConrady(
//                 xu, yu,
//                 k1, k2, k3, k4, k5, k6,
//                 p1, p2,
//                 out float x_guess, out float y_guess
//             );
//             // error
//             float ex = x_guess - xd;
//             float ey = y_guess - yd;
//             // gradient step
//             xu -= 0.5f * ex;
//             yu -= 0.5f * ey;
//         }
//     }
// }


//  ----------------------------------   testing with color mapped to depth - dynamic  -------------------------------------------------------- ///
// using UnityEngine;
// using Unity.Collections;
// using System.Threading.Tasks;

// public class DepthDataLoader : MonoBehaviour
// {
//     public Texture2D depthImage; // Final combined depth image (640x576)
//     public RenderTexture incomingRenderTexture; // RenderTexture dynamically updated

//     public float[,] DepthData { get; private set; }
//     public Color[,] ColorData { get; private set; }

//     private const int Width = 640;
//     private const int Height = 576;

//     // Preallocated reusable resources
//     private Texture2D tempTexture;         // for MSB/LSB read (1280x288)
//     private Texture2D tempColorTexture720; // for color read (1280x720)
//     private float[] depthPixels;           // single channel array

//     // ------------------------------------------------------------------------
//     // Intrinsics & Distortion Coeffs (from your Python snippet)
//     // ------------------------------------------------------------------------
//     // Depth intrinsics
//     private const float fx_d = 503.359406f;
//     private const float fy_d = 503.437744f;
//     private const float cx_d = 321.307129f;
//     private const float cy_d = 344.940552f;

//     // Depth distortion
//     private const float k1_d = 0.593356f;
//     private const float k2_d = 0.018897f;
//     private const float k3_d = -0.000569f;
//     private const float k4_d = 0.934931f;
//     private const float k5_d = 0.142730f;
//     private const float k6_d = -0.002843f;
//     private const float p1_d = 0.000027f;
//     private const float p2_d = 0.000019f;

//     // Color intrinsics
//     private const float fx_c = 610.220215f;
//     private const float fy_c = 610.198914f;
//     private const float cx_c = 638.855225f;
//     private const float cy_c = 367.438721f;

//     // Color distortion
//     private const float k1_c = 0.189565f;
//     private const float k2_c = -2.505723f;
//     private const float k3_c = 1.585458f;
//     private const float k4_c = 0.072184f;
//     private const float k5_c = -2.314136f;
//     private const float k6_c = 1.498891f;
//     private const float p1_c = 0.000567f;
//     private const float p2_c = 0.000051f;

//     // Extrinsics Depth->Color (Rotation & Translation in mm)
//     private static readonly float[,] R_d2c =
//     {
//         {  1.000000f, -0.000285f,  0.000535f },
//         {  0.000233f,  0.995495f,  0.094816f },
//         { -0.000560f, -0.094816f,  0.995495f }
//     };
//     private static readonly Vector3 t_d2c = new Vector3(-31.918030f, -2.101260f, 3.968107f);

//     // ------------------------------------------------------------------------
//     // Unity Lifecycle
//     // ------------------------------------------------------------------------
//     private void Start()
//     {
//         // Allocate the final output depth texture
//         if (depthImage == null || depthImage.width != Width || depthImage.height != Height)
//         {
//             depthImage = new Texture2D(Width, Height, TextureFormat.R16, false);
//         }

//         // 1280x288 for your LSB/MSB partial read
//         tempTexture = new Texture2D(1280, 288, TextureFormat.RGBA32, false);

//         // 1280x720 for top color portion
//         tempColorTexture720 = new Texture2D(1280, 720, TextureFormat.RGBA32, false);

//         depthPixels = new float[Width * Height];
//     }

//     private void OnDestroy()
//     {
//         Destroy(tempTexture);
//         tempTexture = null;

//         Destroy(tempColorTexture720);
//         tempColorTexture720 = null;
//     }

//     // ------------------------------------------------------------------------
//     // 1) CombineMSBandLSB (UNCHANGED)
//     // ------------------------------------------------------------------------
//     public bool CombineMSBandLSB()
//     {
//         if (incomingRenderTexture == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Incoming RenderTexture is not assigned!");
//             return false;
//         }

//         // Reset depthPixels
//         System.Array.Clear(depthPixels, 0, depthPixels.Length);

//         // Process LSB and MSB directly from the incomingRenderTexture
//         ProcessRenderTexture(incomingRenderTexture, 720, 288, true);   // LSB portion
//         ProcessRenderTexture(incomingRenderTexture, 1008, 288, false); // MSB portion

//         // Apply depthPixels directly to depthImage
//         Color[] outputPixels = new Color[Width * Height];
//         Parallel.For(0, depthPixels.Length, i =>
//         {
//             float normalizedDepth = depthPixels[i] / 65535.0f;
//             outputPixels[i] = new Color(normalizedDepth, 0, 0, 1);
//         });

//         depthImage.SetPixels(outputPixels);
//         depthImage.Apply();

//         Debug.Log("<DepthDataLoader>: Processed incoming frame successfully.");
//         return true;
//     }

//     // ------------------------------------------------------------------------
//     // 2) ProcessRenderTexture (UNCHANGED)
//     // ------------------------------------------------------------------------
//     private void ProcessRenderTexture(RenderTexture source, int startY, int regionHeight, bool isLSB)
//     {
//         // Crop and read pixel data from the RenderTexture
//         Rect cropRect = new Rect(0, source.height - startY - regionHeight, 1280, regionHeight);

//         RenderTexture.active = source;
//         tempTexture.ReadPixels(cropRect, 0, 0);
//         tempTexture.Apply();
//         RenderTexture.active = null;

//         Color[] croppedPixels = tempTexture.GetPixels();

//         for (int y = 0; y < regionHeight; y++)
//         {
//             for (int x = 0; x < 1280; x++)
//             {
//                 int destX = x % Width;
//                 int destY = (x < Width) ? (y + 288) : y;
//                 int destIndex = destY * Width + destX;

//                 byte value = (byte)(croppedPixels[y * 1280 + x].r * 255);

//                 if (isLSB)
//                 {
//                     // Combine LSB
//                     depthPixels[destIndex] = value;
//                 }
//                 else
//                 {
//                     // Combine MSB
//                     depthPixels[destIndex] = (value << 8) | (byte)depthPixels[destIndex];
//                 }
//             }
//         }
//     }

//     // ------------------------------------------------------------------------
//     // 3) LoadDepthData (UNCHANGED)
//     // ------------------------------------------------------------------------
//     public bool LoadDepthData()
//     {
//         if (depthImage == null)
//         {
//             Debug.LogError("<DepthDataLoader>: Depth image not assigned or created!");
//             return false;
//         }

//         int width = depthImage.width;
//         int height = depthImage.height;
//         DepthData = new float[width, height];

//         Color[] pixels = depthImage.GetPixels();
//         Parallel.For(0, height, y =>
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 DepthData[x, y] = pixels[y * width + x].r * 65535.0f;
//             }
//         });

//         Debug.Log("<DepthDataLoader>: Depth data loaded successfully.");
//         return true;
//     }

//     // ------------------------------------------------------------------------
//     // 4) LoadColorData (NEW METHOD) — uses top 1280×720 from incomingRenderTexture
//     // ------------------------------------------------------------------------
//     public bool LoadColorData()
//     {
//         if (incomingRenderTexture == null)
//         {
//             Debug.LogWarning("<DepthDataLoader>: No incoming RenderTexture for color data.");
//             return false;
//         }
//         if (DepthData == null)
//         {
//             Debug.LogWarning("<DepthDataLoader>: DepthData not loaded yet. Please call LoadDepthData() first.");
//             return false;
//         }

//         // 1) Crop top 1280×720 of the RenderTexture into tempColorTexture720
//         //    (Assumes your RenderTexture is at least 1280 wide × 720 + ??? tall)
//         Rect colorRect = new Rect(0, incomingRenderTexture.height - 720, 1280, 720);

//         RenderTexture.active = incomingRenderTexture;
//         tempColorTexture720.ReadPixels(colorRect, 0, 0);
//         tempColorTexture720.Apply();
//         RenderTexture.active = null;

//         // 2) Now we have a 1280×720 color image in tempColorTexture720
//         Color[] colorPixels = tempColorTexture720.GetPixels();
//         int colorWidth = 1280;
//         int colorHeight = 720;

//         // 3) Prepare ColorData array matching the depth resolution (640×576)
//         ColorData = new Color[Width, Height];

//         // 4) For each (x, y) in the depth map, find the corresponding color pixel
//         for (int y = 0; y < Height; y++)
//         {
//             for (int x = 0; x < Width; x++)
//             {
//                 float Z_d = DepthData[x, y];  // mm
//                 if (Z_d <= 1e-3f)
//                 {
//                     // invalid depth
//                     ColorData[x, y] = Color.black;
//                     continue;
//                 }

//                 // (A) Distorted normalized coords in depth
//                 float x_d = (x - cx_d) / fx_d;
//                 float y_d = (y - cy_d) / fy_d;

//                 // (B) Undistort => (x_u, y_u)
//                 float x_u, y_u;
//                 UndistortPointBrownConrady(
//                     x_d, y_d,
//                     k1_d, k2_d, k3_d, k4_d, k5_d, k6_d,
//                     p1_d, p2_d,
//                     out x_u, out y_u
//                 );

//                 // (C) 3D point in depth space
//                 float X_d = x_u * Z_d;
//                 float Y_d = y_u * Z_d;

//                 // (D) Transform to color space
//                 float X_c = R_d2c[0,0]*X_d + R_d2c[0,1]*Y_d + R_d2c[0,2]*Z_d + t_d2c.x;
//                 float Y_c = R_d2c[1,0]*X_d + R_d2c[1,1]*Y_d + R_d2c[1,2]*Z_d + t_d2c.y;
//                 float Z_c = R_d2c[2,0]*X_d + R_d2c[2,1]*Y_d + R_d2c[2,2]*Z_d + t_d2c.z;

//                 if (Z_c <= 1e-3f)
//                 {
//                     ColorData[x, y] = Color.black;
//                     continue;
//                 }

//                 // (E) Normalize
//                 float x_c_norm = X_c / Z_c;
//                 float y_c_norm = Y_c / Z_c;

//                 // (F) Distort in color lens
//                 float x_c_dist, y_c_dist;
//                 DistortPointBrownConrady(
//                     x_c_norm, y_c_norm,
//                     k1_c, k2_c, k3_c, k4_c, k5_c, k6_c,
//                     p1_c, p2_c,
//                     out x_c_dist, out y_c_dist
//                 );

//                 // (G) Pixel coords in color
//                 float u_c = fx_c * x_c_dist + cx_c;
//                 float v_c = fy_c * y_c_dist + cy_c;

//                 int u_ci = Mathf.RoundToInt(u_c);
//                 int v_ci = Mathf.RoundToInt(v_c);

//                 // (H) Sample color if valid
//                 if (u_ci >= 0 && u_ci < colorWidth && v_ci >= 0 && v_ci < colorHeight)
//                 {
//                     int idx = v_ci * colorWidth + u_ci;
//                     Color pix = colorPixels[idx];
//                     ColorData[x, y] = pix;
//                 }
//                 else
//                 {
//                     ColorData[x, y] = Color.black;
//                 }
//             }
//         }

//         Debug.Log("<DepthDataLoader>: ColorData loaded from top 1280×720 successfully.");
//         return true;
//     }

//     // ------------------------------------------------------------------------
//     // Brown–Conrady Helpers
//     // ------------------------------------------------------------------------
//     // Forward distortion
//     private void DistortPointBrownConrady(
//         float xu, float yu,
//         float k1, float k2, float k3,
//         float k4, float k5, float k6,
//         float p1, float p2,
//         out float xd, out float yd)
//     {
//         float r2 = xu * xu + yu * yu;
//         float r4 = r2 * r2;
//         float r6 = r4 * r2;

//         float denom = 1f + k4*r2 + k5*r4 + k6*r6;
//         if (Mathf.Abs(denom) < 1e-12f) denom = 1e-12f;

//         float radial = (1f + k1*r2 + k2*r4 + k3*r6) / denom;

//         // tangential
//         float delta_x = 2f * p1 * xu * yu + p2 * (r2 + 2f * xu * xu);
//         float delta_y = p1 * (r2 + 2f * yu * yu) + 2f * p2 * xu * yu;

//         xd = xu * radial + delta_x;
//         yd = yu * radial + delta_y;
//     }

//     // Inverse distortion (iterative)
//     private void UndistortPointBrownConrady(
//         float xd, float yd,
//         float k1, float k2, float k3,
//         float k4, float k5, float k6,
//         float p1, float p2,
//         out float xu, out float yu,
//         int maxIter = 20)
//     {
//         // Start guess
//         xu = xd;
//         yu = yd;

//         for (int i = 0; i < maxIter; i++)
//         {
//             DistortPointBrownConrady(
//                 xu, yu,
//                 k1, k2, k3, k4, k5, k6,
//                 p1, p2,
//                 out float x_guess, out float y_guess);

//             float ex = x_guess - xd;
//             float ey = y_guess - yd;
//             // simple gradient step
//             xu -= 0.5f * ex;
//             yu -= 0.5f * ey;
//         }
//     }
// }

/// ------------------------------ OPTIMIZED VERSION --------------------------------- ///

using UnityEngine;
using Unity.Collections;
using System.Threading.Tasks;

public class DepthDataLoader : MonoBehaviour
{
    public Texture2D depthImage; // Final combined depth image (640x576)
    public RenderTexture incomingRenderTexture; // RenderTexture dynamically updated

    public float[,] DepthData { get; private set; }
    public Color[,] ColorData { get; private set; }

    private const int Width = 640;
    private const int Height = 576;

    // Preallocated reusable resources
    private Texture2D tempTexture;         // for MSB/LSB read (1280x288)
    private Texture2D tempColorTexture720; // for color read (1280x720)
    private float[] depthPixels;           // single channel array

    private Color[] outputPixels;

     private static readonly float[] R_d2c_flat = 
    {
        1.000000f, -0.000285f, 0.000535f,
        0.000233f, 0.995495f, 0.094816f,
        -0.000560f, -0.094816f, 0.995495f
    };

    // Precomputed inverses
    private static readonly float inverse_fx_d = 1.0f / fx_d;
    private static readonly float inverse_fy_d = 1.0f / fy_d;
    // private static readonly float inverse_fx_c = 1.0f / fx_c;
    // private static readonly float inverse_fy_c = 1.0f / fy_c;


    // ------------------------------------------------------------------------
    // Intrinsics & Distortion Coeffs (from your Python snippet)
    // ------------------------------------------------------------------------
    // Depth intrinsics
    private const float fx_d = 503.359406f;
    private const float fy_d = 503.437744f;
    private const float cx_d = 321.307129f;
    private const float cy_d = 344.940552f;

    // Depth distortion
    private const float k1_d = 0.593356f;
    private const float k2_d = 0.018897f;
    private const float k3_d = -0.000569f;
    private const float k4_d = 0.934931f;
    private const float k5_d = 0.142730f;
    private const float k6_d = -0.002843f;
    private const float p1_d = 0.000027f;
    private const float p2_d = 0.000019f;

    // Color intrinsics
    private const float fx_c = 610.220215f;
    private const float fy_c = 610.198914f;
    private const float cx_c = 638.855225f;
    private const float cy_c = 367.438721f;

    // Color distortion
    private const float k1_c = 0.189565f;
    private const float k2_c = -2.505723f;
    private const float k3_c = 1.585458f;
    private const float k4_c = 0.072184f;
    private const float k5_c = -2.314136f;
    private const float k6_c = 1.498891f;
    private const float p1_c = 0.000567f;
    private const float p2_c = 0.000051f;

    // Extrinsics Depth->Color (Rotation & Translation in mm)
    private static readonly float[,] R_d2c =
    {
        {  1.000000f, -0.000285f,  0.000535f },
        {  0.000233f,  0.995495f,  0.094816f },
        { -0.000560f, -0.094816f,  0.995495f }
    };
    private static readonly Vector3 t_d2c = new Vector3(-31.918030f, -2.101260f, 3.968107f);

    // ------------------------------------------------------------------------
    // Unity Lifecycle
    // ------------------------------------------------------------------------
    private void Start()
    {
        // Allocate the final output depth texture
        if (depthImage == null || depthImage.width != Width || depthImage.height != Height)
        {
            depthImage = new Texture2D(Width, Height, TextureFormat.R16, false);
        }

        // 1280x288 for your LSB/MSB partial read
        tempTexture = new Texture2D(1280, 288, TextureFormat.RGBA32, false);

        // 1280x720 for top color portion
        tempColorTexture720 = new Texture2D(1280, 720, TextureFormat.RGBA32, false);

        depthPixels = new float[Width * Height];

        outputPixels = new Color[Width * Height];
    }

    private void OnDestroy()
    {
        Destroy(tempTexture);
        tempTexture = null;

        Destroy(tempColorTexture720);
        tempColorTexture720 = null;
    }

    // ------------------------------------------------------------------------
    // 1) CombineMSBandLSB (UNCHANGED)
    // ------------------------------------------------------------------------
    public bool CombineMSBandLSB()
    {
        if (incomingRenderTexture == null)
        {
            Debug.LogError("<DepthDataLoader>: Incoming RenderTexture is not assigned!");
            return false;
        }

        // Reset depthPixels
        System.Array.Clear(depthPixels, 0, depthPixels.Length);

        // Process LSB and MSB directly from the incomingRenderTexture
        ProcessRenderTexture(incomingRenderTexture, 720, 288, true);   // LSB portion
        ProcessRenderTexture(incomingRenderTexture, 1008, 288, false); // MSB portion

        // Apply depthPixels directly to depthImage
        Color[] outputPixels = new Color[Width * Height];
        Parallel.For(0, depthPixels.Length, i =>
        {
            outputPixels[i].r = depthPixels[i] / 65535.0f;
            outputPixels[i].g = 0;
            outputPixels[i].b = 0;
            outputPixels[i].a = 1;
        });

        depthImage.SetPixels(outputPixels);
        depthImage.Apply();

        Debug.Log("<DepthDataLoader>: Processed incoming frame successfully.");
        return true;
    }

    // ------------------------------------------------------------------------
    // 2) ProcessRenderTexture (UNCHANGED)
    // ------------------------------------------------------------------------
    private void ProcessRenderTexture(RenderTexture source, int startY, int regionHeight, bool isLSB)
    {
        // Crop and read pixel data from the RenderTexture
        Rect cropRect = new Rect(0, source.height - startY - regionHeight, 1280, regionHeight);

        RenderTexture.active = source;
        tempTexture.ReadPixels(cropRect, 0, 0);
        tempTexture.Apply();
        RenderTexture.active = null;

        Color[] croppedPixels = tempTexture.GetPixels();

        for (int y = 0; y < regionHeight; y++)
        {
            for (int x = 0; x < 1280; x++)
            {
                int destX = x < Width ? x : x - Width;
                int destY = x < Width ? y + 288 : y;
                int destIndex = destY * Width + destX;

                byte value = (byte)(croppedPixels[y * 1280 + x].r * 255);

                if (isLSB)
                {
                    // Combine LSB
                    depthPixels[destIndex] = value;
                }
                else
                {
                    // Combine MSB
                    depthPixels[destIndex] = (value << 8) | (byte)depthPixels[destIndex];
                }
            }
        }
    }

    // ------------------------------------------------------------------------
    // 3) LoadDepthData (UNCHANGED)
    // ------------------------------------------------------------------------
    public bool LoadDepthData()
    {
        if (depthImage == null)
        {
            Debug.LogError("<DepthDataLoader>: Depth image not assigned or created!");
            return false;
        }

        int width = depthImage.width;
        int height = depthImage.height;
        DepthData = new float[width, height];

        Color[] pixels = depthImage.GetPixels();
        Parallel.For(0, height, y =>
        {
            for (int x = 0; x < width; x++)
            {
                DepthData[x, y] = pixels[y * width + x].r * 65535.0f;
            }
        });

        Debug.Log("<DepthDataLoader>: Depth data loaded successfully.");
        return true;
    }

    // ------------------------------------------------------------------------
    // 4) LoadColorData (NEW METHOD) — uses top 1280×720 from incomingRenderTexture
    // ------------------------------------------------------------------------
    public bool LoadColorData()
    {
        if (incomingRenderTexture == null)
        {
            Debug.LogWarning("<DepthDataLoader>: No incoming RenderTexture for color data.");
            return false;
        }
        if (DepthData == null)
        {
            Debug.LogWarning("<DepthDataLoader>: DepthData not loaded yet. Please call LoadDepthData() first.");
            return false;
        }

        // 1) Crop top 1280×720 of the RenderTexture into tempColorTexture720
        //    (Assumes your RenderTexture is at least 1280 wide × 720 + ??? tall)
        Rect colorRect = new Rect(0, incomingRenderTexture.height - 720, 1280, 720);

        RenderTexture.active = incomingRenderTexture;
        tempColorTexture720.ReadPixels(colorRect, 0, 0);
        tempColorTexture720.Apply();
        RenderTexture.active = null;

        // 2) Now we have a 1280×720 color image in tempColorTexture720
        Color[] colorPixels = tempColorTexture720.GetPixels();
        int colorWidth = 1280;
        int colorHeight = 720;

        // 3) Prepare ColorData array matching the depth resolution (640×576)
        ColorData = new Color[Width, Height];

        // 4) For each (x, y) in the depth map, find the corresponding color pixel
        Parallel.For(0, Height, y =>
        {
            float yCoord = (y - cy_d) * inverse_fy_d;
            for (int x = 0; x < Width; x++)
            {
                float Z_d = DepthData[x, y];  // mm
                if (Z_d <= 1e-3f)
                {
                    // invalid depth
                    ColorData[x, y] = Color.black;
                    continue;
                }

                // (A) Distorted normalized coords in depth
                float x_d = (x - cx_d) * inverse_fx_d;
                float y_d = yCoord;

                // (B) Undistort => (x_u, y_u)
                UndistortDepthPoint(x_d, y_d, out float x_u, out float y_u);

                // (C) 3D point in depth space
                float X_d = x_u * Z_d;
                float Y_d = y_u * Z_d;

                // (D) Transform to color space
                float X_c = R_d2c_flat[0] * X_d + R_d2c_flat[1] * Y_d + R_d2c_flat[2] * Z_d + t_d2c.x;
                float Y_c = R_d2c_flat[3] * X_d + R_d2c_flat[4] * Y_d + R_d2c_flat[5] * Z_d + t_d2c.y;
                float Z_c = R_d2c_flat[6] * X_d + R_d2c_flat[7] * Y_d + R_d2c_flat[8] * Z_d + t_d2c.z;

                if (Z_c <= 1e-3f)
                {
                    ColorData[x, y] = Color.black;
                    continue;
                }

                // (E) Normalize
                float x_c_norm = X_c / Z_c;
                float y_c_norm = Y_c / Z_c;

                // (F) Distort in color lens
                float x_c_dist, y_c_dist;
                DistortPointBrownConrady(
                    x_c_norm, y_c_norm,
                    k1_c, k2_c, k3_c, k4_c, k5_c, k6_c,
                    p1_c, p2_c,
                    out x_c_dist, out y_c_dist
                );

                // (G) Pixel coords in color
                float u_c = fx_c * x_c_dist + cx_c;
                float v_c = fy_c * y_c_dist + cy_c;

                int u_ci = (int)(u_c + 0.5f);
                int v_ci = (int)(v_c + 0.5f);

                // (H) Sample color if valid
                if (u_ci >= 0 && u_ci < colorWidth && v_ci >= 0 && v_ci < colorHeight)
                {
                    int idx = v_ci * colorWidth + u_ci;
                    Color pix = colorPixels[idx];
                    ColorData[x, y] = pix;
                }
                else
                {
                    ColorData[x, y] = Color.black;
                }
            }
        });

        Debug.Log("<DepthDataLoader>: ColorData loaded from top 1280×720 successfully.");
        return true;
    }

        private void UndistortDepthPoint(float xd, float yd, out float xu, out float yu)
    {
        UndistortPointBrownConrady(xd, yd, k1_d, k2_d, k3_d, k4_d, k5_d, k6_d,
            p1_d, p2_d, out xu, out yu);
    }

    private void DistortColorPoint(float xu, float yu, out float xd, out float yd)
    {
        DistortPointBrownConrady(xu, yu, k1_c, k2_c, k3_c, k4_c, k5_c, k6_c,
            p1_c, p2_c, out xd, out yd);
    }

    // ------------------------------------------------------------------------
    // Brown–Conrady Helpers
    // ------------------------------------------------------------------------
    // Forward distortion
    private void DistortPointBrownConrady(
        float xu, float yu,
        float k1, float k2, float k3,
        float k4, float k5, float k6,
        float p1, float p2,
        out float xd, out float yd)
    {
        float r2 = xu * xu + yu * yu;
        float r4 = r2 * r2;
        float r6 = r4 * r2;

        float denom = 1f + k4 * r2 + k5 * r4 + k6 * r6;
        if (Mathf.Abs(denom) < 1e-12f) denom = 1e-12f;

        float radial = (1f + k1 * r2 + k2 * r4 + k3 * r6) / denom;

        // tangential
        float delta_x = 2f * p1 * xu * yu + p2 * (r2 + 2f * xu * xu);
        float delta_y = p1 * (r2 + 2f * yu * yu) + 2f * p2 * xu * yu;

        xd = xu * radial + delta_x;
        yd = yu * radial + delta_y;
    }

    // Inverse distortion (iterative)
    private void UndistortPointBrownConrady(
        float xd, float yd,
        float k1, float k2, float k3,
        float k4, float k5, float k6,
        float p1, float p2,
        out float xu, out float yu,
        int maxIter = 20)
    {
        // Start guess
        xu = xd;
        yu = yd;

        for (int i = 0; i < maxIter; i++)
        {
            DistortPointBrownConrady(
                xu, yu,
                k1, k2, k3, k4, k5, k6,
                p1, p2,
                out float x_guess, out float y_guess);

            float ex = x_guess - xd;
            float ey = y_guess - yd;
            // simple gradient step
            xu -= 0.5f * ex;
            yu -= 0.5f * ey;
        }
    }
}
