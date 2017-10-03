using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class Bot
    {
        public float TotalMass = 1000;
        public float TotalMassLeft = 1000;
        public int PointsForWin = 5;
        public float BluffChance = 0.3f;     // In %
        public float BluffIncrease = 0.2f;   // In %
        public float BluffMargin = 0.1f;     // In %
        public bool AllowBluff = true;
        public float MinBluffMassMargin = 0.1f;  // In %
        public float MaxBluffMassMargin = 0.3f;  // In %
        public float MinNoneBluffMassMultiplier = 0.3f;
        public float MaxNoneBluffMassMultiplier = 1.5f;
        public int DebugMode = 0;

        private int _botScore;
        private int _playerScore;
        private int _roundIndex;
        private readonly int _maxRounds;
        private readonly float _averageMassPerRound;
        private readonly bool _bluffedLastRound;
        private float _currentBluffChance;

        public Bot()
        {
            _maxRounds = PointsForWin * 2 - 1;
            _averageMassPerRound = TotalMass / _maxRounds;
            _bluffedLastRound = false;
            _currentBluffChance = BluffChance;
        }

        public float GetNextMove()
        {
            _botScore = Manager.GameManager.Opponent.Points;
            _playerScore = Manager.GameManager.Player.Points;
            float nextMoveMass = 0;
            int gameLengthPrediction = GameLengthPrediction();
            float averageMassPerRoundStatic = (_averageMassPerRound + (TotalMass / gameLengthPrediction)) / 2;              // Get average Mass for rounds only account TotalMass
            float averageMassPerRoundDynamic = ((TotalMassLeft / _maxRounds) + (TotalMass / gameLengthPrediction)) / 2;     // Get average Mass for rounds and account TotalMassLeft
            nextMoveMass = (averageMassPerRoundStatic + averageMassPerRoundDynamic) / 2;                                    // Get average of Static and Dynamic Mass average

            if (DebugMode >= 2)
            {
                Debug.Log("Base Move Mass: " + nextMoveMass);
                Debug.Log("Average Mass Per Round Static: " + averageMassPerRoundStatic);
                Debug.Log("Average Mass Per Round Dynamic: " + averageMassPerRoundDynamic);
            }

            if (Bluff())
            {
                if(DebugMode >= 1) Debug.Log("Bluff");
                nextMoveMass *= Random.Range(MinBluffMassMargin, MaxBluffMassMargin);
            }
            else
            {
                if(DebugMode >= 1) Debug.Log("No Bluff");
                nextMoveMass *= Random.Range(MinNoneBluffMassMultiplier, MaxNoneBluffMassMultiplier);
            }

            if (_roundIndex + 1 == _maxRounds)
            {
                if(DebugMode >= 1) Debug.Log("All in");
                nextMoveMass = TotalMassLeft;
            }

            if (nextMoveMass > TotalMassLeft) nextMoveMass = TotalMassLeft;

            TotalMassLeft -= nextMoveMass;

            if (DebugMode >= 1)
            {
                Debug.Log("Move Mass: " + nextMoveMass + "\nMass Left: " + TotalMassLeft);
                Debug.Log("\n--------------------------------------------------");
            }

            _roundIndex++;
            return nextMoveMass;
        }

        private bool Bluff()
        {
            _currentBluffChance = BluffChance;
            if (_bluffedLastRound) _currentBluffChance -= BluffIncrease + Random.Range(-BluffMargin, BluffMargin);  // Decrease bluff chance based on recent bluffs
            _currentBluffChance += (_botScore - _playerScore) * (BluffIncrease + Random.Range(-BluffMargin, BluffMargin));  // Increase/Dexrease bluff chance based on point advantage/disadvantage
            if (DebugMode >= 1) Debug.Log("Bluff Chance: " + _currentBluffChance);
            return Random.Range(0.0f, 1.0f) < _currentBluffChance;
        }

        private int GameLengthPrediction()
        {
            if (_playerScore > _botScore) return Random.Range(PointsForWin - _playerScore, _maxRounds);
            return Random.Range(PointsForWin - _botScore, _maxRounds);
        }
    }
}
