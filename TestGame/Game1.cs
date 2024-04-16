using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.MainMenu;
using TestGame.PlayingState.Events;
using TestGame.StateMachine;

namespace TestGame;

public class Game1 : Game {
    private readonly GraphicsDeviceManager _graphics;
    private readonly GameStateManager _stateManager;

    private static SpriteBatch _spriteBatch;

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _stateManager = new GameStateManager();
    }

    protected override void Initialize() {
        _stateManager.ChangeState(new MainMenuState(_stateManager, this));
        Window.Title = "ManicBird";

        var eventRegistration = new EventRegistration();

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime) {
        _stateManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _stateManager.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}