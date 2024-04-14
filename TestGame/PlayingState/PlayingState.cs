using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.MainMenu;
using TestGame.Resource;
using TestGame.StateMachine;
using TestGame.Utils;

namespace TestGame.PlayingState;

public class PlayingState : AbstractState {
    private SpritePreservation _mainBackgroundSprite;
    private SpritePreservation _bluebirdPreservation;
    private SpritePreservation _bottomFloor;

    private const float Gravity = 10f;
    private const float JumpStrength = -130f;
    private float _birdVelocity = 300;

    private GameStateManager _gameStateManager;
    private readonly Game _game;

    private Texture2D _mainBackground;
    private Texture2D _bluebird;

    private readonly ResourceManager _mainMenuResource;
    private Vector2 _bottomFloorPosition;
    private Rectangle _backgroundPositionAnimation;



    public PlayingState(GameStateManager gameStateManager, Game game): base(gameStateManager, game) {
        _gameStateManager = gameStateManager;
        _game = game;
        _mainMenuResource = ResourceManager.GetInstance();
    }

    public override void Enter() {
        _mainBackgroundSprite = _mainMenuResource.GetSprite("sprites/background-day");
        _bluebirdPreservation = _mainMenuResource.GetSprite("sprites/bluebird-downflap");
        _bottomFloor = _mainMenuResource.GetSprite("sprites/base");

        _mainBackground = _mainBackgroundSprite.Texture;
        _bluebird = _bluebirdPreservation.Texture;

        _backgroundPositionAnimation = _mainBackgroundSprite.Rectangle;
    }

    public override void Exit() {
    }

    public override void Update(GameTime gameTime) {
        var birdY = _bluebirdPreservation.Position.Y;
        var birdX = _bluebirdPreservation.Position.X;

        SetupBirdMovement(gameTime, ref birdX, ref birdY);


        var birdTop = birdY - _bluebirdPreservation.Texture.Height;
        if (ScreenTopCollision(birdTop, birdX, ref birdY)) {

        }

        var birdBottom = birdY + _bluebirdPreservation.Texture.Height;
        if (ScreenBottomCollision(birdBottom, _bottomFloorPosition.Y, birdX, ref birdY)) {

        }

        _bottomFloorPosition.X -= 1;
        _backgroundPositionAnimation.X -= 1;

        if (_backgroundPositionAnimation.X < -_mainBackground.Width) {
            _backgroundPositionAnimation.X = 0;
        }


    }

    public override void Draw(SpriteBatch spriteBatch) {
        for (var updatedPosX = _backgroundPositionAnimation.X; updatedPosX < _game.GraphicsDevice.Viewport.Width; updatedPosX += _mainBackground.Width) {
            spriteBatch.Draw(_mainBackground, new Vector2(updatedPosX, 0), Color.White);
        }
        spriteBatch.Draw(_bluebird, _bluebirdPreservation.Position, Color.White);


        var scaleWidth = _game.GraphicsDevice.Viewport.Width;
        var floorBottom = _game.GraphicsDevice.Viewport.Height;
        const float scaleHeight = 0.9f;
        var newFloorScale = new Vector2(scaleWidth, scaleHeight);
        _bottomFloorPosition = new Vector2(0, floorBottom - (_bottomFloor.Texture.Height * scaleHeight));

        spriteBatch.Draw(
            _bottomFloor.Texture,
            _bottomFloorPosition,
            null,
            Color.White,
            0f,
            Vector2.Zero,
            newFloorScale,
            SpriteEffects.None,
            0f
            );
    }

    private void SetupBirdMovement(GameTime gameTime, ref float birdX, ref float birdY) {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
            _birdVelocity = JumpStrength;
        }
        _birdVelocity += Gravity;
        birdY += _birdVelocity * deltaTime;
        _bluebirdPreservation.Position = new Vector2(birdX, birdY);
    }

    private bool ScreenTopCollision(float birdTop, float birdX, ref float birdY) {
        if (!(birdTop <= 0)) return false;

        birdY = Math.Max(birdY, 0);
        _bluebirdPreservation.Position = new Vector2(birdX, birdY);
        return true;
    }

    private bool ScreenBottomCollision(float birdBottom, float floorTop, float birdX, ref float birdY) {
        if (!(birdBottom >= floorTop)) return false;

        birdY = Math.Clamp(birdY, 0, floorTop - _bluebirdPreservation.Texture.Height);
        _bluebirdPreservation.Position = new Vector2(birdX, birdY);
        return true;
    }
}