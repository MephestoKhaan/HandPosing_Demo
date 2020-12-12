using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedChest : MonoBehaviour
{
    [SerializeField]
    private Attacher key;
    [SerializeField]
    private HingeJoint joint;

    private JointLimits _openLimits;
    private JointLimits _closedLimits = new JointLimits()
    {
        min = 0,
        max = 0
    };

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
        _openLimits = joint.limits;
    }

    private void Start()
    {
        LockChest(true);
    }

    private void KeyAttached()
    {
        LockChest(false);
    }

    private void LockChest(bool locked)
    {
        if (locked)
        {
            joint.limits = _closedLimits;
        }
        else
        {
            joint.limits = _openLimits;
        }
    }
}
