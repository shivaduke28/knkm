using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace KNKM
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class EmissiveLightsIntensitySlider : UdonSharpBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] private EmissiveLightController[] lights;
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
                foreach (var l in lights)
                {
                    l.intensity = value;
                }
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

