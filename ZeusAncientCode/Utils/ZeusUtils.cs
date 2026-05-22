using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace ZeusAncient.ZeusAncientCode.Utils;

public class ZeusUtils
{
    public static async Task DealLightningDamage(PlayerChoiceContext choiceContext, Creature owner, Decimal damage)
    {
        await Cmd.Wait(0.25f);
        await CreatureCmd.Damage(choiceContext, owner, damage, ValueProp.Unpowered, owner);
        VfxCmd.PlayOnCreature(owner, VfxCmd.lightningPath);
    }
}