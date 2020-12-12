using UnityEngine;

public class MessageInABottle : MonoBehaviour
{
    [SerializeField]
    private Attacher cork;
    [SerializeField]
    private Attacher message;
    [SerializeField]
    private Collider messageCollider;

    private bool _messageWasReleased;

    private void OnEnable()
    {
        cork.OnDetacched += CorkReleased;
        cork.OnReAttached += CorkAttached;

        message.OnDetacched += MessageReleased;
        message.OnReAttached += MessageAttached;
    }
    private void OnDisable()
    {
        cork.OnDetacched -= CorkReleased;
        cork.OnReAttached -= CorkAttached;

        message.OnDetacched -= MessageReleased;
        message.OnReAttached -= MessageAttached;
    }

    private void Start()
    {
        messageCollider.enabled = false;
    }


    private void MessageReleased()
    {
        _messageWasReleased = true;
    }

    private void MessageAttached()
    {
        _messageWasReleased = false;
    }

    private void CorkReleased()
    {
        if(!_messageWasReleased)
        {
            messageCollider.enabled = true;
        }
    }

    private void CorkAttached()
    {
        if (!_messageWasReleased)
        {
            messageCollider.enabled = false;
        }
    }


}
