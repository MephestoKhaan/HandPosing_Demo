using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MassCentre : MonoBehaviour
{
    [SerializeField]
    private Vector3 massCentrePosition;
    [SerializeField]
    private Vector3 tensorRotation;

    private void Awake()
    {
        Rigidbody body = this.GetComponent<Rigidbody>();
        body.centerOfMass = massCentrePosition;
        body.inertiaTensorRotation = Quaternion.Euler(tensorRotation);
    }
}
