
using UdonSharp;
using UnityEngine;

public class ObjectToggleSwitch : UdonSharpBehaviour
{
    [SerializeField] GameObject[] objectGroupOn;
    [SerializeField] GameObject[] objectGroupOff;
    [SerializeField] Renderer renderer;
    [ColorUsage(false, true), SerializeField] Color colorOn = Color.white;
    [ColorUsage(false, true), SerializeField] Color colorOff = Color.black;

    bool isOn;

    void Start()
    {
        renderer.material.SetColor("_EmissionColor", isOn ? colorOn : colorOff);
    }

    public override void Interact()
    {
        isOn = !isOn;

        foreach (var go in objectGroupOn)
        {
            go.SetActive(isOn);
        }

        foreach (var go in objectGroupOff)
        {
            go.SetActive(!isOn);
        }

        renderer.material.SetColor("_EmissionColor", isOn ? colorOn : colorOff);
    }
}
