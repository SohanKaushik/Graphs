#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
        StructuredBuffer<float3> _positions;
#endif

float _step; // Global scale for instances

void ConfigureProcedural()
{
#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
        // Get position for the current instance
        float3 position = _positions[unity_InstanceID];
        
        // Build the transformation matrix
        unity_ObjectToWorld = 0.0;
        unity_ObjectToWorld._m03_m13_m23_m33 = float4(position, 1.0); // Position
        unity_ObjectToWorld._m00_m11_m22 = _step ; // Scale
#endif
}

void ShaderGraphFunction_float(float3 In, out float3 Out)
{
    Out = In;
}

void ShaderGraphFunction_half(half3 In, out half3 Out)
{
    Out = In;
}