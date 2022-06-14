using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour
{
    private Vector3[] doorMark = new Vector3[3];
    private int doorMarkIdx = 0;
    public GameObject calibrationPassthrough;
    public GameObject calibrationMarker;
    public GameObject passthroughRoom;

    void Start()
    {
        setCalibrationMode(true);
    }

    void setCalibrationMode(bool b) {
        calibrationPassthrough.SetActive(b);
        calibrationMarker.SetActive(b);
        if(b) {
            passthroughRoom.transform.position = new Vector3(100f, 100f, 100f);
        }
        doorMarkIdx = 0;
    }

    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.One))
        {
            if(doorMarkIdx < 3) {
                doorMark[doorMarkIdx] = calibrationMarker.transform.position;
                doorMarkIdx++;
            }
            if(doorMarkIdx == 3) {
                Vector3 doorX = doorMark[1] - doorMark[0];
                Vector3 doorY = doorMark[0] - doorMark[2] + Vector3.Project(doorMark[2]-doorMark[0], doorX);
                Vector3 doorPos = doorMark[0] + doorX/2.0f - doorY/2.0f;
                Quaternion doorRot = Quaternion.LookRotation(Vector3.Cross(doorX, doorY), doorY);
                passthroughRoom.GetComponent<PassthroughRoomController>().setSize(doorPos, doorRot, doorX.magnitude, doorY.magnitude);
                setCalibrationMode(false);
            }
        }
        if (OVRInput.GetUp(OVRInput.Button.Two))
        {
            setCalibrationMode(true);
        }
    }
}
