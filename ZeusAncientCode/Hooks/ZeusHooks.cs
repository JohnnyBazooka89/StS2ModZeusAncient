using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace ZeusAncient.ZeusAncientCode;

public class ZeusHooks
{
    public static bool ShouldPlayTargeting(
        IRunState? runState,
        ICombatState? combatState,
        CardModel card,
        Creature? cardTarget,
        AutoPlayType autoPlayType,
        out AbstractModel? preventer)
    {
        preventer = null;

        foreach (AbstractModel model in runState?.IterateHookListeners(combatState) ?? [])
        {
            if (model is not IShouldPlayTargeting shouldPlay)
                continue;

            if (shouldPlay.ShouldPlayTargeting(card, cardTarget))
                continue;

            preventer = model;
            return false;
        }

        return true;
    }
}