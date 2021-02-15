using UnityEngine;
using UnityEngine.Events;

public class LockedChest : MonoBehaviour
{
    [SerializeField]
    private Attacher key;
    [SerializeField]
    private ConfigurableJoint joint;

    public UnityEvent OnUnlocked;

    private SoftJointLimit _lockedLimit = new SoftJointLimit()
    {
        bounciness = 0,
        contactDistance = 0.01f,
        limit = 0f
    };

    private SoftJointLimit _openLimitLow;
    private SoftJointLimit _openLimitHigh;


    private void OnEnable()
    {
        key.OnReAttached += KeyAttached;
    }

    private void OnDisable()
    {
        key.OnReAttached -= KeyAttached;
    }

    private void Awake()
    {
        _openLimitLow = joint.lowAngularXLimit;
        _openLimitHigh = joint.highAngularXLimit;
    }

    private void Start()
    {
        LockChest(true);
    }

    private void KeyAttached()
    {
        LockChest(false);
        OnUnlocked?.Invoke();
    }

    private void LockChest(bool locked)
    {
        if (locked)
        {
            joint.lowAngularXLimit = joint.highAngularXLimit = _lockedLimit;
        }
        else
        {
            joint.lowAngularXLimit = _openLimitLow;
            joint.highAngularXLimit = _openLimitHigh;
        }
    }
}
