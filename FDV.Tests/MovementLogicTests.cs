using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FDV.Logic;
using NUnit.Framework;

namespace FDV.Tests
{
    public class MovementLogicTests
    {
        // Helper para comparar Vec3 con tolerancia
        private static void AssertVec3Approximately(Vec3 a, Vec3 b, float eps = 1e-4f)
        {
            Assert.That(a.x, Is.EqualTo(b.x).Within(eps));
            Assert.That(a.y, Is.EqualTo(b.y).Within(eps));
            Assert.That(a.z, Is.EqualTo(b.z).Within(eps));
        }

        [Test]
        public void ComputeHorizontalMove_CalculatesCorrectVector()
        {
            // (Right*1 + Forward*0.5) * (speed*dt = 10*0.02 = 0.2) -> (0.2,0,0.1)
            var result = MovementLogic.ComputeHorizontalMove(
                Vec3.Right, Vec3.Forward,
                axisX: 1f, axisZ: 0.5f,
                speed: 10f, deltaTime: 0.02f);

            AssertVec3Approximately(result, new Vec3(0.2f, 0f, 0.1f));
        }

        [Test]
        public void IntegrateGravity_AccumulatesOverTime()
        {
            float vy = MovementLogic.IntegrateGravity(0f, gravity: -9.81f, deltaTime: 0.02f);
            Assert.That(vy, Is.EqualTo(-0.1962f).Within(1e-4));
        }

        [Test]
        public void SnapToGround_WhenGroundedAndFalling_SetsMinus2()
        {
            float vy = MovementLogic.SnapToGroundIfNeeded(isGrounded: true, velocityY: -0.1f);
            Assert.That(vy, Is.EqualTo(-2f).Within(1e-4));
        }

        [Test]
        public void SnapToGround_WhenNotGrounded_KeepsVelocity()
        {
            float vy = MovementLogic.SnapToGroundIfNeeded(isGrounded: false, velocityY: -3f);
            Assert.That(vy, Is.EqualTo(-3f).Within(1e-4));
        }
    }
}
