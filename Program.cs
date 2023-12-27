using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System.Runtime.InteropServices.Marshalling;
using System.Security;
using System.Reflection.Metadata.Ecma335;
using System.Linq.Expressions;
using System.Threading.Channels;

class Program
{

    // Anpassbare Werte
    static uint screenx = 800;
    static int maxSpeed = 20;
    static uint screeny = screenx * 3 / 4;
    static int movespeed = 10;
    static int ballspeed = 5;

    // verschiedene Spiel-Level
    enum GameLevel
    {
        Start,
        Ende,
        Standard,
    }

    static GameLevel currentLevel = GameLevel.Start;
    static bool Ende;


    // Objekte und Spieler
    static RectangleShape player1 = new RectangleShape();
    static RectangleShape player2 = new RectangleShape();
    static RectangleShape devide = new RectangleShape();
    static CircleShape ball = new CircleShape();



    // Größenbestimmung in Abhängigkeit zur Bildschirmgröße
    static int screenyi = (int)screeny;
    static int pY_size = screenyi / 7;
    static int pX_size = pY_size / 10;

    

    // Bestimmungen für SFML Window
    static Color back = Color.Black;
    static RenderWindow sfmlWindow = new RenderWindow(new VideoMode(screenx, screeny), "June`s Pong");
    static Random r = new Random();


    static Vector2f ballVelocity = new Vector2f(ballspeed, -ballspeed);



    static void InitAllObjects()
    {
        //Devider
        devide.Size = new Vector2f(pX_size/2, pY_size / 2);
        devide.FillColor = Color.White;
        devide.Position = new Vector2f(screenx / 2, screeny / 2);

        //Spieler 1
        player1.Size = new Vector2f(pX_size, pY_size);
        player1.FillColor = Color.White;
        player1.Position = new Vector2f(screenx / 10, screeny / 2);

        //Spieler 2
        player2.Size = new Vector2f(pX_size, pY_size);
        player2.FillColor = Color.White;
        player2.Position = new Vector2f(screenx - (screenx / 10), screeny / 2);

        //Ball
        ball.Radius = screeny / 50;
        ball.FillColor = Color.White;
        ball.Position = new Vector2f(screenx / 2, screeny / 2);

        Console.WriteLine(ball.Position.X + ball.Position.Y);


    }

    static void Stop(RectangleShape player, float pY_size)
    {
        //Wenn Bildschirm nach oben verlassen will, immer zurück schicken
        if (player.Position.Y <= (0))
        {
            player.Position = new Vector2f(player.Position.X, 0);

        }
        //Wenn Bildschirm nach unten verlassen will, immer zurück schicken
        if (player.Position.Y >= (screeny - pY_size))
        {
            player.Position = new Vector2f(player.Position.X, screeny - pY_size);

        }
    }

    static void Gamestatus()
    {
        if (Ende == true)
        {
            ball.Position = new Vector2f(screenx / 2, screeny / 2);

            Ende = false;
        }
    }

    static void WallCollision()
    {
        //Prallt von oberer Wand ab
        if (ball.Position.Y < 0)
        {
            ballVelocity.Y *= -1;
        }

        //Prallt von unterer Wand ab
        if (ball.Position.Y > screeny - (2 + ball.Radius))
        {
            ballVelocity.Y *= -1;
        }

        //Event Endee wenn Wand berührt rechts
        if (ball.Position.X > (screenx - (2 * ball.Radius)))
        {
            ball.Position = new Vector2f(screenx / 2, screeny / 2);
            Ende = true;
        }
        //Event Endee wenn Wand berührt links
        if (ball.Position.X < 0)
        {
            ball.Position = new Vector2f(screenx / 2, screeny / 2);
            Ende = true;
        }

    }
    //        if (player.Position.X >= 800)
    //        {
    //
    //            player1.Position = new Vector2f(0 - pX_size + 5, player1.Position.Y);
    //            RandomEvent();
    //        }
    //        if (player.Position.X <= (0 - pX_size))
    //        {
    //
    //            player1.Position = new Vector2f(800 - 5, player1.Position.Y);
    //            RandomEvent();
    //        }

//    static void RandomEvent()
//    {
//        two.FillColor = new Color((byte)r.Next(0, 256), (byte)r.Next(0, 256), (byte)r.Next(0, 256));
//        one.FillColor = new Color((byte)r.Next(0, 256), (byte)r.Next(0, 256), (byte)r.Next(0, 256));
//        back = new Color((byte)r.Next(0, 256), (byte)r.Next(0, 256), (byte)r.Next(0, 256));

//        two.Position = new Vector2f((byte)r.Next(200, 600), (byte)r.Next(200, 800));
//        one.Position = new Vector2f((byte)r.Next(200, 600), (byte)r.Next(200, 800));

//        two.Size = new Vector2f((byte)r.Next(0, 250), (byte)r.Next(0, 250));
//        one.Size = new Vector2f((byte)r.Next(0, 250), (byte)r.Next(0, 250));
//    }

    static void MoveP1(RectangleShape player)
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
        {
            player.Position += new Vector2f(0, -movespeed);
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.S))
        {
            player.Position += new Vector2f(0, +movespeed);
        }

    }

    static void MoveP2(RectangleShape player)
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
        {
            player.Position += new Vector2f(0, -movespeed);
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
        {
            player.Position += new Vector2f(0, movespeed);
        }

    }

    static void MoveP1RL()
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
        {
            player1.Position += new Vector2f(-5, 0);
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.D))
        {
            player1.Position += new Vector2f(+5, 0);
        }
    }

    static void Moveball()
    {
        ball.Position += ballVelocity;

    }

    static void MoveP2RL()
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
        {
            player2.Position += new Vector2f(-5, 0);
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {
            player2.Position += new Vector2f(+5, 0);
        }
    }

    static bool collisionPlayer1 = new bool();
    static bool collisionPlayer2 = new bool();

    //Kollision Player and ball
    static (bool, bool) CheckCollision()
    {
        FloatRect rectball = ball.GetGlobalBounds();
        FloatRect rectplayer1 = player1.GetGlobalBounds();
        FloatRect rectplayer2 = player2.GetGlobalBounds();

        collisionPlayer1 = rectball.Intersects(rectplayer1);
        collisionPlayer2 = rectball.Intersects(rectplayer2);


        return (collisionPlayer1, collisionPlayer2);
    }

    static void HandleCollision()
    {

        if (collisionPlayer1 == true)
        {
            
            Bounce(player1);
            Console.WriteLine("Collision Player 1: " + collisionPlayer1);
        }

        if (collisionPlayer2 == true)
        {
            Bounce(player2);
            Console.WriteLine("Collision Player 2: " + collisionPlayer2);
        }
    }

    static void Bounce(RectangleShape player)
    {
        // Mittelpunkte beider Objekte errechnen
        float yCenterPosPlayer = player.Position.Y + (player.Size.Y / 2);
        float yCenterPosBall = ball.Position.Y + ball.Radius;
        // Distanz errechnen
        float yCollisionDist = yCenterPosBall - yCenterPosPlayer;
        // bounce!
        ballVelocity.X *= -1;
        // Toppart collision = bounce up; Bottompart collision = bounce down
        //ballVelocity.Y = MathF.Sign(yCollisionDist) * MathF.Abs(ballVelocity.Y); 
        ballVelocity.Y = (maxSpeed * (yCollisionDist * 100 / pY_size)) / 100;

        // Wenn Kollision in Mitte gerade wegfliegen
//        if ( MathF.Abs(yCollisionDist) <= pY_size * 0.3)
//        {
//            ballVelocity.Y = 0; 
//        }
        // Kollision am Ende -> -/+5 Winkel
//        else if ( MathF.Abs(yCollisionDist) < pY_size * 0.7)
//        {
//            ballVelocity.Y = 5;
        // }
        //Kollision normal -> scharfer Winkel
//        else if ( MathF.Abs(yCollisionDist) < pY_size)
//        {
//            ballVelocity.Y = 8;
//        }


        Console.WriteLine("[Debug] Collision y-Distance: " + yCollisionDist + "Playersize: " + pY_size + " ballVelocityY: " + ballVelocity.Y);

    }

    static void Main()
    {


        sfmlWindow.SetFramerateLimit(60);

        InitAllObjects();

        sfmlWindow.DispatchEvents();


        while (true)
        {
            Stop(player1, pY_size);
            Stop(player2, pY_size);
            MoveP1(player1);
            MoveP2(player2);
            Moveball();
            WallCollision();
            CheckCollision();
            HandleCollision();

            sfmlWindow.Clear(back);
            sfmlWindow.Draw(player2);
            sfmlWindow.Draw(player1);
            sfmlWindow.Draw(ball);
            sfmlWindow.Draw(devide);
            
            sfmlWindow.Display();

        }

    }


}

