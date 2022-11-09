using Boxing;
using static Boxing.GameUtils;
using static System.Console;

WriteLine(new string('\t', 33) + "BOXING");
WriteLine("{0}{0}{0}BOXING ESTILO OLIMPICO (3 ROUNDS -- 2 DE 3 VENCE){0}", Environment.NewLine);

var opponent = new Opponent();
opponent.SetName("Qual o nome do seu oponente?");
var player = new Boxer();
player.SetName("Qual o nome do seu lutador?");

PrintPunchDescription();
player.BestPunch = GetPunch("Qual a melhor habilidade doi seu lutador?");
player.Vulnerability = GetPunch("Qual a sua vulnerabilidade?");
opponent.SetRandomPunches();
WriteLine($"{opponent}'S vantegem é {opponent.BestPunch.ToFriendlyString()} e a vunerabilidade é segredo.");


for (var i = 1; i <= 3; i ++)
{
    var round = new Round(player, opponent, i);
    round.Start();
    round.CheckOpponentWin();
    round.CheckPlayerWin();
    if (round.GameEnded) break;
}
WriteLine("{0}{0}ADEUS.{0}", Environment.NewLine);
