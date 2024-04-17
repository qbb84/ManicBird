using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.Resource;

public class ResourceManager {
    private readonly ContentManager _content;
    private readonly Dictionary<string, SpritePreservation> _sprites;

    private static readonly Lazy<ResourceManager> LazyInstance = new(() =>
                new ResourceManager(ContentManagerProvider.ContentManager),
                LazyThreadSafetyMode.ExecutionAndPublication);

    public static ResourceManager Instance => LazyInstance.Value;


    private ResourceManager(ContentManager content) {
        _content = content ?? throw new InvalidOperationException("ContentManager cannot be null.");
        _sprites = new Dictionary<string, SpritePreservation>();
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

    public static class ContentManagerProvider {
        public static ContentManager ContentManager { get; set; }
    }
}