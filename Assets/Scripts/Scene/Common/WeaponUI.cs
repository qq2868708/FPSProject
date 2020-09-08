using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public WeaponManager weaponManager;
    //武器脚本关联的UI部分的根
    public GameObject slotRoot;
    public GameObject countRoot;
    public GameObject weaponSlot;
    public List<GameObject> weaponSlots;
    private int oldIndex;

    // Start is called before the first frame update
    void Start()
    {
        weaponManager = this.transform.root.GetComponent<WeaponManager>();
        for(int i=0;i<slotRoot.transform.childCount;i++)
        {
            weaponSlots.Add(slotRoot.transform.GetChild(i).gameObject);
        }
        UpdateUI();
        weaponManager.UpdateUI += UpdateUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        //重复生成太耗费性能
        //foreach(var weapon in weaponManager.weaponList)
        //{
        //    var fireArm = weaponManager.weaponDict[weapon];
        //    var tmp_UI = Instantiate(weaponSlot);
        //    tmp_UI.name = tmp_UI.name.Remove(tmp_UI.name.Length - 7);
        //    tmp_UI.transform.SetParent(slotRoot.transform);
        //    tmp_UI.transform.localScale = new Vector3(1, 1, 1);
        //}

        for(int i=0;i<slotRoot.transform.childCount;i++)
        {
            slotRoot.transform.GetChild(i).gameObject.SetActive(false);
        }

        if(weaponManager.weaponList.Count==0)
        {
            countRoot.transform.Find("WeaponName").GetComponent<Text>().text = "WeaponName: " + "BareHand";
            countRoot.transform.Find("WeaponCount").GetComponent<Text>().text = "Bullets: " + "null" + "/" + "null";
            return;
        }

        for(int i=0;i<weaponManager.weaponList.Count;i++)
        {
            var slot = slotRoot.transform.GetChild(i);
            slot.gameObject.SetActive(true);
            slot.GetComponentInChildren<Image>().sprite = weaponManager.weaponDict[weaponManager.weaponList[i]].sprite;
        }

        slotRoot.transform.GetChild(oldIndex).GetComponent<Image>().color = Color.white;
        var index = weaponManager.weaponList.IndexOf(weaponManager.currentWeaponTransform.name);
        slotRoot.transform.GetChild(index).GetComponent<Image>().color = Color.red;
        oldIndex = index;

        //显示当前装备的名字
        var fireArm = weaponManager.weaponDict[weaponManager.weaponList[index]];
        countRoot.transform.Find("WeaponName").GetComponent<Text>().text ="WeaponName: "+ fireArm.weapn_Name;
        countRoot.transform.Find("WeaponCount").GetComponent<Text>().text = "Bullets: " + fireArm.currentAmmoInMag + "/" + fireArm.currentAmmoCarried;
    }
}
