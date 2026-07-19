using System.Collections.Generic;
using UnityEngine;

public class DealingSystem 
{
    public BoardCardController boardCardController;
    public DealingCardController dealingCardController;
    public HandCardController handCardController;

    public DealingSystem Init()
    {
        boardCardController = new BoardCardController().Init();
        dealingCardController = new DealingCardController().Init();
        handCardController = new HandCardController().Init();
        return this;
    }
}
