using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace ZeusAncient.ZeusAncientCode.Enchantments;

public sealed class DoubleEnchantment : ZeusEnchantment
{
    private const string _timesKey = "Times";

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar(_timesKey, 1M)
    ];

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.ReplayDynamic, DynamicVars[_timesKey])
    ];

    public override bool CanEnchant(CardModel c)
    {
        if (!base.CanEnchant(c))
            return false;
        return c.Tags.Contains(CardTag.Strike);
    }

    public override int EnchantPlayCount(int originalPlayCount)
    {
        return originalPlayCount + DynamicVars[_timesKey].IntValue;
    }
}