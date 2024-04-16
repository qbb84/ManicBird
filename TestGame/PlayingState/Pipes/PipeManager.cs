using System;
using System.Collections;
using System.Collections.Generic;

namespace TestGame.PlayingState.Pipes;

public static class PipeManager {
    private static Queue<Tuple<Pipe, Pipe>> PipePool { get; }

    static PipeManager() {
        PipePool = new Queue<Tuple<Pipe, Pipe>>();
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

}