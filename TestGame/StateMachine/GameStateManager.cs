using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.PlayingState.Keybindings;

namespace TestGame.StateMachine;

public class GameStateManager {

    private IGameState _currentState;
    private bool _isPaused;

    public void ChangeState(IGameState newState) {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Update(GameTime gameTime) {
        DefaultKeybinds.Instance.Setup(ref _isPaused);

        if (!_isPaused) {
            _currentState?.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        _currentState?.Draw(spriteBatch);
    }


}