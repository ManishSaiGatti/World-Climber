using System;
using System.Collections.Generic;
using System.Text;


class DirtEnemy : Enemy
{
    public bool moveLeft;
    public DirtEnemy()
    {
        enemyTexture = Engine.LoadResizableTexture("Underground Enemy.png", 0, 0, 0, 0);
        moveLeft = true;
    }

    public override void drawEnemy()
    {
        Engine.DrawResizableTexture(enemyTexture, new Bounds2(enemyX, enemyY, 30, 30));
    }

    public override void enemyAct()
    {
        if (!moveLeft)
        {
            setEnemyX(getEnemyX() + 1);
            if (getEnemyX() == getInitialX())
            {
                moveLeft = true;
            }
        }
        else if (moveLeft)
        {
            setEnemyX(getEnemyX() - 1);
            if (getEnemyX() < getInitialX() - 50)
            {
                moveLeft = false;
            }
        }
    }
}

