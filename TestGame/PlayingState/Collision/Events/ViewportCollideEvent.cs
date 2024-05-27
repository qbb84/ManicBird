using System;
using Microsoft.Xna.Framework;
using TestGame.PlayingState.Collision.CollisionTypes;
using TestGame.PlayingState.Collision.EventArgs;
using TestGame.PlayingState.EventRegister;

namespace TestGame.PlayingState.Collision.Events;

[RegisterEvent]
public class ViewportCollideEvent {

    public ViewportCollideEvent() {
        CollideManager.Instance.ViewportCollideEvent += InstanceViewportCollideEvent;
    }

    private void InstanceViewportCollideEvent(object sender, ViewportCollideEventArgs e) {
        var bluebirdPreservation = e.Player;
        var birdTop = bluebirdPreservation.Position.Y - bluebirdPreservation.Texture.Height;
        var birdBottom = bluebirdPreservation.Position.Y + bluebirdPreservation.Texture.Height;

        var birdX = bluebirdPreservation.Position.X;


        switch (e.ViewportCollisionType) {
            case ViewportCollisionType.ViewportTop: {
                if (!(birdTop <= 0)) return;

                ref var birdY = ref birdTop;
                birdY = 0;

                bluebirdPreservation.Position = new Vector2(birdX, 0);
                break;
            }
            case ViewportCollisionType.ViewportBottom: {
                if (!(birdBottom >= e.CollisionFloorTop)) return;

                ref var birdY = ref birdBottom;
                birdY = Math.Clamp(birdY, 0, (int) e.CollisionFloorTop - bluebirdPreservation.Texture.Height);

                bluebirdPreservation.Position = new Vector2(birdX, birdY);
                break;
            }
            case ViewportCollisionType.Sprite:
            default:
                throw new ArgumentOutOfRangeException();
        }

    }
}