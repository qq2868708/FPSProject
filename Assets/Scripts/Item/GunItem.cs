using UnityEngine;
using System.Collections;

public enum WeaponType
{
    MainWeapon,
    SecondaryWeapon,
    SpecialWeapon,
}

public class GunItem : Item
{
    //这是武器的预设
    public GameObject gun;
    public string weaponName;
    public string gunType;

    public int currentInMag;
    public int currentMagCarried;

    // Use this for initialization
    void Start()
    {
        //获取武器的种类和武器的名字
        weaponName = this.transform.GetChild(0).name;
        gunType = gun.name;
    }

   

    public override void Pick()
    {
        
    }

  

   
}
