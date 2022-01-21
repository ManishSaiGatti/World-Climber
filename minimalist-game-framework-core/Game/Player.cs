using System;
using System.Collections.Generic;
using System.Text;

class Player
{
    private Texture texture = Engine.LoadTexture("pickaxeSpriteRight.png");
    private Texture leftTexture = Engine.LoadTexture("pickaxeSpriteLeft.png");
    private Texture rightTexture = Engine.LoadTexture("pickaxeSpriteRight.png");

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
        xPos += 3;
        texture = leftTexture;
}

    public void left()
    {
        xPos -= 3;
        texture = rightTexture;
}

    public void up(float amount)
    {
        yPos -= amount;
    }
}
