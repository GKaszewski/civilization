using Civilization.Core.Actions;
using Civilization.Core.Interfaces;
using Civilization.Core.Units;

namespace Civilization.Core.Game;

public class GameState(GameMap map, List<Player> players)
{
    private readonly Dictionary<Guid, Unit> _units = new();
    private readonly Dictionary<Guid, City> _cities = new();
    
    public GameMap Map { get; } = map;
    public List<Player> Players { get; } = players;
    public TurnManager TurnManager { get; } = new(players);
    public ActionQueue ActionQueue { get; } = new();
    
    public IEnumerable<Unit> Units => _units.Values;
    public Player CurrentPlayer => TurnManager.CurrentPlayer;
    
    public IEnumerable<City> Cities => _cities.Values;
    
    public void NextTurn()
    {
        foreach (var listener in GetAllTurnListeners(CurrentPlayer.Id)) listener.OnTurnEnd(this);
        
        TurnManager.AdvanceTurn();
        
        foreach (var listener in GetAllTurnListeners(CurrentPlayer.Id)) listener.OnTurnStart(this);
        
        ActionQueue.ExecuteAll(new GameActionContext(this));
    }
    
    public void AddUnit(Unit unit) => _units.Add(unit.Id, unit);
    
    public Unit? FindUnit(Guid id) => _units.GetValueOrDefault(id);
    
    public IEnumerable<Unit> GetUnitsForPlayer(int playerId) => _units.Values.Where(u => u.OwnerId == playerId);
    
    public void RemoveUnit(Guid id) {
        // Later, let's check if player can remove this unit (e.g. his turn, unit is not in combat, etc.)
        _units.Remove(id);
    }
    
    public void AddCity(City city) => _cities.Add(city.Id, city);
    
    public City? FindCity(Guid id) => _cities.GetValueOrDefault(id);
    
    public IEnumerable<City> GetCitiesForPlayer(int playerId) 
        => _cities.Values.Where(c => c.OwnerId == playerId);

    private IEnumerable<IOnTurnListener> GetAllTurnListeners(int playerId)
    {
        foreach (var unit in GetUnitsForPlayer(playerId)) yield return unit;
        foreach (var city in GetCitiesForPlayer(playerId)) yield return city;
    }
}