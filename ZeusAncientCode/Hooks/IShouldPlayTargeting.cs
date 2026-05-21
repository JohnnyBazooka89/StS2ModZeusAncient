using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;

namespace ZeusAncient.ZeusAncientCode;

public interface IShouldPlayTargeting
{
    bool ShouldPlayTargeting(CardModel card, Creature? cardTarget, AutoPlayType autoPlayType);
}