
using HoshinoLabs.Udon.IwaSync3;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace KNKM
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class VideoPlayerToggle : UdonSharpBehaviour
    {
        [SerializeField] VideoController iwaSyncVideoController;
        [SerializeField] UdonBehaviour topazChatPlayer;

        [SerializeField] GameObject[] offObjects;
        [SerializeField] GameObject[] onObjects;

        [SerializeField] Transform iwaSyncRoot;
        Vector3 iwaSyncRootInitialPosition;

        [UdonSynced, FieldChangeCallback(nameof(SyncedValue))]
        bool syncedValue;
        public bool SyncedValue
        {
            get => syncedValue;
            set
            {
                if (value)
                {
                    // topaz
                    iwaSyncVideoController.Stop();
                    iwaSyncRoot.position = iwaSyncRootInitialPosition + Vector3.forward * 100f;
                }
                else
                {
                    // iwasync
                    topazChatPlayer.SendCustomEvent("Stop");
                    iwaSyncRoot.position = iwaSyncRootInitialPosition;
                }

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

        void Start()
        {
            iwaSyncRootInitialPosition = iwaSyncRoot.position;
        }

        // off = false = iwasync
        public void OnInteractOff()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            SyncedValue = false;
            RequestSerialization();
        }

        // on = true = topaz
        public void OnInteractOn()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            SyncedValue = true;
            RequestSerialization();
        }
    }
}
