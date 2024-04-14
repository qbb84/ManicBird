using System;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.Resource;
using TestGame.StateMachine;
using TestGame.Utils;

namespace TestGame.MainMenu;

public class MainMenuState : AbstractState {
    private readonly ResourceManager _mainMenuResource;

    private Texture2D _mainMenuBackground;
    private Texture2D _startGraphics;
    private Texture2D _bluebird;
    private Texture2D _redbird;
    private Texture2D _yellowbird;
    private Texture2D _currentBirdSprite;

    private readonly int _maxWidth;
    private readonly int _maxHeight;
    private readonly int _widthBaseSize;
    private readonly int _heightBaseSize;

    private readonly List<Texture2D> _birdList;
    private TimedUpdate _timedUpdate;
    private Rectangle _mainBackgroundBounds;

    private SpritePreservation _mainBackgroundSprite;
    private SpritePreservation _bluebirdSprite;

    private readonly Vector2 _currentBirdLocation;
    private int _currentBirdIndex;



    public MainMenuState(GameStateManager gameStateManager, Game game, GraphicsDeviceManager graphicsDeviceManager): base(gameStateManager, game, graphicsDeviceManager) {
        _mainMenuResource = ResourceManager.GetInstance(game.Content);

        _maxWidth = GraphicsDeviceManager.GraphicsDevice.Viewport.Width;
        _maxHeight = GraphicsDeviceManager.GraphicsDevice.Viewport.Height;

        _widthBaseSize = _maxWidth - 600;
        _heightBaseSize = _maxHeight - 200;

        _currentBirdLocation.X = (_maxWidth / 2) - 17;
        _currentBirdLocation.Y = (_maxHeight / 2) + 37;

        _birdList = new List<Texture2D>();
    }

    public override void Enter() {
        _mainBackgroundSprite = _mainMenuResource.GetSprite("sprites/background-day");
        _startGraphics = _mainMenuResource.GetSprite("sprites/message").Texture;
        _bluebirdSprite = _mainMenuResource.GetSprite("sprites/bluebird-downflap");
        _redbird = _mainMenuResource.GetSprite("sprites/redbird-downflap").Texture;
        _yellowbird = _mainMenuResource.GetSprite("sprites/yellowbird-downflap").Texture;

        _mainMenuBackground = _mainBackgroundSprite.Texture;
        _bluebird = _bluebirdSprite.Texture;
        _currentBirdSprite = _bluebird;

        _birdList.Add(_bluebird);
        _birdList.Add(_redbird);
        _birdList.Add(_yellowbird);

        _timedUpdate = new TimedUpdate(TimedUpdate.CheckTime.ONE_SECOND);
    }

    public override void Exit() {
        _mainMenuResource.UnloadResource("sprites/message");

        _bluebirdSprite.Position = _currentBirdLocation;
        _mainBackgroundSprite.Rectangle = _mainBackgroundBounds;
    }

    public override void Update(GameTime gameTime) {
        if (UserPressedStart()) {
            StateManager.ChangeState(new PlayingState.PlayingState(StateManager, Game, GraphicsDeviceManager, _mainMenuResource));
        }

        if (_timedUpdate.UpdateTimer(gameTime)) {
            _currentBirdSprite = _birdList[_currentBirdIndex];
            _currentBirdIndex = (_currentBirdIndex + 1) % _birdList.Count;
        }

    }

    public override void Draw(SpriteBatch spriteBatch) {
        _mainBackgroundBounds = new Rectangle(0, 0, _maxWidth, _maxHeight);
        spriteBatch.Draw(_mainMenuBackground, _mainBackgroundBounds, Color.White);

        var relativeCenterX = _maxWidth / 2 - _widthBaseSize / 2;
        var relativeCenterY = _maxHeight / 2 - _heightBaseSize / 2;


        spriteBatch.Draw(_startGraphics, new Rectangle(relativeCenterX, relativeCenterY, _widthBaseSize, _heightBaseSize), Color.White);
        spriteBatch.Draw(_currentBirdSprite, _currentBirdLocation, Color.White);
    }

    private bool UserPressedStart() {
        var mouseState = Mouse.GetState();
        var pos = mouseState.Position;
        return _mainMenuBackground.GraphicsDevice.Viewport.Bounds.Contains(pos.X, pos.Y) && mouseState.LeftButton == ButtonState.Pressed;
    }
}