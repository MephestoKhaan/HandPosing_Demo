using UnityEngine;

public class AnalyticsEventSender : MonoBehaviour
{
    public void SendEvent(string title)
    {
#if ANALYTICS_PROD
        UnityEngine.Analytics.Analytics.CustomEvent(title);
#else
        Debug.Log($"AnalyticsEventSender: {title}");
#endif
    }
}
