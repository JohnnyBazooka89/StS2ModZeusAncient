using BaseLib.Abstracts;
using BaseLib.Extensions;
using ZeusAncient.ZeusAncientCode.Extensions;

namespace ZeusAncient.ZeusAncientCode.Relics;

/// <summary>
/// This is the base class for your mod's relics, which is set up to load the relic's images from your mod's resources.
/// When creating a relic, right click the Relics folder and create a new file with the Custom Relic template.
/// This will generate a class that extends this one.
/// You can also just create the class manually; just make sure to inherit from this class.
/// </summary>
public abstract class ZeusAncientRelic : CustomRelicModel
{
    //ZeusAncient/images/relics
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
    public override string PackedIconOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicOutlineImagePath();
    public override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}