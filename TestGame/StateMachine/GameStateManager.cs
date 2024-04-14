using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.StateMachine;

public class GameStateManager {

    private IGameState _currentState;

    public void ChangeState(IGameState newState) {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Update(GameTime gameTime) {
        _currentState?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch) {
        _currentState?.Draw(spriteBatch);
    }


}