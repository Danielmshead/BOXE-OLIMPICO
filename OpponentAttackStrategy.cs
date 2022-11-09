using static Boxing.GameUtils;
using static System.Console;

namespace Boxing;

public class OpponentAttackStrategy : AttackStrategy
{
    private readonly Opponent _opponent;

    public OpponentAttackStrategy(Opponent opponent, Boxer player,  Action notifyGameEnded, Stack<Action> work) : base(player, work, notifyGameEnded)
    {
        _opponent = opponent;
    }

    protected override AttackPunch GetPunch()
    {
        var punch = (Punch)Roll(4);
        return new AttackPunch(punch, punch == _opponent.BestPunch);
    }

    protected override void FullSwing()
    {
        Write($"{_opponent}  TOMOU UM FULL SWING E");
        if (Other.Vulnerability == Punch.FullSwing)
        {
            ScoreFullSwing();
        }
        else
        {
            if (RollSatisfies(60, x => x < 30))
            {
                WriteLine(" FOI BLOQUEADO!");
            }
            else
            {
                ScoreFullSwing();
            }
        }

        void ScoreFullSwing()
        {
            WriteLine(" POW!!!!! ELE ACERTOU BEM NA CARA!");
            if (Other.DamageTaken > KnockoutDamageThreshold)
            {
                Work.Push(RegisterOtherKnockedOut);
            }
            Other.DamageTaken += 15;
        }
    }

    protected override void Hook()
    {
        Write($"{_opponent} TOMOU {Other} NO QUEIXO (OUCH!)");
        Other.DamageTaken += 7;
        WriteLine("....E NOVAMENTE!");
        Other.DamageTaken += 5;
        if (Other.DamageTaken > KnockoutDamageThreshold)
        {
            Work.Push(RegisterOtherKnockedOut);
        }
    }

    protected override void Uppercut()
    {
        Write($"{Other} FOI ATACADO COM UM UPPERCUT (OH,OH)...");
        if (Other.Vulnerability == Punch.Uppercut)
        {
            ScoreUppercut();
        }
        else
        {
            if (RollSatisfies(200, x => x > 75))
            {
                WriteLine($" BLOQUEOU E ACERTOU O {_opponent} COM UM HOOK.");
                _opponent.DamageTaken += 5;
            }
            else
            {
                ScoreUppercut();
            }
        }

        void ScoreUppercut()
        {
            WriteLine($"E {_opponent} CONECTOU...");
            Other.DamageTaken += 8;
        }
    }

    protected override void Jab()
    {
        Write($"{_opponent}  JABS E ");
        if (Other.Vulnerability == Punch.Jab)
        {
            ScoreJab();
        }
        else
        {
            if (RollSatisfies(7, x => x > 4))
            {
                WriteLine("CUSPIU SANGUE !!!");
                ScoreJab();
            }
            else
            {
                WriteLine("BLOQUEADO!");
            }
        }

        void ScoreJab() => Other.DamageTaken += 5;
    }

    private void RegisterOtherKnockedOut()
        => RegisterKnockout($"{Other} FOI NOCAUTEADO {_opponent} É O VITORIOSO!");
}
