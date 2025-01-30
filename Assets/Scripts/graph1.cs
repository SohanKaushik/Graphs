using System;
using Unity.Mathematics;
using UnityEngine;
public class GPUGraph : MonoBehaviour
{ 
    [SerializeField, Range(10, 1000)] int resolution = 10;
    [SerializeField] float size, speed;

    [SerializeField, Min(0)] float transitionDuration, functionDuration;

    [SerializeField]
    ComputeShader shader;

    [SerializeField] Material material;
    [SerializeField] Mesh mesh;


    //PRIVATES
    private float elapsed = 0f;
    bool transitioning;

    static readonly int
        positionsId = Shader.PropertyToID("_Positions"),
        resId = Shader.PropertyToID("_reesolution"),
        stepId = Shader.PropertyToID("_step"),
        timeId = Shader.PropertyToID("_time");

    // Compute Shader Variables
    private ComputeBuffer buffer_position;
    private ComputeBuffer buffer_scale;
    private int kernel;
    private float3[] positions;

    private int total;
    private float step;

    void OnEnable()
    {
        buffer_position = new ComputeBuffer(resolution * resolution, sizeof(float) * 3);

        kernel = shader.FindKernel("CSMain");
    }

    void Update()
    {
        UpdateFunction();
    }

    void UpdateFunction()
    {
        float step = 2f / resolution;
        shader.SetInt(resId, resolution);
        shader.SetFloat(stepId, step);
        shader.SetFloat(timeId, Time.time);

        // Find the kernel by name
        int kernel = shader.FindKernel("CSMain");

        shader.SetBuffer(kernel, "positions", buffer_position);

        int groups = Mathf.CeilToInt(resolution / 8.0f);
        shader.Dispatch(kernel, groups, groups, 1);

        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, buffer_position.count);
    }
    private void OnDisable()
    {
        buffer_position.Release();
        buffer_position = null;
    }
}
