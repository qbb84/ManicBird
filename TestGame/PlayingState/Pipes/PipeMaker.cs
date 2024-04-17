using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TestGame.Resource;

namespace TestGame.PlayingState.Pipes;

public class PipeMaker: IPipeData<PipeMaker, Pipe> {
    public SpritePreservation SpritePreservation { get; set; }
    public Vector2? Position { get; set; }
    public Vector2? Scale { get; set; }
    public Rectangle Rectangle { get; set; }

    private static ContentManager _contentManager;


    public PipeMaker SetSprite(SpritePreservation pipeSprite) {
        SpritePreservation = pipeSprite;
        return this;
    }

    public PipeMaker SetPosition(Vector2 position) {
        Position = position;
        return this;
    }

    public PipeMaker SetScale(Vector2 scale) {
        Scale = scale;
        return this;
    }

    public PipeMaker SetRectangle(Rectangle rectangle) {
        Rectangle = rectangle;
        return this;
    }

    public PipeMaker SetContent(ContentManager contentManager) {
        _contentManager = contentManager;
        return this;
    }

    public Pipe CreatePipe() {
        if (SpritePreservation == null) throw new InvalidOperationException("Cannot create Pipe: SpritePreservation is not set.");
        if (_contentManager == null) throw new InvalidOperationException("Cannot create Pipe: ContentManager is not set.");

        var defaultPositionCheck = Position ?? Vector2.Zero;
        var defaultScaleCheck = Scale ?? Vector2.One;

        return new Pipe(SpritePreservation, defaultPositionCheck, defaultScaleCheck, Rectangle);
    }
}