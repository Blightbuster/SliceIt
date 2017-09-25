using UnityEngine;
using Random = UnityEngine.Random;

public static class Bot
{
    public static float TotalMass = 1000;
    public static float TotalMassLeft = 1000;
    public static int PointsForWin = 5;
    public static float BluffChance = 0.3f;     // In %
    public static float BluffIncrease = 0.2f;   // In %
    public static float BluffMargin = 0.1f;     // In %
    public static bool AllowBluff = true;
    public static float MinBluffMassMargin = 0.1f;  // In %
    public static float MaxBluffMassMargin = 0.3f;  // In %
    public static float MinNoneBluffMassMultiplier = 0.3f;
    public static float MaxNoneBluffMassMultiplier = 1.5f;
    public static bool DebugMode = true;

    private static int _botScore;
    private static int _playerScore;
    private static int _roundIndex;
    private static int _maxRounds = PointsForWin * 2 - 1;
    private static float _averageMassPerRound = TotalMass / _maxRounds;
    private static bool _bluffedLastRound = false;
    private static float _currentBluffChance = BluffChance;

    public static float GetNextMove()
    {
        _botScore = Manager.GameManager.GetComponent<GameManager>().OpponentScore;
        _playerScore = Manager.GameManager.GetComponent<GameManager>().PlayerScore;
        _roundIndex++;
        float nextMoveMass = 0;
        float averageMassPerRoundStatic = (_averageMassPerRound + (TotalMass / GameLengthPrediction())) / 2;            // Get average mass for rounds only account TotalMass
        float averageMassPerRoundDynamic = ((TotalMassLeft / _maxRounds) + (TotalMass / GameLengthPrediction())) / 2;   // Get average mass for rounds and account TotalMassLeft
        nextMoveMass = (averageMassPerRoundStatic + averageMassPerRoundDynamic) / 2;                                    // Get average of Static and Dynamic mass average

        if (DebugMode)
        {
            Debug.Log("Next Base Move Mass: " + nextMoveMass);
            Debug.Log("Average Mass Per Round Static: " + averageMassPerRoundStatic);
            Debug.Log("Average Mass Per Round Dynamic: " + averageMassPerRoundDynamic);
        }

        if (Bluff())
        {
            if(DebugMode) Debug.Log("Bluff");
            nextMoveMass *= Random.Range(MinBluffMassMargin, MaxBluffMassMargin);
        }
        else
        {
            if(DebugMode) Debug.Log("No Bluff");
            nextMoveMass *= Random.Range(MinNoneBluffMassMultiplier, MaxNoneBluffMassMultiplier);
        }

        if (_playerScore == PointsForWin - 1)
        {
            if(DebugMode) Debug.Log("All in");
            nextMoveMass = TotalMassLeft;
        }

        if (nextMoveMass > TotalMassLeft) nextMoveMass = TotalMassLeft;

        TotalMassLeft -= nextMoveMass;

        if(DebugMode) Debug.Log("Next Move Mass: " + nextMoveMass + "\n--------------------------------------------------");
        return nextMoveMass;
    }

    private static bool Bluff()
    {
        _currentBluffChance = BluffChance;
        if (_bluffedLastRound) _currentBluffChance -= BluffIncrease + Random.Range(-BluffMargin, BluffMargin);  // Decrease bluff chance based on recent bluffs
        _currentBluffChance += (_botScore - _playerScore) * (BluffIncrease + Random.Range(-BluffMargin, BluffMargin));  // Increase/Dexrease bluff chance based on point advantage/disadvantage
        if (DebugMode) Debug.Log("Bluff Chance: " + _currentBluffChance);
        return Random.Range(0.0f, 1.0f) < _currentBluffChance;
    }

    private static int GameLengthPrediction()
    {
        if (_playerScore > _botScore) return Random.Range(PointsForWin - _playerScore, _maxRounds);
        return Random.Range(PointsForWin - _botScore, _maxRounds);
    }
}
