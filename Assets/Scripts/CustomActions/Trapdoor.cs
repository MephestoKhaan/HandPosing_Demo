using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Trapdoor : MonoBehaviour
{
    [SerializeField]
    private Transform lever;
    [SerializeField]
    [Range(-1f,1f)]
    private float dotThresold;
    [SerializeField]
    private PlayableDirector director;

    private bool _hasPlayed;

    public UnityEvent OnOpened;


    private void Update()
    {
        float dot = Vector3.Dot(lever.right, Vector3.down);
        if (dot >= dotThresold)
        {
            if(!_hasPlayed)
            {
                director.Play();
                _hasPlayed = true;
                OnOpened?.Invoke();
            }
        }
        else if(dot <= -dotThresold)
        {
            _hasPlayed = false;
        }
    }
}
