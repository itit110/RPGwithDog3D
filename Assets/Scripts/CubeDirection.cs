using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDirection : MonoBehaviour
{
    // 土台となるブロックの定数
    public const int CUBE_NUM = 30;

    // 土台のキューブ
    public GameObject[] prefabCube;

    [SerializeField] float CubeSize = 1f;
    [SerializeField] float CubeY = 0.55f;

    public enum CubeType
    {
        Cube,
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int x = 0; x < CUBE_NUM; x++)
        {
            for(int y = 0; y < CUBE_NUM; y++)
            {
                Vector3 posCube = new Vector3(
                    (x - CUBE_NUM / 2f) * CubeSize,
                    CubeY,
                    (y - CUBE_NUM / 2f) * CubeSize);

                CubeType type = GetCube(x, y);
                GameObject prefab = GetPrefabCube(type);

                Instantiate(prefab, posCube, Quaternion.identity);
            }
        }
        
    }

    CubeType GetCube(int x, int y)
    {
        int value = (x + y) % 1;
        switch (value)
        {
            case 0: return CubeType.Cube;
            default: return CubeType.Cube;
        }
    }

    GameObject GetPrefabCube(CubeType type)
    {
        int index = (int)type;

        if (prefabCube == null || prefabCube.Length <= index) return null;

        return prefabCube[index];
    }

}
