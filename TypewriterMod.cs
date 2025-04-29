using System;
using System.Collections.Generic;
using ExtremelySimpleLogger;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Data;
using MLEM.Data.Content;
using MLEM.Textures;
using TinyLife;
using TinyLife.Actions;
using TinyLife.Mods;
using TinyLife.Objects;
using TinyLife.Utilities;
using TinyLife.World;

namespace TypewriterMod;

public class TypewriterMod : Mod {
    // visual data about this mod
    public override string Name => "Typewriter mod";
    public override string Description => "A small mod to add a Typewriter!";
    public override TextureRegion Icon => this.uiTextures[new Point(0, 0)];
    public override string IssueTrackerUrl => "https://github.com/VincentFoulon80/TinyLifeTypewriterMod/issues";
    public override string TestedVersionRange => "[0.47.0,0.47.4]";

    private Dictionary<Point, TextureRegion> uiTextures;

    public override void Initialize(Logger logger, RawContentManager content, RuntimeTexturePacker texturePacker, ModInfo info) {
        texturePacker.Add(new UniformTextureAtlas(content.Load<Texture2D>("UiTextures"), 8, 8), r => this.uiTextures = r, 1, true, true);
    }

    public override void AddGameContent(GameImpl game, ModInfo info) {
        // The typewriter and pen and paper furniture are using the Computer type but only allowing the "WriteBook" action set
        FurnitureType.Register(new FurnitureType.TypeSettings("TypewriterMod.Typewriter", new Point(1, 1), ObjectCategory.Computer|ObjectCategory.DeskObject, 150, ColorScheme.White) {
            ConstructedType = typeof(Typewriter),
            Icon = this.Icon,
            DefaultRotation = MLEM.Maths.Direction2.Right,
            EfficiencyModifier = 0.6f,
        });

        FurnitureType.Register(new FurnitureType.TypeSettings("TypewriterMod.PenAndPaper", new Point(1, 1), ObjectCategory.Computer|ObjectCategory.DeskObject, 20, ColorScheme.White) {
            ConstructedType = typeof(Typewriter),
            Icon = this.Icon,
            DefaultRotation = MLEM.Maths.Direction2.Right,
            EfficiencyModifier = 0.3f
        });
    }

    public override IEnumerable<string> GetCustomFurnitureTextures(ModInfo info) {
        yield return "Typewriter";
    }
}

public class Typewriter : Furniture {

    public Typewriter(Guid id, FurnitureType type, int[] colors, Map map, Vector2 pos, float floor) : base(id, type, colors, map, pos, floor) {
    }

    // ensures only the "WriteBook" actions are displayed when appropriate
    public override CanExecuteResult CanExecuteAction(ActionType action, ActionInfo info, bool automatic, bool isAuxiliary) {
        if (action.Settings.Name.Contains("WriteBook") || (action.Settings.Name.Contains("Writing") && action.Settings.Name.Contains("Practice"))) {
            return base.CanExecuteAction(action, info, automatic, isAuxiliary);
        }

        return CanExecuteResult.Hidden;
    }

}
