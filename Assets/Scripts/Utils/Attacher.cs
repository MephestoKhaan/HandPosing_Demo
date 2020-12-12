using HandPosing;
using HandPosing.Interaction;
using UnityEngine;

public class Attacher : MonoBehaviour
{
    [SerializeField]
    private Transform attach;
    [InspectorButton("UpdateOffset")]
    public string updateOffset;
    [SerializeField]
    private Pose offset;

    [Header("Reattach parameters")]
    [SerializeField]
    private bool canReattach = true;
    [SerializeField]
    private float reattachDistance = 0.05f;
    [SerializeField]
    [Range(-1f, 1f)]
    private float reattachRotation;
    [SerializeField]
    private Vector3 rotationAxisSelf = Vector3.up;
    [SerializeField]
    private Vector3 rotationAxisTo = Vector3.up;

    public System.Action OnDetacched;
    public System.Action OnReAttached;

    private Grabbable _grabbable;
    private Rigidbody _body;

    private bool _isAttached = true;
    private bool _isGrabbed;

    private void UpdateOffset()
    {
        if (attach != null)
        {
            offset = attach.RelativeOffset(this.transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Pose point = attach.GlobalPose(offset);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(point.position, 0.01f);
    }

    private void Awake()
    {
        _grabbable = this.GetComponent<Grabbable>();
        _body = this.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Pose attachPoint = attach.GlobalPose(offset);
        if (IsCloseToPoint(attachPoint))
        {
            Attach(true);
        }
        else
        {
            Attach(false);
        }
    }

    private void LateUpdate()
    {
        bool justDropped = false;
        if(_grabbable.IsGrabbed != _isGrabbed)
        {
            _isGrabbed = _grabbable.IsGrabbed;
            justDropped = !_isGrabbed;
        }

        if (_grabbable.IsGrabbed
            && _isAttached)
        {
            Attach(false);
            OnDetacched?.Invoke();
        }

        Pose attachPoint = attach.GlobalPose(offset);

        if (_isAttached)
        {
            this.transform.SetPose(attachPoint, Space.World);
        }
        else if (justDropped)
        {
            if(canReattach && IsCloseToPoint(attachPoint))
            {
                Attach(true);
                OnReAttached?.Invoke();
            }
            else
            {
                _body.isKinematic = false;
            }
        }
    }

    private bool IsCloseToPoint(Pose attachPoint)
    {
        bool isCloseEnough = Vector3.Distance(this.transform.position, attachPoint.position) <= reattachDistance;
        bool isRotatedEnough = Vector3.Dot(this.transform.rotation * rotationAxisSelf, attach.transform.rotation * rotationAxisTo) >= reattachRotation;

        return isCloseEnough && isRotatedEnough;
    }

    private void Attach(bool attach)
    {
        IgnoreAttachCollisions(attach);
        _isAttached = attach;
        if (attach)
        {
            _body.isKinematic = true;
        }
    }

    private void IgnoreAttachCollisions(bool ignore)
    {
        Collider[] selfColliders = this.GetComponentsInChildren<Collider>();
        Collider[] attachColliders = attach.GetComponentsInChildren<Collider>();

        foreach (var collider in selfColliders)
        {
            foreach (var attachCollider in attachColliders)
            {
                Physics.IgnoreCollision(collider, attachCollider, ignore);
            }
        }
    }
}
