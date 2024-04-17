using TestGame.Resource;

namespace TestGame.PlayingState.Collision.EventArgs;

public class OnCollideEventArgs : System.EventArgs {
    public CollisionType CollisionType { get; init; }
    public SpritePreservation PlayerPreservation { get; init; }

    #nullable enable
    public float? CollisionFloorTop { get; init; }
    public SpritePreservation? CollidingObject { get; init; }
}