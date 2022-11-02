using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerIndicator : MonoBehaviour
{
    [SerializeField] TMP_Text textPlayerName;

    public void SetName(string name) => textPlayerName.text = name;
}
