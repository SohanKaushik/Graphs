using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;

    [SerializeField]
    float size, speed;
    [SerializeField]
    FunctionLibrary.FunctionName functionName;

    [SerializeField, Range(10, 1000)]
    int resolution = 10;

    Transform[] points;

    void Start()
    {

    }
    void Awake()
    {
        float step = 2f / (resolution - 1);        // making spaces
        var scale = Vector3.one * step;             // scaling based on resolution

        points = new Transform[resolution * resolution];         // matrix x matrix = 2D

        for (int i = 0; i < resolution * resolution; i++)
        {
            Transform point = points[i] = Instantiate(pointPrefab);

            point.localScale = scale;
            point.SetParent(transform, false);
        }

    }

    void Update()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(functionName);
        float step = 2f / (resolution - 1); // Spacing between points

        int index = 0; // Linear index for 1D array
        for (int z = 0; z < resolution; z++) // Iterate over rows (z-axis)
        {
            float v = z * step - 1.0f;
            for (int x = 0; x < resolution; x++, index++) // Iterate over columns (x-axis)
          {
                float u = x * step - 1.0f;
                points[index].localPosition = f(u, v, Time.time, speed);
            }
        }

    }
}
