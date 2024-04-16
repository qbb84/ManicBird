using TestGame.PlayingState.Collision.Events;

namespace TestGame.PlayingState.Events;

public class EventRegistration {

    public EventRegistration() {
        var collisionEvent = new ViewportCollideEvent();
    }
}