using Unity.Mathematics;
using UnityEngine;

public class compute : MonoBehaviour
{
    [SerializeField] ComputeShader m_ComputeBuffer;
    [SerializeField] GameObject m_prefab;

    private ComputeBuffer pos_buff;

    private float3[] positions;

    void Start()
    {
        int size = 1000; // Number of objects to instantiate



        positions = new float3[size];

        // Create a compute buffer
        pos_buff = new ComputeBuffer(size, sizeof(float) * 3);

        // Find the kernel by name
        int kernel = m_ComputeBuffer.FindKernel("CSMain");

        // Set the buffer
        m_ComputeBuffer.SetBuffer(kernel, "positions", pos_buff);

        // Dispatch the compute shader
        int thread = Mathf.CeilToInt(size / 64.0f); // Calculate the number of thread groups
        m_ComputeBuffer.Dispatch(kernel, thread, 1, 1);

        // Read back the data
        pos_buff.GetData(positions);

        // Print the values
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(m_prefab, positions[i], Quaternion.identity);
        }

        // Release the buffer
        pos_buff.Release();
    }
}
