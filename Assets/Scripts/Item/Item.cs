using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour,IPickableObject
{
    public abstract void Pick();
}
