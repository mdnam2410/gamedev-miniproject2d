using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviourSingletonPersistent<ConfigManager>
{
    public ConfigHero ConfigHero;
    public ConfigTank ConfigTank;
    public ConfigAvatar ConfigAvatar;
}
