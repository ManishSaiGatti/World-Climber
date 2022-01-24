using System;
using System.Collections.Generic;
using System.Text;

class Enemy
{
    public ResizableTexture enemyTexture;
    public int enemyX;
    public int initialX;
    public int enemyY;
    public Enemy()
    {
        Random rand = new Random();

        enemyY = -30;
        initialX = rand.Next(100, 500);
        enemyX = initialX;
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

    public int getEnemyX()
    {
        return enemyX;
    }

    public int getInitialX()
    {
        return initialX;
    }
    
    public virtual void drawEnemy()
    {
        Engine.DrawResizableTexture(enemyTexture, new Bounds2(enemyX, enemyY, 30, 30));
    }
    public virtual void enemyAct()
    {

    }
    
}
