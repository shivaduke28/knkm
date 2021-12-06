using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace KNKM
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class ColorSamplerIntensitySlider : UdonSharpBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] private Kanikama.Udon.KanikamaCamera kanikamaCamera;
        [UdonSynced, FieldChangeCallback(nameof(SyncedValue))]
        float syncedValue;
        public float SyncedValue
        {
            get => syncedValue;
            set
            {
                deserializing = true;
                if (!Networking.IsOwner(gameObject))
                {
                    slider.value = value;
                }
                syncedValue = value;
                kanikamaCamera.intensity = value;
                deserializing = false;
            }
        }
        bool deserializing;

        public void OnValueChanged()
        {
            if (deserializing) return;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            SyncedValue = slider.value;
            RequestSerialization();
        }
    }
}
