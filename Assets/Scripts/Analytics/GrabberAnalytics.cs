using HandPosing.OVRIntegration;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GrabberAnalytics : MonoBehaviour
{
    [SerializeField]
    private GrabberHybridOVR grabber;

    [SerializeField]
    private HandPosing.Handeness handeness;

    private void OnEnable()
    {
        grabber.OnGrabStarted += GrabStarted;
        grabber.OnGrabTimedEnded += GrabEnded;
        grabber.OnGrabAttemptFail += GrabAttemptFail;
    }

    private void OnDisable()
    {
        grabber.OnGrabStarted -= GrabStarted;
        grabber.OnGrabTimedEnded -= GrabEnded;
        grabber.OnGrabAttemptFail -= GrabAttemptFail;
    }

    private void GrabStarted(GameObject obj)
    {
#if ANALYTICS_PROD
        Analytics.CustomEvent("GrabStarted", new Dictionary<string, object>
        {
            { "handeness", handeness.ToString()},
            { "flex", grabber.Flex.ToString()},
            { "grabbable", obj.name}
        });
#else
        Debug.Log($"GrabberAnalytics: GrabStarted {obj?.name}");
#endif
    }

    private void GrabEnded(GameObject obj, float time)
    {
#if ANALYTICS_PROD
        Analytics.CustomEvent("GrabEnded", new Dictionary<string, object>
        {
            { "handeness", handeness.ToString()},
            { "flex", grabber.Flex?.InterfaceFlexType.ToString()},
            { "grabbable", obj?.name},
            { "duration", time.ToString("F1") }
        });

#else
        Debug.Log($"GrabberAnalytics: GrabEnded {obj?.name}");
#endif
    }

    private void GrabAttemptFail(GameObject obj)
    {
#if ANALYTICS_PROD
        Analytics.CustomEvent("GrabFailed", new Dictionary<string, object>
        {
            { "handeness", handeness.ToString()},
            { "flex", grabber.Flex?.InterfaceFlexType.ToString()},
            { "grabbable", obj?.name}
        });

#else
        Debug.Log($"GrabberAnalytics: GrabFailed {obj?.name}");
#endif
    }

}
