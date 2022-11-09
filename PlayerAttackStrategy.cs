using static Boxing.GameUtils;
using static System.Console;
namespace Boxing;

public class PlayerAttackStrategy : AttackStrategy
{
    private readonly Boxer _player;

    public PlayerAttackStrategy(Boxer player, Opponent opponent, Action notifyGameEnded, Stack<Action> work)
        : base(opponent, work, notifyGameEnded) => _player = player;

    protected override AttackPunch GetPunch()
    {
        var punch = GameUtils.GetPunch($"{_player}'S PUNCH");
        return new AttackPunch(punch, punch == _player.BestPunch);
    }

    protected override void FullSwing()
    {
        Write($"{_player} SWINGS E ");
        if (Other.Vulnerability == Punch.FullSwing)
        {
            ScoreFullSwing();
        }
        else
        {
            if (RollSatisfies(30, x => x < 10))
            {
                ScoreFullSwing();
            }
            else
            {
                WriteLine("ERROU");
            }
        }

        void ScoreFullSwing()
        {
            WriteLine("CONECTOU!");
            if (Other.DamageTaken > KnockoutDamageThreshold)
            {
                Work.Push(() => RegisterKnockout($"{Other} FOI NOCAUTEADO E {_player} É O VENCEDOR!"));
            }
            Other.DamageTaken += 15;
        }
    }

    protected override void Uppercut()
    {
        Write($"{_player} TENTOU UM UPPERCUT ");
        if (Other.Vulnerability == Punch.Uppercut)
        {
            ScoreUpperCut();
        }
        else
        {
            if (RollSatisfies(100, x => x < 51))
            {
                ScoreUpperCut();
            }
            else
            {
                WriteLine("BLOQUEADO");
            }
        }

        void ScoreUpperCut()
        {
            WriteLine("E ELE CONECTOU!");
            Other.DamageTaken += 4;
        }
    }

    protected override void Hook()
    {
        Write($"{_player} DEU UM HOOK... ");
        if (Other.Vulnerability == Punch.Hook)
        {
            ScoreHookOnOpponent();
        }
        else
        {
            if (RollSatisfies(2, x => x == 1))
            {
                WriteLine("MAS FOI  BLOQUEADO!!!!!!!!!!!!!");
            }
            else
            {
                ScoreHookOnOpponent();
            }
        }

        void ScoreHookOnOpponent()
        {
            WriteLine("CONECTOU...");
            Other.DamageTaken += 7;
        }
    }

    protected override void Jab()
    {
        WriteLine($"{_player} JABS NA CABEÇA DO {Other}");
        if (Other.Vulnerability == Punch.Jab)
        {
            ScoreJabOnOpponent();
        }
        else
        {
            if (RollSatisfies(8, x => x < 4))
            {
                WriteLine("BLOQUEADO!.");
            }
            else
            {
                ScoreJabOnOpponent();
            }
        }

        void ScoreJabOnOpponent() => Other.DamageTaken += 3;
    }
}
