using UnityEngine;

public class DiamondSquare
{

    // System.Random random;
    private int _size;
    private float[,] map;
    private float min, max;

    public float[,] GenerateMap(int size, float smoothness = .5f, int seed = 1337 , float start = 1)
    {
        _size = size;  
        map = new float[size , size];

        min = float.MaxValue;
        max = float.MinValue;

        //random = new System.Random(seed);
        Random.InitState(seed);

        int side = size - 1;
        float roughnessValue = 1;

        //initialize the four corners
        map[0,0] = RecordMinMax(Gauss(roughnessValue * start));
        map[0, side] = RecordMinMax(Gauss(roughnessValue * start));
        map[side, side] = RecordMinMax(Gauss(roughnessValue * start));
        map[side, 0] = RecordMinMax(Gauss(roughnessValue * start));


        while(side > 1)
        {
            Step(map, side, roughnessValue);
            side /= 2;
            roughnessValue *= Mathf.Pow(2, -smoothness);
        }

        //normalize the result
        float error = max - min;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++) 
            {
                map[x,y] = (map[x,y] - min) / error;
                //map[x,y] = Mathf.InverseLerp(min, max, map[x, y]);

            }
        }

        return map;
    }

    public void Step(float[,] map, int side, float roughness)
    {
        int size = _size - 1;
        int halfSide = side / 2;

        //Diamond
        for (int y = 0; y < size; y += side)
        {
            for (int x = 0; x < size; x += side)
            {
                float c00 = map[x, y];
                float c01 = map[x+side, y];
                float c11 = map[x+side, y+side];
                float c10 = map[x, y+side];

                //center is average plus random offset
                map[x + halfSide, y + halfSide] = RecordMinMax(((c00 + c01 + c11 + c10) / 4f) + Gauss(roughness));
            }
        }

        //Square
        for (int y = 0; y < _size; y += halfSide)
        {
            for (int x = (y + halfSide) % side; x < _size; x += side)
            {

                int k = 0;
                float avg = 0;

                Offset(x - halfSide, y, ref avg, ref k);
                Offset(x + halfSide, y, ref avg, ref k);
                Offset(x, y - halfSide, ref avg, ref k);
                Offset(x, y + halfSide, ref avg, ref k);

                //center is average plus random offset
                map[x, y] = RecordMinMax(avg / k + Gauss(roughness));
            }
        }

    }

    private void Offset(int x, int y, ref float avg, ref int k)
    {
        if (x < 0 || x >= _size) return;
        if (y < 0 || y >= _size) return;
        avg += map[x, y];
        k++;
    }
    //its not normal distribution, or "gaussian", I don't know how... just random in Range[-distribution, distribution]
    private float Gauss(float distribution)
    {
        float range = Random.value * distribution * 2 - distribution;
        return range;
    }

    private float RecordMinMax(float value)
    {
        min = Mathf.Min(value, min);
        max = Mathf.Max(value, max);
        return value;
    }

}