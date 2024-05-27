using System;
using Microsoft.Xna.Framework;
using TestGame.PlayingState.Collision.CollisionTypes;
using TestGame.PlayingState.Collision.EventArgs;
using TestGame.PlayingState.Pipes;
using TestGame.Resource;
using TestGame.Utils;

namespace TestGame.PlayingState.Collision;

internal sealed class CollideManager {
    private static readonly Lazy<CollideManager> Lazy = new(() => new CollideManager());
    public static CollideManager Instance => Lazy.Value;

    public event EventHandler<ViewportCollideEventArgs> ViewportCollideEvent;
    public event EventHandler<PipeCollideEventArgs> PipeCollideEvent;



    private CollideManager() { }


    private void InvokeOnBirdCollide(ViewportCollideEventArgs e) {
        ViewportCollideEvent?.Invoke(this, e);
    }

    public void InvokeViewportCollisionEvent(ViewportCollisionType viewportCollisionType,
        SpritePreservation spritePreservation, float? bottomFloorPosition = null,
        SpritePreservation collidingObject = null) {

        var screenTopEventArgs = new ViewportCollideEventArgs {
            ViewportCollisionType = viewportCollisionType,
            Player = spritePreservation,
            CollisionFloorTop = viewportCollisionType == ViewportCollisionType.ViewportBottom
                ? bottomFloorPosition ?? 0f
                : null,
            CollidingObject = collidingObject
        };
        InvokeOnBirdCollide(screenTopEventArgs);
    }


    private  void OnPipeCollideEvent(PipeCollideEventArgs e) {
        PipeCollideEvent?.Invoke(this, e);
    }

    private void InvokePipeCollisionEvent(PipeCollisionType pipeCollisionType, SpritePreservation player,
        Pipe topPipe = null, Pipe bottomPipe = null,
        bool? topPipeTop = null, bool? bottomPipeTop = null) {

        var screenTopEventArgs = new PipeCollideEventArgs {
            PipeCollisionType = pipeCollisionType,
            Player = player,
            CollideTopPipe = topPipe,
            CollideBottomPipe = bottomPipe,
            TopPipeTop = topPipeTop,
            BottomPipeTop = bottomPipeTop
        };
        OnPipeCollideEvent(screenTopEventArgs);
    }

    public static void HandleTopPipeCollision(Rectangle birdRectangle, Pipe topPipe, SpritePreservation player, float pipeScale) {
        var topBottomRect = new Rectangle((int) topPipe.Position.X, topPipe.Rectangle.Height, (int)(topPipe.Rectangle.Width * pipeScale), 1);
        var topPipeRect = Utility.CreateRectangle(topPipe.Position, topPipe.Rectangle.Width * pipeScale, topPipe.Rectangle.Height);

        if (birdRectangle.Intersects(topBottomRect)) {
            Instance.InvokePipeCollisionEvent(
                PipeCollisionType.Top, player, topPipe, null, true);
            return;
        }

        if (birdRectangle.Intersects(topPipeRect)) {
            Instance.InvokePipeCollisionEvent(
                PipeCollisionType.Top, player, topPipe);
        }
    }


    public static void HandleBottomPipeCollision(Rectangle birdRectangle, Pipe bottomPipe, SpritePreservation player,
        float pipeScale, float bottomFloorPosition) {

        var bottomTopRect = new Rectangle((int)bottomPipe.Position.X, (int)bottomFloorPosition - bottomPipe.Rectangle.Height, (int)(bottomPipe.Rectangle.Width * pipeScale), 3);
        var bottomPipeRect = Utility.CreateRectangle(bottomPipe.Position, bottomPipe.Rectangle.Width * pipeScale, bottomPipe.Rectangle.Height);

        if (birdRectangle.Intersects(bottomTopRect)) {
            Instance.InvokePipeCollisionEvent(
                PipeCollisionType.Bottom, player, null, bottomPipe, null, true);
            return;
        }

        if (birdRectangle.Intersects(bottomPipeRect)) {
            Instance.InvokePipeCollisionEvent(
                PipeCollisionType.Bottom, player, null, bottomPipe);
        }
    }
}