using HandPosing;
using UnityEngine;

public class PaperFold : MonoBehaviour
{
    [SerializeField]
    private Transform targetCorner;
    [SerializeField]
    private Pose foldedPose;
    [SerializeField]
    private Pose unfoldedPose;

    [SerializeField]
    private float foldSpeed;

    private int _collisions = 0;
    private float _foldRange;
    private int _foldDirection = 0;

    private void Update()
    {
        _foldRange = Mathf.Clamp01(_foldRange + Time.deltaTime * foldSpeed * _foldDirection);
        Unfold(_foldRange);
    }

    private void Unfold(float factor)
    {
        Pose target = PoseUtils.Lerp(foldedPose, unfoldedPose, factor);
        targetCorner.transform.SetPose(target, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_collisions == 0)
        {
            _foldDirection = 1;
        }
        _collisions++;
    }

    private void OnTriggerExit(Collider other)
    {
        _collisions--;
        if(_collisions == 0)
        {
            _foldDirection = -1;
        }
    }
}
