using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystem> explosion;
    [SerializeField]
    private float strenght;

    [SerializeField]
    private Transform cannonMouth;
    [SerializeField]
    private AudioSource explossionSound;
    [SerializeField]
    private Collider detectorCollider;

    private static readonly WaitForSeconds DETECTION_TIMEOUT = new WaitForSeconds(0.5f);

    private HashSet<Rigidbody> _ammunition = new HashSet<Rigidbody>();

    private Rigidbody _cannonBody;

    private void Awake()
    {
        _cannonBody = this.GetComponentInParent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammunition"))
        {
            if (!_ammunition.Contains(other.attachedRigidbody))
            {
                _ammunition.Add(other.attachedRigidbody);
                other.attachedRigidbody.transform.position = cannonMouth.position;
                other.attachedRigidbody.gameObject.SetActive(false);
            }
        }
    }

    public void Fire()
    {
        StartCoroutine(DisableDetection());
        foreach (var item in _ammunition)
        {
            item.transform.position = cannonMouth.position;
            item.gameObject.SetActive(true);
            item.AddForce(cannonMouth.forward * strenght, ForceMode.Impulse);
        }
        _cannonBody.AddForceAtPosition(cannonMouth.up * strenght * 0.2f, cannonMouth.position, ForceMode.Impulse);
        _ammunition.Clear();
        explosion.ForEach(ps => ps.Play());
        explossionSound.Play();
    }


    private IEnumerator DisableDetection()
    {
        detectorCollider.enabled = false;
        yield return DETECTION_TIMEOUT;
        detectorCollider.enabled = true;
    }


}
