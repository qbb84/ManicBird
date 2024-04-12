using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestGame.StateMachine;

namespace TestGame.MainMenu;

public class MainMenuState : IGameState {

    private readonly GameStateManager _stateManager;
    private readonly ContentStateManager _contentManager;
    private readonly GraphicsDeviceManager _graphicsDeviceManager;

    private Texture2D mainMenuBackground;
    private Texture2D startGraphics;

    private readonly int _maxWidth;
    private readonly int _maxHeight;
    private readonly int _widthBaseSize;
    private readonly int _heightBaseSize;


    public MainMenuState(GameStateManager gameStateManager, ContentStateManager contentStateManager, GraphicsDeviceManager graphicsDeviceManager) {
        _stateManager = gameStateManager;
        _contentManager = contentStateManager;
        _graphicsDeviceManager = graphicsDeviceManager;

        _maxWidth = _graphicsDeviceManager.GraphicsDevice.Viewport.Width;
        _maxHeight = _graphicsDeviceManager.GraphicsDevice.Viewport.Height;

        _widthBaseSize = _maxWidth - 600;
        _heightBaseSize = _maxHeight - 200;

    }

    public void Enter() {
        mainMenuBackground = _contentManager.Load("sprites/background-day");
        startGraphics = _contentManager.Load("sprites/message");
    }

    public void Exit() {
    }

    public void Update(GameTime gameTime) {
        if (UserPressedStart()) {
            _stateManager.ChangeState(new PlayingState.PlayingState(_stateManager));
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(mainMenuBackground, new Rectangle(0, 0, _maxWidth, _maxHeight), Color.White);

        var relativeCenterX = (_maxWidth / 2) - (_widthBaseSize / 2);
        var relativeCenterY = (_maxHeight / 2) - (_heightBaseSize / 2);
        spriteBatch.Draw(startGraphics, new Rectangle(relativeCenterX, relativeCenterY, _widthBaseSize, _heightBaseSize), Color.White);
    }

    private bool UserPressedStart() {
        return false;
    }
}