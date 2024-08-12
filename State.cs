using System;
using System.Numerics;

struct State
{
    public State(
        double remainingSeconds,
        bool isTriggerFree,
        Vector2 targetPosition,
        int score,
        Vector2 canvasDimensions
    )
    {
        RemainingSeconds = remainingSeconds;
        IsTriggerFree = isTriggerFree;
        TargetPosition = targetPosition;
        Score = score;
        CanvasDimensions = canvasDimensions;
    }

    public double RemainingSeconds { get; }
    public bool IsTriggerFree { get; }
    public Vector2 TargetPosition { get; }
    public int Score { get; }
    public Vector2 CanvasDimensions { get; }
}

struct Input
{
    public Input(
        Vector2 shotPosition,
        double elapsedSeconds,
        bool isTriggerPulled
        )
    {
        this.shotPosition = shotPosition;
        this.elapsedSeconds = elapsedSeconds;
        this.isTriggerPulled = isTriggerPulled;
    }

    public Vector2 shotPosition { get; }
    public double elapsedSeconds { get; }
    public bool isTriggerPulled { get; }
}

class Engine
{
    private const int targetRadius = 45;

    public static State generateInitialState(Vector2 canvasDimensions) => new(
        remainingSeconds: 10,
        isTriggerFree: true,
        targetPosition: ComputeTargetPosition(canvasDimensions, new Random()),
        score: 0,
        canvasDimensions: canvasDimensions
    );

    public static State UpdateState(State state, Input input)
    {
        double newRemainingSeconds = calculateRemainingSeconds(
            state.RemainingSeconds,
            input.elapsedSeconds
        );

        if (IsValidShot(state, input))
        {
            return new State(
                remainingSeconds: newRemainingSeconds,
                isTriggerFree: false,
                targetPosition: ComputeTargetPosition(
                    state.CanvasDimensions,
                    Random.Shared
                ),
                score: state.Score + 1,
                canvasDimensions: state.CanvasDimensions
            );
        }

        if (!input.isTriggerPulled)
        {
            return new State(
                remainingSeconds: newRemainingSeconds,
                isTriggerFree: true,
                targetPosition: state.TargetPosition,
                score: state.Score,
                canvasDimensions: state.CanvasDimensions
            );
        }

        return state;
    }

    public static bool ShouldDrawTarget(State state) => state.RemainingSeconds > 0;

    private static bool IsValidShot(State state, Input input) =>
        input.isTriggerPulled
            && state.IsTriggerFree
            && hasHitTarget(state, input.shotPosition)
            && state.RemainingSeconds > 0;

    private static double calculateRemainingSeconds(double currentRemainingSeconds, double elapsedSeconds)
    {
        double newRemainingSeconds = currentRemainingSeconds - elapsedSeconds;
        if (newRemainingSeconds < 0) return 0;
        return newRemainingSeconds;
    }

    private static bool hasHitTarget(
        State state,
        Vector2 shotPosition
    ) => Vector2.Distance(state.TargetPosition, shotPosition) <= targetRadius;

    private static Vector2 ComputeTargetPosition(
        Vector2 canvasSize,
        Random random
    ) => new Vector2(
        random.Next(0, (int)canvasSize.X),
        random.Next(0, (int)canvasSize.Y)
    );
}
