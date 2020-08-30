using UnityEngine;
using System.Collections;

public enum WeaponType
{
    MainWeapon,
    SecondaryWeapon,
    SpecialWeapon,
}

public class GunItem : MonoBehaviour,IPickableObject
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private WeaponManager manager;
    public string weaponName;

    public GameObject gun;
    public string gunType;

    // Use this for initialization
    void Start()
    {
        player = null;
        manager = null;
        weaponName = this.transform.GetChild(0).name;
        gunType = gun.name;
    }

   

    public void Pick()
    {
        manager.AddNewWeapon(weaponName,gunType,gun);
    }

  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;

            manager = player.GetComponent<WeaponManager>();
        }
    }
}
