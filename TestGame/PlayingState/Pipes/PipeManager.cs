using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TestGame.Utils;

namespace TestGame.PlayingState.Pipes;

public static class PipeManager {
    private static Queue<Tuple<Pipe, Pipe>> PipePool { get; }


    static PipeManager() {
        PipePool = new Queue<Tuple<Pipe, Pipe>>();
    }

    public static void AddPipe(Tuple<Pipe, Pipe> pipeTuple) {
        PipePool.Enqueue(pipeTuple);
    }

    public static void AddPipe(Pipe topPipe, Pipe bottomPipe) {
        PipePool.Enqueue(new Tuple<Pipe, Pipe>(topPipe, bottomPipe));
    }

    public static Tuple<Pipe, Pipe> RemovePipe() {
        return PipePool.Dequeue();
    }

    public static bool IsEmpty() {
        return PipePool.Count == 0;
    }

    public static Queue<Tuple<Pipe, Pipe>> GetPipeQueue() {
        return PipePool;
    }

    #nullable enable
    public static Tuple<Pipe, Pipe>? PeekFirst() {
        return !IsEmpty() ? PipePool.Peek() : null;
    }

    public static Tuple<Rectangle, Rectangle> CalculateRandomPipeHeight(GameTime gameTime,
        int viewportHeight,
        int pipeTextureHeight,
        int pipeTextureWidth,
        int? distanceBetweenPipe = null,
        Vector2? scale = null) {

        var (x, y) = scale ?? Vector2.One;
        x = 1; //TODO temp reset till bounding is fixed

        var pipeHeight =
            100 +
            (int)Math.Sin(Utility.CreateRandomNumber(gameTime.ElapsedGameTime.Milliseconds) + 1) +
            Utility.CreateRandomNumber(pipeTextureHeight / 2);
        var topPipeRectangle = new Rectangle( 0, 0, (int)(pipeTextureWidth * x), pipeHeight);

        var bottomHeight = viewportHeight  -
                           pipeTextureHeight / 2 -
                           pipeHeight -
                           (distanceBetweenPipe ?? 15);
        var bottomPipeRectangle = new Rectangle( 0, 0, (int)(pipeTextureWidth * x), bottomHeight);


        return new Tuple<Rectangle, Rectangle>(topPipeRectangle, bottomPipeRectangle);
    }


}