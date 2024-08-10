using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShootingGallery;

public class Game1 : Game
{
    private const int targetRadius = 45;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D targetSprite;
    private Texture2D crosshairsSprite;
    private Texture2D backgroundSprite;
    private SpriteFont gameFont;

    private Vector2 targetPosition = new Vector2(300, 300);
    private MouseState mouseState;
    private int score = 0;
    private bool mouseReleased = true;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        targetSprite = Content.Load<Texture2D>("target");
        crosshairsSprite = Content.Load<Texture2D>("crosshairs");
        backgroundSprite = Content.Load<Texture2D>("sky");
        gameFont = Content.Load<SpriteFont>("galleryFont");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        mouseState = Mouse.GetState();
        if (mouseState.LeftButton == ButtonState.Pressed && mouseReleased)
        {
            mouseReleased = false;
            if (hasHitTarget(mouseState.Position.ToVector2()))
            {
                score++;

                Random random = new Random();
                targetPosition.X = random.Next(0, _graphics.PreferredBackBufferWidth);
                targetPosition.Y = random.Next(0, _graphics.PreferredBackBufferHeight);
            }
        }

        if (mouseState.LeftButton == ButtonState.Released)
        {
            mouseReleased = true;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);


        _spriteBatch.Begin();
        _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);
        _spriteBatch.DrawString(gameFont, score.ToString(), new Vector2(100, 100), Color.White);
        _spriteBatch.Draw(targetSprite, computeTargetRenderPosition(), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private bool hasHitTarget(Vector2 position)
    {
        return Vector2.Distance(targetPosition, position) <= targetRadius;
    }

    private Vector2 computeTargetRenderPosition()
    {
        return new Vector2(targetPosition.X - targetRadius, targetPosition.Y - targetRadius);
    }
}