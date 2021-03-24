using UnityEngine;
using UnityEngine.Analytics;

public static class AnalyticsAnonymizer
{
    private const string PP_KEY = "PRIVACY_ACCEPTED";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitStepOne()
    {
        Analytics.initializeOnStartup = false;
    }

    private static void EnableAnonymizingData()
    {
        Analytics.limitUserTracking = true;
        Analytics.deviceStatsEnabled = false;
        PerformanceReporting.enabled = false;
    }

    public static void PrivacyAccepted(bool record)
    {
        if(record)
        {
            PlayerPrefs.SetInt(PP_KEY, 1);
            PlayerPrefs.Save();
        }

        EnableAnonymizingData();
        Analytics.ResumeInitialization();
    }

    public static bool HasAcceptedPrivacy()
    {
        return PlayerPrefs.GetInt(PP_KEY, 0) > 0;
    }
}