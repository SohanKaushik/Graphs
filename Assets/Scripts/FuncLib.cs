using UnityEngine;

public static class FunctionLibrary
{
    public delegate Vector3 Function(float u, float v, float t, float s);
    static Function[] functions = { wave, mul_wave, ripple, sphere, torus};

    public enum FunctionName { Wave, MultiWave, Ripple, Sphere, Torus}

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

    public static Vector3 sphere(float u, float v, float t, float s)
    {
        Vector3 _sphere;

        float r = 0.9f + 0.1f * Mathf.Sin(Mathf.PI * (6f * u + 4f * v + t));
        _sphere.x = r * Mathf.Sin(Mathf.PI * u + t);
        _sphere.y = Mathf.Sin(Mathf.PI * 0.5f * v);
        _sphere.z = r * Mathf.Cos(Mathf.PI * u + t);
        return _sphere;
    }

    public static Vector3 torus(float u, float v, float t, float sp)
    {
        float r1 = 0.7f + 0.1f * Mathf.Sin(Mathf.PI * (6f * u + 0.5f * t));
        float r2 = 0.15f + 0.05f * Mathf.Sin(Mathf.PI * (8f * u + 4f * v + 2f * t));
        float s = r1 + r2 * Mathf.Cos(Mathf.PI * v);

        Vector3 _torus;
        _torus.x = s * Mathf.Sin(Mathf.PI * u);
        _torus.y = r2 * Mathf.Sin(Mathf.PI * v);
        _torus.z = s * Mathf.Cos(Mathf.PI * u);
        return _torus;
    }
}
