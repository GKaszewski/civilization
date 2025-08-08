namespace Civilization.Core.Game;

public class TurnManager
{
    private readonly List<Player> _players = [];
    private int _currentIndex = 0;

    public int TurnNumber { get; private set; } = 1;
    public Player CurrentPlayer => _players[_currentIndex];

    public TurnManager(IEnumerable<Player> players)
    {
        _players = players.ToList();
    }

    public void AdvanceTurn()
    {
        _currentIndex++;
        if (_currentIndex < _players.Count) return;
        
        _currentIndex = 0;
        TurnNumber++;
    }
}