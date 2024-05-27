using System;
using System.Linq.Expressions;
using System.Resources;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.Resource;
using ResourceManager = TestGame.Resource.ResourceManager;

namespace TestGame.PlayingState.Keybindings;

public class DefaultKeybinds {

    private static readonly Lazy<DefaultKeybinds> Lazy = new(() => new DefaultKeybinds());
    public static DefaultKeybinds Instance => Lazy.Value;
    private bool _keyDown;


    private DefaultKeybinds() {
    }

    public void Setup(ref bool isPaused) {
        Settings(ref isPaused);
    }

    private void Settings(ref bool isPaused) {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
            _keyDown = true;
        } else if (Keyboard.GetState().IsKeyUp(Keys.Escape) && _keyDown) {
            isPaused = !isPaused;
            _keyDown = false;
        }
    }
}