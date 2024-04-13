using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.MainMenu;
using TestGame.StateMachine;

namespace TestGame;

public class Game1 : Game {

    private readonly GraphicsDeviceManager _graphics;
    private readonly GameStateManager stateManager;
    private readonly ContentStateManager _contentStateManager;

    private static SpriteBatch spriteBatch;

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        stateManager = new GameStateManager();
    }

    protected override void Initialize() {
        stateManager.ChangeState(new MainMenuState(stateManager, new ContentStateManager(Content), _graphics, spriteBatch, this.Services));
        base.Initialize();
    }

    protected override void LoadContent() {
        spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime) {
        stateManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin();
        stateManager.Draw(spriteBatch);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}