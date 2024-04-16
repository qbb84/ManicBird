using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.MainMenu;
using TestGame.PlayingState.Pipes;
using TestGame.Resource;
using TestGame.StateMachine;
using TestGame.Utils;

namespace TestGame.PlayingState;

public sealed class PlayingState : AbstractState {
    private SpritePreservation _mainBackgroundSprite;
    private SpritePreservation _bluebirdPreservation;
    private SpritePreservation _bottomFloor;

    private const float Gravity = 8f;
    private const float JumpStrength = -150f;
    private float _birdVelocity = 300;
    private bool _isHoldingSpace;
    private bool _hasFirstPipePassedLeftMostScreen;

    private GameStateManager _gameStateManager;
    private readonly Game _game;

    private Texture2D _mainBackground;
    private Texture2D _bluebird;
    private Texture2D _pipeTexture;

    private readonly ResourceManager _resourceManager;
    private TimedUpdate _pipeSpawnTime;
    private Vector2 _bottomFloorPosition;
    private Rectangle _backgroundPositionAnimation;

    private readonly Func<int, int> _createRandomNumber = input => new Random().Next(input);



    public PlayingState(GameStateManager gameStateManager, Game game): base(gameStateManager, game) {
        _gameStateManager = gameStateManager;
        _game = game;
        _resourceManager = ResourceManager.GetInstance();
    }

    public override void Enter() {
        _mainBackgroundSprite = _resourceManager.GetSprite("sprites/background-day");
        _bluebirdPreservation = _resourceManager.GetSprite("sprites/bluebird-downflap");
        _bottomFloor = _resourceManager.GetSprite("sprites/base");
        _pipeTexture = _resourceManager.GetSprite("sprites/pipe-green").Texture;

        _mainBackground = _mainBackgroundSprite.Texture;
        _bluebird = _bluebirdPreservation.Texture;

        _backgroundPositionAnimation = _mainBackgroundSprite.Rectangle;

        _hasFirstPipePassedLeftMostScreen = false;
        _pipeSpawnTime = new TimedUpdate(TimedUpdate.CheckTime.TwoSecond);
    }

    public override void Exit() {
    }

    public override void Update(GameTime gameTime) {
        UpdateBirdPosition(gameTime);
        UpdateCheckCollisions();
        UpdateScrollingBackground();
        UpdateSpawnedPipes(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch) {
        DrawScrollingBackground(spriteBatch);
        DrawBird(spriteBatch);
        DrawFloor(spriteBatch);
        DrawPipes(spriteBatch);
    }

    private void UpdateBirdPosition(GameTime gameTime) {
        var birdX = _bluebirdPreservation.Position.X;
        var birdY = _bluebirdPreservation.Position.Y;
        UpdateBirdMovement(gameTime, ref birdX, ref birdY);
        _bluebirdPreservation.Position = new Vector2(birdX, birdY);
    }

    private void UpdateCheckCollisions() {
        var birdTop = _bluebirdPreservation.Position.Y - _bluebirdPreservation.Texture.Height;
        var birdBottom = _bluebirdPreservation.Position.Y + _bluebirdPreservation.Texture.Height;

        if (ScreenTopCollision(birdTop, _bluebirdPreservation.Position.X, ref birdTop)) {

        }
        if (ScreenBottomCollision(birdBottom, _bottomFloorPosition.Y, _bluebirdPreservation.Position.X, ref birdBottom)) {

        }
    }

    private void UpdateScrollingBackground() {
        _bottomFloorPosition.X -= 1;
        _backgroundPositionAnimation.X -= 1;

        if (_backgroundPositionAnimation.X < -_mainBackground.Width) {
            _backgroundPositionAnimation.X = 0;
        }
    }


    private void DrawScrollingBackground(SpriteBatch spriteBatch) {
        for (var updatedPosX = _backgroundPositionAnimation.X; updatedPosX < _game.GraphicsDevice.Viewport.Width; updatedPosX += _mainBackground.Width) {
            spriteBatch.Draw(_mainBackground, new Vector2(updatedPosX, 0), Color.White);
        }
    }

    private void DrawBird(SpriteBatch spriteBatch) {
        spriteBatch.Draw(_bluebird, _bluebirdPreservation.Position, Color.White);
    }

    private void DrawFloor(SpriteBatch spriteBatch) {
        var scaleWidth = _game.GraphicsDevice.Viewport.Width;
        var floorBottom = _game.GraphicsDevice.Viewport.Height;
        const float scaleHeight = 0.9f;
        var newFloorScale = new Vector2(scaleWidth, scaleHeight);
        _bottomFloorPosition = new Vector2(0, floorBottom - _bottomFloor.Texture.Height * scaleHeight);

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

    private void DrawPipes(SpriteBatch spriteBatch) {
        TryDrawPipes(spriteBatch);
    }

    private void UpdateBirdMovement(GameTime gameTime, ref float birdX, ref float birdY) {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
            _isHoldingSpace = true;
        }

        if (Keyboard.GetState().IsKeyUp(Keys.Space) & _isHoldingSpace) {
            _birdVelocity = JumpStrength;
            _isHoldingSpace = false;
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

    private void TryDrawPipes(SpriteBatch spriteBatch) {
        foreach (var (topPipe, bottomPipe) in PipeManager.GetPipeQueue().ToArray()) {
            Console.Out.WriteLine("[Top Pipe: " + topPipe.Position + ", " + topPipe.SpritePreservation.Texture);
            Console.Out.WriteLine("Bottom Pipe: " + bottomPipe.Position + ", " + bottomPipe.SpritePreservation.Texture + "]");

            var topSprite = topPipe.SpritePreservation;
            spriteBatch.Draw(
                topSprite.Texture,
                topPipe.Position,
                topPipe.Rectangle,
                Color.White,
                0f,
                Vector2.Zero,
                topPipe.Scale,
                SpriteEffects.FlipVertically,
                0f
            );

            var bottomSprite = bottomPipe.SpritePreservation;
            spriteBatch.Draw(
                bottomSprite.Texture,
                bottomPipe.Position,
                bottomPipe.Rectangle,
                Color.White,
                0f,
                Vector2.Zero,
                bottomPipe.Scale,
                SpriteEffects.None,
                0f
            );
        }
    }

    private void UpdateSpawnedPipes(GameTime gameTime) {
        var hasTwoSecondsPassed = _pipeSpawnTime.UpdateTimer(gameTime);

        if (hasTwoSecondsPassed && !_hasFirstPipePassedLeftMostScreen) {
            var pipeHeight = _pipeTexture.Height - _createRandomNumber(3);
            var pipeRectangle = new Rectangle( 0, 0, _pipeTexture.Width, pipeHeight);

            var topSprite = new SpritePreservation(_pipeTexture);
            var topPipe = new PipeMaker()
                .SetContent(_game.Content)
                .SetSprite(topSprite)
                .SetPosition(new Vector2(_game.GraphicsDevice.Viewport.Width - 100, 0))
                .SetRectangle(pipeRectangle)
                .CreatePipe();

            var bottomSprite = new SpritePreservation(_pipeTexture);
            var bottomPipe = new PipeMaker()
                .SetContent(_game.Content)
                .SetSprite(bottomSprite)
                .SetPosition(new Vector2(_game.GraphicsDevice.Viewport.Width - 100, _game.GraphicsDevice.Viewport.Height - pipeHeight))
                .SetRectangle(pipeRectangle)
                .CreatePipe();

            PipeManager.AddPipe(topPipe, bottomPipe);
        }


        if (!TryMovePipes()) return;

        var peekFirstPipeSet = PipeManager.PeekFirst();
        var topPipePair = peekFirstPipeSet?.Item1;
        var bottomPipePair = peekFirstPipeSet?.Item2;

        var leftScreenPosition = _game.GraphicsDevice.Viewport.X;
        _hasFirstPipePassedLeftMostScreen = topPipePair!.Position.X < leftScreenPosition &&
                                            bottomPipePair!.Position.X < leftScreenPosition;
    }

    private bool TryMovePipes() {
        if (PipeManager.IsEmpty()) return false;

        foreach (var (topPipe, bottomPipe) in PipeManager.GetPipeQueue()) {
            topPipe.DecreasePosX(1);
            bottomPipe.DecreasePosX(1);
        }
        return true;
    }
}