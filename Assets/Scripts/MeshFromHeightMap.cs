using UnityEngine;

public class MeshFromHeightMap : MonoBehaviour
{
    
    public MeshFilter meshFilter;

    public Mesh FromHeightMap(float[,] map, AnimationCurve curve, float multiplier = 1)
    {
        Mesh mesh = new Mesh();

        int w = map.GetLength(0);
        int h = map.GetLength(1);

        float topLeftX = (w - 1) / -2.0f;
        float topLeftZ = (h - 1) / 2.0f;

        Vector3[] points = new Vector3[w * h];
        Vector2[] uvs = new Vector2[w * h];
        int[] indicies = new int[(w-1) * (h-1) * 6];

        int index = 0;
        int indexIndex = 0;
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                points[index] = new Vector3(x + topLeftX, curve.Evaluate(map[x,y])*multiplier, topLeftZ-y);


                uvs[index] = new Vector2(x / (float)w, y / (float)h);
                if ((x < (w - 1)) && (y < (h - 1)))
                {
                    indicies[indexIndex++] = index;
                    indicies[indexIndex++] = index + w + 1;
                    indicies[indexIndex++] = index + w;

                    indicies[indexIndex++] = index;
                    indicies[indexIndex++] = index + 1;
                    indicies[indexIndex++] = index + w + 1;
                }


                index++;
            }
        }


        mesh.vertices = points;
        mesh.triangles = indicies;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        if (meshFilter)
        {
            meshFilter.sharedMesh = mesh;
        }

        return mesh;
    }
}
