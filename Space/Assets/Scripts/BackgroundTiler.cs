using UnityEngine;

public class BackgroundTiler : MonoBehaviour
{
	public const int TILES_PER_ROW = 3;
	public Transform playerTransform;
	
	private int m_numTiles;
	private float m_totalWidth;
	private float m_halfWidth;
	
	private Vector2 m_textureOffset;
	
	private Transform[] m_tiles;
	private Material m_material;
	
	void Start ()
	{
		m_tiles = new Transform[ transform.childCount ];
		m_numTiles = m_tiles.Length;
		
		for( int i = 0; i < m_numTiles; i++ )
		{
			m_tiles[ i ] = transform.GetChild( i );
		}
		
		Renderer renderer = m_tiles[ 0 ].GetComponent<Renderer>();
		float tileWidth = renderer.bounds.size.x;
		m_totalWidth = tileWidth * m_numTiles / TILES_PER_ROW;
		m_halfWidth = m_totalWidth * 0.5f;
		
		m_material = renderer.sharedMaterial;
		m_textureOffset = m_material.mainTextureOffset;
	}
	
	void Update ()
	{
		CheckTilePositions();
	}
	
	private void CheckTilePositions()
	{
		if(playerTransform == null)
			return;

		for( int i = 0; i < m_numTiles; i++ )
		{
			// X
			if( m_tiles[ i ].position.x > playerTransform.position.x + m_halfWidth )
			{
				m_tiles[ i ].position -= new Vector3( m_totalWidth, 0, 0 );
			}
			else if( m_tiles[ i ].position.x < playerTransform.position.x - m_halfWidth )
			{
				m_tiles[ i ].position += new Vector3( m_totalWidth, 0, 0 );
			}
			
			// Y
			if( m_tiles[ i ].position.y > playerTransform.position.y + m_halfWidth )
			{
				m_tiles[ i ].position -= new Vector3( 0, m_totalWidth, 0 );
			}
			else if( m_tiles[ i ].position.y < playerTransform.position.y - m_halfWidth )
			{
				m_tiles[ i ].position += new Vector3( 0, m_totalWidth, 0 );
			}
		}
	}
	
	private void AnimateWater()
	{
		m_textureOffset.x = Mathf.Repeat( Time.time * 0.01f, 1.0f );
		m_textureOffset.y = m_textureOffset.x;
		m_material.mainTextureOffset = m_textureOffset;
	}
}