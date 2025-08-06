using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PositionObject : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI debugText;

    public static PositionObject instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(RepositionObject());
    }

    // Update is called once per frame
    IEnumerator RepositionObject()
    {
        yield return new WaitForSeconds(1f);
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1f;
        // transform.rotation = Quaternion.LookRotation(Camera.main.transform.position);
        transform.rotation = Quaternion.Euler(Vector3.up * Camera.main.transform.eulerAngles.y);// + Vector3.up * 180f);

    }


    public void BroadcastDebug(string text)
    {
        debugText.text += "\n" + text;
    }
}
