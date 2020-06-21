using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GetMesh : MonoBehaviour
{

    public GameObject bounceValue;
    public GameObject stiffnessValue;
    public GameObject pressureCheck;
    public GameObject forceValue;

    public GameObject sliderBounce;
    public GameObject sliderStiffness;
    public GameObject sliderForce;

    private MeshFilter meshFilter;
    private Mesh mesh;

    SoftBody[] softBodyVertices;
    Vector3[] currentMeshVertices;


    private int holdForce;

    bool activeHolding;
    bool iWasStatic;

    public GameObject redDot;
    public GameObject greendDot;
    private List<GameObject> dots = new List<GameObject>();

    private Vector3 forceDirection;
    private Vector3 moveDirection;

    private void GetVertices()
    {
        softBodyVertices = new SoftBody[mesh.vertices.Length];
        currentMeshVertices = new Vector3[mesh.vertices.Length];
        for (int a = 0; a < mesh.vertices.Length; a++)
        {
            softBodyVertices[a] = new SoftBody(a, mesh.vertices[a], mesh.vertices[a], Vector3.zero);
            currentMeshVertices[a] = mesh.vertices[a];
        }
    }

    private void UpdateVertices()
    {
        for (int a = 0; a < softBodyVertices.Length; a++)
        {
            softBodyVertices[a].updateVelocity(sliderBounce.GetComponent<UnityEngine.UI.Slider>().value);
            softBodyVertices[a].settleObject(sliderStiffness.GetComponent<UnityEngine.UI.Slider>().value);

            softBodyVertices[a].currentVertexPosition += softBodyVertices[a].currentVelocity * Time.deltaTime;
            currentMeshVertices[a] = softBodyVertices[a].currentVertexPosition;
        }

        mesh.vertices = currentMeshVertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    private void checkBoxForce()
    {
        if (pressureCheck.GetComponent<UnityEngine.UI.Toggle>().isOn)
        {
            if (iWasStatic)
            {
                holdForce = 0;
                iWasStatic = false;
            }

            sliderForce.active = false;

            if (activeHolding == true)
            {

                if (Input.GetMouseButton(0))
                {
                    holdForce = holdForce + 10;
                }

                if (holdForce >= 2000)
                {
                    holdForce = 2000;
                }
            }

            forceValue.GetComponent<UnityEngine.UI.Text>().text = holdForce.ToString();
        }

        

        else
        {
            iWasStatic = true;
            sliderForce.active = true;
            holdForce = (int) sliderForce.GetComponent<UnityEngine.UI.Slider>().value;
            forceValue.GetComponent<UnityEngine.UI.Text>().text = holdForce.ToString();
        }

        bounceValue.GetComponent<UnityEngine.UI.Text>().text = ((int)sliderBounce.GetComponent<UnityEngine.UI.Slider>().value).ToString();

        stiffnessValue.GetComponent<UnityEngine.UI.Text>().text = ((int)sliderStiffness.GetComponent<UnityEngine.UI.Slider>().value).ToString();
    }

    public void ApplyForceToPoint(Vector3 point, float force)
    {
        for (int a = 0; a < softBodyVertices.Length; a++)
        {
            softBodyVertices[a].forceToVertex(transform, point, force);
        }

        holdForce = 0;
    }

    public void OnMouseEnter()
    {
        activeHolding = true;
    }

    public void OnMouseExit()
    {
        activeHolding = false;
        holdForce = 0;
    }

    public void OnMouseUp()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {      
            greendDot.transform.position = hit.point;

            forceDirection = transform.InverseTransformPoint(transform.position) - transform.InverseTransformPoint(hit.point);

            ApplyForceToPoint(hit.point, holdForce);
        }
    }

    public void exitApp()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        holdForce = 0;

        GetVertices();

        for (int a = 0; a < currentMeshVertices.Length; a++)
        {
            GameObject newDot = Instantiate(redDot, new Vector3(currentMeshVertices[a].x, currentMeshVertices[a].y, currentMeshVertices[a].z), Quaternion.identity);
            dots.Add(newDot);
        }

        bounceValue.GetComponent<UnityEngine.UI.Text>().text = ((int)sliderBounce.GetComponent<UnityEngine.UI.Slider>().value).ToString();
        stiffnessValue.GetComponent<UnityEngine.UI.Text>().text = ((int)sliderStiffness.GetComponent<UnityEngine.UI.Slider>().value).ToString();

        iWasStatic = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkBoxForce();
        UpdateVertices();

        for (int a = 0; a < softBodyVertices.Length; a++)
        {
            dots[a].transform.position = transform.TransformPoint(currentMeshVertices[a]);
        }
    }
}
