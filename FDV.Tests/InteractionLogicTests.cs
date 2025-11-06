using NUnit.Framework;
using FDV.Logic; // referencia a tu lógica

namespace FDV.Tests
{
    public class InteractionLogicTests
    {
        [Test]
        public void NoHit_ResetsUI()
        {
            var ctx = new InteractionContext
            {
                HasHit = false
            };

            var dec = InteractionLogic.Decide(ctx);

            Assert.IsTrue(dec.ResetUI);
            Assert.IsFalse(dec.CrosshairGreen);
            Assert.IsNull(dec.UiMessage);
            Assert.AreEqual(InteractionDecisionType.ResetUI, dec.Type);
        }

        [Test]
        public void HoverOnInteractable_ShowsMessageAndGreen_NoAction()
        {
            var ctx = new InteractionContext
            {
                HasHit = true,
                Message = "Pulsa E",
                InteractionKeyDown = false
            };

            var dec = InteractionLogic.Decide(ctx);

            Assert.IsFalse(dec.ResetUI);
            Assert.IsTrue(dec.CrosshairGreen);
            Assert.AreEqual("Pulsa E", dec.UiMessage);
            Assert.AreEqual(InteractionDecisionType.None, dec.Type);
        }

        [Test]
        public void PressKey_OnLocker_EnterLocker()
        {
            var ctx = new InteractionContext
            {
                HasHit = true,
                IsLocker = true,
                IsInsideLocker = false,
                InteractionKeyDown = true,
                Message = "Entrar"
            };

            var dec = InteractionLogic.Decide(ctx);

            Assert.AreEqual(InteractionDecisionType.EnterLocker, dec.Type);
            Assert.AreEqual("Entrar", dec.UiMessage);
            Assert.IsTrue(dec.CrosshairGreen);
        }

        [Test]
        public void PressKey_InLocker_ExitLocker()
        {
            var ctx = new InteractionContext
            {
                HasHit = true,
                IsLocker = true,
                IsInsideLocker = true,
                InteractionKeyDown = true,
                Message = "Salir"
            };

            var dec = InteractionLogic.Decide(ctx);

            Assert.AreEqual(InteractionDecisionType.ExitLocker, dec.Type);
        }

        [Test]
        public void PressKey_OnInteractable_AdvanceTutorial_IfRequired()
        {
            var ctx = new InteractionContext
            {
                HasHit = true,
                IsLocker = false,
                InteractionKeyDown = true,
                TutorialEnabled = true,
                TutorialNeedsInteract = true,
                Message = "Interactuar"
            };

            var dec = InteractionLogic.Decide(ctx);

            Assert.AreEqual(InteractionDecisionType.AdvanceTutorial, dec.Type);
        }

        [Test]
        public void PressKey_OnInteractable_NormalInteract_WhenNoTutorial()
        {
            var ctx = new InteractionContext
            {
                HasHit = true,
                IsLocker = false,
                InteractionKeyDown = true,
                TutorialEnabled = false,
                TutorialNeedsInteract = false,
                Message = "Usar"
            };

            var dec = InteractionLogic.Decide(ctx);

            Assert.AreEqual(InteractionDecisionType.Interact, dec.Type);
        }
    }
}
