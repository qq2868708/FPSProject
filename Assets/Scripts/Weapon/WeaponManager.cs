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
    private Dictionary<string, FireArm> weaponDict = new Dictionary<string, FireArm>();
    //将武器和管理他们的transform建立关系，如果没有武器，就不在此字典注册
    private Dictionary<FireArm, Transform> transformDic = new Dictionary<FireArm, Transform>();

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

    public int testInt;

    //做一些初始化的设置
    private void InitManager()
    {
        //统计当前的武器信息
        CollectWeapon();

        //禁用所有武器组件，屏蔽编辑器操作，不管在编辑器中如何配置，运行时结果不变
        foreach (var transform in weaponTransform.Values)
        {
            transform.gameObject.SetActive(false);
        }
        
    }

    //统计当前的武器信息，当捡起或者丢弃武器时调用
    public void CollectWeapon()
    {
        transformDic.Clear();
        weaponDict.Clear();
        weaponList.Clear();
        weaponTransform.Clear();
        mainWeaponTransform = TransformHelper.FindChild(this.transform, "MainWeapon");
        secondaryWeaponTransform = TransformHelper.FindChild(this.transform, "SecondaryWeapon");
        specialWeaponTransform = TransformHelper.FindChild(this.transform, "SpecialWeapon");
        if(mainWeaponTransform!=null)
        {
            weaponTransform.Add("MainWeapon", mainWeaponTransform);
            mainWeapon = mainWeaponTransform.GetComponentInChildren<FireArm>();
            transformDic.Add(mainWeapon, mainWeaponTransform);
            weaponDict.Add("MainWeapon", mainWeapon);
            weaponList.Add("MainWeapon");
        }
        if(secondaryWeaponTransform!=null)
        {
            weaponTransform.Add("Secondary", secondaryWeaponTransform);
            secondaryWeapon = secondaryWeaponTransform.GetComponentInChildren<FireArm>();
            transformDic.Add(secondaryWeapon, secondaryWeaponTransform);
            weaponDict.Add("SecondaryWeapon", secondaryWeapon);
            weaponList.Add("SecondaryWeapon");

        }
        if (specialWeaponTransform!=null)
        {
            weaponTransform.Add("SpecialWeapon", specialWeaponTransform);
            specialWeapon = specialWeaponTransform.GetComponentInChildren<FireArm>();
            transformDic.Add(specialWeapon, specialWeaponTransform);
            weaponDict.Add("SpecialWeapon", specialWeapon);
            weaponList.Add("SecondaryWeapon");
        }
    }

    private void Start()
    {
        InitManager();

        currentWeaponTransform = mainWeaponTransform;
        currentWeapon = mainWeapon;
        currentIndex = 0;

        //配置默认武器
        if(currentWeapon!=null)
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
        else
        {
            Debug.LogError("no Weapon");
            return;
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
            currentWeapon.Aim();
        }
        else if (Input.GetKeyUp(InputSettings.Aim))
        {
            currentWeapon.Aim();
        }

        //切换武器
        if (Input.GetKeyDown(InputSettings.MainWeapon))
        {
            SwapWeapon(0);
        }
        else if (Input.GetKeyDown(InputSettings.SecondaryWeapon))
        {
            SwapWeapon(1);
        }

        //鼠标滚轮切换武器
        if(Input.GetAxisRaw(InputSettings.MouseScrollWheel)>0)
        {
            currentIndex++;
            SwapWithMouse();
        }
        else if (Input.GetAxisRaw(InputSettings.MouseScrollWheel)<0)
        {
            if (currentIndex < 1)
            {
                currentIndex = weaponList.Count-1;
            }
            else
            {
                currentIndex--;
            }
            SwapWithMouse();
        }

        //丢弃武器
        if(Input.GetKeyDown(InputSettings.DropWeapon))
        {
            DropWeapon();
        }

    }

    //根据索引切换武器
    public void SwapWeapon(int index)
    {
        switch(index)
        {
            case 0:
                {
                    //切换的是另一把武器，且武器不为空
                    if(currentWeapon!= mainWeapon && weaponDict["MainWeapon"] != null)
                    {
                        currentWeaponTransform.gameObject.SetActive(false);
                        currentWeaponTransform = mainWeaponTransform;
                        currentWeapon = mainWeapon;
                        currentWeaponTransform.gameObject.SetActive(true);
                        currentIndex = 0;
                    }
                    break;
                }

            case 1:
                {
                    if (currentWeapon != secondaryWeapon && weaponDict["SecondaryWeapon"]!=null)
                    {
                        currentWeaponTransform.gameObject.SetActive(false);
                        currentWeaponTransform = secondaryWeaponTransform;
                        currentWeapon = secondaryWeapon;
                        currentWeaponTransform.gameObject.SetActive(true);
                        currentIndex = 1;
                    }
                    break;
                }
            case 2:
                {
                    if (currentWeapon != specialWeapon && weaponDict["SpecialWeapon"] != null)
                    {
                        currentWeaponTransform.gameObject.SetActive(false);
                        currentWeaponTransform = specialWeaponTransform;
                        currentWeapon = specialWeapon;
                        currentWeaponTransform.gameObject.SetActive(true);
                        currentIndex = 2;
                    }
                    break;
                }
        }
    }

    //滚轮切换武器
    public void SwapWithMouse()
    {
        currentIndex = Mathf.Abs(currentIndex) % weaponList.Count;
        SwapWeapon(currentIndex);
    }

    public void AddNewWeapon(FireArm fireArm)
    {

    }

    //判断是否可以丢弃武器，如果可以让对象池回收以节省性能，并设置下一把活动武器，如果不行则不执行
    public void DropWeapon()
    {
        if(weaponList.Count==1)
        {
            return;
        }
        //移除武器
        currentWeaponTransform.SetParent(GameObjectPool.instance.transform);
        currentWeaponTransform.gameObject.SetActive(false);
        GameObjectPool.instance.CollectGameObject(currentWeaponTransform.gameObject);

        currentIndex--;
        if(currentIndex<0)
        {
            currentIndex = 0;
        }
        CollectWeapon();
        currentWeapon = weaponDict[weaponList[currentIndex]];
        currentWeaponTransform = transformDic[currentWeapon];
        currentWeaponTransform.gameObject.SetActive(true);
        
    }

   
    
}
