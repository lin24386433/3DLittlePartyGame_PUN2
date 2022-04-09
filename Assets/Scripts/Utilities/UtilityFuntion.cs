using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityFuntion 
{
    public static Sprite Texture2DToSprite(Texture2D texture2D)
    {
        Texture2D myImg = texture2D;
        Sprite sprite = Sprite.Create(myImg, new Rect(0, 0, myImg.width, myImg.height), Vector2.zero);
        return sprite;
    }
}
