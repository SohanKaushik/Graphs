using System;
using Unity.Mathematics;
using UnityEngine;
public class GPUGraph : MonoBehaviour
{ 
    [SerializeField, Range(10, 1000)] int resolution = 10;
    [SerializeField] float size, speed;


    [SerializeField]
    ComputeShader shader;

    [SerializeField] Material material;
    [SerializeField] Mesh mesh;



    static readonly int
        positionsId = Shader.PropertyToID("_positions"),
        resId = Shader.PropertyToID("_resolution"),
        stepId = Shader.PropertyToID("_step"),
        timeId = Shader.PropertyToID("_time");

    // Compute Shader Variables
    private ComputeBuffer buffer_position;
    private int kernel;
    private float3[] positions;

    private int total;
    private float step;

    private void Start()
    {

    }
    void OnEnable()
    {
        total = resolution * resolution;
        step = size;

        buffer_position = new ComputeBuffer(total, 4 * 3);
        positions = new float3[total]; // Initialize the positions array

        kernel = shader.FindKernel("CSMain");
    }
    void Update()
    {
        UpdateFunction();
    }

    void UpdateFunction()
    {
        shader.SetInt(resId, resolution);
        shader.SetFloat(stepId, step);
        shader.SetFloat(timeId, Time.time);

        shader.SetBuffer(kernel, positionsId, buffer_position);

        int groups = Mathf.CeilToInt((float)resolution / 8.0f);
        shader.Dispatch(kernel, groups, groups, 1);

        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));

        material.SetBuffer(positionsId, buffer_position);
        material.SetFloat(stepId, step);

        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, resolution * resolution);

        buffer_position.GetData(positions); // Optional: Debug positions
    }

    private void OnDisable()
    {
        buffer_position.Release();
        //buffer_position = null;
    }
}
