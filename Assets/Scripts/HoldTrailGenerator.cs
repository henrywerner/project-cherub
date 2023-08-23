using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Heavily borrowed from https://www.youtube.com/watch?v=8lXDLy24rJw
// Great tutorial btw.


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HoldTrailGenerator : MonoBehaviour
{
    public float maxLength; // the target length (which can extend offscreen)
    public float impactPoint; // The judgement line's location. (We'll shrink the trail as we hold the note)
    // yes, impactPoint is somewhat redundant since it's just at [0,0].

    public float tailLength;
    public float tailWidth;

    private List<Vector3> verticesList;
    private List<int> trianglesList;

    Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = mesh;

        GenerateTrail(); // remove later
    }

    public void GenerateTrail()
    {
        //setup
        verticesList = new List<Vector3>();
        trianglesList = new List<int>();

        //stem setup
        Vector3 tailOrigin = Vector3.zero;
        float tailhalfWidth = tailWidth / 2f;

        //Stem points
        verticesList.Add(tailOrigin + (tailhalfWidth * Vector3.right));
        verticesList.Add(tailOrigin + (tailhalfWidth * Vector3.left));
        verticesList.Add(verticesList[0] + (tailLength * Vector3.forward));
        verticesList.Add(verticesList[1] + (tailLength * Vector3.forward));

        //Stem triangles
        trianglesList.Add(0);
        trianglesList.Add(1);
        trianglesList.Add(3);

        trianglesList.Add(0);
        trianglesList.Add(3);
        trianglesList.Add(2);

        //assign lists to mesh.
        mesh.vertices = verticesList.ToArray();
        mesh.triangles = trianglesList.ToArray();
    }
}
