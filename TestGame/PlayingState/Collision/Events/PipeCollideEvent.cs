using System;
using Microsoft.Xna.Framework;
using TestGame.PlayingState.Collision.CollisionTypes;
using TestGame.PlayingState.Collision.EventArgs;
using TestGame.PlayingState.EventRegister;
using TestGame.PlayingState.Pipes;
using TestGame.Resource;

namespace TestGame.PlayingState.Collision.Events;

[RegisterEvent]
public class PipeCollideEvent {

    public PipeCollideEvent() {
        CollideManager.Instance.PipeCollideEvent += InstancePipeCollideEvent;
    }

    private void InstancePipeCollideEvent(object sender, PipeCollideEventArgs e) {
        var player = e.Player;

        var topPipe = e.CollideTopPipe;
        var bottomPipe = e.CollideBottomPipe;

        var hitTopPipeBottom = e.TopPipeTop;
        var hitBottomPipeTop = e.BottomPipeTop;

        switch (e.PipeCollisionType) {
            case PipeCollisionType.Top:
                if (hitTopPipeBottom != null) {
                    var birdY = player.Position.Y + player.Texture.Height;
                    player.Position = new Vector2(player.Position.X, birdY);
                    return;
                }
                var birdTopX = Math.Clamp(player.Position.X, 0, topPipe!.Position.X - player.Texture.Width);
                player.Position = new Vector2(birdTopX, player.Position.Y);
                break;
            case PipeCollisionType.Bottom:
                if (hitBottomPipeTop != null) {
                    var birdY = player.Position.Y - player.Texture.Height;
                    player.Position = new Vector2(player.Position.X, birdY);
                    return;
                }
                var birdBottomX = Math.Clamp(player.Position.X, 0, bottomPipe!.Position.X - player.Texture.Width);
                player.Position = new Vector2(birdBottomX, player.Position.Y);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}