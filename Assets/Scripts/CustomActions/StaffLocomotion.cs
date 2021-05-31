using HandPosing.Interaction;
using UnityEngine;

public class StaffLocomotion : Grabbable
{
    [Space]
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform head;
    [SerializeField]
    private float movementSpeed = 2f;


    private Vector3? _lastGrabPosition;
    private Transform _grabber;

    public override void GrabBegin(BaseGrabber hand)
    {
        base.GrabBegin(hand);
        _grabber = hand.transform;
    }

    public override void GrabEnd(BaseGrabber hand, Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(hand, linearVelocity, angularVelocity);
        _grabber = null;
    }

    public override void MoveTo(Vector3 desiredPos, Quaternion desiredRot)
    {
        if (desiredPos.y < 0f)
        {
            Vector3 stuckPos = this.transform.position;
            stuckPos.y = 0f;
            this.transform.position = stuckPos;

            if(_grabber != null)
            {
                if (_lastGrabPosition.HasValue)
                {
                    Vector3 delta = _lastGrabPosition.Value - _grabber.position;
                    _lastGrabPosition = _grabber.position;
                    MovePlayer(delta * movementSpeed);
                }
                else
                {
                    _lastGrabPosition = _grabber.position;
                }
            }
        }
        else
        {
            this.transform.rotation = desiredRot;
            this.transform.position = desiredPos;
            _lastGrabPosition = null;
        }
    }

    private void MovePlayer(Vector3 delta)
    {
        delta.y = 0f;
        Vector3 headDir = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;

        bool movingForward = Vector3.Dot(delta, headDir) > 0f;
        if (movingForward)
        {
            Vector3 movement = Vector3.Project(delta, headDir);
            player.Translate(movement);
        }
    }

}