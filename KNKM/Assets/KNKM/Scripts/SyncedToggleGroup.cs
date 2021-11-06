using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace KNKM
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SyncedToggleGroup : UdonSharpBehaviour
    {
        [SerializeField] GameObject[] offObjects;
        [SerializeField] GameObject[] onObjects;

        [UdonSynced, FieldChangeCallback(nameof(SyncedValue))]
        bool syncedValue;
        public bool SyncedValue
        {
            get => syncedValue;
            set
            {
                foreach (var go in offObjects)
                {
                    go.SetActive(!value);
                }

                foreach (var go in onObjects)
                {
                    go.SetActive(value);
                }

                syncedValue = value;
            }
        }

        public void OnInteractOff()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            SyncedValue = false;
            RequestSerialization();
        }

        public void OnInteractOn()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            SyncedValue = true;
            RequestSerialization();
        }
    }
}