using Data;

namespace Enemies
{
    public interface IDamagable
    {
        void TakeDamage(int damage, DamageType damageType);
        // добавить дебаффы
    }
}