using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //TEMP
    [SerializeField] private Transform m_player;

    private Node[,] m_grid;
    [SerializeField] private Vector2 m_gridWorldSize;
    [SerializeField] private float m_nodeRadius;
    [SerializeField] private LayerMask m_unwalkableMask;

    private float m_nodeDiameter;
    private int m_gridSizeX;
    private int m_gridSizeY;

    public int MaxSize {get {return m_gridSizeX * m_gridSizeY;}}

    //TEMP
    public List<Node> path;

    private void Start() 
    {
        m_nodeDiameter = m_nodeRadius * 2;
        m_gridSizeX = Mathf.RoundToInt(m_gridWorldSize.x / m_nodeDiameter);
        m_gridSizeY = Mathf.RoundToInt(m_gridWorldSize.y / m_nodeDiameter);

        CreateGrid();
    }

    private void CreateGrid()
    {
        m_grid = new Node[m_gridSizeX,m_gridSizeY];

        Vector3 worldBottomLeft = transform.position - Vector3.right * m_gridWorldSize.x / 2 - Vector3.forward * m_gridWorldSize.y / 2;

        for(int x = 0; x < m_gridSizeX; x++)
        {
            for(int y = 0; y < m_gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * m_nodeDiameter + m_nodeRadius) + Vector3.forward * (y * m_nodeDiameter + m_nodeRadius);
                
                bool isWalkable = !(Physics.CheckSphere(worldPoint,m_nodeRadius,m_unwalkableMask));
                m_grid[x,y] = new Node(isWalkable,worldPoint,x,y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                //Check if inside grid               
				if (checkX >= 0 && checkX < m_gridSizeX && checkY >= 0 && checkY < m_gridSizeY) {
					neighbours.Add(m_grid[checkX,checkY]);
				}
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPosition(Vector3 worldPosition)
    {
        float posX = ((worldPosition.x - transform.position.x) + m_gridWorldSize.x * 0.5f) / m_nodeDiameter;
        float posY = ((worldPosition.z - transform.position.z) + m_gridWorldSize.y * 0.5f) / m_nodeDiameter;

        posX = Mathf.Clamp(posX, 0, m_gridWorldSize.x - 1);
        posY = Mathf.Clamp(posY, 0, m_gridWorldSize.y - 1);

        int x = Mathf.FloorToInt(posX);
        int y = Mathf.FloorToInt(posY);

        return m_grid[x, y];
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireCube(transform.position,new Vector3(m_gridWorldSize.x,1,m_gridWorldSize.y));

        if(m_grid != null)
        {
            Node playerNode = NodeFromWorldPosition(m_player.position);

            foreach(Node node in m_grid)
            {
                Gizmos.color = node.walkable? Color.white: Color.red;

                if(playerNode.worldPosition == node.worldPosition)
                    Gizmos.color = Color.cyan; 

                if(path != null) 
                {
                    if(path.Contains(node))
                    {
                        Gizmos.color = Color.black; 
                    }
                }   
                
                Gizmos.DrawCube(node.worldPosition,Vector3.one * (m_nodeDiameter - .1f));

                
            }
        }
    }

}
