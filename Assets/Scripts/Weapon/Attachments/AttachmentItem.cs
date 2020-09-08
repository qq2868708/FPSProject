using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentItem : MonoBehaviour
{
    public AttachmentType attachmentType;
    public string attachmentName;
}

public enum AttachmentType
{
    Scope,
    other,
}
