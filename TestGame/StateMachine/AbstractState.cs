using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.StateMachine;

public abstract class AbstractState : IGameState {
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);

    protected readonly GameStateManager StateManager;
    protected readonly Game Game;
    protected readonly GraphicsDeviceManager GraphicsDeviceManager;

    protected AbstractState(GameStateManager gameStateManager, Game game, GraphicsDeviceManager graphicsDeviceManager) {
        StateManager = gameStateManager;
        Game = game;
        GraphicsDeviceManager = graphicsDeviceManager;
    }

}