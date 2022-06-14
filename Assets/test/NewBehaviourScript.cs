using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Vector3[] doorPos = new Vector3[4];
    private int verticesNum = 0;
    private OVRPassthroughLayer passthroughLayer;
    public Material skybox;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = null;
        GameObject ovrCameraRig = GameObject.Find("OVRCameraRig");
        if (ovrCameraRig == null)
        {
            Debug.LogError("Scene does not contain an OVRCameraRig");
            return;
        }
        passthroughLayer = ovrCameraRig.GetComponent<OVRPassthroughLayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.Two))
        {
            RenderSettings.skybox = null;
            passthroughLayer.overlayType = OVROverlay.OverlayType.Underlay;
            verticesNum = 0;
            passthroughLayer.enabled = false;
            passthroughLayer.RemoveSurfaceGeometry(gameObject);
            passthroughLayer.projectionSurfaceType = OVRPassthroughLayer.ProjectionSurfaceType.Reconstructed;
            passthroughLayer.enabled = true;
        }

        if (OVRInput.GetUp(OVRInput.Button.One))
        {
            if(verticesNum < 3) {
                Vector3 pos = GameObject.Find("Marker").transform.position;
                doorPos[verticesNum] = pos;
                verticesNum++;
            }
            if(verticesNum == 3) {
                RenderSettings.skybox = skybox;
                float gl = doorPos[2].y;
                Vector3[] vertices = new Vector3[13];
                vertices[3] = doorPos[1] + 30 * Vector3.Normalize(doorPos[0] - doorPos[1]);
                vertices[4] = doorPos[0];
                vertices[5] = doorPos[1];
                vertices[6] = doorPos[0] + 30 * Vector3.Normalize(doorPos[1] - doorPos[0]);
                vertices[7] = new Vector3(vertices[3].x, gl, vertices[3].z);
                vertices[8] = new Vector3(vertices[4].x, gl, vertices[4].z);
                vertices[9] = new Vector3(vertices[5].x, gl, vertices[5].z);
                vertices[10]= new Vector3(vertices[6].x, gl, vertices[6].z);
                // vertices[8] = doorPos[2];
                // vertices[9] = doorPos[3];

                vertices[1] = new Vector3(vertices[3].x, 30, vertices[3].z);
                vertices[2] = new Vector3(vertices[6].x, 30, vertices[6].z);

                vertices[11] = vertices[7]  + 30 * Vector3.Normalize(Vector3.Cross(vertices[1] - vertices[7], vertices[10] - vertices[7]));
                vertices[12] = vertices[10] + 30 * Vector3.Normalize(Vector3.Cross(vertices[7] - vertices[10], vertices[2] - vertices[10]));

                int[] triangles = new int[42];
                triangles[0] = 3;
                triangles[1] = 4;
                triangles[2] = 8;
                triangles[3] = 8;
                triangles[4] = 7;
                triangles[5] = 3;
                triangles[6] = 5;
                triangles[7] = 6;
                triangles[8] = 10;
                triangles[9] = 10;
                triangles[10]= 9;
                triangles[11] = 5;

                triangles[12]= 1;
                triangles[13]= 2;
                triangles[14]= 6;
                triangles[15]= 6;
                triangles[16]= 3;
                triangles[17]= 1;

                triangles[18]= 7;
                triangles[19]= 10;
                triangles[20]= 12;
                triangles[21]= 12;
                triangles[22]= 11;
                triangles[23]= 7;
                triangles[24]= 1;
                triangles[25]= 3;
                triangles[26]= 11;
                triangles[27]= 2;
                triangles[28]= 12;
                triangles[29]= 6;
                triangles[30]= 1;
                triangles[31]= 11;
                triangles[32]= 2;
                triangles[33]= 2;
                triangles[34]= 11;
                triangles[35]= 12;
                triangles[36]= 3;
                triangles[37]= 2;
                triangles[33]= 11;
                triangles[39]= 6;
                triangles[40]= 12;
                triangles[41]= 10;

                Mesh mesh = new Mesh();
                mesh.SetVertices(vertices);
                mesh.SetTriangles(triangles, 0);
                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                meshFilter.mesh = mesh;
                
                passthroughLayer.enabled = false;
                passthroughLayer.projectionSurfaceType = OVRPassthroughLayer.ProjectionSurfaceType.UserDefined;
                passthroughLayer.AddSurfaceGeometry(gameObject, true);
                // gameObject.GetComponent<MeshRenderer>().enabled = false;
                passthroughLayer.overlayType = OVROverlay.OverlayType.Overlay;
                passthroughLayer.enabled = true;
            }
        }
        
    }
}
