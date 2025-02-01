using UnityEngine;

public static class FunctionLibrary
{
    public delegate Vector3 Function(float u, float v, float t, float s);
    static Function[] functions = { wave1, ripple, torus, helix, terrain, sphere, checkerboardWave};

    public enum FunctionType { Wave, MultiWave, Ripple, Sphere, Torus}

    public static Function GetFunction(FunctionType name)
    {
        int index = (int)name;

        if(index >= functions.Length)
        {
            index = index % functions.Length;
        }
        return functions[index];
    }
    public static Vector3 sphere(float u, float v, float t, float speed)
    {
        float r = 0.9f + 0.1f * Mathf.Sin(Mathf.PI * (6f * u + 4f * v + t));
        float s = r * Mathf.Cos(0.5f * Mathf.PI * v);
        Vector3 p;
        p.x = s * Mathf.Sin(Mathf.PI * u);
        p.y = r * Mathf.Sin(0.5f * Mathf.PI * v);
        p.z = s * Mathf.Cos(Mathf.PI * u);
        return p;
    }
    public static Vector3 wave1(float u ,float v , float t , float s)
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

    public static Vector3 helix(float u, float v, float time, float speed)
    {
        Vector3 helix;
        helix.x = Mathf.Sin(Mathf.PI * (u + time * speed));
        helix.y = v; // Controls the height
        helix.z = Mathf.Cos(Mathf.PI * (u + time * speed));
        return helix;
    }

    public static Vector3 waveCascade(float u, float v, float time, float speed)
    {
        Vector3 cascade;
        cascade.x = u;
        cascade.y = Mathf.Sin(Mathf.PI * (u + time * speed)) + Mathf.Cos(2f * Mathf.PI * (v + 0.5f * time * speed));
        cascade.z = v;
        return cascade;
    }


    public static Vector3 terrain(float u, float v, float time, float speed)
    {
        Vector3 terrain;
        terrain.x = u;
        terrain.y = Mathf.PerlinNoise(u * speed, v * speed + time);
        terrain.z = v;
        return terrain;
    }
    public static Vector3 checkerboardWave(float u, float v, float time, float speed)
    {
        float checker = Mathf.Sin(u) * Mathf.Sin(v); // Checkerboard pattern

        Vector3 wave;
        wave.x = u;
        wave.y = checker * Mathf.Sin(Mathf.PI * (u + v + time * speed)); // Combine with sine wave
        wave.z = v;

        return wave;
    }

    public static Vector3 morph(float u, float v, float time, float speed, Function from, Function to, float progress)
    {
        return Vector3.LerpUnclamped(
            from(u, v, time, speed),
            to(u, v, time, speed),
            Mathf.SmoothStep(0f, 1f, progress));
    }
}
