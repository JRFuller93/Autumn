using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject m_levelNodePrefab;

    private const int m_levelWidth = 8;
    private const int m_levelDepth = 8;

    private void Awake()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        for(int i = 0; i < m_levelWidth; i++)
        {
            for(int j = 0; j < m_levelDepth; j++)
            {
                Vector3 nodePosition = new Vector3(i,0,j);
                GameObject levelNode = Instantiate(m_levelNodePrefab,nodePosition,Quaternion.identity);

                levelNode.transform.parent = this.transform; 
                levelNode.name = $"{i} x {j}";
            }
        }
    }
}
