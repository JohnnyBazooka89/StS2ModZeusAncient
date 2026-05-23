using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using ZeusAncient.ZeusAncientCode.Enchantments;

namespace ZeusAncient.ZeusAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class ThunderRush : ZeusAncientRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new RepeatVar(3)
    ];

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        ..HoverTipFactory.FromEnchantment<Thundercall>()
    ];

    public override Task AfterObtained()
    {
        foreach (CardModel card in (IEnumerable<CardModel>)PileType.Deck.GetPile(Owner).Cards.ToList())
        {
            if (ModelDb.Enchantment<Thundercall>().CanEnchant(card))
            {
                CardCmd.Enchant<Thundercall>(card, 1M);
                NCardEnchantVfx child = NCardEnchantVfx.Create(card);
                if (child != null)
                {
                    NRun instance = NRun.Instance;
                    if (instance != null)
                        instance.GlobalUi.CardPreviewContainer.AddChildSafely(child);
                }
            }
        }

        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature) || Owner.PlayerCombatState.TurnNumber > 1)
            return;
        Flash();
        await OrbCmd.AddSlots(Owner, DynamicVars.Repeat.IntValue);
    }
}