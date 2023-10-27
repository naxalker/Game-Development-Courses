using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontRQ : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMPro.TextMeshPro>().fontMaterial.renderQueue = 2001;
    }
}
