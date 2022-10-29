using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    Speed,
    Health,
    Power,
    Shield
}

[System.Serializable]
public struct BuffData
{
    public BuffType buffType;
    public float buffValue;
};

public class BuffManager : MonoBehaviour
{
    int buffNum = 0;
    bool[] isOccupiedPos;

    public List<Transform> listPos;
    public int maxNum;
    public float leftSpawnTime;
    public float maxTimeRange;
    public float minTimeRange;
    public List<GameObject> listBuff;

    // Start is called before the first frame update
    void Start()
    {
        isOccupiedPos = new bool[listPos.Count];
        leftSpawnTime = GetNextLeftTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (buffNum < maxNum)
        {
            if (leftSpawnTime > 0)
            {
                leftSpawnTime -= Time.deltaTime;
            }
            else
            {
                int posIndex = GetNextPosIndex();
                if (posIndex != -1)
                {
                    leftSpawnTime = GetNextLeftTime();
                    GameObject buff = listBuff[GetNextBuffIndex()];

                    Transform transform = listPos[posIndex];
                    GameObject tmp = Instantiate(buff, transform);
                    tmp.transform.position = transform.position;

                    isOccupiedPos[posIndex] = true;
                    buffNum++;
                }

            }
        }
    }

    float GetNextLeftTime()
    {
        return Random.Range(minTimeRange, maxTimeRange);
    }

    int GetNextPosIndex()
    {
        int index = Random.Range(0, listPos.Count - buffNum);
        int count = 0;

        for (int i = 0; i < listPos.Count; ++i)
        {
            if (!isOccupiedPos[i])
            {
                if (count == index)
                    return i;
                count++;
            }
        }

        return -1;
    }

    int GetNextBuffIndex()
    {
        return Random.Range(0, listBuff.Count);
    }
}
