using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public GameObject go;
    public float time;

    // Use this for initialization
    IEnumerator Start()
    {
        go.GetComponent<CanvasRenderer>().SetAlpha(0f);
        go.gameObject.GetComponent<RawImage>().CrossFadeAlpha(1f, time / 2, true);
        yield return new WaitForSeconds(time / 2);
        go.GetComponent<CanvasRenderer>().SetAlpha(1f);
        go.gameObject.GetComponent<RawImage>().CrossFadeAlpha(0, time / 2, true);
        Destroy(go, time);
    }
}
