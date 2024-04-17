using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Resource;

namespace TestGame.PlayingState.Pipes;

public class Pipe {
    public SpritePreservation SpritePreservation { get; private set; }
    public Vector2 Position { get; set; }
    public Vector2 Scale { get; private set; }
    public Rectangle Rectangle { get; set; }

    private readonly Texture2D _texture2DSave;
    private Vector2 StartPositionSave { get; }


    public Pipe(SpritePreservation spritePreservation, Vector2 position, Vector2 scale, Rectangle rectangle) {
        SpritePreservation = spritePreservation;
        Position = position;
        Scale = scale;
        Rectangle = rectangle;

        _texture2DSave = spritePreservation.Texture;
        StartPositionSave = Position;
    }

    public void ResetPipe() {
        SpritePreservation = new SpritePreservation(_texture2DSave);
        Position = StartPositionSave;
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

