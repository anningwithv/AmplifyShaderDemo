using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanController : MonoBehaviour
{
    public Material ScanMat;
    public Transform ScanCenterObj;

    private float m_TotalTime = 5;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SceneScanCor());
        }
    }

    private IEnumerator SceneScanCor()
    {
        float timer = 0;
        float scanRange = 1;
        float opacity = 1;

        ScanMat.SetVector("_CenterPos", ScanCenterObj.position);

        while (true)
        {
            timer += Time.deltaTime;
            if (timer < m_TotalTime)
            {
                scanRange = Mathf.Lerp(0, 200, timer/ m_TotalTime);
                opacity = Mathf.Lerp(1, 0, timer/ m_TotalTime);
                ScanMat.SetFloat("_ScanRange", scanRange);
                ScanMat.SetFloat("_ScanOpacity", opacity);
            }
            else
                yield break;

            yield return null;
        }
    }
}
