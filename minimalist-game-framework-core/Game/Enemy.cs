using System;
using System.Collections.Generic;
using System.Text;

class Enemy
{
    readonly ResizableTexture enemy1= Engine.LoadResizableTexture("Underground Enemy.png", 0, 0, 0, 0);
    int enemyX;
    int enemyY;
    public Enemy()
    {
        Random rand = new Random();

        enemyY = -95;
        enemyX = rand.Next(100, 500);      
    }

    public int setEnemyX(int x)
    {
        enemyX = x;
        return enemyX;
    }

    public int setEnemyY(int y)
    {
        enemyY = y;
        return enemyY;
    }

    public int getEnemyY()
    {
        return enemyY;
    }

    public void drawEnemy()
    {
        Engine.DrawResizableTexture(enemy1, new Bounds2(enemyX, enemyY, 100, 100));
    }
}
