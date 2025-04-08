Shader "Custom/ColorPointCloud"
{
    Properties
    {
        // Here you can define properties if needed. For a simple vertex color shader, we may not need any.
        // _PointSize("Point Size", Range(1.0, 50.0)) = 2.0
    }
    SubShader
    {
        // Basic pipeline settings
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }
        Cull Off           // Optional: turn off back-face culling for points
        ZWrite On          // Ensure we write to depth buffer (so points can occlude each other)
        ZTest LEqual       // Default depth test

        Pass
        {
            CGPROGRAM
            // Tell Unity we are writing both a vertex and fragment program
            #pragma vertex vert
            #pragma fragment frag
            
            // Include commonly used Unity shader macros and functions
            #include "UnityCG.cginc"

            // Our input struct from the Mesh data
            // 'appdata' stands for "application data"
            struct appdata
            {
                float4 vertex : POSITION;  // The vertex position in object space
                float4 color  : COLOR;     // The vertex color (from the mesh)
            };

            // Our output struct going from vertex to fragment
            struct v2f
            {
                float4 pos   : SV_POSITION; // The clip-space position
                float4 color : COLOR;       // The color to pass to the fragment
            };

            // Vertex shader function
            v2f vert (appdata IN)
            {
                v2f OUT;
                
                // Convert object-space position to clip-space
                // This applies model, view, projection matrices.
                OUT.pos = UnityObjectToClipPos(IN.vertex);

                // Pass the vertex color along to the fragment
                OUT.color = IN.color;

                return OUT;
            }

            // Fragment (pixel) shader function
            float4 frag (v2f IN) : SV_Target
            {
                // Output the vertex color as the final color
                return IN.color;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Color"
}
