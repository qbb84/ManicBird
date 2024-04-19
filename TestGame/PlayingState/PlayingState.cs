using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.PlayingState.Collision;
using TestGame.PlayingState.Collision.CollisionTypes;
using TestGame.PlayingState.Pipes;
using TestGame.Resource;
using TestGame.StateMachine;
using TestGame.Utils;

namespace TestGame.PlayingState;

public sealed class PlayingState : AbstractState {
    private SpritePreservation _mainBackgroundSprite;
    private SpritePreservation _player;
    private SpritePreservation _bottomFloor;

    private const float Gravity = 8f;
    private const float JumpStrength = -180f;
    private const int DistanceBetweenPipe = 18;
    private const int BackgroundScrollingSpeed = 2;

    private float _birdVelocity = 300;
    private bool _isHoldingSpace;
    private bool _hasFirstPipePassedLeftMostScreen;

    private Texture2D _mainBackground;
    private Texture2D _bluebird;
    private Texture2D _pipeTexture;

    private readonly ResourceManager _resourceManager;
    private TimedUpdate _pipeSpawnTime;

    private Vector2 _bottomFloorPosition;
    private Vector2 _pipeScale;
    private Rectangle _backgroundPositionAnimation;



    public PlayingState(GameStateManager gameStateManager, Game game) : base(gameStateManager, game) {
        _resourceManager = ResourceManager.Instance;
    }

    public override void Enter() {
        _mainBackgroundSprite = _resourceManager.GetSprite("sprites/background-day");
        _player = _resourceManager.GetSprite("sprites/bluebird-downflap");
        _bottomFloor = _resourceManager.GetSprite("sprites/base");
        _pipeTexture = _resourceManager.GetSprite("sprites/pipe-green").Texture;

        _mainBackground = _mainBackgroundSprite.Texture;
        _bluebird = _player.Texture;

        _backgroundPositionAnimation = _mainBackgroundSprite.Rectangle;

        _hasFirstPipePassedLeftMostScreen = false;
        _pipeSpawnTime = new TimedUpdate(TimedUpdate.CheckTime.TwoSecond);

        _pipeScale = new Vector2(1.5f, 1.0f);
    }

    public override void Exit() { }

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
        var birdX = _player.Position.X;
        var birdY = _player.Position.Y;
        UpdateBirdMovement(gameTime, ref birdX, ref birdY);
        _player.Position = new Vector2(birdX, birdY);
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
        _player.Position = new Vector2(birdX, birdY);
    }

    private void UpdateCheckCollisions() {
        var birdPositionY = _player.Position.Y;

        var birdTop = birdPositionY - _player.Texture.Height;
        var birdBottom = birdPositionY+ _player.Texture.Height;

        ScreenTopCollision(birdTop);
        ScreenBottomCollision(birdBottom, _bottomFloorPosition.Y);

        PipeCollision();
    }

    private void UpdateScrollingBackground() {
        _bottomFloorPosition.X -= BackgroundScrollingSpeed;
        _backgroundPositionAnimation.X -= BackgroundScrollingSpeed;

        if (_backgroundPositionAnimation.X < -_mainBackground.Width) {
            _backgroundPositionAnimation.X = 0;
        }
    }

    private void UpdateSpawnedPipes(GameTime gameTime) {
        var hasTimePassed = _pipeSpawnTime.UpdateTimer(gameTime);

        var viewportHeight = Game.GraphicsDevice.Viewport.Height;
        var pipeTextureHeight = _pipeTexture.Height;
        var pipeTextureWidth = _pipeTexture.Width;

        var (topPipeRectangle, bottomPipeRectangle) = PipeManager.CalculateRandomPipeHeight(
            gameTime,
            viewportHeight,
            pipeTextureHeight,
            pipeTextureWidth,
            DistanceBetweenPipe,
            _pipeScale
        );

        switch (hasTimePassed) {
            case true when !_hasFirstPipePassedLeftMostScreen: {
                var topSprite = new SpritePreservation(_pipeTexture);
                var topPipe = new PipeMaker()
                    .SetContent(Game.Content)
                    .SetSprite(topSprite)
                    .SetPosition(new Vector2(Game.GraphicsDevice.Viewport.Width, 0))
                    .SetRectangle(topPipeRectangle)
                    .SetScale(_pipeScale)
                    .CreatePipe();


                var bottomSprite = new SpritePreservation(_pipeTexture);
                var bottomPipe = new PipeMaker()
                    .SetContent(Game.Content)
                    .SetSprite(bottomSprite)
                    .SetPosition(new Vector2(
                        Game.GraphicsDevice.Viewport.Width,
                        _bottomFloorPosition.Y - bottomPipeRectangle.Height))
                    .SetRectangle(bottomPipeRectangle)
                    .SetScale(_pipeScale)
                    .CreatePipe();


                PipeManager.AddPipe(topPipe, bottomPipe);
                break;
            }
            case true: {
                var pipeSet = PipeManager.RemovePipe();
                pipeSet.Item1.ResetPipe();
                pipeSet.Item2.ResetPipe();

                pipeSet.Item1.Rectangle = topPipeRectangle;

                pipeSet.Item2.Rectangle = bottomPipeRectangle;
                pipeSet.Item2.Position = new Vector2(
                    Game.GraphicsDevice.Viewport.Width,
                    _bottomFloorPosition.Y - bottomPipeRectangle.Height
                );


                PipeManager.AddPipe(pipeSet);
                break;
            }
        }
        if (!TryMovePipes()) return;

        var peekFirstPipeSet = PipeManager.PeekFirst();
        var topPipePair = peekFirstPipeSet?.Item1;
        var bottomPipePair = peekFirstPipeSet?.Item2;

        var leftScreenPosition = Game.GraphicsDevice.Viewport.X;
        _hasFirstPipePassedLeftMostScreen = topPipePair!.Position.X < leftScreenPosition &&
                                            bottomPipePair!.Position.X < leftScreenPosition;
    }


    private void DrawScrollingBackground(SpriteBatch spriteBatch) {
        for (var updatedPosX = _backgroundPositionAnimation.X;
             updatedPosX < Game.GraphicsDevice.Viewport.Width;
             updatedPosX += _mainBackground.Width) {

            spriteBatch.Draw(_mainBackground, new Vector2(updatedPosX, 0), Color.White);
        }
    }

    private void DrawBird(SpriteBatch spriteBatch) {
        spriteBatch.Draw(_bluebird, _player.Position, Color.White);
    }

    private void DrawFloor(SpriteBatch spriteBatch) {
        var scaleWidth = Game.GraphicsDevice.Viewport.Width;
        var floorBottom = Game.GraphicsDevice.Viewport.Height;
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
        foreach (var (topPipe, bottomPipe) in PipeManager.GetPipeQueue().ToArray()) {

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
                Color.LightGray,
                0f,
                Vector2.Zero,
                bottomPipe.Scale,
                SpriteEffects.None,
                0f
            );
        }
    }


    private void ScreenTopCollision(float birdTop) {
        if (!(birdTop <= 0)) return;
        CollideManager.Instance.InvokeViewportCollisionEvent(ViewportCollisionType.ViewportTop, _player);
    }

    private void ScreenBottomCollision(float birdBottom, float floorTop) {
        if (!(birdBottom >= floorTop)) return;
        CollideManager.Instance.InvokeViewportCollisionEvent(ViewportCollisionType.ViewportBottom, _player,
            _bottomFloorPosition.Y);
    }

    private void PipeCollision() {
        var birdRectangle = Utility.CreateRectangle(_player.Position, _bluebird.Width, _bluebird.Height);

        foreach (var (topPipe, bottomPipe) in PipeManager.GetPipeQueue().ToArray()) {
            if (topPipe.Position.X < _player.Position.X - topPipe.Rectangle.Width * _pipeScale.X) continue;

            CollideManager.HandleTopPipeCollision(birdRectangle, topPipe, _player, _pipeScale.X);
            CollideManager.HandleBottomPipeCollision(birdRectangle, bottomPipe, _player, _pipeScale.X, _bottomFloorPosition.Y);
        }
    }

    private bool TryMovePipes() {
            if (PipeManager.IsEmpty()) return false;

            foreach (var (topPipe, bottomPipe) in PipeManager.GetPipeQueue()) {
                topPipe.DecreasePosX(BackgroundScrollingSpeed);
                bottomPipe.DecreasePosX(BackgroundScrollingSpeed);
            }
            return true;
        }

}