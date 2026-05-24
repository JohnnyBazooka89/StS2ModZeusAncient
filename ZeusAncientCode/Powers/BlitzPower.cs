using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ZeusAncient.ZeusAncientCode.Utils;

namespace ZeusAncient.ZeusAncientCode.Powers;

public class BlitzPower : ZeusAncientPower
{
    private const string UnblockedDamageLeftKey = "UnblockedDamageLeft";

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => DynamicVars[UnblockedDamageLeftKey].IntValue;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(UnblockedDamageLeftKey, 20M),
        new DamageVar(5, ValueProp.Unpowered)
    ];

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if (target != Owner || !props.IsPoweredAttack())
        {
            return;
        }

        DynamicVars[UnblockedDamageLeftKey].BaseValue -=
            Math.Min(DynamicVars[UnblockedDamageLeftKey].BaseValue, result.UnblockedDamage);
        InvokeDisplayAmountChanged();
        if (DynamicVars[UnblockedDamageLeftKey].IntValue <= 0)
        {
            await ZeusUtils.DealLightningDamage(choiceContext, dealer, Owner, DynamicVars.Damage.BaseValue);
            await PowerCmd.Remove(this);
        }
    }
}