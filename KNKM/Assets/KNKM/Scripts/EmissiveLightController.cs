
using UdonSharp;
using UnityEngine;

public class EmissiveLightController : UdonSharpBehaviour
{
    [SerializeField] private Renderer renderer;
    [SerializeField] int step;
    public float intensity = 1f;

    float t = 0f;

    void Update()
    {
        t += Time.deltaTime;
        var x = (Mathf.Floor(t) % 4f) == step ? 1 : 0;
        renderer.material.SetColor("_EmissionColor", Color.white * (Mathf.Sin((t - 0.25f) * 2 * Mathf.PI) + 1f) * x * intensity);
    }
}
