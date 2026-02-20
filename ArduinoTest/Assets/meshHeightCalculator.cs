using UnityEngine;

public class meshHeightCalculator : MonoBehaviour
{
    public Mesh mesh;
    public float ObjectHeight;
    public Material mat;
    public int matIndex;
    void Start()
    {
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mat = gameObject.GetComponent<MeshRenderer>().materials[matIndex];
    }

    // Update is called once per frame
    void Update()
    {
        ObjectHeight = mesh.bounds.size.y;
        mat.SetFloat("_ObjectHeight", ObjectHeight);
    }
}
