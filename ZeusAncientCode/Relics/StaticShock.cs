using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;
using ZeusAncient.ZeusAncientCode.Utils;

namespace ZeusAncient.ZeusAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class StaticShock : ZeusAncientRelic
{
    private const string PercentDamageKey = "PercentDamage";
    private const string PercentDamageSingleKey = "PercentDamageSingle";

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(PercentDamageKey, 50M)
    ];

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if ((dealer != Owner.Creature && dealer != Owner.Osty) || !props.IsPoweredAttack() ||
            result.UnblockedDamage <= 0)
        {
            return;
        }

        IEnumerable<Creature> targets = Owner.Creature.CombatState.GetOpponentsOf(Owner.Creature)
            .Where(c => c.IsAlive);
        List<Creature> otherTargets = targets.Where(t => t != target).ToList();

        Decimal finalDamage = result.UnblockedDamage * DynamicVars[PercentDamageKey].BaseValue / 100M;

        if (otherTargets.Count >= 1)
        {
            Owner.RunState.Rng.CombatTargets.Shuffle(otherTargets);
            await ZeusUtils.DealLightningDamage(choiceContext, otherTargets[0], finalDamage);
        }
        else
        {
            await ZeusUtils.DealLightningDamage(choiceContext, target, finalDamage);
        }
    }
}