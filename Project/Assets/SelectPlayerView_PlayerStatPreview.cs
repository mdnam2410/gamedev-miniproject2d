using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectPlayerView_PlayerStatPreview : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] Image avatar;
    [SerializeField] SelectPlayerView_StatBar statBarSpeed;
    [SerializeField] SelectPlayerView_StatBar statBarPower;
    [SerializeField] SelectPlayerView_StatBar statBarHealth;
    [SerializeField] TMP_Text specialAbility;

    private ConfigAvatarData config;

    public void Init(ConfigAvatarData config)
    {
        this.config = config;

        statBarSpeed.Init(abilityName: "Speed", 5);
        statBarPower.Init(abilityName: "Power", 5);
        statBarHealth.Init(abilityName: "Health", 5);
    }

    public void DisplayPlayerStat()
    {
        playerName.text = config.Name;
        avatar.sprite = config.Sprite;
        avatar.preserveAspect = true;

        statBarSpeed.SetStat(config.Speed);
        statBarPower.SetStat(config.Power);
        statBarHealth.SetStat(config.Health);

        specialAbility.text = config.SpecialAbility;
    }
}
