using Microsoft.Xna.Framework;
using TestGame.Resource;

namespace TestGame.PlayingState.Pipes;

public interface IPipeData<out T, out TE> where T: class {
    SpritePreservation SpritePreservation { get; set; }
    Vector2? Position { get; set; }
    float Scale { get; set; }
    Rectangle Rectangle { get; set; }

    T SetSprite(SpritePreservation pipeSprite);
    T SetPosition(Vector2 position);
    T SetScale(float scale);

    TE CreatePipe();
}