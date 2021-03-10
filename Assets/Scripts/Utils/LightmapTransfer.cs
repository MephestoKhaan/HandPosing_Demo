using UnityEngine;

public class LightmapTransfer : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer from;
    [SerializeField]
    private SkinnedMeshRenderer to;

    private void OnValidate()
    {
        if(to != null && from != null)
        {
            to.material.SetVector("_LightmapCoordinates", from.lightmapScaleOffset);
        }
    }
}
