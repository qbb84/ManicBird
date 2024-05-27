using TestGame.PlayingState.Collision.CollisionTypes;
using TestGame.PlayingState.Pipes;
using TestGame.Resource;

namespace TestGame.PlayingState.Collision.EventArgs;

public class PipeCollideEventArgs : System.EventArgs {
    public PipeCollisionType PipeCollisionType { get; init; }
    public SpritePreservation Player { get; init; }

    #nullable enable
    public Pipe? CollideTopPipe { get; init; }
    public Pipe? CollideBottomPipe { get; init; }
    public bool? TopPipeTop { get; init; }
    public bool? BottomPipeTop { get; init; }
}