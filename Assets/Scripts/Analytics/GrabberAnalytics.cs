using HandPosing.OVRIntegration;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

    private string SimplifyName(string name)
    {
        if(!string.IsNullOrEmpty(name))
        {
            name = Regex.Replace(name, @"\s\(\d*\)", "");
            return name;
        }
        return string.Empty;
    }

    private void GrabStarted(GameObject obj)
    {
#if ANALYTICS_PROD
        Analytics.CustomEvent("GrabStarted", new Dictionary<string, object>
        {
            { "handeness", handeness.ToString()},
            { "flex", grabber.Flex?.ToString()},
            { "grabbable", SimplifyName(obj?.name)}
        });
#else
        Debug.Log($"GrabberAnalytics: GrabStarted {SimplifyName(obj?.name)}");
#endif
    }

    private void GrabEnded(GameObject obj, float time)
    {
#if ANALYTICS_PROD
        Analytics.CustomEvent("GrabEnded", new Dictionary<string, object>
        {
            { "handeness", handeness.ToString()},
            { "flex", grabber.Flex?.ToString()},
            { "grabbable", SimplifyName(obj?.name)},
            { "duration", time.ToString("F1") }
        });

#else
        Debug.Log($"GrabberAnalytics: GrabEnded {SimplifyName(obj?.name)}");
#endif
    }

    private void GrabAttemptFail(GameObject obj)
    {
#if ANALYTICS_PROD
        Analytics.CustomEvent("GrabFailed", new Dictionary<string, object>
        {
            { "handeness", handeness.ToString()},
            { "flex", grabber.Flex?.ToString()},
            { "grabbable", SimplifyName(obj?.name)}
        });

#else
        Debug.Log($"GrabberAnalytics: GrabFailed {SimplifyName(obj?.name)}");
#endif
    }

}
