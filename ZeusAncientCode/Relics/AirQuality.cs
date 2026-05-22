using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace ZeusAncient.ZeusAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class AirQuality : ZeusAncientRelic
{
    private const int _damageMinimum = 10;
    private const string _damageMinimumKey = "DamageMinimum";
    private const string _damageThresholdKey = "DamageThreshold";
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(_damageMinimumKey, _damageMinimum),
        new(_damageThresholdKey, 9M)
    ];

    public override Decimal ModifyHpLostAfterOstyLate(
        Creature target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        return (dealer != Owner.Creature && dealer != Owner.Osty) || target == Owner.Creature ||
               !props.IsPoweredAttack() || amount < 1M ||
               amount >= DynamicVars[_damageMinimumKey].BaseValue
            ? amount
            : DynamicVars[_damageMinimumKey].BaseValue;
    }

    public override Task AfterModifyingHpLostAfterOsty()
    {
        Flash();
        return Task.CompletedTask;
    }
}