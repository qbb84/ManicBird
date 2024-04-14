using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.MainMenu;
using TestGame.Resource;
using TestGame.StateMachine;

namespace TestGame.PlayingState;

public class PlayingState : AbstractState {
    private readonly ResourceManager _mainMenuResource;

    private SpritePreservation _mainBackgroundSprite;
    private SpritePreservation _bluebirdPreservation;

    private Texture2D _mainBackground;
    private Texture2D _bluebird;

    public PlayingState(GameStateManager gameStateManager, Game game, GraphicsDeviceManager graphicsDeviceManager, ResourceManager resourceManager): base(gameStateManager, game, graphicsDeviceManager) {
        _mainMenuResource = ResourceManager.GetInstance();
    }

    public override void Enter() {
        _mainBackgroundSprite = _mainMenuResource.GetSprite("sprites/background-day");
        _bluebirdPreservation = _mainMenuResource.GetSprite("sprites/bluebird-downflap");

        _mainBackground = _mainBackgroundSprite.Texture;
        _bluebird = _bluebirdPreservation.Texture;
    }

    public override void Exit() {
    }

    public override void Update(GameTime gameTime) {
    }

    public override void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(_mainBackground, _mainBackgroundSprite.Rectangle, Color.White);
        spriteBatch.Draw(_bluebird, _bluebirdPreservation.Position, Color.White);
    }
}