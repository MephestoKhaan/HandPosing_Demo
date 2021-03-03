using UnityEngine;
using UnityEngine.AI;

public class Locomotion : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private float snapDegrees = 45f;

    [SerializeField]
    private Transform directioner;

    private int _navLayer;
    private bool _snapped;

    private void Awake()
    {
        _navLayer = 1 << NavMesh.GetAreaFromName("Default");
    }

    void Update()
    {
        UpdateMove();
        UpdateRotation();
    }

    private void UpdateMove()
    {
        Vector2 move = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
        if (move.sqrMagnitude > 0)
        {
            Quaternion direction = Quaternion.LookRotation(Vector3.ProjectOnPlane(directioner.forward, Vector3.up), Vector3.up);
            Vector3 offset = new Vector3(move.x, 0f, move.y) * Time.deltaTime * moveSpeed;
            Vector3 targetPosition = this.transform.position
                + direction * offset;
            if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 0.5f, _navLayer))
            {
                this.transform.position = hit.position;
            }
        }
    }

    private void UpdateRotation()
    {
        float rot = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
        bool canSnap = Mathf.Abs(rot) > 0.75f;
        
        if (!canSnap)
        {
            _snapped = false;
        }
        else if(!_snapped
            && canSnap)
        {
            _snapped = true;
            this.transform.Rotate(0f, Mathf.Sign(rot) * snapDegrees, 0f, Space.World);
        }
    }
}
