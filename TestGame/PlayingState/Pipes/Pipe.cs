using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Resource;

namespace TestGame.PlayingState.Pipes;

public class Pipe {
    public SpritePreservation SpritePreservation { get; private set; }
    public Vector2 Position { get; private set; }
    public float Scale { get; private set; }
    public Rectangle Rectangle { get; set; }

    private readonly Texture2D _texture2DSave;


    public Pipe(SpritePreservation spritePreservation, Vector2 position, float scale, Rectangle rectangle) {
        SpritePreservation = spritePreservation;
        Position = position;
        Scale = scale;
        Rectangle = rectangle;

        _texture2DSave = spritePreservation.Texture;
    }

    public void ResetPipe() {
        SpritePreservation = new SpritePreservation(_texture2DSave);
        Position = Vector2.Zero;
        Scale = 0;
    }

    public void DecreasePosX(int amount) {
        Position = new Vector2(Position.X - amount, Position.Y);
    }

    public void IncreasePosX(int amount) {
        Position = new Vector2(Position.X + amount, Position.Y);
    }

    public void DecreasePosY(int amount) {
        Position = new Vector2(Position.X, Position.Y - amount);
    }

    public void IncreasePosY(int amount) {
        Position = new Vector2(Position.X, Position.Y + amount);
    }



}

