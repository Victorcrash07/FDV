
namespace FDV.Logic
{
    // Similar a Vector3, pero solo para tests (sin Unity)
    public struct Vec3
    {
        public float x, y, z;

        public Vec3(float x, float y, float z)
        { this.x = x; this.y = y; this.z = z; }

        // Utilidades tipo "Vector3.right/forward"
        public static readonly Vec3 Right = new Vec3(1f, 0f, 0f);
        public static readonly Vec3 Forward = new Vec3(0f, 0f, 1f);

        // Operadores básicos
        public static Vec3 operator +(Vec3 a, Vec3 b)
            => new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);

        public static Vec3 operator *(Vec3 v, float s)
            => new Vec3(v.x * s, v.y * s, v.z * s);
    }

    // Clase sin MonoBehaviour para poder testear sin Unity
    public static class MovementLogic
    {
        // Calcula el movimiento horizontal del jugador
        public static Vec3 ComputeHorizontalMove(
            Vec3 right, Vec3 forward,
            float axisX, float axisZ,
            float speed, float deltaTime)
        {
            var move = right * axisX + forward * axisZ;
            return move * (speed * deltaTime);
        }

        // Aplica gravedad acumulada
        public static float IntegrateGravity(float velocityY, float gravity, float deltaTime)
        {
            return velocityY + gravity * deltaTime;
        }

        // Si está tocando suelo y bajando, lo "pega" al suelo
        public static float SnapToGroundIfNeeded(bool isGrounded, float velocityY, float snap = -2f)
        {
            if (isGrounded && velocityY < 0f)
                return snap;
            return velocityY;
        }
    }
}
