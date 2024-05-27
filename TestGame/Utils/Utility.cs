using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.Utils;

public static class Utility {
    private static readonly Random Random = new();

    public static int CreateRandomNumber(int max) {
        return Random.Next(max);
    }

    public static Rectangle CreateRectangle(Vector2 position, float width, float height, int offsetY = 0) {
        return new Rectangle(
            (int)position.X,
            (int)position.Y + offsetY,
            (int)width,
            (int)height
        );
    }

    public static void DrawRectangle(SpriteBatch spriteBatch, GraphicsDevice graphics, Rectangle rectangle, Color color,
        int thickness = 2) {

        var pixel = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
        pixel.SetData(new[] { Color.White });

        var tempScale = 1.5; //TODO temp scale till bounding fix

        spriteBatch.Draw(pixel, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
        spriteBatch.Draw(pixel, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
        spriteBatch.Draw(pixel,
            new Rectangle(rectangle.X + rectangle.Width - thickness, rectangle.Y, thickness, rectangle.Height), color);
        spriteBatch.Draw(pixel,
            new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - thickness, rectangle.Width, thickness), color);
    }
}
