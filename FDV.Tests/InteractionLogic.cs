namespace FDV.Logic
{
    // Tipos simples para no depender de Unity
    public enum InteractionDecisionType
    {
        None,           // No hace acción (solo hover)
        Interact,       // Interactuable normal
        EnterLocker,    // Entrar al armario
        ExitLocker,     // Salir del armario
        AdvanceTutorial,// Avanzar tutorial (si aplicaba)
        ResetUI         // No hay hit: resetear UI
    }

    public sealed class InteractionContext
    {
        public bool HasHit { get; init; }               // ¿Raycast golpeó algo?
        public bool IsLocker { get; init; }             // ¿El objetivo es un Armario?
        public bool IsInsideLocker { get; init; }       // ¿Ya estamos dentro?
        public string Message { get; init; }            // Texto a mostrar al hover
        public bool InteractionKeyDown { get; init; }   // ¿Se pulsó la tecla (E)?
        public bool TutorialEnabled { get; init; }      // ¿Tutorial activo?
        public bool TutorialNeedsInteract { get; init; }// ¿Paso actual = interactuar?
    }

    public sealed class InteractionDecision
    {
        public InteractionDecisionType Type { get; init; }
        public string UiMessage { get; init; }          // Mensaje para UI (si aplica)
        public bool CrosshairGreen { get; init; }       // ¿Pintar mira en verde?
        public bool ResetUI { get; init; }              // ¿Resetear (no hit)?

        public static InteractionDecision Reset() => new InteractionDecision
        {
            Type = InteractionDecisionType.ResetUI,
            UiMessage = null,
            CrosshairGreen = false,
            ResetUI = true
        };

        public static InteractionDecision Hover(string msg) => new InteractionDecision
        {
            Type = InteractionDecisionType.None,
            UiMessage = msg,
            CrosshairGreen = true,
            ResetUI = false
        };

        public static InteractionDecision Act(InteractionDecisionType t, string msg) => new InteractionDecision
        {
            Type = t,
            UiMessage = msg,
            CrosshairGreen = true,
            ResetUI = false
        };
    }

    public static class InteractionLogic
    {
        // Regla principal: dada la "percepción" (hit, tipo, tecla, tutorial),
        // decide QUÉ hacer y QUÉ mostrar, sin tocar APIs de Unity.
        public static InteractionDecision Decide(InteractionContext c)
        {
            // 1) Sin hit → reset UI
            if (!c.HasHit)
                return InteractionDecision.Reset();

            // 2) Siempre que hay algo interactuable, se muestra hover (verde + msg)
            //    Luego, si hay pulsación, se evalúa acción.
            if (!c.InteractionKeyDown)
                return InteractionDecision.Hover(c.Message);

            // 3) Con pulsación:
            if (c.IsLocker)
            {
                // Entrar o salir del armario según estado
                if (c.IsInsideLocker)
                    return InteractionDecision.Act(InteractionDecisionType.ExitLocker, c.Message);
                else
                    return InteractionDecision.Act(InteractionDecisionType.EnterLocker, c.Message);
            }

            // 4) Interactuable normal
            if (c.TutorialEnabled && c.TutorialNeedsInteract)
                return InteractionDecision.Act(InteractionDecisionType.AdvanceTutorial, c.Message);

            return InteractionDecision.Act(InteractionDecisionType.Interact, c.Message);
        }
    }
}
