using BaseLib.Abstracts;
using BaseLib.Extensions;
using ZeusAncient.ZeusAncientCode.Extensions;

namespace ZeusAncient.ZeusAncientCode.Enchantments;

public abstract class ZeusEnchantment : CustomEnchantmentModel
{
    protected override string CustomIconPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".EnchantmentImagePath();
}