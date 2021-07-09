using HandPosing;
using HandPosing.Interaction;
using UnityEngine;
using UnityEngine.AI;

public class StaffLocomotion : Grabbable
{
    [Space]
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform head;
    [Header("Ring")]
    [SerializeField]
    private Renderer staff;
    [SerializeField]
    private AnimationCurve penetrationStaffGlow;
    [Header("Staff")]
    [SerializeField]
    private Transform ring;
    [SerializeField]
    private AnimationCurve penetrationRingSize;
    [Header("Movement")]
    [SerializeField]
    private float movementSpeed = 2f;
    [Header("Sound")]
    [SerializeField]
    private AudioSource hitSound;
    [SerializeField]
    private float hitPitchRange = 0.1f;
    [SerializeField]
    private AudioSource whispersSound;
    [SerializeField]
    private AnimationCurve penetrationWhisperVolume;
    [SerializeField]
    private float whisperVolumeDecay = 0.1f;

    private bool _ringEnabled = true;
    private Renderer _ringRenderer;
    private Quaternion _ringOrientation;

    private Material _staffMaterial;
    private int _glowID;
    private Color _glowColor;

    private Vector3? _lastGrabPosition;
    private Transform _grabber;
    private Pose _handToStaff;
    private float _grabAltitude;
    private float _previousPenetration;
    private int _navLayer;

    private bool _hitPlayed;

    protected override void Awake()
    {
        base.Awake();

        _navLayer = 1 << NavMesh.GetAreaFromName("Default");

        _ringRenderer = ring.GetComponent<Renderer>();
        _ringOrientation = ring.rotation;

        _staffMaterial = staff.material;
        _glowID = Shader.PropertyToID("_EmissionColor");
    }

    private void Start()
    {
        HighlightStaff(0f);
        UpdateRing(0f);
    }

    public override void GrabBegin(BaseGrabber hand)
    {
        _grabber = hand.transform;
        Vector3 projectedHand = Vector3.Project(_grabber.position - this.transform.position, Vector3.up);
        Pose grabPoint = new Pose(this.transform.position + projectedHand, Quaternion.identity);
        _handToStaff = _grabber.RelativeOffset(grabPoint);
        _grabAltitude = projectedHand.magnitude;

        base.GrabBegin(hand);
    }

    public override void GrabEnd(BaseGrabber hand, Vector3 linearVelocity, Vector3 angularVelocity)
    {
        _grabber = null;
        base.GrabEnd(hand, linearVelocity, angularVelocity);
    }

    public override void MoveTo(Vector3 desiredPos, Quaternion desiredRot)
    {
        desiredPos = PoseUtils.Multiply(_grabber.GetPose(), _handToStaff).position - Vector3.up * _grabAltitude;
        desiredRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(desiredRot * Vector3.forward, Vector3.up).normalized, Vector3.up);

        float penetration = 0f;
        bool freeMovement = true;
        Vector3 hitQueryPoint = desiredPos;
        hitQueryPoint.y = 0f;
        if (NavMesh.SamplePosition(hitQueryPoint, out NavMeshHit hit, 0.1f, _navLayer))
        {
            if (desiredPos.y < hit.position.y)
            {
                freeMovement = false;
                penetration = hit.position.y - desiredPos.y;
                Vector3 stuckPos = this.transform.position;
                stuckPos.y = 0f;
                this.transform.position = stuckPos;

                if (_grabber != null)
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
        }

        if (freeMovement)
        {
            this.transform.rotation = desiredRot;
            this.transform.position = desiredPos;
            _lastGrabPosition = null;
            _hitPlayed = false;
        }
        else if (!_hitPlayed)
        {
            _hitPlayed = true;
            hitSound.pitch = Random.Range(1f - hitPitchRange, 1f + hitPitchRange);
            hitSound.Play();
        }

        if (penetration != _previousPenetration)
        {
            _previousPenetration = penetration;
            UpdateRing(penetration);
            HighlightStaff(penetration);
            UpdateWhispers(penetration);
        }
    }

    private void LateUpdate()
    {
        if (_previousPenetration > 0f)
        {
            ring.rotation = _ringOrientation;
        }
    }

    private void MovePlayer(Vector3 delta)
    {
        delta.y = 0f;

        Vector3 bodyDir = Vector3.ProjectOnPlane(head.transform.forward, Vector3.up).normalized;
        bool movingForward = Vector3.Dot(delta, bodyDir) > 0f;
        if (movingForward)
        {
            Vector3 movement = Vector3.Project(delta, bodyDir);
            player.Translate(movement);
        }
    }

    private void HighlightStaff(float penetration)
    {
        _glowColor.r = _glowColor.g = _glowColor.b = penetrationStaffGlow.Evaluate(penetration);
        _staffMaterial.SetColor(_glowID, _glowColor);
    }

    private void UpdateRing(float penetration)
    {
        if (penetration <= 0f && _ringEnabled)
        {
            _ringRenderer.enabled = _ringEnabled = false;
        }

        else if (penetration > 0f)
        {
            if (!_ringEnabled)
            {
                _ringRenderer.enabled = _ringEnabled = true;
            }
            ring.localScale = penetrationRingSize.Evaluate(penetration) * Vector3.one;
        }
    }

    private void UpdateWhispers(float penetration)
    {
        if (penetration <= 0f)
        {
            if (whispersSound.isPlaying)
            {
                whispersSound.volume -= Time.deltaTime * whisperVolumeDecay;
                if(whispersSound.volume <= 0f)
                {
                    whispersSound.Pause();
                }
            }
        }
        else
        {
            if (!whispersSound.isPlaying)
            {
                whispersSound.Play();
            }
            whispersSound.volume = Mathf.Max(whispersSound.volume, penetrationWhisperVolume.Evaluate(penetration));
        }
    }
}
