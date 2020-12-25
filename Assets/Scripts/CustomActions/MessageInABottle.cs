using System.Collections.Generic;
using UnityEngine;

public class MessageInABottle : MonoBehaviour
{
    [SerializeField]
    private Attacher cork;
    [SerializeField]
    private Attacher message;

    private List<Collider> _messageColliders;

    private bool _messageWasReleased;

    private void Awake()
    {
        _messageColliders = new List<Collider>(message.GetComponentsInChildren<Collider>());
    }

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
        _messageColliders.ForEach(c=>c.enabled = false);
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
            _messageColliders.ForEach(c => c.enabled = true);
        }
    }

    private void CorkAttached()
    {
        if (!_messageWasReleased)
        {
            _messageColliders.ForEach(c => c.enabled = false);
        }
    }


}
