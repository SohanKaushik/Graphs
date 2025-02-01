using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public class GPUGraph : MonoBehaviour
{
    const int maxResolution = 5000;


    [SerializeField] TextMeshProUGUI count;
    [SerializeField] private Slider slider;

    [SerializeField]
    ComputeShader shader;

    [SerializeField] Material material;
    [SerializeField] Mesh mesh;

    private int resolution;


    static readonly int
        positionsId = Shader.PropertyToID("_positions"),
        resId = Shader.PropertyToID("_resolution"),
        stepId = Shader.PropertyToID("_step"),
        timeId = Shader.PropertyToID("_time");

    // Compute Shader Variables
    private ComputeBuffer buffer_position;
    private int kernel;
    private float3[] positions;

    private float step;

    private void Awake()
    {

        // Set up slider limits
        slider.minValue = 10;
        slider.maxValue = maxResolution;
        slider.wholeNumbers = true;

        slider.onValueChanged.AddListener(OnSliderChanged);
        resolution = (int)slider.value; // Set resolution to initial slider value

        kernel = shader.FindKernel("CSMain");
    }

    // Max Resolution -> creates buffer positions for maximum number of enteries;
    void OnEnable()
    {
        buffer_position = new ComputeBuffer(maxResolution * maxResolution, sizeof(float) * 3);  // stride -> position has 3 floats
    }
    void Update()
    {
        UpdateFunction();
    }

    void UpdateFunction()
    {
        step = 2f / (resolution - 1);

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
    }

    public void OnSliderChanged(float value)
    {
        resolution = (int)value; // Update resolution from slider
        int total = resolution * resolution;
        count.text = total.ToString("N0");
    }


    private void OnDisable()
    {
        buffer_position.Release();
        buffer_position = null;
    }
}