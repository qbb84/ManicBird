using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.Resource;

public class ResourceManager {
    private readonly ContentManager _content;
    private readonly Dictionary<string, SpritePreservation> _sprites;

    private static ResourceManager _instance;

    private ResourceManager(ContentManager content) {
        _content = content;
        _sprites = new Dictionary<string, SpritePreservation>();
    }

    public static ResourceManager GetInstance(ContentManager contentManager = null) {
        if (_instance != null) return _instance;

        if (contentManager == null) {
            throw new InvalidOperationException("ContentManager must be provided when creating the ResourceManager instance for the first time.");
        }
        _instance = new ResourceManager(contentManager);
        return _instance;
    }

    public SpritePreservation GetSprite(string assetName) {
        if (_sprites.TryGetValue(assetName, out var sprite)) return sprite;

        var texture = _content.Load<Texture2D>(assetName);
        _sprites[assetName] = new SpritePreservation(texture);
        return _sprites[assetName];
    }

    public void UnloadResource(string assetName) {
        if (!_sprites.ContainsKey(assetName)) return;

        _sprites[assetName].Texture.Dispose();
        _sprites.Remove(assetName);
    }

    public void UnloadAll() {
        foreach (var sprites in _sprites.Values) {
            sprites.Texture.Dispose();
        }
        _sprites.Clear();
    }
}