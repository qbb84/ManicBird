using TestGame.Resource;

namespace TestGame.PlayingState.Collision.EventArgs;

public class OnViewportCollideEventArgs : System.EventArgs {
    public CollisionType CollisionType { get; init; }
    public SpritePreservation PlayerPreservation { get; set; }
}