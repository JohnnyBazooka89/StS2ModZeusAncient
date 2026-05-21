using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace ZeusAncient.ZeusAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class StormRing : ZeusAncientRelic, IShouldPlayTargeting
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private Creature? CreatureToTarget { get; set; }

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1)
    ];

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.ForEnergy(this)
    ];

    public bool ShouldPlayTargeting(CardModel card, Creature? cardTarget, AutoPlayType autoPlayType)
    {
        if (card.Owner != Owner)
        {
            return true;
        }

        if (cardTarget == null)
        {
            return true;
        }

        if (!(Owner.Creature.CombatState?.Enemies.Contains(cardTarget) ?? false))
        {
            return true;
        }

        Creature? lockedTarget = CreatureToTarget;

        if (lockedTarget == null)
        {
            return true;
        }

        if (!card.IsValidTarget(lockedTarget))
        {
            return true;
        }

        return cardTarget == lockedTarget;
    }

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        return player != Owner ? amount : amount + DynamicVars.Energy.IntValue;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null && (Owner.Creature.CombatState?.Enemies.Contains(cardPlay.Target) ?? false))
        {
            CreatureToTarget = cardPlay.Target;
        }

        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        CreatureToTarget = null;
        return Task.CompletedTask;
    }
}