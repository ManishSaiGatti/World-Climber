using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Text;


class Block
{
    int blockX;
    int blockY;
    int blockHitCount;
    Vector2 blockLocation;
    Vector2 blockSize;
    readonly Texture _block;

    public Block(int x, int y, int hp)
    {
        _block = Engine.LoadTexture("square.png");
        blockX = x;
        blockY = y;
        blockLocation = new Vector2(blockX, blockY);
        blockSize = new Vector2(20, 25);
        blockHitCount = hp;
    }

    public int getX()
    {
        return blockX;
    }

    public int getY()
    {
        return blockY;
    }

    public Vector2 getLocation()
    {
        return blockLocation;
    }

    public int getBlockHp()
    {
        return blockHitCount;
    }

    public void changeY(int y)
    {
        this.blockY = y;
        blockLocation = new Vector2(blockX, y);
    }

    public void drawBlock()
    {
        Engine.DrawTexture(_block, blockLocation, null, blockSize);
    }

    public void blockHit()
    {
        blockHitCount -= 1;
    }
}
