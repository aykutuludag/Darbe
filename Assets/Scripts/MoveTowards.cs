using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public GameObject go;
    public Vector3 target;
    public float speed;

    void Update()
    {
        float step = speed * Time.deltaTime;
        go.transform.localPosition = Vector3.MoveTowards(go.transform.localPosition, target, step);
    }
}