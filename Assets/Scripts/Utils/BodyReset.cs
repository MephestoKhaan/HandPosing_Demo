using HandPosing;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BodyReset : MonoBehaviour
{
    private Rigidbody _body;
    private Pose _originalPose;

    private void Awake()
    {
        _body = this.GetComponent<Rigidbody>();
        _originalPose = this.transform.GetPose();
    }

    public void ResetPosition()
    {
        this.transform.SetPose(_originalPose);
        _body.velocity = Vector3.zero;
        _body.angularVelocity = Vector3.zero;
    }
}
