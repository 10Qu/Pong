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
using System.Dynamic;
using System.Runtime.CompilerServices;

class Program
{

    // Adaptable values
    static uint screenx = 800;
    static uint screeny = screenx * 3 / 4;
    static int PlayerMoveSpeed = 10;
    static int maxScore = 10;
    // Standard Min: 5 Max: 10
    static int minBallSpeed = 5;
    static int maxBallSpeed = 10;

    // Objects and Player
    static RectangleShape playerleft = new RectangleShape();
    static RectangleShape playerright = new RectangleShape();
    static RectangleShape devide = new RectangleShape();
    static CircleShape ball = new CircleShape();

    static Vector2f ballVelocity = new Vector2f(minBallSpeed, -minBallSpeed);
    static int scoreLeft = 0;
    static int scoreRight = 0;
    static RectangleShape transparent = new RectangleShape();

    // Texts
    static Font titanOneFont = new Font("Fonts/TitanOne.ttf");
    static Text scoreRightText = new Text();
    static Text scoreLeftText = new Text();
    static Text Greeting = new Text();
    static Text Congrats = new Text();



    // Determination of size depending on the screen size
    static int screenyi = (int)screeny;
    static int pY_size = screenyi / 6;
    static int pX_size = pY_size / 10;



    // SFML Window Regulations
    static Color back = Color.Black;
    static RenderWindow sfmlWindow = new RenderWindow(new VideoMode(screenx, screeny), "June`s Pong");
    static Random r = new Random();




    static GameState currentState = GameState.StartMenu;
    static Level currentLevel;
    static bool PlayerScored;
    static bool collisionCooldown = false;
    static bool collisionplayerleft = new bool();
    static bool collisionplayerright = new bool();
    static bool WinnerLeft = new bool();
    static bool WinnerRight = new bool();

    // Different StandardLevelFunctions
    enum GameState
    {
        StartMenu,
        InGame,
        EndMenu,
    }

    enum Level
    {
        Standard,
        Crazy
        
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
    //        two.FillColor = new Color(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));
    //        one.FillColor = new Color(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));
    //        back = new Color(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));

    //        two.Position = new Vector2f(r.Next(200, 600), r.Next(200, 800));
    //        one.Position = new Vector2f(r.Next(200, 600), r.Next(200, 800));

    //        two.Size = new Vector2f(r.Next(0, 250), r.Next(0, 250));
    //        one.Size = new Vector2f(r.Next(0, 250), r.Next(0, 250));
    //    }





    static void Main()
    {
        sfmlWindow.SetFramerateLimit(60);

        InitAllObjects();

        GameLoop();
    }


    // General GameLoop
    static void GameLoop()
    {
        while (true)
        {

            sfmlWindow.DispatchEvents();

            switch (currentState)
            {
                case GameState.StartMenu:

                    DisplayStartMenu();

                    // Waiting for player input
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                    {
                        currentState = GameState.InGame;
                    }

                    break;

                // Waiting until player scores
                case GameState.InGame:

                    //Start with standard Level0
//                    Level0();
                    // Random next Level until
                    while (!WinnerLeft && !WinnerRight)
                    {
                        switch (currentLevel)
                        {
                            case Level.Standard:
                                StandardLevelFunctions();
                                CheckWinnerCondition();
                                if (PlayerScored == true)
                                {
                                    NextLevel();
                                }
                                PlayerScored = false;
                                break;

                            case Level.Crazy:
                                StandardLevelFunctions();
                                CrazyBalls();
                                CheckWinnerCondition();
                                if (PlayerScored == true)
                                {
                                    NextLevel();
                                }
                                PlayerScored = false;
                                break;
                        }

                        StandardLevelFunctions();
                        CheckWinnerCondition();
                    }

                    break;


                case GameState.EndMenu:
                    GameEnd();
                    Restart();
                    break;
            }
        }
    }

    static void NextLevel()
    {
        // Random Background color
        back = new Color((byte) r.Next(0, 256),(byte) r.Next(0, 256),(byte) r.Next(0, 256));

        // calculate random Level
        Random random = new Random();
        Array values = Enum.GetValues(typeof(Level));
        currentLevel = (Level)values.GetValue(random.Next(values.Length));

        Console.WriteLine("Next Level: " + currentLevel);
    }

    static void DisplayStartMenu()
    {
        // StartMenu

        sfmlWindow.DispatchEvents();



        sfmlWindow.Clear(back);
        sfmlWindow.Draw(playerright);
        sfmlWindow.Draw(playerleft);
        sfmlWindow.Draw(ball);
        sfmlWindow.Draw(transparent);
        
        Greeting.DisplayedString = "Welcome!";
        FloatRect textBounds = Greeting.GetLocalBounds();
        Greeting.Origin = new Vector2f(textBounds.Width / 2, textBounds.Height / 2);
        Greeting.Position = new Vector2f(screenx / 2, screeny / 2);
        sfmlWindow.Draw(Greeting);

        Greeting.DisplayedString = "Have fun playing Pong";
        textBounds = Greeting.GetLocalBounds();
        Greeting.Origin = new Vector2f(textBounds.Width / 2, textBounds.Height / 2);
        Greeting.Position = new Vector2f(screenx / 2, screeny / 2 + Greeting.CharacterSize);
        sfmlWindow.Draw(Greeting);

        Greeting.DisplayedString = "made by June";
        textBounds = Greeting.GetLocalBounds();
        Greeting.Origin = new Vector2f(textBounds.Width / 2, textBounds.Height / 2);
        Greeting.Position = new Vector2f(screenx / 2, screeny / 2 + 2*Greeting.CharacterSize);
        sfmlWindow.Draw(Greeting);

        Greeting.DisplayedString = "press Space to start the Game";
        textBounds = Greeting.GetLocalBounds();
        Greeting.Origin = new Vector2f(textBounds.Width / 2, textBounds.Height / 2);
        Greeting.Position = new Vector2f(screenx / 2, screeny / 2 + 4*Greeting.CharacterSize);
        sfmlWindow.Draw(Greeting);
        
        DrawDevider();

        sfmlWindow.Display();


    }


    static void StandardLevelFunctions()
    {
        ConstrainPlayerPosition(playerleft, pY_size);
        ConstrainPlayerPosition(playerright, pY_size);
        MovePlayerLeft(playerleft);
        MovePlayerRight(playerright);
        Moveball();
        WallCollision();
        if (!collisionCooldown)
        {
            CheckCollision();
            HandleCollision();
        }



        sfmlWindow.Clear(back);
        sfmlWindow.Draw(scoreRightText);
        sfmlWindow.Draw(scoreLeftText);
        sfmlWindow.Draw(playerright);
        sfmlWindow.Draw(playerleft);
        sfmlWindow.Draw(ball);


        DrawDevider();

        sfmlWindow.Display();


    }
    static void CrazyBalls()
    {
        int count = 0;
        if (collisionplayerleft || collisionplayerright)
        {
            count += 1;
        }
        if (PlayerScored)
        {
            count = 0;
        }

        if (count >= 5)
        {
            count = 0;
            Console.WriteLine("New Ball is generated");
            sfmlWindow.Draw(ball);
        }


    }
    

    static void GameEnd()
    {
        sfmlWindow.DispatchEvents();

        sfmlWindow.Clear(back);
        // Scoreboard
        sfmlWindow.Draw(scoreLeftText);
        sfmlWindow.Draw(scoreRightText);

        Congrats.DisplayedString = "You won!";
        FloatRect textBounds2 = Congrats.GetLocalBounds();
        Congrats.Origin = new Vector2f(textBounds2.Width / 2, textBounds2.Height / 2);
        Congrats.Position = new Vector2f(screenx / 2, screeny / 2 + Congrats.CharacterSize);

        sfmlWindow.Draw(Congrats);
        // Winner animation
        if (WinnerLeft == true)
        {
            Congrats.DisplayedString = "Congratulation left Player!";
            textBounds2 = Congrats.GetLocalBounds();
            Congrats.Origin = new Vector2f(textBounds2.Width / 2, textBounds2.Height / 2);
            Congrats.Position = new Vector2f(screenx / 2, screeny / 2);
            back = new Color((byte) r.Next(0, 256),(byte) r.Next(0, 256),(byte) r.Next(0, 256));

        }
        if (WinnerRight == true)
        {
            Congrats.DisplayedString = "Congratulation right Player";
            textBounds2 = Congrats.GetLocalBounds();
            Congrats.Origin = new Vector2f(textBounds2.Width / 2, textBounds2.Height / 2);
            Congrats.Position = new Vector2f(screenx / 2, screeny / 2);
            back = new Color((byte) r.Next(0, 256),(byte) r.Next(0, 256),(byte) r.Next(0, 256));

        }

        sfmlWindow.Draw(Congrats);
        sfmlWindow.Draw(transparent);


        Text Restart = new Text();
        Restart.DisplayedString = "Press: Space to Restart";
        FloatRect textBounds = Restart.GetLocalBounds();
        Restart.Origin = new Vector2f(textBounds.Width / 2, textBounds.Height / 2);
        Restart.Position = new Vector2f(screenx / 2, screeny / 2);
        Restart.CharacterSize = screeny / 20;
        sfmlWindow.Draw(Restart);
        
        Restart.DisplayedString = "Press: Esc to Exit";
        textBounds = Restart.GetLocalBounds();
        Restart.Origin = new Vector2f(textBounds.Width / 2, textBounds.Height / 2);
        Restart.Position = new Vector2f(screenx / 2, screeny / 2 + Restart.CharacterSize);
        sfmlWindow.Draw(Restart);

        sfmlWindow.Display();
        Thread.Sleep(1000);
    }
    static void Restart()
    {
    }

    static void InitAllObjects()
    {
        Console.WriteLine("Spiel wird gestartet");
        // Devider
        devide.Size = new Vector2f(pX_size / 2, pY_size / 2);
        devide.FillColor = Color.White;

        // Transparent
        transparent.FillColor = new Color(150, 50, 50, 50);
        transparent.Size = new Vector2f(screenx, screeny);

        // Player Left
        playerleft.Size = new Vector2f(pX_size, pY_size);
        playerleft.FillColor = Color.White;
        playerleft.Position = new Vector2f(screenx / 10, screeny / 2);

        scoreLeft = 0;

        // Player Right
        playerright.Size = new Vector2f(pX_size, pY_size);
        playerright.FillColor = Color.White;
        playerright.Position = new Vector2f(screenx - (screenx / 10), screeny / 2);

        scoreRight = 0;

        // Ball
        ball.Radius = screeny / 50;
        ball.FillColor = Color.White;
        ball.Position = new Vector2f(screenx / 2, screeny / 2);

        Console.WriteLine(ball.Position.X + ball.Position.Y);

        // Scoring
        scoreRightText = new Text
        {
            Font = titanOneFont,
            DisplayedString = scoreRight.ToString(),
            CharacterSize = screeny / 20,
            Position = new Vector2f(3 * screenx / 4, 10),
        };
        scoreLeftText = new Text
        {
            Font = titanOneFont,
            DisplayedString = scoreLeft.ToString(),
            CharacterSize = screeny / 20,
            Position = new Vector2f(screenx / 4, 10),
        };

        // Greeting for Game Start
        Greeting = new Text
        {
            Font = titanOneFont,
            CharacterSize = screeny / 20,
            FillColor = new Color((byte) r.Next(0, 256),(byte) r.Next(0, 256),(byte) r.Next(0, 256)),
        };

/*         // Set text centered
        FloatRect textBounds = Greeting.GetLocalBounds();
        Greeting.Origin = new Vector2f(textBounds.Width / 2, textBounds.Height / 2);
        Greeting.Position = new Vector2f(screenx / 2, screeny / 2); */

        // Congrats EndScreen
        Congrats = new Text{
            Font = titanOneFont,
            CharacterSize = screeny / 15,
        };
        


    }

    static void DrawDevider()
    {
        for (int x = pY_size / 4; x < screenyi; x += pY_size)
        {
            devide.Position = new Vector2f(screenx / 2, x);
            sfmlWindow.Draw(devide);
        }
    }

    // Ball Movement
    static void Moveball()
    {
        ball.Position += ballVelocity;

    }


    // Player Movement Player Left
    static void MovePlayerLeft(RectangleShape player)
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
    static void MovePlayerLeftUpDown()
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


    // Player Movement Player Right
    static void MovePlayerRight(RectangleShape player)
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
    static void MovePlayerRightUpDown()
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

    // Check and prevent player from going out of boundaries
    static void ConstrainPlayerPosition(RectangleShape player, float pY_size)
    {
        // check if goes to far up
        if (player.Position.Y <= (0))
        {
            player.Position = new Vector2f(player.Position.X, 0);

        }
        // check if goes to far down
        if (player.Position.Y >= (screeny - pY_size))
        {
            player.Position = new Vector2f(player.Position.X, screeny - pY_size);

        }
    }

    // Check and handles ball collision with walls
    static void WallCollision()
    {
        // bounce up wall
        if (ball.Position.Y < 0)
        {
            ballVelocity.Y *= -1;
        }

        // bounce down wall 
        if (ball.Position.Y > screeny - (2 + ball.Radius))
        {
            ballVelocity.Y *= -1;
        }

        // Score wall right
        if (ball.Position.X > (screenx - (2 * ball.Radius)))
        {
            ball.Position = new Vector2f(screenx / 2, screeny / 2);
            scoreLeft++;
            scoreLeftText.DisplayedString = scoreLeft.ToString();

            PlayerScored = true;
        }
        // Score wall left
        if (ball.Position.X < 0)
        {
            ball.Position = new Vector2f(screenx / 2, screeny / 2);
            scoreRight++;
            scoreRightText.DisplayedString = scoreRight.ToString();
            PlayerScored = true;
        }

    }

    // Collision Player and ball
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

            BounceOfPlayer(playerleft);
            Console.WriteLine("Collision Player 1: " + collisionplayerleft);
        }

        if (collisionplayerright == true)
        {
            BounceOfPlayer(playerright);
            Console.WriteLine("Collision Player 2: " + collisionplayerright);
        }

    }

    static void BounceOfPlayer(RectangleShape player)
    {
        // Mittelpunkte beider Objekte errechnen
        float yCenterPosPlayer = player.Position.Y + (player.Size.Y / 2);
        float yCenterPosBall = ball.Position.Y + ball.Radius;
        // Distanz errechnen
        float yCollisionDist = yCenterPosBall - yCenterPosPlayer;

        // Bugcontrol if Ball is above or below player
        if (Math.Abs(yCollisionDist) >= pY_size / 2)
        {
            if (yCollisionDist > 0)
            {
                ballVelocity.Y *= -1;
            }
            else
            {
                ballVelocity.Y = -maxBallSpeed;
            }
            Console.WriteLine("Bugcontrol: Case 1");
        }

        else if (Math.Abs(yCollisionDist) < pY_size / 2)
        {
            // Abprallverhalten für X-Achse
            if (Math.Abs(yCollisionDist) <= pY_size / 5)
            {
                ballVelocity.X = MathF.Sign(-ballVelocity.X) * MathF.Abs(minBallSpeed * 2);
                Console.WriteLine("Collisioncontrol X: double");
            }

            else if (Math.Abs(yCollisionDist) <= pY_size / 3)
            {
                ballVelocity.X = MathF.Sign(-ballVelocity.X) * MathF.Abs(minBallSpeed * 3 / 2);
                Console.WriteLine("Collisioncontrol X: triple");
            }
            else
            {
                ballVelocity.X = MathF.Sign(-ballVelocity.X) * MathF.Abs(minBallSpeed);
                Console.WriteLine("Collisioncontrol X: normal");
            }


            // Abprallverhalten für Y-Achse
            float calculatedSpeed = yCollisionDist * 100 / pY_size / 5;

            // Begrenzung mit Mindestgeschwindigkeit & MaxGeschwindigkeit
            if (MathF.Abs(calculatedSpeed) <= minBallSpeed)
            {
                ballVelocity.Y = MathF.Sign(ballVelocity.Y) * MathF.Abs(minBallSpeed);
                Console.WriteLine("Collisioncontrol Y: minspeed");
            }

            else if (MathF.Abs(calculatedSpeed) > maxBallSpeed)
            {
                ballVelocity.Y = MathF.Sign(ballVelocity.Y) * MathF.Abs(maxBallSpeed);
                Console.WriteLine("Collisioncontrol Y: maxspeed");
            }
            else
            {
                ballVelocity.Y = MathF.Sign(ballVelocity.Y) * MathF.Abs(calculatedSpeed);
                Console.WriteLine("Collisioncontrol Y: calculated speed " + calculatedSpeed);
            }

            Console.WriteLine("[Debug] Collision y-Distance: " + yCollisionDist + " calculatedSpeed: " + calculatedSpeed + " ballVelocity.Y: " + ballVelocity.Y + " ballVelocity.X: " + ballVelocity.X);
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


    }

    static void CheckWinnerCondition()
    {
        if (scoreLeft >= maxScore)
        {
            WinnerLeft = true;
            Console.WriteLine("Player Left wins.");
            currentState = GameState.EndMenu;
        }

        if (scoreRight >= maxScore)
        {
            WinnerRight = true;
            Console.WriteLine("Player Right wins.");
            currentState = GameState.EndMenu;
        }
    }

}

