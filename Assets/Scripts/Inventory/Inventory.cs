using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class Inventory : MonoBehaviour
{
    public IPickableObject pickableObject;

    private void Update()
    {
        //交互道具
        if (Input.GetKeyDown(InputSettings.Interact))
        {
            if(pickableObject!=null)
            {
                Debug.Log(12);
                pickableObject.Pick();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        pickableObject = other.transform.GetComponent<IPickableObject>();
    }
}
