using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System.Runtime.InteropServices.Marshalling;
using System.Security;
using System.Reflection.Metadata.Ecma335;
using System.Linq.Expressions;
using System.Threading.Channels;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

class Program
{

    // Anpassbare Werte
    static uint screenx = 800;
    static uint screeny = screenx * 3 / 4;
    static int PlayerMoveSpeed = 10;
    static int minBallSpeed = 5;
    static int maxBallSpeed = 10;

    // Unterschiedliche Level
    enum GameLevel
    {
        Start,
        Ende,
        Standard,
    }


    static void GameLoop()
    {
        while (true)
        {
            Stop(playerleft, pY_size);
            Stop(playerright, pY_size);
            MoveP1(playerleft);
            MoveP2(playerright);
            Moveball();
            WallCollision();
            if (!collisionCooldown)
            {
                CheckCollision();
                HandleCollision();
            }
            Gamestatus();

            sfmlWindow.Clear(back);
            sfmlWindow.Draw(playerright);
            sfmlWindow.Draw(playerleft);
            sfmlWindow.Draw(ball);
            sfmlWindow.Draw(scoreRightText);
            sfmlWindow.Draw(scoreLeftText);

            DrawDevider();

            sfmlWindow.Display();

        }
    }

    static GameLevel currentLevel = GameLevel.Start;
    static bool Ende;


    // Objekte und Spieler
    static RectangleShape playerleft = new RectangleShape();
    static RectangleShape playerright = new RectangleShape();
    static RectangleShape devide = new RectangleShape();
    static CircleShape ball = new CircleShape();
    static Vector2f ballVelocity = new Vector2f(minBallSpeed, -minBallSpeed);
    static int scoreLeft = 0;
    static int scoreRight = 0;
    static Text scoreRightText;
    static Text scoreLeftText;



    // Größenbestimmung in Abhängigkeit zur Bildschirmgröße
    static int screenyi = (int)screeny;
    static int pY_size = screenyi / 6;
    static int pX_size = pY_size / 10;



    // Bestimmungen für SFML Window
    static Color back = Color.Black;
    static RenderWindow sfmlWindow = new RenderWindow(new VideoMode(screenx, screeny), "June`s Pong");
    static Random r = new Random();



    static void InitAllObjects()
    {
        //Devider
        devide.Size = new Vector2f(pX_size / 2, pY_size / 2);
        devide.FillColor = Color.White;

        //Spieler 1
        playerleft.Size = new Vector2f(pX_size, pY_size);
        playerleft.FillColor = Color.White;
        playerleft.Position = new Vector2f(screenx / 10, screeny / 2);

        scoreLeft = 0;

        //Spieler 2
        playerright.Size = new Vector2f(pX_size, pY_size);
        playerright.FillColor = Color.White;
        playerright.Position = new Vector2f(screenx - (screenx / 10), screeny / 2);

        scoreRight = 0;

        //Ball
        ball.Radius = screeny / 50;
        ball.FillColor = Color.White;
        ball.Position = new Vector2f(screenx / 2, screeny / 2);

        Console.WriteLine(ball.Position.X + ball.Position.Y);

        scoreRightText = new Text(scoreLeft.ToString(), default, 30);
        scoreLeftText = new Text(scoreRight.ToString(), default, 30);
        // Text Positionieren
        scoreRightText.Position = new Vector2f(screenx / 4, 10);
        scoreLeftText.Position = new Vector2f(3 * screenx / 4, 10);

        // verschiedene GameLevel initiieren
        switch (currentLevel)
        {
            case GameLevel.Start:

                break;
            case GameLevel.Ende:

                break;
            case GameLevel.Standard:

                break;

        }

        Ende = false;


    }

    static void Gamestatus()
    {
        if (Ende == true)
        {

            switch (currentLevel)
            {
                case GameLevel.Start:
                    // Aktionen für Start-Level
                    break;

                case GameLevel.Ende:
                    // Aktionen für Ende-Level
                    ball.Position = new Vector2f(screenx / 2, screeny / 2);
                    ballVelocity.Y = minBallSpeed;
                    ballVelocity.X = minBallSpeed;
                    break;

                case GameLevel.Standard:
                    // Aktionen für Standard-Level
                    ball.Position = new Vector2f(screenx / 2, screeny / 2);
                    break;

            }

            Ende = false;
        }
    }

    static void DrawDevider()
    {
        for (int x = pY_size / 4; x < screenyi; x += pY_size)
        {
            devide.Position = new Vector2f(screenx / 2, x);
            sfmlWindow.Draw(devide);
        }
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
            scoreLeft++;
            Ende = true;
        }
        //Event Endee wenn Wand berührt links
        if (ball.Position.X < 0)
        {
            ball.Position = new Vector2f(screenx / 2, screeny / 2);
            scoreRight++;
            Ende = true;
        }

    }
    //        if (player.Position.X >= 800)
    //        {
    //
    //            playerleft.Position = new Vector2f(0 - pX_size + 5, playerleft.Position.Y);
    //            RandomEvent();
    //        }
    //        if (player.Position.X <= (0 - pX_size))
    //        {
    //
    //            playerleft.Position = new Vector2f(800 - 5, playerleft.Position.Y);
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
            player.Position += new Vector2f(0, -PlayerMoveSpeed);
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.S))
        {
            player.Position += new Vector2f(0, +PlayerMoveSpeed);
        }

    }

    static void MoveP2(RectangleShape player)
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
        {
            player.Position += new Vector2f(0, -PlayerMoveSpeed);
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
        {
            player.Position += new Vector2f(0, PlayerMoveSpeed);
        }

    }

    static void MoveP1RL()
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
        {
            playerleft.Position += new Vector2f(-5, 0);
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.D))
        {
            playerleft.Position += new Vector2f(+5, 0);
        }
    }
    static void MoveP2RL()
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
        {
            playerright.Position += new Vector2f(-5, 0);
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {
            playerright.Position += new Vector2f(+5, 0);
        }
    }

    static void Moveball()
    {
        ball.Position += ballVelocity;

    }


    static bool collisionplayerleft = new bool();
    static bool collisionplayerright = new bool();

    //Kollision Player and ball
    static (bool, bool) CheckCollision()
    {
        FloatRect rectball = ball.GetGlobalBounds();
        FloatRect rectplayerleft = playerleft.GetGlobalBounds();
        FloatRect rectplayer2 = playerright.GetGlobalBounds();

        collisionplayerleft = rectball.Intersects(rectplayerleft);
        collisionplayerright = rectball.Intersects(rectplayer2);


        return (collisionplayerleft, collisionplayerright);
    }

    static void HandleCollision()
    {
        if (collisionplayerleft == true)
        {

            Bounce(playerleft);
            Console.WriteLine("Collision Player 1: " + collisionplayerleft);
        }

        if (collisionplayerright == true)
        {
            Bounce(playerright);
            Console.WriteLine("Collision Player 2: " + collisionplayerright);
        }

    }

    static void Bounce(RectangleShape player)
    {
        // Mittelpunkte beider Objekte errechnen
        float yCenterPosPlayer = player.Position.Y + (player.Size.Y / 2);
        float yCenterPosBall = ball.Position.Y + ball.Radius;
        // Distanz errechnen
        float yCollisionDist = yCenterPosBall - yCenterPosPlayer;


        // Abprallverhalten für X-Achse        
        if (Math.Abs(yCollisionDist) <= pY_size / 5)
        {
            ballVelocity.X = MathF.Sign(-ballVelocity.X) * MathF.Abs(minBallSpeed * 2);
        }

        else if (Math.Abs(yCollisionDist) <= pY_size / 3)
        {
            ballVelocity.X = MathF.Sign(-ballVelocity.X) * MathF.Abs(minBallSpeed * 3 / 2);
        }
        else
        {
            ballVelocity.X = MathF.Sign(-ballVelocity.X) * MathF.Abs(minBallSpeed);
        }


        // Abprallverhalten öfür Y-Achse
        float calculatedSpeed = yCollisionDist * 100 / pY_size / 5;

        // Begrenzung mit Mindestgeschwindigkeit & MaxGeschwindigkeit
        if (MathF.Abs(calculatedSpeed) <= minBallSpeed)
        {
            ballVelocity.Y = MathF.Sign(ballVelocity.Y) * MathF.Abs(minBallSpeed);
        }

        if (MathF.Abs(calculatedSpeed) > minBallSpeed && calculatedSpeed < maxBallSpeed)
        {
            ballVelocity.Y = MathF.Sign(ballVelocity.Y) * MathF.Abs(calculatedSpeed);
        }

        if (MathF.Abs(calculatedSpeed) > maxBallSpeed)
        {
            ballVelocity.Y = MathF.Sign(ballVelocity.Y) * MathF.Abs(maxBallSpeed);
        }

        // Balljumpbug verhindern
        collisionCooldown = true;

        // Timer für die Kollisionsverzögerung  
        System.Threading.Timer timer = null;
        timer = new System.Threading.Timer((state) =>
        {
            collisionCooldown = false;
            timer.Dispose();
        }, null, 250, System.Threading.Timeout.Infinite);

        Console.WriteLine("[Debug] Collision y-Distance: " + yCollisionDist + " calculatedSpeed: " + calculatedSpeed + " ballVelocity.Y: " + ballVelocity.Y + " ballVelocity.X: " + ballVelocity.X);

    }

    static bool collisionCooldown = false;

    static void Main()
    {


        sfmlWindow.SetFramerateLimit(60);

        InitAllObjects();

        sfmlWindow.DispatchEvents();

        GameLoop();



    }


}

