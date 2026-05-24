using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using ZeusAncient.ZeusAncientCode.Powers;

namespace ZeusAncient.ZeusAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class StormRing : ZeusAncientRelic, IShouldPlayTargeting
{
    private const string ForcedTargetingCardCountKey = "ForcedTargetingCardCount";

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1),
        new(ForcedTargetingCardCountKey, 2M)
    ];

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.ForEnergy(this)
    ];

    public bool ShouldPlayTargeting(CardModel card, Creature? cardTarget, AutoPlayType autoPlayType)
    {
        if (card.Owner != Owner || cardTarget == null || !IsEnemy(cardTarget))
        {
            return true;
        }

        IReadOnlyList<Creature> markedEnemies = GetMarkedEnemies();

        if (markedEnemies.Count == 0 || !markedEnemies.Any(card.IsValidTarget))
        {
            return true;
        }

        return markedEnemies.Contains(cardTarget);
    }

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        return player != Owner ? amount : amount + DynamicVars.Energy.IntValue;
    }

    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player != Owner)
        {
            return;
        }

        await MarkRandomEnemyForTurn(choiceContext);
    }

    private IReadOnlyList<Creature> GetMarkedEnemies()
    {
        return Owner.Creature.CombatState?.HittableEnemies
            .Where(enemy => enemy.HasPower(ModelDb.GetId<MarkedPower>()))
            .ToList() ?? [];
    }

    private async Task MarkRandomEnemyForTurn(PlayerChoiceContext choiceContext)
    {
        IList<Creature> enemies = Owner.Creature.CombatState?.HittableEnemies
            .ToList() ?? [];

        if (enemies.Count <= 1)
        {
            return;
        }

        Owner.RunState.Rng.CombatTargets.Shuffle(enemies);

        await PowerCmd.Apply<MarkedPower>(choiceContext, enemies[0], DynamicVars[ForcedTargetingCardCountKey].BaseValue,
            Owner.Creature, null);
    }

    private bool IsEnemy(Creature creature)
    {
        return Owner.Creature.CombatState?.Enemies.Contains(creature) ?? false;
    }
}