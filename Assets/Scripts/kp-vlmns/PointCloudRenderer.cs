// using UnityEngine;
// using System.Collections.Generic;

// public class PointCloudRenderer : MonoBehaviour
// {
//     private PointCloudGenerator pointCloudGenerator; 
//     private Mesh mesh;

//     public Material pointMaterial; // Should be a shader that supports vertex colors

//     public void RenderPointCloud()
//     {
//         pointCloudGenerator = GetComponent<PointCloudGenerator>();
//         if (pointCloudGenerator == null)
//         {
//             Debug.LogError("<PointCloudRenderer>: PointCloudGenerator is missing!");
//             return;
//         }

//         if (pointCloudGenerator.Points == null || pointCloudGenerator.Points.Count == 0)
//         {
//             Debug.LogError("<PointCloudRenderer>: No points available for rendering.");
//             return;
//         }

//         if (pointCloudGenerator.PointColors == null || pointCloudGenerator.PointColors.Count != pointCloudGenerator.Points.Count)
//         {
//             Debug.LogError("<PointCloudRenderer>: PointColors are missing or mismatched with points.");
//             return;
//         }

//         // Create a new mesh
//         mesh = new Mesh();
//         mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // Support large point counts

//         Vector3[] vertices = pointCloudGenerator.Points.ToArray();
//         Color[] colors = pointCloudGenerator.PointColors.ToArray();

//         // Create indices
//         int[] indices = new int[vertices.Length];
//         for (int i = 0; i < vertices.Length; i++)
//         {
//             indices[i] = i;
//         }

//         // Assign data to the mesh
//         mesh.vertices = vertices;
//         mesh.colors = colors;
//         mesh.SetIndices(indices, MeshTopology.Points, 0);

//         // Add or reuse MeshFilter and MeshRenderer
//         MeshFilter filter = gameObject.GetComponent<MeshFilter>();
//         if (filter == null)
//         {
//             filter = gameObject.AddComponent<MeshFilter>();
//         }
//         filter.mesh = mesh;

//         MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
//         if (renderer == null)
//         {
//             renderer = gameObject.AddComponent<MeshRenderer>();
//         }
//         renderer.material = pointMaterial;

//         Debug.Log($"<PointCloudRenderer>: Rendered {vertices.Length} points with colors.");
//     }
// }


/// ----------------- Optimized --------------------
/// 
using UnityEngine;
using System.Collections.Generic;
public class PointCloudRenderer : MonoBehaviour
{
    private PointCloudGenerator pointCloudGenerator;
    private Mesh mesh;

    public Material pointMaterial; // Should be a shader that supports vertex colors

    public void RenderPointCloud()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        }
        else
        {
            mesh.Clear();
        }

        pointCloudGenerator = GetComponent<PointCloudGenerator>();
        if (pointCloudGenerator == null)
        {
            Debug.LogError("<PointCloudRenderer>: PointCloudGenerator is missing!");
            return;
        }

        // If 'pointCloudGenerator.Points' is a Vector3[] then do this:
        // If it's a List<Vector3>, you can revert back to using .Count or .ToArray() as before.
        if (pointCloudGenerator.Points == null || pointCloudGenerator.Points.Length == 0)
        {
            Debug.LogError("<PointCloudRenderer>: No points available for rendering.");
            return;
        }

        // Same idea for 'pointCloudGenerator.PointColors' if it's Color[]
        if (pointCloudGenerator.PointColors == null ||
            pointCloudGenerator.PointColors.Length != pointCloudGenerator.Points.Length)
        {
            Debug.LogError("<PointCloudRenderer>: PointColors are missing or mismatched with points.");
            return;
        }

        // Here, 'Points' and 'PointColors' are already arrays, so we can assign them directly
        Vector3[] vertices = pointCloudGenerator.Points;
        Color[] colors = pointCloudGenerator.PointColors;

        // Create indices for each vertex
        int[] indices = new int[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            indices[i] = i;
        }

        // Assign data to the mesh
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.SetIndices(indices, MeshTopology.Points, 0);

        // Add or reuse MeshFilter and MeshRenderer
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        if (filter == null)
        {
            filter = gameObject.AddComponent<MeshFilter>();
        }
        filter.mesh = mesh;

        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            renderer = gameObject.AddComponent<MeshRenderer>();
        }
        renderer.material = pointMaterial;

        Debug.Log($"<PointCloudRenderer>: Rendered {vertices.Length} points with colors.");
    }
}