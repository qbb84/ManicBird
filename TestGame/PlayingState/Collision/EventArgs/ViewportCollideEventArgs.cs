using TestGame.PlayingState.Collision.CollisionTypes;
using TestGame.Resource;

namespace TestGame.PlayingState.Collision.EventArgs;

public class ViewportCollideEventArgs : System.EventArgs {
    public ViewportCollisionType ViewportCollisionType { get; init; }
    public SpritePreservation Player { get; init; }

    #nullable enable
    public float? CollisionFloorTop { get; init; }
    public SpritePreservation? CollidingObject { get; init; }
}