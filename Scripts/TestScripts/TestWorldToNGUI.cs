using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWorldToNGUI : MonoBehaviour
{
    public UIButton btnPrefab;

    public UIButton btnRandomHero;

    public GameObject presentHit;
    // Start is called before the first frame update
    void Start()
    {
        TouchScreen();

    }

    public void TouchScreen()
    {
        StartCoroutine(TouchScreenImpl());
    }
    IEnumerator TouchScreenImpl()
    {
        while (true)
        {
            yield return null;
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 1f);
                RaycastHit hit;
                int layerMask = 1 << LayerMask.NameToLayer("Hero");

                if (Physics.Raycast(ray, out hit, 100, layerMask))
                {
                    var point = hit.point;
                   
                    //Debug.Log(hit.transform.position);
                    //targetposition을 게임카메라의 viewPort 좌표로 변경.

                    //Vector3 position = Camera.main.WorldToViewportPoint(hit.transform.position);

                    ////해당 좌표를 uiCamera의 World좌표로 변경.
                    //btnRandomHero.transform.position = UICamera.mainCamera.ViewportToWorldPoint(position);
                    ////값 정리.
                    //position = btnRandomHero.transform.localPosition;
                    //position.x = Mathf.RoundToInt(position.x);
                    //position.y = Mathf.RoundToInt(position.y);
                    //position.z = 0.0f;
                    //btnRandomHero.transform.localPosition = position;
                    //yield return new WaitForSeconds(0.3f);



                    Vector3 v = Camera.main.WorldToScreenPoint(point);
                    //Debug.Log(v);

                    v.x = (v.x / Screen.width) * 1080;
                    v.y = (v.y / Screen.height) * 1920;
                    //Debug.Log(v);
                    //Debug.Log(v - new Vector3(1080 / 2, 1920 / 2, 0));
                    btnPrefab.transform.localPosition = v - new Vector3(1080 / 2, 1920 / 2, 0);

                    if(btnRandomHero==null)
                    {
                        btnRandomHero = Instantiate(btnPrefab);
                        btnRandomHero.transform.position = btnRandomHero.transform.localPosition;
                    }
                  
                   
                }


            }
        }

    }
}
