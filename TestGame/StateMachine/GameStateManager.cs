using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.StateMachine;

public class GameStateManager {

        private IGameState currentState;

        public void ChangeState(IGameState newState) {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void Update(GameTime gameTime) {
            currentState?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) {
            currentState?.Draw(spriteBatch);
        }


}