using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.StateMachine;

public class ContentStateManager {

    private ContentManager _contentManager { get; }


    public ContentStateManager(ContentManager contentManager) {
        _contentManager = contentManager;
    }

    public Texture2D Load(string texture) {
        return _contentManager.Load<Texture2D>(texture);
    }
}