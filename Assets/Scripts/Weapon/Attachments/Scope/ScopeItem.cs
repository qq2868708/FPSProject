using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeItem : AttachmentItem
{
    //开启倍镜后，调整相机到特定位置对准倍镜准心
    public Vector3 gunTransform;
    //倍镜开启后的相机视野
    public int eyeCameraFOV;
}
