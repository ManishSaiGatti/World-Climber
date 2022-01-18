using System;
using System.Collections.Generic;

class Game
{
    public static readonly string Title = "IceClimber";
    public static readonly Vector2 Resolution = new Vector2(640, 480);

    readonly Texture intro = Engine.LoadTexture("1.png");
    readonly Texture introHover = Engine.LoadTexture("2.png");
    readonly Texture end = Engine.LoadTexture("3.png");
    readonly Texture endHover = Engine.LoadTexture("4.png");

    int totalPoints = 0;
    public Game()
    {
    }

    public void Update()
    {
        Engine.DrawTexture(intro, Vector2.Zero);


    }
}
