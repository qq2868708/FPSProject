using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//这个可以被修改成配置文件
public static class AnimationSettings
{
    //动画机变量
    public static string idle = "Idle";
    public static string run = "Run";
    public static string walk = "Walk";
    public static string reloadOutof = "ReloadOutof";
    public static string reloadLeft = "ReloadLeft";

    //动画片段的名称
    public static string takeOutClip = "take_out_weapon@assault_rifle_01";
    public static string idleClip = "idle@assault_rifle_01";
    public static string walkClip = "walk@assault_rifle_01";
    public static string runClip = "run@assault_rifle_01";
    public static string shootClip = "fire@assault_rifle_01 0";

    

    public static Dictionary<string ,string> Dic = new Dictionary<string, string>();

    static AnimationSettings()
    {
        Dic.Add(idle, idleClip);
        Dic.Add(run, runClip);
        Dic.Add(walk, walkClip);
    }
}
