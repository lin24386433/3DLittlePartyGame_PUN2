using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INeedTarget 
{
    void Init(PlayerModel ownerPlayerModel, PlayerModel targetPlayerModel);
}
