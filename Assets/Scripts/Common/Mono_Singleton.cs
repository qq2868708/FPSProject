/// <summary>
/// Generic Mono singleton.
/// </summary>
using UnityEngine;

//这个自己继承自己的作用应该是，限定了可以继承本抽象类的类的种类。比如如果T是int类型
//就会返回一个int类型的实例，但是这是没有意义的，可能会出错，所以限制了必须是已经可以
//产生单例效果的类继承本类
public abstract class Mono_Singleton<T> : MonoBehaviour where T : Mono_Singleton<T>{
	
	private static T m_Instance = null;
    
	public static T instance{
        get{
			if( m_Instance == null ){
            	m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if( m_Instance == null ){
                    m_Instance = new GameObject("Singleton of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();
					 m_Instance.Init();
                }
               
            }
            return m_Instance;
        }
    }

    //如果挂在物体上，Unity自动调用，在get方法中就会直接返回对象的实例；如果没挂，会从get方法中安全的获得实例
    private void Awake(){
   
        if( m_Instance == null ){
            m_Instance = this as T;
        }
    }
 
    public virtual void Init(){}
 

    private void OnApplicationQuit(){
        m_Instance = null;
    }
}