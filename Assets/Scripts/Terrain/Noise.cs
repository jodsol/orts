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
                float amplitude = 1; // 진폭
                float frequency = 1; // 주파수
                float noiseHeight = 0;

                // octaves 만큼 노이즈맵 생성
                for (int i = 0; i < octaves; i++  )
                {
                    // (x - halfWidth): 중앙을 scaleUp 하기 위해
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x; // 주파수만큼 곱해준다.
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y; // 주파수만큼 곱해준다.

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // * 2 - 1을 해서 -1 ~ 1 사이의 범위에 있도록 보정
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance; // persistance(지속성): 진폭의 변화량
                    frequency *= lacunarity; // lacunarity(레큐널리티): 주파수의 변화량
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
                // 정규화 0 ~ 1 값으로 보정
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
