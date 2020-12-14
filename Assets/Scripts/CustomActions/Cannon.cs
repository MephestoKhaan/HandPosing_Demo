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
    private Collider detectorCollider;

    private static readonly WaitForSeconds DETECTION_TIMEOUT = new WaitForSeconds(0.5f); 

    private HashSet<Rigidbody> ammunition = new HashSet<Rigidbody>();



    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ammunition"))
        {
            if(!ammunition.Contains(other.attachedRigidbody))
            {
                ammunition.Add(other.attachedRigidbody);
                other.attachedRigidbody.transform.position = cannonMouth.position;
                other.attachedRigidbody.gameObject.SetActive(false);
            }
        }
    }

    public void Fire()
    {
        StartCoroutine(DisableDetection());
        foreach (var item in ammunition)
        {
            item.transform.position = cannonMouth.position;
            item.gameObject.SetActive(true);
            item.AddForce(cannonMouth.forward * strenght, ForceMode.Impulse);
        }
        ammunition.Clear();
        explosion.ForEach(ps => ps.Play());
    }


    private IEnumerator DisableDetection()
    {
        detectorCollider.enabled = false;
        yield return DETECTION_TIMEOUT;
        detectorCollider.enabled = true;
    }


}
