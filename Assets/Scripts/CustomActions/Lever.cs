using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [SerializeField]
    private Transform lever;
    [SerializeField]
    private float dotThresold;

    public UnityEvent OnPulled;

    private bool _hasPlayed;

    private void Update()
    {
        float dot = Vector3.Dot(lever.right, Vector3.down);
        if (dot >= dotThresold)
        {
            if (!_hasPlayed)
            {
                _hasPlayed = true;
                OnPulled?.Invoke();
            }
        }
        else if (dot <= -dotThresold)
        {
            _hasPlayed = false;
        }
    }
}
