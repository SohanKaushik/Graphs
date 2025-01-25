using UnityEngine;

public static class FunctionLibrary
{
    public delegate Vector3 Function(float u, float v, float t, float s);
    static Function[] functions = { wave, mul_wave, ripple };

    public enum FunctionName { wave, MultiWave, Ripple }

    public static Function GetFunction(FunctionName name)
    {
        return functions[(int)name];
    }

    public static Vector3 wave(float u ,float v , float t , float s)
    {
        Vector3 _wave;
        _wave.x = u;
        _wave.y = Mathf.Sin(Mathf.PI * (u + v + t * s));
        _wave.z = v;
        return _wave;
    }

    public static Vector3 mul_wave(float u, float v, float t, float s)
    {
        Vector3 _mul_wave;
        _mul_wave.x = u;
        _mul_wave.y =  Mathf.Sin(Mathf.PI * ( u + 0.5f * t));     
        _mul_wave.y += 0.5f * Mathf.Sin(2.0f * Mathf.PI * (v + t));
        _mul_wave.y += Mathf.Sin(Mathf.PI * (u + v + 0.25f * t));
        _mul_wave.z = v;

        return _mul_wave;                                      // ensure peak is in range of [-1, 1]


        //y += Mathf.Sin(Mathf.PI * (x + t));                 // constructive interference
        //y += Mathf.Sin(Mathf.PI * (x + t) + Mathf.PI);     // destructive interference 

    }

    public static Vector3 ripple(float u, float v, float t, float s)
    {
        float d = Mathf.Sqrt(u * u + v * v);

        Vector3 _ripple;
        _ripple.x = u;
        _ripple.y = Mathf.Sin(8f * d - t * s);
        _ripple.y /= (1f + 10f * d);
        _ripple.z = v;
        return _ripple;
    }
}
