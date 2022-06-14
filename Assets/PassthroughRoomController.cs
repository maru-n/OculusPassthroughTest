using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughRoomController : MonoBehaviour
{
    [Range(-90.0f, 90.0f)]
    public float doorAngle = 0.0f;
    public GameObject door;
    public GameObject doorWall;
    public GameObject DoorHinge;
    public GameObject wall;
    public GameObject headAnchor;

    void Start()
    {
    }

    void Update()
    {
        const float HEAD_SIZE_OFFSET = 0.1f;
        Vector3 headFromDoor = Quaternion.Inverse(transform.rotation) * (headAnchor.transform.position - transform.position);
        wall.SetActive(headFromDoor.z < HEAD_SIZE_OFFSET);
        DoorHinge.transform.rotation = transform.rotation * Quaternion.Euler(0, doorAngle, 0);
    }

    void openDoor(float angle) {
        
    }

    public void setSize(Vector3 pos, Quaternion rot, float doorWidth, float doorHeight)
    {
        const float PASSTHROUGH_ROOM_SCALE = 10.0f;
        float w = doorWidth / PASSTHROUGH_ROOM_SCALE;
        float h = doorHeight / PASSTHROUGH_ROOM_SCALE;
        transform.position = pos;
        transform.rotation = rot;
        Material m = doorWall.GetComponent<Renderer>().material;
        // Material m = GameObject.Find("DoorWall")
        m.SetFloat("_DoorWidth", w);
        m.SetFloat("_DoorHeight", h);
        // GameObject door = GameObject.Find("Door");
        door.transform.localScale = new Vector3(w, h, 0.01f);
    } 
}
