using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BackgroundScript : MonoBehaviour
{
    public float speedX;
    public float speedY;

    [SerializeField]
    private Renderer bgRenderer;

    // Update is called once per frame
    void Update()
    {
        bgRenderer.material.mainTextureOffset += new Vector2(speedX * Time.deltaTime, speedY * Time.deltaTime);
    }
}
