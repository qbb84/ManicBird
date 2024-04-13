using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.StateMachine;

namespace TestGame.PlayingState;

public class PlayingState : IGameState {

    private readonly GameStateManager _gameStateManager;

    public PlayingState(GameStateManager gameStateManager) {
        _gameStateManager = gameStateManager;
    }

    public void Enter() {
    }

    public void Exit() {
    }

    public void Update(GameTime gameTime) {
    }

    public void Draw(SpriteBatch spriteBatch) {

    }
}