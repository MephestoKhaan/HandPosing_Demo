using HandPosing;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fuse : MonoBehaviour
{
    [SerializeField]
    private Flare flare;

    [SerializeField]
    private float stopVelocity = 4f;
    [SerializeField]
    private float velocitySensitivity = 10f;

    [Header("Timing")]
    [SerializeField]
    private List<Transform> waypoints;
    [SerializeField]
    private float fuseDuration = 2f;

    public UnityEvent OnFuseEnded;

    private float _velocity;
    private float? _startTime;
    private List<float> _waypointsAccumDistance;
    private float _totalDistance;
    private Vector3 _lastPosition;

    private void Awake()
    {
        InitializeWaypoints();
        ResetSpeed();
    }

    private void InitializeWaypoints()
    {
        float[] waypointsDistance = new float[waypoints.Count];
        waypointsDistance[0] = 0;
        _totalDistance = 0;
        for (int i = 1; i < waypoints.Count; i++)
        {
            _totalDistance += Vector3.Distance(waypoints[i].position, waypoints[i - 1].position);
            waypointsDistance[i] = _totalDistance;
        }
        _waypointsAccumDistance = new List<float>(waypointsDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FireSource"))
        {
            if (!flare.IsPlaying)
            {
                Light(true);
                if (fuseDuration > 0f)
                {
                    _startTime = Time.timeSinceLevelLoad;
                }
            }
        }
    }

    private void Update()
    {
        if (flare.IsPlaying)
        {
            UpdateVelocity();
            if (_velocity > stopVelocity)
            {
                _startTime = null;
                Light(false);
            }
        }

        if (_startTime.HasValue)
        {
            float normalisedTime = fuseDuration > 0f ? (Time.timeSinceLevelLoad - _startTime.Value) / fuseDuration : 0f;
            MoveFuse(Mathf.Clamp01(normalisedTime));
            if (normalisedTime > 1f)
            {
                _startTime = null;
                Light(false);
                OnFuseEnded?.Invoke();
            }
        }
    }

    private void UpdateVelocity()
    {
        float instantVelocity = Vector3.Distance(_lastPosition, flare.transform.position) / Time.deltaTime;
        _lastPosition = flare.transform.position;
        _velocity = Mathf.Lerp(_velocity, instantVelocity, Time.deltaTime * velocitySensitivity);
    }

    private void Light(bool enable)
    {
        if (flare.IsPlaying != enable)
        {
            flare.Play(enable);
            MoveFuse(enable ? 0f : 1f);
            ResetSpeed();
        }
    }

    private void ResetSpeed()
    {
        _velocity = 0f;
        _lastPosition = flare.transform.position;
    }

    private void MoveFuse(float normalisedTime)
    {
        float desiredDistance = _totalDistance * normalisedTime;
        int index = _waypointsAccumDistance.FindLastIndex(wp => wp <= desiredDistance);
        Pose fusePose;
        if (index == -1)
        {
            fusePose = waypoints[0].GetPose(Space.Self);
        }
        else if (index == _waypointsAccumDistance.Count - 1)
        {
            fusePose = waypoints[index].GetPose(Space.Self);
        }
        else
        {
            float t = (desiredDistance - _waypointsAccumDistance[index]) / (_waypointsAccumDistance[index + 1] - _waypointsAccumDistance[index]);
            Pose fromPose = waypoints[index].GetPose(Space.Self);
            Pose toPose = waypoints[index + 1].GetPose(Space.Self);
            fusePose = PoseUtils.Lerp(fromPose, toPose, t);
        }
        flare.transform.SetPose(fusePose, Space.Self);
    }
}
