using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour,IInteractable
{
    public Transform joint;
    public bool isClosed=true;

    public void Interact()
    {
        isClosed = !isClosed;
        if(isClosed)
        {
            StartCoroutine(Move(0));
        }
        else
        {
            StartCoroutine(Move(100));
        }
        
    }

    private IEnumerator Move(int target)
    {
        float tmp_angle = joint.localEulerAngles.y;
        while(Mathf.Abs( target-tmp_angle)>1)
        {
            yield return null;
            var tmp = (target - tmp_angle) * 0.2f;
            tmp_angle += tmp;
            joint.localEulerAngles = new Vector3(0, tmp_angle, 0);
        }
    }
}
