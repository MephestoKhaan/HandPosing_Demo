using HandPosing;
using HandPosing.SnapRecording;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[ExecuteInEditMode]
public class PoseTranslator : MonoBehaviour
{
    [SerializeField]
    private HandGhostProvider ghostProvider;

    [InspectorButton("UndoTransformations")]
    public string undoTransformation;


    public void UndoTransformations()
    {
        SnapPoint[] snapPoints = (SnapPoint[]) Resources.FindObjectsOfTypeAll(typeof(SnapPoint));
        foreach(var sp in snapPoints)
        {
            if (!EditorUtility.IsPersistent(sp.transform.root.gameObject) && !(sp.gameObject.hideFlags == HideFlags.NotEditable || sp.gameObject.hideFlags == HideFlags.HideAndDontSave))
            {
                SetRawSnappoint(sp);
            }
        }
    }


    public void SetRawSnappoint(SnapPoint point)
    {
        HandPuppet puppet = ghostProvider.GetHand(point.pose.handeness).GetComponent<HandPuppet>();
        List<BoneRotation> bones = new List<BoneRotation>();

        for(int i = 0; i < point.pose.Bones.Count; i++)
        {
            BoneRotation bone = point.pose.Bones[i];
            BoneMap puppetBone = puppet.Bones.Find(b => b.id == bone.boneID);
            bone.rotation = Quaternion.Inverse(puppetBone.RotationOffset) * bone.rotation;
            bones.Add(bone);
        }

        HandPose pose = point.pose;
        pose._bones = bones;
        point.pose = pose;

        EditorUtility.SetDirty(point);
    }


}
