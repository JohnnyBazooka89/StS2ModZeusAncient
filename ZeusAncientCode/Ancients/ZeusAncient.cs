using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using ZeusAncient.ZeusAncientCode.Extensions;

namespace ZeusAncient.ZeusAncientCode.Ancients;

[Pool(typeof(AncientEventModel))]
public class ZeusAncient : CustomAncientModel
{
    public override string CustomScenePath => "zeus.tscn".AncientImagePath();
    public override string CustomMapIconPath => "map_icon.png".AncientImagePath();
    public override string CustomMapIconOutlinePath => "map_icon_outline.png".AncientImagePath();
    public override string CustomRunHistoryIconPath => "run_history_icon.png".AncientImagePath();
    public override string CustomRunHistoryIconOutlinePath => "run_history_icon_outline.png".AncientImagePath();

    protected override OptionPools MakeOptionPools
    {
        get
        {
            List<AncientOption> relics =
            [
                AncientOption<Vajra>(),
                AncientOption<OddlySmoothStone>(),
                AncientOption<DataDisk>()
            ];

            return new OptionPools(
                MakePool(relics.ToArray())
            );
        }
    }

    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 2;
    }
}