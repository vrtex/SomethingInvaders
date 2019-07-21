using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [Serializable]
    public enum MoveAlignment
    {
        Tilt,
        Align,
        None
    }

    public Tuple<float, float> horizontalBounds = null;
    public Tuple<float, float> verticalBounds = null;
    public Vector2 speed = new Vector2(1, 1);
    public MoveAlignment alignment = MoveAlignment.Align;
    public float tiltAmount = 10;
    public float rotationOffset = 0f;

    public bool autoMove = false;

    private void Update()
    {
        if(!autoMove)
            return;
        Move(1, 1);
    }

    public void Move(float horizontal, float vertical)
    {
        if(!enabled)
            return;
        Vector3 previousPosition = transform.position;
        Vector3 position = transform.position;
        position += new Vector3(horizontal * speed.x * Time.deltaTime, 0, vertical * speed.y * Time.deltaTime);

        if(horizontalBounds != null)
            position.x = Mathf.Clamp(position.x, horizontalBounds.Item1, horizontalBounds.Item2);
        if(verticalBounds != null)
            position.z = Mathf.Clamp(position.z, verticalBounds.Item1, verticalBounds.Item2);

        transform.position = position;

        switch(alignment)
        {
            case MoveAlignment.Align:
                if(Math.Abs(horizontal) > 0.01 || Math.Abs(vertical) > 0.01)
                    transform.rotation = Quaternion.LookRotation(position - previousPosition, Vector3.up);
                break;
            case MoveAlignment.Tilt:
                transform.rotation = Quaternion.Euler(0, rotationOffset, Mathf.Clamp(-horizontal * tiltAmount, -45, 45));
                break;
            case MoveAlignment.None:
                break;
        }
    }
}
