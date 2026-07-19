using System;
using UnityEngine;

[Serializable]
public class LevelData
{
    [SerializeField] private int level; public int Level => level;
    [SerializeField] private int[] mapLevels;
    public CardData[] boardCardDatas;
    public CardData[] handCardDatas;

    public LevelData(int level, int[] mapLevels, CardData[] boardCardDatas, CardData[] handCardDatas)
    {
        this.level = level;
        this.mapLevels = mapLevels;
        this.boardCardDatas = boardCardDatas;
        this.handCardDatas = handCardDatas;
    }

    
}
