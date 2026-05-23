using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;
using ZeusAncient.ZeusAncientCode.Utils;

namespace ZeusAncient.ZeusAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class PowerSurge : ZeusAncientRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(4M, ValueProp.Unpowered)
    ];

    public override async Task BeforeSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        Decimal finalDamage = DynamicVars.Damage.BaseValue * Owner.PlayerCombatState?.TurnNumber ?? 0;
        if (!participants.Contains(Owner.Creature) || Owner.PlayerCombatState?.Energy <= 0 || finalDamage <= 0)
            return;

        await ZeusUtils.DealLightningDamageToAll(choiceContext, Owner.Creature,
            Owner.Creature.CombatState?.HittableEnemies ?? [], finalDamage);
    }
}