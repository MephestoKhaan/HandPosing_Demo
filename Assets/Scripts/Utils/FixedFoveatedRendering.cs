using UnityEngine;

public class FixedFoveatedRendering : MonoBehaviour
{
    void Start()
    {
        OVRPlugin.SystemHeadset headset = OVRPlugin.GetSystemHeadsetType();
        if (headset == OVRPlugin.SystemHeadset.Oculus_Quest)
        {
            OVRManager.fixedFoveatedRenderingLevel = OVRManager.FixedFoveatedRenderingLevel.Medium;

        }
        else if (headset == (OVRPlugin.SystemHeadset.Oculus_Quest_2))
        {
            OVRManager.fixedFoveatedRenderingLevel = OVRManager.FixedFoveatedRenderingLevel.Off;

        }
    }
}
