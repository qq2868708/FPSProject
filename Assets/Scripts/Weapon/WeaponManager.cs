using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class WeaponManager : MonoBehaviour
{
    //主武器对象
    public Transform mainWeaponTransform;
    public FireArm mainWeapon;
    public string mainWeaponStr = "MainWeapon";

    //副武器对象
    public Transform secondaryWeaponTransform;
    public FireArm secondaryWeapon;
    public string secondaryWeaponStr = "SecondaryWeapon";

    //特殊武器对象
    public Transform specialWeaponTransform;
    public FireArm specialWeapon;
    public string specialWeaponStr = "SpecialWeapon";

    //管理所用可用武器，包括主，副武器，所有武器类型都会被注册但是武器可能不存在
    [SerializeField]
    private Dictionary<string, FireArm> weaponDict = new Dictionary<string, FireArm>();
    //将武器和管理他们的transform建立关系，如果没有武器，就不在此字典注册
    [SerializeField]
    private Dictionary<FireArm, Transform> transformDic = new Dictionary<FireArm, Transform>();
    [SerializeField]
    private Dictionary<string, Transform> weaponTransform = new Dictionary<string, Transform>();

    //实际拥有的武器列表，全部非空
    [SerializeField]
    private List<string> weaponList = new List<string>();

    //当前武器对象
    [SerializeField]
    private Transform currentWeaponTransform;
    [SerializeField]
    private FireArm currentWeapon;
    //当前武器索引，用于鼠标滚轮切换武器
    [SerializeField]
    private int currentIndex;

    //所有武器的直接父级
    public Transform weaponHolder;
    //背包
    public Inventory inventory;
    //是否可以切枪
    public bool canSwap;

    //注册后坐力
    public FPSMouseLook mouseLook;

    //做一些初始化的设置
    private void InitManager()
    {
        canSwap = true;

        inventory = GetComponent<Inventory>();
        mouseLook = GetComponentInChildren<FPSMouseLook>();

        //统计当前的武器信息
        CollectWeapon();

        //禁用所有武器组件，屏蔽编辑器操作，不管在编辑器中如何配置，运行时结果不变
        foreach (var transform in weaponTransform.Values)
        {
            transform.gameObject.SetActive(false);
        }
        AddInventory();
        
    }

    //统计当前的武器信息，当捡起或者丢弃武器时调用
    public void CollectWeapon()
    {
        transformDic.Clear();
        weaponDict.Clear();
        weaponList.Clear();
        weaponTransform.Clear();
        mainWeaponTransform = TransformHelper.FindChild(weaponHolder, "MainWeapon");
        secondaryWeaponTransform = TransformHelper.FindChild(weaponHolder, "SecondaryWeapon");
        specialWeaponTransform = TransformHelper.FindChild(weaponHolder, "SpecialWeapon");
        //对主武器赋值
        if(mainWeaponTransform!=null)
        {
            weaponTransform.Add("MainWeapon", mainWeaponTransform);
            mainWeapon = mainWeaponTransform.GetComponentInChildren<FireArm>();
            transformDic.Add(mainWeapon, mainWeaponTransform);
            weaponDict.Add("MainWeapon", mainWeapon);
            weaponList.Add("MainWeapon");
            mainWeapon.weaponHolder = this.weaponHolder;
        }
        else
        {
            mainWeapon = null;
        }
        //对副武器赋值
        if(secondaryWeaponTransform!=null)
        {
            weaponTransform.Add("SecondaryWeapon", secondaryWeaponTransform);
            secondaryWeapon = secondaryWeaponTransform.GetComponentInChildren<FireArm>();
            transformDic.Add(secondaryWeapon, secondaryWeaponTransform);
            weaponDict.Add("SecondaryWeapon", secondaryWeapon);
            weaponList.Add("SecondaryWeapon");
            secondaryWeapon.weaponHolder = this.weaponHolder;
        }
        else
        {
            secondaryWeapon = null;
        }
        //对特殊武器赋值
        if (specialWeaponTransform!=null)
        {
            weaponTransform.Add("SpecialWeapon", specialWeaponTransform);
            specialWeapon = specialWeaponTransform.GetComponentInChildren<FireArm>();
            transformDic.Add(specialWeapon, specialWeaponTransform);
            weaponDict.Add("SpecialWeapon", specialWeapon);
            weaponList.Add("SpecialWeapon");
            specialWeapon.weaponHolder = this.weaponHolder;
        }
        else
        {
            specialWeapon = null;
        }

    }

    //将武器信息放入inventory，用于管理
    public void AddInventory()
    {
        foreach(var gunType in weaponList)
        {
            inventory.AddItem(gunType,weaponDict[gunType]);
        }
    }

    private void Awake()
    {
        InitManager();

        currentWeaponTransform = mainWeaponTransform;
        currentWeapon = mainWeapon;
        currentIndex = 0;

        //配置默认武器
        if(mainWeapon!=null)
        {
            currentWeaponTransform.gameObject.SetActive(false);
            currentWeaponTransform = mainWeaponTransform;
            currentWeapon = mainWeapon;
            currentWeaponTransform.gameObject.SetActive(true);
            currentIndex = 0;
        }
        else if(secondaryWeapon!=null)
        {
            currentWeaponTransform = secondaryWeaponTransform;
            currentWeaponTransform.gameObject.SetActive(false);
            currentWeapon = secondaryWeapon;
            currentWeaponTransform.gameObject.SetActive(true);
            currentIndex = 1;
        }
        else if (specialWeapon != null)
        {
            currentWeaponTransform = specialWeaponTransform;
            currentWeaponTransform.gameObject.SetActive(false);
            currentWeapon = specialWeapon;
            currentWeaponTransform.gameObject.SetActive(true);
            currentIndex = 1;
        }
        if(weaponList.Count==0)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex = weaponList.IndexOf(currentWeaponTransform.name);
            mouseLook.recoilRange = currentWeapon.recoil;
        }
    }

    private void Update()
    {
        //没有武器，这是一个错误
        if (currentWeapon == null)
        {
            return;
        }

        //射击
        if (Input.GetKey(InputSettings.Fire))
        {
            currentWeapon.DoAttack();
        }

        //装填
        if (Input.GetKeyDown(InputSettings.Reload))
        {
            currentWeapon.Reload();
        }

        //瞄准
        if (Input.GetKeyDown(InputSettings.Aim))
        {
            currentWeapon.Aim(true);
        }
        else if (Input.GetKeyUp(InputSettings.Aim))
        {
            currentWeapon.Aim(false);
        }

        //切换武器
        if (Input.GetKeyDown(InputSettings.MainWeapon))
        {
            StartCoroutine( SwapWeapon(0));
        }
        else if (Input.GetKeyDown(InputSettings.SecondaryWeapon))
        {
            StartCoroutine( SwapWeapon(1));
        }
        else if (Input.GetKeyDown(InputSettings.SpecialWeapon))
        {
            StartCoroutine(SwapWeapon(2));
        }

        //鼠标滚轮切换武器
        if (Input.GetAxisRaw(InputSettings.MouseScrollWheel)>0)
        {
            if(canSwap)
            {
                currentIndex++;
                currentIndex = currentIndex % weaponList.Count;
                if (weaponList.Count > 1)
                {
                    canSwap = false;
                    StartCoroutine(SwapWithMouse());
                }
            }
            
        }
        else if (Input.GetAxisRaw(InputSettings.MouseScrollWheel)<0)
        {
            if(canSwap)
            {
                if (currentIndex <= 0)
                {
                    currentIndex = weaponList.Count - 1;
                }
                else
                {
                    currentIndex--;
                }
                
                if (weaponList.Count > 1)
                {
                    canSwap = false;
                    StartCoroutine(SwapWithMouse());
                }
                
            }
            
        }

        //丢弃武器
        if(Input.GetKeyDown(InputSettings.DropWeapon))
        {
            DropWeapon();
        }
    }

    //根据索引切换武器,index不会超出范围
    public IEnumerator SwapWeapon(int index)
    {
        switch(index)
        {
            case 0:
                {
                    //切换的是另一把武器，且武器不为空
                    if(currentWeapon!= mainWeapon && weaponDict["MainWeapon"] != null)
                    {
                        currentIndex = 0;
                        yield return SwapWeaponAnimation();
                        currentWeaponTransform.gameObject.SetActive(false);
                        currentWeaponTransform = mainWeaponTransform;
                        currentWeapon = mainWeapon;
                        currentWeaponTransform.gameObject.SetActive(true);
                        mouseLook.recoilRange = currentWeapon.recoil;
                    }
                    break;
                }

            case 1:
                {
                    if (currentWeapon != secondaryWeapon && weaponDict["SecondaryWeapon"]!=null)
                    {
                        currentIndex = 1;
                        yield return SwapWeaponAnimation();
                        currentWeaponTransform.gameObject.SetActive(false);
                        currentWeaponTransform = secondaryWeaponTransform;
                        currentWeapon = secondaryWeapon;
                        currentWeaponTransform.gameObject.SetActive(true);
                        mouseLook.recoilRange = currentWeapon.recoil;
                    }
                    break;
                }
            case 2:
                {
                    if (currentWeapon != specialWeapon && weaponDict["SpecialWeapon"] != null)
                    {
                        currentIndex = 2;
                        yield return SwapWeaponAnimation();
                        currentWeaponTransform.gameObject.SetActive(false);
                        currentWeaponTransform = specialWeaponTransform;
                        currentWeapon = specialWeapon;
                        currentWeaponTransform.gameObject.SetActive(true);
                        mouseLook.recoilRange = currentWeapon.recoil;
                    }
                    break;
                }
        }
    }

    //滚轮切换武器
    public IEnumerator SwapWithMouse()
    {
        yield return SwapWeaponAnimation();
        var weaponType = weaponList[currentIndex];
        var next_weapon = weaponDict[weaponType];
        currentWeaponTransform.gameObject.SetActive(false);
        currentWeaponTransform = transformDic[next_weapon];
        currentWeapon = next_weapon;
        currentWeaponTransform.gameObject.SetActive(true);
        canSwap = true;
        mouseLook.recoilRange = currentWeapon.recoil;
    }

    //添加新武器
    public void AddNewWeapon(string weaponName, string gunType, GameObject gunPrefab)
    {
        //返回的是MainWeapon等的物体
        var newWeapon = GameObjectPool.instance.CreateObject(weaponName,gunType,gunPrefab);
        Debug.Log(newWeapon.name);

        //先把该武器调整到weaponholder下进行管理，再根据需求进行显影
        if (weaponTransform.ContainsKey(gunType))
        {
            var weaponNeedToCollectTransform = weaponTransform[gunType];
            GameObjectPool.instance.CollectGameObject(weaponNeedToCollectTransform.gameObject);
            weaponNeedToCollectTransform.SetParent(GameObjectPool.instance.transform);
        }

        if(weaponList.Contains(newWeapon.transform.name))
        {
            inventory.ChangeItem(newWeapon.transform.name, weaponDict[newWeapon.transform.name]);
        }

        //把新的武器的位置设置好
        newWeapon.transform.SetParent(weaponHolder);
        newWeapon.GetComponentInChildren<FireArm>().weaponHolder = weaponHolder;
        newWeapon.GetComponentInChildren<FireArm>().eyeCamera = TransformHelper.FindChild(weaponHolder, "Main Camera").GetComponent<Camera>();
        newWeapon.transform.localPosition = new Vector3(0, 0, 0);
        newWeapon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //设置新的武器的弹药
        newWeapon.GetComponentInChildren<FireArm>().currentAmmoCarried = inventory.inventoryDic[gunType].currentMagCarried;
        newWeapon.GetComponentInChildren<FireArm>().currentAmmoInMag = inventory.inventoryDic[gunType].currentInMag;

        //按照新的列表组织武器
        CollectWeapon();

        //由于武器列表按照优先级排序，永远显示当前索引即可
       var weaponTypeNeedToActive = weaponList[0];
        //判断，如果本身拿着一把武器
        if (currentWeaponTransform != null)
        {
            //如果马上要装备的武器和现在的武器索引不相同，就说明当前武器的优先级低于捡起的武器，所以要做显影
            if (currentWeaponTransform != weaponTransform[weaponTypeNeedToActive])
            {
                currentWeaponTransform.gameObject.SetActive(false);
                currentWeaponTransform = weaponTransform[weaponTypeNeedToActive];
                currentWeaponTransform.gameObject.SetActive(true);
                currentWeapon = weaponDict[weaponTypeNeedToActive];
                mouseLook.recoilRange = currentWeapon.recoil;
            }
            else
            {
                newWeapon.transform.gameObject.SetActive(false);
            }
        }
        else
        {
            currentWeaponTransform = weaponTransform[weaponTypeNeedToActive];
            currentWeaponTransform.gameObject.SetActive(true);
            currentWeapon = weaponDict[weaponTypeNeedToActive];
            mouseLook.recoilRange = currentWeapon.recoil;
        }

    }

    //判断是否可以丢弃武器，如果可以让对象池回收以节省性能，并设置下一把活动武器，如果不行则不执行
    public void DropWeapon()
    {
        //if(weaponList.Count==1)
        //{
        //    return;
        //}

        //移除武器
        currentWeaponTransform.SetParent(GameObjectPool.instance.transform);
        currentWeaponTransform.gameObject.SetActive(false);
        GameObjectPool.instance.CollectGameObject(currentWeaponTransform.gameObject);

        inventory.DropItem(currentWeaponTransform.name,currentWeapon.GetComponent<FireArm>());

        CollectWeapon();
        currentIndex--;
        if(currentIndex<0)
        {
            currentIndex = 0;
        }
        if(weaponList.Count==0)
        {
            currentWeapon = null;
            currentWeaponTransform = null;
            return;
        }
        currentWeapon = weaponDict[weaponList[currentIndex]];
        currentWeaponTransform = transformDic[currentWeapon];
        currentWeaponTransform.gameObject.SetActive(true);

    }

    //移除指定武器
    public void DropWeapon(Transform weaponTransform)
    {
        //if(weaponList.Count==1)
        //{
        //    return;
        //}

        //移除武器
        weaponTransform.SetParent(GameObjectPool.instance.transform);
        weaponTransform.gameObject.SetActive(false);
        GameObjectPool.instance.CollectGameObject(weaponTransform.gameObject);

        var tmp = weaponDict[weaponTransform.name];

        inventory.DropItem(weaponTransform.name, tmp);
    }

    private IEnumerator SwapWeaponAnimation()
    {
        yield return currentWeapon.SwapWeapon();
    }
}
