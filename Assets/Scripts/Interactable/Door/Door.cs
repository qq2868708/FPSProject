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
            Debug.Log(transform.localRotation.eulerAngles);
            StartCoroutine(Move(100));
        }
        
    }

    private IEnumerator Move(int target)
    {
        //float sum = 0;
        //while(target-sum>1)
        //{
        //    yield return null;
        //    var tmp = (target-sum) * 0.3f;
        //    sum += tmp;
        //    Debug.Log(sum);
        //}

        float tmp_angle = joint.localEulerAngles.y;
        Debug.Log(tmp_angle);
        while(Mathf.Abs( target-tmp_angle)>1)
        {
            yield return null;
            var tmp = (target - tmp_angle) * 0.2f;
            tmp_angle += tmp;
            joint.localEulerAngles = new Vector3(0, tmp_angle, 0);
        }
    }
}
