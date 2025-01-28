using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;

    [SerializeField]
    float size, speed;

    [SerializeField, Min(0)]
    float transitionDuration, functionDuration;


    [SerializeField, Range(10, 1000)]
    int resolution = 10;


    [SerializeField]
    FunctionLibrary.FunctionType type;


    //PRIVATES
    private float elapsed = 0f;
    bool transitioning;
    Transform[] points;

    void Awake()
    {
        float step = 2f / (resolution - 1);        // making spaces
        var scale = Vector3.one * step;             // scaling based on resolution

        points = new Transform[resolution * resolution];         // matrix x matrix = 2D

        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i] = Instantiate(pointPrefab);

            point.localScale = scale;
            point.SetParent(transform, false);
        }

    }

    void Update()
    {
        elapsed += Time.deltaTime;

        if (transitioning)
        {
            if (elapsed >= transitionDuration)
            {
                transitioning = false;
                elapsed = 0f; // Reset elapsed only after the transition is complete
                type = type + 1; // Move to the next function
            }
        }
        else if (elapsed >= functionDuration)
        {
            transitioning = true;
            elapsed = 0f;
        }

        if (transitioning)
        {
            TransitionFunction();
        }
        else
        {
            UpdateFunction();
        }
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

                points[index].localPosition = function(u, v, Time.time, speed);
            }
        }
        //END
    }
}
