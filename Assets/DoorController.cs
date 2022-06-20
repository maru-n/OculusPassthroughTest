using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public SerialHandler serialHandler;    
    // Start is called before the first frame update

    private float doorSerialValueClose = 1112;
    private float doorSerialValueOpen = 722;
    void Start()
    {
        serialHandler.OnDataReceived += OnDataReceived;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDataReceived(string message)
    {
        var data = message.Split(
                new string[] { "\n" }, System.StringSplitOptions.None);
        try
        {
            float v = float.Parse(data[0]);
            Debug.Log("###Door###");
            Debug.Log(v);
            Debug.Log("##########");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);//エラーを表示
        }
    }
}
