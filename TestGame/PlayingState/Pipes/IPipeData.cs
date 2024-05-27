using Microsoft.Xna.Framework;
using TestGame.Resource;

namespace TestGame.PlayingState.Pipes;

public interface IPipeData<out T, out TE> where T: class where TE: class {
    SpritePreservation SpritePreservation { get; set; }
    Vector2? Position { get; set; }
    Vector2? Scale { get; set; }
    Rectangle Rectangle { get; set; }

    T SetSprite(SpritePreservation pipeSprite);
    T SetPosition(Vector2 position);
    T SetScale(Vector2 scale);

    TE CreatePipe();
}