using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ZeusAncient.ZeusAncientCode.Utils;

namespace ZeusAncient.ZeusAncientCode.Powers;

public class BlitzPower : ZeusAncientPower, IHasSecondAmount
{
    private const string UnblockedDamageLeftKey = "UnblockedDamageLeft";
    private const string BlitzDamageIncreaseKey = "BlitzDamageIncrease";
    private const string BlitzDamageKey = "BlitzDamage";
    private const string BlitzDamageBaseKey = BlitzDamageKey + "Base";
    private const string BlitzDamageExtraKey = BlitzDamageKey + "Extra";

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => DynamicVars[UnblockedDamageLeftKey].IntValue;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(UnblockedDamageLeftKey, 15M),
        new(BlitzDamageIncreaseKey, 1M),
        new(BlitzDamageBaseKey, 4M),
        new(BlitzDamageExtraKey, 1M),
        new CustomCalculatedDamageVar(BlitzDamageKey, ValueProp.Unpowered).WithMultiplier(static (power, target) =>
            power.DynamicVars[BlitzDamageBaseKey].BaseValue + power.DynamicVars[BlitzDamageExtraKey].BaseValue *
            power.Owner.GetPowerAmount<HeavenStruckPower>())
    ];

    public string GetSecondAmount()
    {
        return $"{(int)((CustomCalculatedDamageVar)DynamicVars[BlitzDamageKey]).CalculateCustom(Owner)}";
    }

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
            await ZeusUtils.DealLightningDamage(choiceContext, dealer, Owner,
                ((CustomCalculatedDamageVar)DynamicVars[BlitzDamageKey]).CalculateCustom(Owner));
            await PowerCmd.Apply<HeavenStruckPower>(choiceContext, Owner, DynamicVars[BlitzDamageIncreaseKey].IntValue,
                dealer, null);
            foreach (BlitzPower blitzPower in Owner.Powers.OfType<BlitzPower>())
            {
                blitzPower.InvokeSecondAmountChanged();
            }

            await PowerCmd.Remove(this);
        }
    }
}