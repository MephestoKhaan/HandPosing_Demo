using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrivacyCheck : MonoBehaviour
{
    [SerializeField]
    private string mainScene = "PirateShip";
    [SerializeField]
    private string policyScene = "Warning";
    [SerializeField]
    private bool checkOnStartup = false;

    private bool _userAccepted = false;

    private void Start()
    {
        if(checkOnStartup)
        {
            if (AnalyticsAnonymizer.HasAcceptedPrivacy())
            {
                UserAccepts(false);
            }
            else
            {
                StartCoroutine(LoadScene(policyScene));
            }
        }
    }

    public void UserAccepted()
    {
        UserAccepts(true);
    }

    private void UserAccepts(bool record)
    {
        if(_userAccepted)
        {
            return;
        }
        _userAccepted = true;

        AnalyticsAnonymizer.PrivacyAccepted(record);
        StartCoroutine(LoadScene(mainScene));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
