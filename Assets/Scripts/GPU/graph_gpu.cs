using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GPUGraph : MonoBehaviour
{
    const int maxResolution = 7071;

    [SerializeField] TextMeshProUGUI count;
    [SerializeField] private Slider slider;

    [SerializeField] Material material;
    [SerializeField] Mesh mesh;

    [SerializeField] ComputeShader shader;
    
    // it uses as range between max resolution
    private int resolution;

    private ComputeBuffer buffer_position;

    // Kernel -> links to the compute shader 
    private int kernel; 
    private float step;

    // Store values for the compute shader : Uniforms IN
    static readonly int
        positionsId = Shader.PropertyToID("_positions"),
        resId = Shader.PropertyToID("_resolution"),
        stepId = Shader.PropertyToID("_step"),
        timeId = Shader.PropertyToID("_time");

    private void Awake()
    {
        slider.minValue = 10;
        slider.maxValue = maxResolution;
        slider.wholeNumbers = true;

        slider.onValueChanged.AddListener(OnSliderChanged);
        resolution = (int)slider.value;

        kernel = shader.FindKernel("CSMain");


        // Instantiate -> single vertex if none are given.
        if(mesh == null)
        {
            mesh = new Mesh();
            int[] indices = { 0 };

            mesh.vertices = new[] { Vector3.zero };
            mesh.SetIndices(indices, MeshTopology.Points, 0);
        }
    }

    void OnEnable()
    {
        // buffer -> allocates the positions into this buffer for maximum resolution
        buffer_position = new ComputeBuffer(maxResolution * maxResolution, sizeof(float) * 3);
    }

    void Update()
    {
        UpdateFunction();
    }

    void UpdateFunction()
    {
        step = 2f / (resolution - 1);


        // Deport values to the compute shader : Set Uniforms
        shader.SetInt(resId, resolution);
        shader.SetFloat(stepId, step);
        shader.SetFloat(timeId, Time.time);

        shader.SetBuffer(kernel, positionsId, buffer_position);

        int groups = Mathf.CeilToInt((float)resolution / 8.0f);
        shader.Dispatch(kernel, groups, groups, 1);


        // It is a frustum culling
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));

        material.SetBuffer(positionsId, buffer_position);
        material.SetFloat(stepId, step);

        // Render as single points
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, resolution * resolution);
    }

    public void OnSliderChanged(float value)
    {
        resolution = (int)value;
        int total = resolution * resolution;
        count.text = total.ToString("N0");
    }

    // clean ups
    private void OnDisable()
    {
        buffer_position.Release();
        buffer_position = null;
    }
}
