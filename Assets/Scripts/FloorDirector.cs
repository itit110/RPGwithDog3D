using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDirector : MonoBehaviour
{
    // フロアーの定数
    public const int FLOOR_X = 30;
    public const int FLOOR_Y = 30;

    // フロアーのサイズ感
    [SerializeField] float floorSize = 1f;
    [SerializeField] float floorThickess = 0.5f; // Floorの厚み

    // 土台ブロックのサイズ
    public float cubeSize = 1f;
    public float cubeY = 0.55f;

    // ブロックに乗せるフロアーの位置
    float floorY;

    // フロアーのプレハブ
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
        floorY = (cubeY + cubeSize / 2f) + floorThickess / 2f;

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
