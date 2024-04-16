using System;
using TestGame.PlayingState.Collision.EventArgs;

namespace TestGame.PlayingState.Collision.Events;

public class ViewportCollideEvent {

    public ViewportCollideEvent() {
        CollideManager.Instance.OnBirdCollide += InstanceOnBirdCollide;
    }

    private void InstanceOnBirdCollide(object sender, OnViewportCollideEventArgs e) {
        Console.WriteLine($"Collision Detected: {e.CollisionType}");
    }
}