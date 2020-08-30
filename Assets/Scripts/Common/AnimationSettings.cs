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
    public static string aim = "Aim";
    public static string fire = "Fire";
    public static string holster = "Holster";

    //适用于大狙
    public static string reloadOnce = "ReloadOnce";

    //动画片段的名称
    public static string takeOutClip = "take_out_weapon";
    public static string idleClip = "idle";
    public static string walkClip = "walk";
    public static string runClip = "run";
    public static string shootClip="Fire";
    public static string aimClip = "Aim";



    public static Dictionary<string ,string> Dic = new Dictionary<string, string>();

    static AnimationSettings()
    {
        Dic.Add(idle, idleClip);
        Dic.Add(run, runClip);
        Dic.Add(walk, walkClip);
    }
}
