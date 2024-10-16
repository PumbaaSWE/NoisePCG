using UnityEngine;

public class ShowTexture : MonoBehaviour
{
    public Renderer rend;
    public Gradient colorGradient;
    public AnimationCurve heightScaleCurve;
    private Texture2D texture;

    [Range(1,9)]public int K = 3;
    public int size;
    [Range(0,1)] public float smoothness = .5f;
    [Range(0, 1)] public float startCornersMultiplier = .5f;
    public int seed = 0;
    public float amplitude = 5f;

    private void OnValidate()
    {
        size = 1 + (int)Mathf.Pow(2, K);
    }


    void Start()
    {
        if(!rend) rend = GetComponent<Renderer>();

        GenerateMap();
    }
    [MakeButton]
    public void GenerateMap()
    {
        DiamondSquare diamondSquare = new DiamondSquare();
        size = 1 + (int)Mathf.Pow(2, K);
        var map = diamondSquare.GenerateMap(size, smoothness, seed, startCornersMultiplier);
        DrawMap(map);

        GetComponent<MeshFromHeightMap>().FromHeightMap(map, heightScaleCurve, amplitude);
    }
    [MakeButton]
    public void GenerateRandomMap()
    {
        RandomSeed();
        smoothness = Random.value;
        GenerateMap();
    }


    [MakeButton]
    public void RandomSeed()
    {
        seed = Random.Range(int.MinValue, int.MaxValue);
    }


    public void DrawMap(float[,] map)
    {
        int w = map.GetLength(0);
        int h = map.GetLength(1);
        texture = new Texture2D(w, h);

        Color32[] colors = new Color32[w * h];

        for (int y = 0; y < h; y++)
        {

            for (int x = 0; x < w; x++)
            {
                //colors[x + y * w] = Color.Lerp(Color.black, Color.white, map[x,y] * .5f + 1f);
                colors[x + y * w] = colorGradient.Evaluate(map[x, y]);
            }
        }


        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels32(colors);
        texture.Apply();

        rend.sharedMaterial.mainTexture = texture;
        //rend.transform.localScale = new Vector3(w, h, 1);
    }
}
