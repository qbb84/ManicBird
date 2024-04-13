using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.StateMachine;
using TestGame.Utils;

namespace TestGame.MainMenu;

public class MainMenuState : IGameState {

    private readonly GameStateManager _stateManager;
    private readonly ContentStateManager _contentManager;
    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    private readonly SpriteBatch _spriteBatch;

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

    private List<Texture2D> _birdList;
    private TimedUpdate _timedUpdate;

    private int _currentBirdIndex = 0;



    public MainMenuState(GameStateManager gameStateManager, ContentStateManager contentStateManager, GraphicsDeviceManager graphicsDeviceManager,
                         SpriteBatch spriteBatch, IServiceProvider serviceProvider) {
        _stateManager = gameStateManager;
        _contentManager = contentStateManager;
        _graphicsDeviceManager = graphicsDeviceManager;
        _spriteBatch = spriteBatch;

        _maxWidth = _graphicsDeviceManager.GraphicsDevice.Viewport.Width;
        _maxHeight = _graphicsDeviceManager.GraphicsDevice.Viewport.Height;

        _widthBaseSize = _maxWidth - 600;
        _heightBaseSize = _maxHeight - 200;

        _birdList = new List<Texture2D>();

    }

    public void Enter() {
        _mainMenuBackground = _contentManager.Load("sprites/background-day");
        _startGraphics = _contentManager.Load("sprites/message");

        _bluebird = _contentManager.Load("sprites/bluebird-downflap");
        _redbird = _contentManager.Load("sprites/redbird-downflap");
        _yellowbird = _contentManager.Load("sprites/yellowbird-downflap");
        _currentBirdSprite = _bluebird;

        _birdList.Add(_bluebird); _birdList.Add(_redbird); _birdList.Add(_yellowbird);

        _timedUpdate = new TimedUpdate(TimedUpdate.CheckTime.ONE_SECOND);
    }

    public void Exit() { }

    public void Update(GameTime gameTime) {
        if (UserPressedStart()) {
            _stateManager.ChangeState(new PlayingState.PlayingState(_stateManager));
        }

        if (_timedUpdate.UpdateTimer(gameTime)) {
            _currentBirdSprite = _birdList[_currentBirdIndex];
            _currentBirdIndex = (_currentBirdIndex + 1) % _birdList.Count;
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(_mainMenuBackground, new Rectangle(0, 0, _maxWidth, _maxHeight), Color.White);

        var relativeCenterX = (_maxWidth / 2) - (_widthBaseSize / 2);
        var relativeCenterY = (_maxHeight / 2) - (_heightBaseSize / 2);
        spriteBatch.Draw(_startGraphics, new Rectangle(relativeCenterX, relativeCenterY, _widthBaseSize, _heightBaseSize), Color.White);

        spriteBatch.Draw(_currentBirdSprite, new Vector2((_maxWidth / 2) - 17, (_maxHeight / 2) + 37), Color.White);
    }

    private bool UserPressedStart() {
        return Mouse.GetState().LeftButton == ButtonState.Pressed;
    }
}