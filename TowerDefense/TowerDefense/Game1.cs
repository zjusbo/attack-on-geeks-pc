using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace AttackOnGeek
{
    /*Indicate the game state*/
    public enum GameState
    {
        GameWait,
        GameStart,
        GameWin,
        help1,
        help2,
        help3,
        Credit,
        GameOver
    }
    /*Indicate the paper state*/
    public enum PaperState
    {
        FLYING,
        HIT,
        MISS
    }
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        /*GUI*/
        Sprite gameoverScreen;
        Sprite gamewinScreen;
        Sprite helpSprite1;
        Sprite helpSprite2;
        Sprite helpSprite3;
      
        GameState gameState;
        Button newGameButton;
        Button exitButton;
        Button creditVideo;
        Video video;
        VideoPlayer videoPlayer;
        Texture2D videoTexture;
        Video video2;
        VideoPlayer videoPlayer2;
        Texture2D videoTexture2;

        /*Game Running*/
        Button t1Button;
        Button t2Button;
        Button t3Button;
        Button t4Button;
        Button t5Button;
        Button t6Button;
        Toolbar toolBar;
        SoundEffect bgMusic;
        SoundEffectInstance bgMusicInstance;
        SoundEffect setSound;
        SoundEffect spawnSound;

        Map map = new Map();
        StudentWaweManager studentWaweManager;
        Player player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            /*Set the windows size*/
            graphics.PreferredBackBufferWidth = map.Width * 60;
            graphics.PreferredBackBufferHeight = 60 + map.Height * 60;
            graphics.ApplyChanges();
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gameState = GameState.GameWait;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            /*Load the pictures, videos, audioes needed and initialize them*/
            spriteBatch = new SpriteBatch(GraphicsDevice);
    
            Texture2D gameover = Content.Load<Texture2D>("Movies\\gameover");
            gameoverScreen = new Sprite(gameover, new Vector2(200, 0));
            Texture2D gamewin = Content.Load<Texture2D>("Movies\\gamewin");
            gamewinScreen = new Sprite(gamewin, new Vector2(200, 0));
            Texture2D[] helpTextureArray;
            helpTextureArray = new Texture2D[]
            {
                 Content.Load<Texture2D>("help\\help1"),
                 Content.Load<Texture2D>("help\\help2"),
                 Content.Load<Texture2D>("help\\help3"),
            };
            helpSprite1 = new Sprite(helpTextureArray[0], new Vector2(0, 0));
            helpSprite2 = new Sprite(helpTextureArray[1], new Vector2(0, 0));
            helpSprite3 = new Sprite(helpTextureArray[2], new Vector2(0, 0));
            Texture2D newGamePic = Content.Load<Texture2D>("GUI\\newGame");
            newGameButton = new Button(newGamePic, newGamePic, newGamePic,
              new Vector2(700, map.Height * 47));
            Texture2D exitGamePic = Content.Load<Texture2D>("GUI\\exit");
            exitButton = new Button(exitGamePic, exitGamePic, exitGamePic,
                new Vector2(700, map.Height * 60));
            Texture2D creditPic = Content.Load<Texture2D>("GUI\\credits");
            creditVideo = new Button(creditPic, creditPic, creditPic,
                new Vector2(700, (float)(map.Height * 53.5)));

            video = Content.Load<Video>("movies\\video");
            videoPlayer = new VideoPlayer();
            video2 = Content.Load<Video>("movies\\creditVideo");
            videoPlayer2 = new VideoPlayer();

            bgMusic = Content.Load<SoundEffect>("Audio\\background");
            bgMusicInstance = bgMusic.CreateInstance();
            bgMusicInstance.IsLooped = true;
            setSound = Content.Load<SoundEffect>("Audio\\set");
            spawnSound = Content.Load<SoundEffect>("Audio\\spawn");
          
            Texture2D topBar = Content.Load<Texture2D>("GUI\\tool bar");
            SpriteFont font = Content.Load<SpriteFont>("Arial");

            toolBar = new Toolbar(topBar, font, new Vector2(0, 165));

            Texture2D graph = Content.Load<Texture2D>("map");

            map.AddTexture(graph); 

            Texture2D paperTexture = Content.Load<Texture2D>("paper");
            Texture2D paperBreakTexture = Content.Load<Texture2D>("paperbreak");

            Texture2D[] teacherTextures = new Texture2D[]
            {
                Content.Load<Texture2D>("Teachers\\t1teacher"),
                Content.Load<Texture2D>("Teachers\\t2teacher"),
                Content.Load<Texture2D>("Teachers\\t3teacher"),
                Content.Load<Texture2D>("Teachers\\t4teacher"),
                Content.Load<Texture2D>("Teachers\\t5teacher"),
                Content.Load<Texture2D>("Teachers\\t6teacher"),
            };

            player = new Player(map, teacherTextures, paperTexture, paperBreakTexture);

            Texture2D[] StudentTextureArray = new Texture2D[]
            {
                 Content.Load<Texture2D>("Students\\student1"),
                 Content.Load<Texture2D>("Students\\student2"),
                 Content.Load<Texture2D>("Students\\student3"),
                 Content.Load<Texture2D>("Students\\student4"),
            };
            
            studentWaweManager = new StudentWaweManager(player, map, 10, StudentTextureArray);

            t1Button = new Button(teacherTextures[0], teacherTextures[0],
                teacherTextures[0], new Vector2(5, map.Height * 60-5), 6);
            t2Button = new Button(teacherTextures[1], teacherTextures[1],
                teacherTextures[1], new Vector2(65, map.Height * 60), 7);
            t3Button = new Button(teacherTextures[2], teacherTextures[2],
                teacherTextures[2], new Vector2(60 * 2+15, map.Height * 60), 6);
            t4Button = new Button(teacherTextures[3], teacherTextures[3],
                teacherTextures[3], new Vector2(60*3+15, map.Height * 60), 5);
            t5Button = new Button(teacherTextures[4], teacherTextures[4],
                teacherTextures[4], new Vector2(60 * 4+20, map.Height * 60), 6);
            t6Button = new Button(teacherTextures[5], teacherTextures[5],
                teacherTextures[5], new Vector2(60 * 5+23, map.Height * 60), 7);

    

            t1Button.OnPress += new EventHandler(t1Button_OnPress);
            t2Button.OnPress += new EventHandler(t2Button_OnPress);
            t3Button.OnPress += new EventHandler(t3Button_OnPress);
            t4Button.OnPress += new EventHandler(t4Button_OnPress);
            t5Button.OnPress += new EventHandler(t5Button_OnPress);
            t6Button.OnPress += new EventHandler(t6Button_OnPress);
        }

        protected override void UnloadContent(){}

        /*Set the button listener*/
        private void t1Button_Clicked(object sender, EventArgs e)
        {
            player.NewTeacherType = "T1 Teacher";
            player.NewTeacherIndex = 0;
        }
        private void t2Button_Clicked(object sender, EventArgs e)
        {
            player.NewTeacherType = "T2 Teacher";
            player.NewTeacherIndex = 1;
        }
        private void t3Button_Clicked(object sender, EventArgs e)
        {
            player.NewTeacherType = "T3 Teacher";
            player.NewTeacherIndex = 2;
        }
        private void t4Button_Clicked(object sender, EventArgs e)
        {
            player.NewTeacherType = "T4 Teacher";
            player.NewTeacherIndex = 3;
        }
        private void t5Button_Clicked(object sender, EventArgs e)
        {
            player.NewTeacherType = "T5 Teacher";
            player.NewTeacherIndex = 4;
        }
        private void t6Button_Clicked(object sender, EventArgs e)
        {
            player.NewTeacherType = "T6 Teacher";
            player.NewTeacherIndex = 5;
        }

        private void t1Button_OnPress(object sender, EventArgs e)
        {
            player.NewTeacherType = "T1 Teacher";
            player.NewTeacherIndex = 0;
        }
        private void t2Button_OnPress(object sender, EventArgs e)
        {
            player.NewTeacherType = "T2 Teacher";
            player.NewTeacherIndex = 1;
        }
        private void t3Button_OnPress(object sender, EventArgs e)
        {
            player.NewTeacherType = "T3 Teacher";
            player.NewTeacherIndex = 2;
        }
        private void t4Button_OnPress(object sender, EventArgs e)
        {
            player.NewTeacherType = "T4 Teacher";
            player.NewTeacherIndex = 3;
        }
        private void t5Button_OnPress(object sender, EventArgs e)
        {
            player.NewTeacherType = "T5 Teacher";
            player.NewTeacherIndex = 4;
        }
        private void t6Button_OnPress(object sender, EventArgs e)
        {
            player.NewTeacherType = "T6 Teacher";
            player.NewTeacherIndex = 5;
        }

        protected override void Update(GameTime gameTime)
        {
            /*Control the game flow*/
            /*Game main page*/
            if (gameState == GameState.GameWait)
            {
                newGameButton.Update(gameTime);
                exitButton.Update(gameTime);
                creditVideo.Update(gameTime);
                if (videoPlayer.State == MediaState.Stopped)
                {
                    videoPlayer.IsLooped = true;
                    videoPlayer.Play(video);
                }
                if (exitButton.State == ButtonStatus.Pressed)
                {
                    Exit();
                }
                if (newGameButton.State == ButtonStatus.Pressed)
                {
                    gameState = GameState.GameStart;
                    videoPlayer.Stop();
                }
                if (creditVideo.State == ButtonStatus.Pressed)
                {
                    gameState = GameState.Credit;
                    videoPlayer.Stop();
                    videoPlayer2.IsLooped = false;
                    videoPlayer2.Play(video2);
                }
            }
            /*Credits video page*/
            else if (gameState== GameState.Credit)
            {
                if(videoPlayer2.State == MediaState.Stopped)
                   gameState = GameState.GameWait;
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    gameState = GameState.GameWait;
                    videoPlayer2.Stop();
                    this.Initialize();
                }
            }
            /*Game playing page*/
            else if (gameState == GameState.GameStart)
            {
                studentWaweManager.Update(gameTime);
                player.Update(gameTime, studentWaweManager.Students);
                t1Button.Update(gameTime);
                t2Button.Update(gameTime);
                t3Button.Update(gameTime);
                t4Button.Update(gameTime);
                t5Button.Update(gameTime);
                t6Button.Update(gameTime);
                if (bgMusicInstance.State == SoundState.Stopped)
                {
                    bgMusicInstance.Volume = 1.0f;
                    bgMusicInstance.IsLooped = true;
                    bgMusicInstance.Play();
                }
                else
                    bgMusicInstance.Resume();
                if (player.TeacherBeingSet == true)
                {
                    setSound.Play();
                    player.TeacherBeingSet = false;
                }
                if (studentWaweManager.SoundFlag == true)
                {
                    spawnSound.Play();
                    studentWaweManager.SoundFlag = false;
                }
                if (player.Lives <= 0)
                {
                    gameState = GameState.GameOver;
                    bgMusicInstance.Stop();
                }

                if (studentWaweManager.Round >= 10)
                {
                    gameState = GameState.GameWin;
                    bgMusicInstance.Stop();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.H))
                {
                    gameState = GameState.help1;
                    Thread.Sleep(300);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Thread.Sleep(300);
                    gameState = GameState.GameWait;
                    bgMusicInstance.Stop();
                    this.Initialize();
                }
            }
            /*Game win or game lose*/
            else if (gameState == GameState.GameWin || gameState == GameState.GameOver)
            {
                newGameButton.Update(gameTime);
                exitButton.Update(gameTime);
                if (videoPlayer.State == MediaState.Stopped)
                {
                    videoPlayer.IsLooped = true;
                    videoPlayer.Play(video);
                }
                if (exitButton.State == ButtonStatus.Pressed)
                {
                    Exit();
                }
                if (newGameButton.State == ButtonStatus.Pressed)
                {
                    gameState = GameState.GameStart;
                    videoPlayer.Stop();
                    this.Initialize();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Thread.Sleep(300);
                    gameState = GameState.GameWait;
                    bgMusicInstance.Stop();
                    this.Initialize();
                }

            }
            /*Three helping pages*/
            else if (gameState == GameState.help1)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    gameState = GameState.GameStart;
                    Thread.Sleep(300);
                    bgMusicInstance.Resume();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.N))
                {
                    Thread.Sleep(300);
                    gameState = GameState.help2;
                }
                else
                {
                    bgMusicInstance.Pause();
                }

            }
            else if (gameState == GameState.help2)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    gameState = GameState.GameStart;
                    Thread.Sleep(300);
                    bgMusicInstance.Resume();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    Thread.Sleep(300);
                    gameState = GameState.help1;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.N))
                {
                    Thread.Sleep(300);
                    gameState = GameState.help3;
                }
            }
            else if (gameState == GameState.help3)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    gameState = GameState.GameStart;
                    Thread.Sleep(300);
                    bgMusicInstance.Resume();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    Thread.Sleep(300);
                    gameState = GameState.help2;
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            /*Draw the different pages*/
            if (gameState == GameState.GameWait)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                if (videoPlayer.State != MediaState.Stopped)
                    videoTexture = videoPlayer.GetTexture();
                Rectangle screen = new Rectangle(GraphicsDevice.Viewport.X,
                             GraphicsDevice.Viewport.Y,
                             GraphicsDevice.Viewport.Width,
                             GraphicsDevice.Viewport.Height);
                if (videoTexture != null)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(videoTexture, screen, Color.White);
                    newGameButton.Draw(spriteBatch);
                    exitButton.Draw(spriteBatch);
                    creditVideo.Draw(spriteBatch);
                    spriteBatch.End();
                }
                base.Draw(gameTime);
            }
            else if (gameState == GameState.Credit)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                if (videoPlayer2.State != MediaState.Stopped)
                    videoTexture2 = videoPlayer2.GetTexture();
                Rectangle screen = new Rectangle(GraphicsDevice.Viewport.X,
                             GraphicsDevice.Viewport.Y,
                             GraphicsDevice.Viewport.Width,
                             GraphicsDevice.Viewport.Height);
                if (videoTexture2 != null)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(videoTexture2, screen, Color.White);
                    spriteBatch.End();
                }
                base.Draw(gameTime);
            }
            else if (gameState == GameState.GameOver)
            {
                GraphicsDevice.Clear(Color.White);
                spriteBatch.Begin();
                gameoverScreen.Draw(spriteBatch);
                newGameButton.Draw(spriteBatch);
                exitButton.Draw(spriteBatch);
                spriteBatch.End();
                base.Draw(gameTime);
            }
            else if (gameState == GameState.GameWin)
            {
                GraphicsDevice.Clear(Color.White);
                spriteBatch.Begin();
                gamewinScreen.Draw(spriteBatch);
                newGameButton.Draw(spriteBatch);
                exitButton.Draw(spriteBatch);
                spriteBatch.End();
                base.Draw(gameTime);
            }

            else if (gameState == GameState.GameStart)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                spriteBatch.Begin();

                map.Draw(spriteBatch);
                studentWaweManager.Draw(spriteBatch);
                player.Draw(spriteBatch);


                toolBar.Draw(spriteBatch, player, studentWaweManager.Round, studentWaweManager.TotalRounds);
                t1Button.Draw(spriteBatch);
                t2Button.Draw(spriteBatch);
                t3Button.Draw(spriteBatch);
                t4Button.Draw(spriteBatch);
                t5Button.Draw(spriteBatch);
                t6Button.Draw(spriteBatch);

                player.DrawPreview(spriteBatch);

                spriteBatch.End();

                base.Draw(gameTime);

            }
            else if (gameState == GameState.help1)
            {
                spriteBatch.Begin();
                helpSprite1.Draw(spriteBatch);
                spriteBatch.End();
                base.Draw(gameTime);
            }
            else if (gameState == GameState.help2)
            {
                spriteBatch.Begin();
                helpSprite2.Draw(spriteBatch);
                spriteBatch.End();
                base.Draw(gameTime);
            }
            else if (gameState == GameState.help3)
            {
                spriteBatch.Begin();
                helpSprite3.Draw(spriteBatch);
                spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }
}
