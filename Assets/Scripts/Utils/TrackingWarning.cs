using HandPosing.TrackingData;
using UnityEngine;

public class TrackingWarning : MonoBehaviour
{
    [SerializeField]
    private SkeletonDataProvider trackingData;
    [SerializeField]
    private Renderer renderedHand;

    [SerializeField]
    private Color okColour = Color.white;
    [SerializeField]
    private Color warningColour = Color.red;

    private Material _handMaterial;
    private bool _wasHighConfidence;

    private void Start()
    {
        _handMaterial = renderedHand.material;
    }


    private void Update()
    {
        bool highConfidence = trackingData.IsHandHighConfidence() 
            || !trackingData.IsTracking;
        if (!highConfidence
            && _wasHighConfidence)
        {
            _handMaterial.color = warningColour;
        }
        else if (highConfidence
            && !_wasHighConfidence)
        {
            _handMaterial.color = okColour;
        }
        _wasHighConfidence = highConfidence;
    }


}
