using UnityEngine;
using System.Collections;

public class PerlinNoise {

	private int _seed;
	
	public PerlinNoise(int seed) => _seed = seed;

    public int CalculateMaxOctavesCount(int worldWidth) {
        int maxOctavesCount = 0;

        while (worldWidth != 0) {
            worldWidth /= 2;
            maxOctavesCount++;
		}

        return maxOctavesCount;
	}
	
	public void Get2DPerlinNoise(int xSize, int ySize, int nOctaves, float fBias, ref float[,] perlinArray) {

		Random.InitState(_seed);
		float [,] seedArray = new float[xSize, ySize];

		for (int x = 0; x < xSize; x++) {
			for (int y = 0; y < ySize; y++) {
				seedArray[x,y] = Random.Range(0.0f, 1.0f);
			}
		}

		PerlinNoise2D(xSize, ySize, seedArray, nOctaves, fBias, ref perlinArray);
	}

	private void PerlinNoise2D(int nWidth, int nHeight, float[,] fSeed, int nOctaves, float fBias, ref float[,] perlinArray) {

		for (int x = 0; x < nWidth; x++) {
			for (int y = 0; y < nHeight; y++)
			{				
				float fNoise = 0.0f;
				float fScaleAcc = 0.0f;
				float fScale = 1.0f;

				for (int o = 0; o < nOctaves; o++)
				{
					int nPitch = nWidth >> o;
					int nSampleX1 = (x / nPitch) * nPitch;
					int nSampleY1 = (y / nPitch) * nPitch;
					
					int nSampleX2 = (nSampleX1 + nPitch) % nWidth;					
					int nSampleY2 = (nSampleY1 + nPitch) % nHeight;

					float fBlendX = (float)(x - nSampleX1) / (float)nPitch;
					float fBlendY = (float)(y - nSampleY1) / (float)nPitch;

					fBlendX = fade(fBlendX);
					fBlendY = fade(fBlendY);

					float fSampleA = lerp(fBlendX, fSeed[nSampleX1, nSampleY1], fSeed[nSampleX2, nSampleY1]);
					float fSampleB = lerp(fBlendX, fSeed[nSampleX1, nSampleY2], fSeed[nSampleX2, nSampleY2]);

					fScaleAcc += fScale;
					fNoise += lerp(fBlendY, fSampleA, fSampleB) * fScale;
					fScale = fScale / fBias;
				}

				// Scale to seed range
				perlinArray[x, y] = fNoise / fScaleAcc;
			}
		}
	}

	private float fade(float t) => t * t * t * (t * (t * 6 - 15) + 10);

	private float lerp(float t, float a, float b) => a + t * (b - a); 
}
