
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace KNKM
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class VideoPlayerSwitch : UdonSharpBehaviour
    {
        [SerializeField] bool isOn;
        [SerializeField] VideoPlayerToggle videoPlayerToggle;

        public override void Interact()
        {
            if (isOn)
            {
                videoPlayerToggle.OnInteractOn();
            }
            else
            {
                videoPlayerToggle.OnInteractOff();
            }
        }
    }
}