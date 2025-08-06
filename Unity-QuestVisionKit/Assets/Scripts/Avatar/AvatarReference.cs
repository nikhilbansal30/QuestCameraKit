using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarReference : MonoBehaviour
{
    [SerializeField] Transform head, leftHand, rightHand;

    AvatarHandlerMirror currentAvatar;
    public AvatarHandlerMirror CurrentAvatar { set { currentAvatar = value; } get { return currentAvatar; } }
    public static AvatarReference instance;


    private void Awake()
    {
        instance = this;
    }


    private void LateUpdate()
    {
        if (currentAvatar != null)
        {
            TransformAvatar();
        }
    }



    public void TransformAvatar()
    {
        currentAvatar.head.transform.SetPositionAndRotation(head.position, head.rotation);
        currentAvatar.leftHand.transform.SetPositionAndRotation(leftHand.position, leftHand.rotation);
        currentAvatar.rightHand.transform.SetPositionAndRotation(rightHand.position, rightHand.rotation);
    }

    // public void ShouldAnnotate()
    // {
    //     Debug.Log("Avatar reference");
    //     currentAvatar.SpawnAnnotation();
    // }


}


