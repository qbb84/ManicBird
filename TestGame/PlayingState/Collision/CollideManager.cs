using System;
using System.Threading;
using TestGame.PlayingState.Collision.EventArgs;
using TestGame.Resource;

namespace TestGame.PlayingState.Collision;

internal sealed class CollideManager {
    private static readonly Lazy<CollideManager> Lazy = new(() => new CollideManager());
    public static CollideManager Instance => Lazy.Value;

    public event EventHandler<OnCollideEventArgs> OnPlayerCollide;



    private CollideManager() {}


    private void InvokeOnBirdCollide(OnCollideEventArgs e) {
        OnPlayerCollide?.Invoke(this, e);
    }

    public void InvokeCollisionEvent(CollisionType collisionType, SpritePreservation spritePreservation, float? bottomFloorPosition = null, SpritePreservation? collidingObject = null) {
        var screenTopEventArgs = new OnCollideEventArgs {
            CollisionType = collisionType,
            PlayerPreservation = spritePreservation,
            CollisionFloorTop = collisionType == CollisionType.ViewportBottom ? bottomFloorPosition ?? -0f : null,
            CollidingObject = collidingObject
        };
        InvokeOnBirdCollide(screenTopEventArgs);
    }
}