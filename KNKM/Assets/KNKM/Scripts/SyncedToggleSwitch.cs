using UdonSharp;
using UnityEngine;

namespace KNKM
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class SyncedToggleSwitch : UdonSharpBehaviour
    {
        [SerializeField] bool isOn;
        [SerializeField] SyncedToggleGroup syncedToggleGroup;

        public override void Interact()
        {
            if (isOn)
            {
                syncedToggleGroup.OnInteractOn();
            }
            else
            {
                syncedToggleGroup.OnInteractOff();
            }
        }
    }
}