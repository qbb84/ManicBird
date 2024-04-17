using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.PlayingState.Collision;
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
    private const int DistanceBetweenPipe = 15;
    private float _birdVelocity = 300;
    private bool _isHoldingSpace;
    private bool _hasFirstPipePassedLeftMostScreen;
    private bool pipeHeightRandomCheck;

    private Texture2D _mainBackground;
    private Texture2D _bluebird;
    private Texture2D _pipeTexture;

    private readonly ResourceManager _resourceManager;
    private TimedUpdate _pipeSpawnTime;

    private Vector2 _bottomFloorPosition;
    private Rectangle _backgroundPositionAnimation;

    private Func<int, int> _createRandomNumber;



    public PlayingState(GameStateManager gameStateManager, Game game): base(gameStateManager, game) {
        _resourceManager = ResourceManager.Instance;
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
        _pipeSpawnTime = new TimedUpdate(TimedUpdate.CheckTime.FourSecond);

        _createRandomNumber = input => new Random().Next(input);
        pipeHeightRandomCheck = false;
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

    private void UpdateCheckCollisions() {
        var birdTop = _bluebirdPreservation.Position.Y - _bluebirdPreservation.Texture.Height;
        var birdBottom = _bluebirdPreservation.Position.Y + _bluebirdPreservation.Texture.Height;

        ScreenTopCollision(birdTop);
        ScreenBottomCollision(birdBottom, _bottomFloorPosition.Y);
    }

    private void UpdateScrollingBackground() {
        _bottomFloorPosition.X -= 1;
        _backgroundPositionAnimation.X -= 1;

        if (_backgroundPositionAnimation.X < -_mainBackground.Width) {
            _backgroundPositionAnimation.X = 0;
        }
    }

     private void UpdateSpawnedPipes(GameTime gameTime) {
        var hasFourSecondsPassed = _pipeSpawnTime.UpdateTimer(gameTime);

        switch (hasFourSecondsPassed) {
            case true when !_hasFirstPipePassedLeftMostScreen: {
                var (topPipeRectangle, bottomPipeRectangle) = CalculateRandomPipeHeight(gameTime);

                var topSprite = new SpritePreservation(_pipeTexture);
                var topPipe = new PipeMaker()
                    .SetContent(Game.Content)
                    .SetSprite(topSprite)
                    .SetPosition(new Vector2(Game.GraphicsDevice.Viewport.Width - 25, 0))
                    .SetRectangle(topPipeRectangle)
                    .SetScale(new Vector2(1.5f, 1f))
                    .CreatePipe();


                var bottomSprite = new SpritePreservation(_pipeTexture);
                var bottomPipe = new PipeMaker()
                    .SetContent(Game.Content)
                    .SetSprite(bottomSprite)
                    .SetPosition(new Vector2(
                        Game.GraphicsDevice.Viewport.Width - 25,
                        _bottomFloorPosition.Y - bottomPipeRectangle.Height))
                    .SetRectangle(bottomPipeRectangle)
                    .SetScale(new Vector2(1.5f, 1f))
                    .CreatePipe();


                PipeManager.AddPipe(topPipe, bottomPipe);
                break;
            }
            case true: {
                var pipeSet = PipeManager.RemovePipe();
                pipeSet.Item1.ResetPipe();
                pipeSet.Item2.ResetPipe();

                var (topPipeRectangle, bottomPipeRectangle) = CalculateRandomPipeHeight(gameTime);
                pipeSet.Item1.Rectangle = topPipeRectangle;

                pipeSet.Item2.Rectangle = bottomPipeRectangle;
                pipeSet.Item2.Position = new Vector2(
                    Game.GraphicsDevice.Viewport.Width - 25,
                    _bottomFloorPosition.Y - bottomPipeRectangle.Height);


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
        spriteBatch.Draw(_bluebird, _bluebirdPreservation.Position, Color.White);
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
        CollideManager.Instance.InvokeCollisionEvent(CollisionType.ViewportTop, _bluebirdPreservation);
    }

    private void ScreenBottomCollision(float birdBottom, float floorTop) {
        if (!(birdBottom >= floorTop)) return;
        CollideManager.Instance.InvokeCollisionEvent(CollisionType.ViewportBottom, _bluebirdPreservation, _bottomFloorPosition.Y);
    }

    private bool TryMovePipes() {
        if (PipeManager.IsEmpty()) return false;

        foreach (var (topPipe, bottomPipe) in PipeManager.GetPipeQueue()) {
            topPipe.DecreasePosX(1);
            bottomPipe.DecreasePosX(1);
        }
        return true;
    }

    private Tuple<Rectangle, Rectangle> CalculateRandomPipeHeight(GameTime gameTime) {
        var pipeHeight =
            100 +
            (int)Math.Sin(_createRandomNumber(gameTime.ElapsedGameTime.Milliseconds % 2)) +
            _createRandomNumber(_pipeTexture.Height / 2);
        var topPipeRectangle = new Rectangle( 0, 0, _pipeTexture.Width, pipeHeight);

        var bottomHeight = Game.GraphicsDevice.Viewport.Height -
                           _pipeTexture.Height / 2 -
                           pipeHeight -
                           DistanceBetweenPipe;
        var bottomPipeRectangle = new Rectangle( 0, 0, _pipeTexture.Width, bottomHeight);

        return new Tuple<Rectangle, Rectangle>(topPipeRectangle, bottomPipeRectangle);
    }

}