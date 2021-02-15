using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsEventSender : MonoBehaviour
{
    public void SendEvent(string title)
    {
#if ANALYTICS_PROD
        Analytics.Custom(title);
#else
        Debug.Log($"AnalyticsEventSender: {title}");
#endif
    }
}
