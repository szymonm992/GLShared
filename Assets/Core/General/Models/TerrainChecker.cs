using UnityEngine;

namespace GLShared.General.Models
{
    public class TerrainChecker
    {
        public string GetLayerName(Vector3 position, Terrain terrain)
        {
            float[] cellMix = GetTextureMix(position, terrain);
            float stringest = 0f;
            int index = 0;

            for (int i = 0; i < cellMix.Length; i++)
            {
                if (cellMix[i] > stringest)
                {
                    index = i;
                    stringest = cellMix[i];
                }
            }

            return terrain.terrainData.terrainLayers[index].name;
        }

        private float[] GetTextureMix(Vector3 position, Terrain terrain)
        {
            var terrainPosition = terrain.transform.position;
            var terrainData = terrain.terrainData;

            int mapX = Mathf.RoundToInt((position.x - terrainPosition.x) / terrainData.size.x * terrainData.alphamapWidth);
            int mapZ = Mathf.RoundToInt((position.z - terrainPosition.z) / terrainData.size.z * terrainData.alphamapHeight);

            float[,,] splatMapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

            float[] cellMix = new float[splatMapData.GetUpperBound(2) + 1];

            for (int i = 0; i < cellMix.Length; i++)
            {
                cellMix[i] = splatMapData[0, 0, i];
            }

            return cellMix;
        }
    }
}
