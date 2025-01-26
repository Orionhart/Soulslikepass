using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    [SerializeField] TMP_Text txt;

    public void SetText(string t)
    {
        txt.text = t;
    }
}
