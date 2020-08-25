using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public interface IWeapon
{
    void DoAttack();

    //取消当前动作
    void CancelCurrent();
}
