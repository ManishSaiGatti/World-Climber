using System;
using System.Collections.Generic;
using System.Text;

class Player
{
    //public Texture texture = Engine.LoadTexture("png");
    private Texture texture = Engine.LoadTexture("pickaxeSprite2.png");

    public float xPos = 240;
    public float yPos = 350;

    public Texture getTexture()
    {
        return texture;
    }

    public void changeTexture(Texture t)
    {
        texture = t;
    }

    public Vector2 getVectorPos()
    {
        return new Vector2(xPos, yPos);
    }

    public void right()
    {
        xPos += 5;
    }

    public void left()
    {
        xPos -= 5;
    }

    public void up(float amount)
    {
        yPos -= amount;
    }
}
