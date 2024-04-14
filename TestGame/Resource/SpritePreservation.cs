using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.Resource;

public class SpritePreservation {
    public Texture2D Texture { get; private set; }
    public Vector2 Position { get; set; }
    public Rectangle Rectangle { get; set; }
    public float Rotation { get; set; }
    public Vector2 Scale { get; set; }

    public SpritePreservation(Texture2D texture) {
        Texture = texture;
        Position = Vector2.Zero;
        Rectangle = Rectangle.Empty;
        Rotation = 0f;
        Scale = Vector2.One;
    }
}