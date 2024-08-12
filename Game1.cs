using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShootingGallery;

public class Game1 : Game
{
    private State state;
    private const int targetRadius = 45;
    private const int crosshairsRadius = 25;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D targetSprite;
    private Texture2D crosshairsSprite;
    private Texture2D backgroundSprite;
    private SpriteFont gameFont;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        state = Engine.generateInitialState(
            new System.Numerics.Vector2(
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight
            )
        );

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

        MouseState mouseState = Mouse.GetState();
        state = Engine.UpdateState(
            state,
            new Input(
                new System.Numerics.Vector2(mouseState.Position.X, mouseState.Position.Y),
                gameTime.ElapsedGameTime.TotalSeconds,
                mouseState.LeftButton == ButtonState.Pressed
            )
        );

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();

        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);

        _spriteBatch.DrawString(
            gameFont,
            "Score: " + state.Score.ToString(),
            new Vector2(3, 3),
            Color.White
        );

        _spriteBatch.DrawString(
            gameFont,
            "Time: " + Math.Ceiling(state.RemainingSeconds).ToString(),
            new Vector2(3, 40),
            Color.White
        );

        if (Engine.ShouldDrawTarget(state))
        {
            _spriteBatch.Draw(targetSprite, computeTargetRenderPosition(), Color.White);
        }

        _spriteBatch.Draw(
            crosshairsSprite,
            new Vector2(
                mouseState.X - crosshairsRadius,
                mouseState.Y - crosshairsRadius
            ),
            Color.White
        );

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private Vector2 computeTargetRenderPosition() => new Vector2(
        state.TargetPosition.X - targetRadius,
        state.TargetPosition.Y - targetRadius
    );
}
