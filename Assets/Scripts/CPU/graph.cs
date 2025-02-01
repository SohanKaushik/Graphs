using Unity.Mathematics;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] Transform m_prefab;
    [SerializeField] ComputeShader shader;

    [SerializeField]
    float size, speed;

    [SerializeField, Min(0)]
    float transitionDuration, functionDuration;


    [SerializeField, Range(10, 1000)]
    int resolution = 10;


    [SerializeField]
    bool parallelism = false;

    [SerializeField]
    FunctionLibrary.FunctionType type;


    //PRIVATES
    private float elapsed = 0f;
    bool transitioning;
    Transform[] points;


    // Compute Shader Variables
    private ComputeBuffer buffer_position;
    private ComputeBuffer buffer_scale;
    private int kernel;
    private float3[] positions;

    private int total;
    private float step;

    void Awake()
    {
        total = resolution * resolution;
        points = new Transform[total];         // matrix x matrix = 2D
    }

    void Start()
    {
        step = 2f / (resolution - 1);
        var scale = Vector3.one * step;             // scaling based on resolution

        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i] = Instantiate(m_prefab);

            point.localScale = scale;
            point.SetParent(transform, false);
        }

        //if (parallelism)
        //{
        //    InitComputeShader();
        //}
    }
    void Update()
    {
        //elapsed += Time.deltaTime;

        //if (transitioning)
        //{
        //    if (elapsed >= transitionDuration)
        //    {
        //        transitioning = false;
        //        elapsed = 0f; // Reset elapsed only after the transition is complete
        //        type = type + 1; // Move to the next function
        //    }
        //}
        //else if (elapsed >= functionDuration)
        //{
        //    transitioning = true;
        //    elapsed = 0f;
        //}

        //if (transitioning)
        //{
        //    TransitionFunction();
        //}
        //else
        //{
        //    UpdateFunction();
        //}

        //if (parallelism)
        //{
        //    UpdateComputeShader();
        //}
        //else
        //{
        //}
        UpdateFunction();

    }


    void TransitionFunction(){
        FunctionLibrary.Function from = FunctionLibrary.GetFunction(type),
                                 to = FunctionLibrary.GetFunction(type + 1);

        float step = 2f / resolution;
        float progress = elapsed  / transitionDuration;

        //BEGIN
        int index = 0; // Linear index for 1D array
        for (int z = 0; z < resolution; z++) // Iterate over rows (z-axis)
        {
            float v = z * step - 1.0f;
            for (int x = 0; x < resolution; x++, index++) // Iterate over columns (x-axis)
            {
                float u = x * step - 1.0f;
                u = Mathf.Clamp(u, -1.0f, 1.0f);

                points[index].localPosition = FunctionLibrary.morph(u, v, Time.time, speed, from, to, progress);
            }
        }
        //END
    }

    void UpdateFunction() {

        float step = 2f / (resolution - 1); // Spacing between points

        FunctionLibrary.Function function = FunctionLibrary.GetFunction(type);
        //BEGIN
        int index = 0; // Linear index for 1D array
        for (int z = 0; z < resolution; z++) // Iterate over rows (z-axis)
        {
            float v = z * step - 1.0f;
            for (int x = 0; x < resolution; x++, index++) // Iterate over columns (x-axis)
            {
                float u = x * step - 1.0f;
                u = Mathf.Clamp(u, -1.0f, 1.0f);
                points[index].localPosition = function(u, v, Time.time, speed);
            }
        }
        //END
    }

    void InitComputeShader()
    {
        // Compute Buffer
        buffer_position = new ComputeBuffer(total, sizeof(float) * 3);    // vector contains -> 3 floats
        kernel = shader.FindKernel("CSMain");

        // Set constant parameters
        float step = 2f / (resolution - 1);
        shader.SetFloat("step", step);
        shader.SetFloat("speed", speed);
        shader.SetInt("resolution", resolution);


        positions = new float3[total];
        shader.SetBuffer(kernel, "positions", buffer_position);   // set buffer -> compute shader
    }

    void UpdateComputeShader()
    {
        shader.SetFloat("time", Time.time);   // as it not constant

        int thread_x = Mathf.CeilToInt(resolution / 64.0f);       // thread for x 
        int thread_y = Mathf.CeilToInt(resolution / 64.0f);       // thread for y

        shader.Dispatch(kernel, thread_x, thread_y, 1);
        buffer_position.GetData(positions);                       // storing data from compute shader -> positions[]

        
        for (int i = 0; i < total; i++){
            points[i].localPosition = positions[i]; 
        }
    }

    private void OnDestroy()
    {
        if (shader && parallelism)
        {
            buffer_position.Release();
        }
    }
}
