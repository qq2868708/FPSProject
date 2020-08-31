using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public IInteractable interactObject;
    public Camera mainCamera;
    public float maxDistance;
    public LayerMask layer;

    private void Start()
    {
        mainCamera = TransformHelper.FindChild(this.transform, "Main Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, maxDistance, layer))
        {
            interactObject = hit.collider.gameObject.GetComponentInParent<IInteractable>();
        }
        else
        {
           interactObject = null;
        }

        //交互道具
        if (Input.GetKeyDown(InputSettings.Interact))
        {
            if (interactObject != null)
            {
                interactObject.Interact();

            }
        }
    }
}
