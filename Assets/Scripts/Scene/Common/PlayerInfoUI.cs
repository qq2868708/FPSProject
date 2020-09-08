using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FPSProject.Character;

public class PlayerInfoUI : MonoBehaviour
{
    public Slider hpBar;
    public PlayerStatus playerStatus;
    public Text playerName;

    void Start()
    {
        hpBar = GetComponentInChildren<Slider>();
        playerStatus = this.transform.root.GetComponent<PlayerStatus>();
        playerStatus.updatePlayerInfo += UpdatePlayerInfo;
        playerName = this.transform.Find("PlayerName").GetComponent<Text>();
        playerName.text = PlayerPrefs.GetString("Player");
    }

    public void UpdatePlayerInfo()
    {
        Debug.Log(playerStatus.currentHp / playerStatus.maxHp);
        hpBar.value = playerStatus.currentHp*1.0f / playerStatus.maxHp;
    }
}
