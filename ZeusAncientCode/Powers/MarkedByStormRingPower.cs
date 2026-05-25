using MegaCrit.Sts2.Core.Entities.Powers;

namespace ZeusAncient.ZeusAncientCode.Powers;

public class MarkedByStormRingPower : ZeusAncientPower
{
    public override PowerType Type => PowerType.None;

    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier;
}