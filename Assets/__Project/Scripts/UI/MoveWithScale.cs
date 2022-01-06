using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MoveWithScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 leftBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 rightTop = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        float left = leftBottom.x;
        float bottom = leftBottom.y;
        float right = rightTop.x;
        float top = rightTop.y;

        float clampedLeft = Mathf.Clamp(left, 0, 4);

        if (clampedLeft <= 0) return;
        gameObject.transform.position = new Vector3(clampedLeft, leftBottom.y, leftBottom.z);

        Debug.Log("Left: " + left + " - CLAMPED LEFT: " + clampedLeft);
    }
}
