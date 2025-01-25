Shader "Custom/point_shader"
{
    Properties
    {
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Color1 ("Color 1", Color) = (1,0,0,1) // Red
        _Color2 ("Color 2", Color) = (0,1,0,1) // Green
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0 // Explicitly set target shader model

      
        // Declare properties
        half _Glossiness;
        float4 _Color1, _Color2;
        float _Value;

        struct Input {
            // it cant be empty
            float3 worldPos : WORLD_POSITION;
            float3 normal : NORMAL;
            float2 uv_MainTex; 
        };

        void surf(Input input, inout SurfaceOutputStandard o) {

          

            // Use the normal components to set Albedo
            o.Albedo.rg = input.worldPos.xy * 0.5 + 0.5;
            o.Smoothness = _Glossiness;
            //o.Smoothness = _Smoothness;
        }

        ENDCG
    }

    Fallback "Diffuse"  // always need for surface shaders
}
