using System;
using TestGame.PlayingState.Collision.EventArgs;

namespace TestGame.PlayingState.Collision;

internal sealed class CollideManager {
    private static readonly Lazy<CollideManager> Lazy = new(() => new CollideManager());
    public static CollideManager Instance => Lazy.Value;

    public event EventHandler<OnViewportCollideEventArgs> OnBirdCollide;


    private CollideManager() {}

    public void InvokeOnBirdCollide(OnViewportCollideEventArgs e) {
        OnBirdCollide?.Invoke(this, e);
    }
}