using System;
using Microsoft.Xna.Framework;
using TestGame.PlayingState.Collision.EventArgs;

namespace TestGame.PlayingState.Collision.Events;

public class ViewportCollideEvent {

    public ViewportCollideEvent() {
        CollideManager.Instance.OnPlayerCollide += InstanceOnPlayerCollide;
    }

    private void InstanceOnPlayerCollide(object sender, OnCollideEventArgs e) {
        var bluebirdPreservation = e.PlayerPreservation;
        var birdTop = bluebirdPreservation.Position.Y - bluebirdPreservation.Texture.Height;
        var birdBottom = bluebirdPreservation.Position.Y + bluebirdPreservation.Texture.Height;

        var birdX = bluebirdPreservation.Position.X;


        switch (e.CollisionType) {
            case CollisionType.ViewportTop: {
                if (!(birdTop <= 0)) return;

                ref var birdY = ref birdTop;
                birdY = 0;

                bluebirdPreservation.Position = new Vector2(birdX, 0);
                break;
            }
            case CollisionType.ViewportBottom: {
                if (!(birdBottom >= e.CollisionFloorTop)) return;

                ref var birdY = ref birdBottom;
                birdY = Math.Clamp(birdY, 0, (int) e.CollisionFloorTop - bluebirdPreservation.Texture.Height);

                bluebirdPreservation.Position = new Vector2(birdX, birdY);
                break;
            }
            case CollisionType.Sprite:
            default:
                throw new ArgumentOutOfRangeException();
        }

    }
}