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
public class DivineVengeance : ZeusAncientRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5M, ValueProp.Unpowered),
    ];

    public override async Task BeforeDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target != Owner.Creature || dealer == null || !props.IsPoweredAttack())
            return;
        Flash();
        await ZeusUtils.DealLightningDamageToAll(choiceContext, Owner.Creature,
            Owner.Creature.CombatState?.HittableEnemies ?? [],
            DynamicVars.Damage.BaseValue);
    }
}