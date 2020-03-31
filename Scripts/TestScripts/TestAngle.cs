using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAngle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var vec1 = new Vector3(2.9f, -0.2f, -7.7f);
        var vec2 = new Vector3(-0.2f, 0, 1.0f);

        var angle = Vector3.Angle(vec1, vec2);

        Debug.Log(angle);
        Debug.DrawLine(Vector3.zero, vec1, Color.red, 100f);
        Debug.DrawLine(Vector3.zero, vec2, Color.blue, 100f);
        Debug.DrawLine(vec1, vec2, Color.white, 100f);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
