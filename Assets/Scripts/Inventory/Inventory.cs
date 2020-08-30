using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class Inventory : MonoBehaviour
{
    public Item pickableObject;
    public Camera mainCamera;
    public float maxDistance;
    public LayerMask layer;
    public WeaponManager manager;

    public Dictionary<string, GunItem> inventoryDic = new Dictionary<string, GunItem>();
    public Dictionary<string, GameObject> prefabsDic = new Dictionary<string, GameObject>();

    //场景中用于同一管理所有中立物品的父对象
    public Transform ItemCollector;
    //扔东西的组件
    public Transform throwPoint;

    private void Start()
    {
        mainCamera = TransformHelper.FindChild(this.transform, "Main Camera").GetComponent<Camera>();
        manager = GetComponent<WeaponManager>();
    }

    private void Update()
    {

        Debug.DrawLine(mainCamera.transform.position, mainCamera.transform.position + mainCamera.transform.forward*maxDistance, Color.green, 0.2f);
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, maxDistance, layer))
        {
            pickableObject = hit.collider.gameObject.GetComponent<Item>();
        }
        else
        {
            pickableObject = null;
        }

        //交互道具
        if (Input.GetKeyDown(InputSettings.Interact))
        {
            if(pickableObject!=null)
            {
                if(pickableObject is GunItem)
                {
                    var tmpGun = pickableObject as GunItem;
                    if(inventoryDic.ContainsKey(tmpGun.gunType))
                    {
                        inventoryDic[tmpGun.gunType] = tmpGun;
                        prefabsDic[tmpGun.gunType] = tmpGun.gameObject;
                        GameObjectPool.instance.CollectGameObject(prefabsDic[tmpGun.gunType]);
                    }
                    else
                    {
                        inventoryDic.Add(tmpGun.gunType, tmpGun);
                        prefabsDic[tmpGun.gunType] = tmpGun.gameObject;
                        GameObjectPool.instance.CollectGameObject(prefabsDic[tmpGun.gunType]);
                    }
                    PickItem(tmpGun.weaponName, tmpGun.gunType, tmpGun.gun);
                }
                
            }
        }
    }


    public void PickItem(string weaponName,string gunType,GameObject gunPrefab)
    {
        manager.AddNewWeapon(weaponName, gunType, gunPrefab);
    }

    //负责把武器扔下来
    public void DropItem(string weaponType,FireArm fireArm)
    {
        //实例化武器对象，并扔出去
        var tmp = prefabsDic[weaponType];
        Debug.Log(tmp.name);
        tmp = GameObjectPool.instance.CreateObject(tmp.name,tmp);
        tmp.transform.SetParent(ItemCollector);
        tmp.transform.position = throwPoint.position;
        tmp.transform.rotation = throwPoint.rotation;
        tmp.GetComponent<Rigidbody>().AddForce(throwPoint.forward * 30);

        tmp.GetComponent<GunItem>().currentInMag = fireArm.currentAmmoInMag;
        tmp.GetComponent<GunItem>().currentMagCarried = fireArm.currentAmmoCarried;

        inventoryDic.Remove(weaponType);
        prefabsDic.Remove(weaponType);

    }

    //注册武器管理
    public void AddItem(string weaponType,FireArm gameObject)
    {
        if(prefabsDic.ContainsKey(weaponType))
        {
            prefabsDic[weaponType] = gameObject.prefab;
            inventoryDic[weaponType] = gameObject.prefab.GetComponent<GunItem>();
            inventoryDic[weaponType].currentInMag = gameObject.currentAmmoInMag;
            inventoryDic[weaponType].currentMagCarried = gameObject.currentAmmoCarried;
        }
        else
        {
            prefabsDic.Add(weaponType, gameObject.prefab);
            inventoryDic.Add(weaponType, gameObject.prefab.GetComponent<GunItem>());
            inventoryDic[weaponType].currentInMag = gameObject.currentAmmoInMag;
            inventoryDic[weaponType].currentMagCarried = gameObject.currentAmmoCarried;
        }
    }
}
