using Content.Shared.EntityEffects;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using System;

namespace Content.Server.EntityEffects.Effects
{
    [DataDefinition]
    public sealed partial class RemoveComponentEffect : EntityEffect
    {
        /// <summary>
        /// The component ID to remove.
        /// </summary>
        [DataField("component", required: true)]
        public string ComponentId = default!;

        protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        {
            return Loc.GetString("reagent-effect-guidebook-remove-component-effect",
                ("chance", Probability),
                ("component", ComponentId));
        }

        public override void Effect(EntityEffectBaseArgs args)
        {
            var entityManager = args.EntityManager;
            var entity = args.TargetEntity;

            var componentType = IoCManager.Resolve<IComponentFactory>().GetRegistration(ComponentId).Type;

            if (entityManager.HasComponent(entity, componentType))
            {
                entityManager.RemoveComponent(entity, componentType);
            }
        }
    }
}
