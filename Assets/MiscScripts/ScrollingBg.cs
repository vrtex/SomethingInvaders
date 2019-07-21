using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBg : MonoBehaviour
{
    public float speed = 1;

    public Renderer bgRenderer;

    void Update()
    {
        bgRenderer.material.mainTextureOffset = new Vector2(0, Time.time * speed);
    }
}
