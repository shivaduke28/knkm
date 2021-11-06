using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace KNKM
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class AmbientLightSlider : UdonSharpBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] Kanikama.Udon.KanikamaColorCollector colorCollector;
        [ColorUsage(false, true), SerializeField] Color ambientColor;
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
                colorCollector.ambientColor = ambientColor * value;
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