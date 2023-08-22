using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i< octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if(scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x =0; x < mapWidth; x++)
            {
                float amplitude = 1; // ����
                float frequency = 1; // ���ļ�
                float noiseHeight = 0;

                // octaves ��ŭ ������� ����
                for (int i = 0; i < octaves; i++  )
                {
                    // (x - halfWidth): �߾��� scaleUp �ϱ� ����
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x; // ���ļ���ŭ �����ش�.
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y; // ���ļ���ŭ �����ش�.

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // * 2 - 1�� �ؼ� -1 ~ 1 ������ ������ �ֵ��� ����
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance; // persistance(���Ӽ�): ������ ��ȭ��
                    frequency *= lacunarity; // lacunarity(��ť�θ�Ƽ): ���ļ��� ��ȭ��
                }

                if(noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                // ����ȭ 0 ~ 1 ������ ����
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
