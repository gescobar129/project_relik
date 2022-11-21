using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearGradient : MonoBehaviour
{
    [SerializeField]
    [ColorUsageAttribute(true, true)]
    private Color color1;
    [ColorUsageAttribute(true, true)]
    [SerializeField]
    private Color color2;

    private Material gradientMaterial;

    // Start is called before the first frame update
    void Start()
    {
        gradientMaterial = GetComponent<Renderer>().material;
        gradientMaterial.SetColor("_Color1", color1);
        gradientMaterial.SetColor("_Color2", color2);
    }

}
