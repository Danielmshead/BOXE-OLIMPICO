namespace Boxing;

class Round
{

    private readonly Boxer _player;
    private readonly Boxer _opponent;
    private readonly int _round;
    private Stack<Action> _work = new();
    private readonly PlayerAttackStrategy _playerAttackStrategy;
    private readonly OpponentAttackStrategy _opponentAttackStrategy;

    public bool GameEnded { get; private set; }

    public Round(Boxer player, Opponent opponent, int round)
    {
        _player = player;
        _opponent = opponent;
        _round = round;
        _work.Push(ResetPlayers);
        _work.Push(CheckOpponentWin);
        _work.Push(CheckPlayerWin);

        void NotifyGameEnded() => GameEnded = true;
        _playerAttackStrategy = new PlayerAttackStrategy(player, opponent, NotifyGameEnded, _work);
        _opponentAttackStrategy = new OpponentAttackStrategy(opponent, player, NotifyGameEnded, _work);
    }

    public void Start()
    {
        while (_work.Count > 0)
        {
            var action = _work.Pop();
            Thread.Sleep(300);
            action();
        }
    }

    public void CheckOpponentWin()
    {
        if (_opponent.IsWinner)
        {
            Console.WriteLine($"{_opponent} Venceu (Maravilha, {_opponent}).");
            GameEnded = true;
        }
    }

    public void CheckPlayerWin()
    {
        if (_player.IsWinner)
        {
            Console.WriteLine($"{_player}  vitória!!");
            GameEnded = true;
        }
    }

    private void ResetPlayers()
    {
        _player.ResetForNewRound();
        _opponent.ResetForNewRound();
        _work.Push(RoundBegins);
    }

    private void RoundBegins()
    {
        Console.WriteLine();
        Console.WriteLine($"ROUND {_round} COMEÇOU...");
        _work.Push(CheckRoundWinner);
        for (var i = 0; i < 7; i++)
        {
            _work.Push(DecideWhoAttacks);
        }
    }

    private void CheckRoundWinner()
    {
        if (_opponent.DamageTaken > _player.DamageTaken)
        {
            Console.WriteLine($"{_player} VENCEU O ROUND {_round}");
            _player.RecordWin();
        }
        else
        {
            Console.WriteLine($"{_opponent} VENCEU O ROUND {_round}");
            _opponent.RecordWin();
        }
    }

    private void DecideWhoAttacks()
    {
        _work.Push( GameUtils.RollSatisfies(10, x => x > 5) ? _opponentAttackStrategy.Attack : _playerAttackStrategy.Attack );
    }
}
