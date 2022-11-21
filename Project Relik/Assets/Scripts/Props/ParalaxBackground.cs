using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxBackground : MonoBehaviour
{
    [SerializeField]
    private Vector2 paralaxEffect;
    [SerializeField]
    Texture2D paralaxImage = null;

    private float texLength = 0;
    private Vector2 camStartPos;
    private Vector2 startPos;
    private Material planeMaterial;

    // Start is called before the first frame update
    void Start()
    {
        var meshRenderer = GetComponent<Renderer>();

        camStartPos = Camera.main.transform.position;
        startPos = transform.position;

        texLength = meshRenderer.bounds.size.x;
        planeMaterial = meshRenderer.material;
        planeMaterial.SetTexture("_MainTex", paralaxImage);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 camPosition = Camera.main.transform.position;
        Vector2 camDistance = new Vector2(camPosition.x, camPosition.y) - camStartPos;

        Vector2 newPos = startPos + camDistance;
        //transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

        planeMaterial.SetVector("_ScrollValue", new Vector2(camDistance.x / texLength * paralaxEffect.x, camDistance.y / texLength * paralaxEffect.y));
    }
}
