using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDirector : MonoBehaviour
{
    // フロアーの数
    public const int FLOOR_X = 30;
    public const int FLOOR_Y = 30;

    [SerializeField] float floorSize = 0.5f;
    [SerializeField] float floorY = 1f;

    // 3色フロアーのプレハブ
    public GameObject[] prefabFloor;

    public enum FloorType// フロアーのタイルを列挙
    {
        Red,
        Blue,
        Green
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("FloorDirector Start 呼ばれた");
        Debug.Log($"prefabFloor.Length = {prefabFloor?.Length}");

        for (int x = 0; x < FLOOR_X; x++)
        {
            for (int y = 0; y < FLOOR_Y; y++)
            {
                Vector3 pos = new Vector3(
                    (x - FLOOR_X / 2f) * floorSize,
                    floorY,
                    (y - FLOOR_Y / 2f) * floorSize);

                FloorType type = GetFloor(x, y);
                GameObject prefab = GetPrefab(type);

                //if (prefab == null) continue;

                Instantiate(prefab, pos, Quaternion.identity);

            }
        }

    }

    FloorType GetFloor(int x, int y)
    {
        int value = (x + y) % 3;

        switch (value)
        {
            case 0: return FloorType.Red;
            case 1: return FloorType.Blue;
            case 2: return FloorType.Green;
            default: return FloorType.Red;
        }
    }

    GameObject GetPrefab(FloorType type)
    {
        int index = (int)type;

        if (prefabFloor == null || prefabFloor.Length <= index) return null;
        
        return prefabFloor[index];
    }
   
}
