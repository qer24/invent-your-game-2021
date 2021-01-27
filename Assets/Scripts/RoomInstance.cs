using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInstance : MonoBehaviour
{
	public Texture2D tex;
	[HideInInspector] public Vector2 gridPos;
	public int type; // 0: normal, 1: enter

	float tileSize = 16;
	Vector2 roomSizeInTiles = new Vector2(9,17);

	public void Setup(Texture2D _tex, Vector2 _gridPos, int _type)
    {
		tex = _tex;
		gridPos = _gridPos;
		type = _type;
	}

	Vector3 positionFromTileGrid(int x, int y)
    {
		Vector3 ret;
		//find difference between the corner of the texture and the center of this object
		Vector3 offset = new Vector3((-roomSizeInTiles.x + 1)*tileSize, (roomSizeInTiles.y/4)*tileSize - (tileSize/4), 0);
		//find scaled up position at the offset
		ret = new Vector3(tileSize * (float) x, -tileSize * (float) y, 0) + offset + transform.position;
		return ret;
	}
}
