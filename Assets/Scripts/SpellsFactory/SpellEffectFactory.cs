using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SpellsFactory
{
    public static class SpellEffectFactory
    {
        //ПЕРЕДЕЛАТЬ ФАБРИКУ ПОД БАФФЫ/ДЕБАФФЫ
        //ЛОГИКУ РАЗДЛЕЕНИЯ ТИПОВ СПЕЛЛОВ ВЫНЕСТИ В SCRIPTABLE OBJECT
        
        private static Dictionary<string, Type> _spellEffects;
        private static bool IsInitialized => _spellEffects != null;

        private static void InitializeFactory()
        {
            if (IsInitialized) return;
            
            var spellTypes = Assembly.GetAssembly(typeof(SpellEffect)).GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(SpellEffect)));

            _spellEffects = new Dictionary<string, Type>();

            foreach (var spellType in spellTypes)
            {
                if (Activator.CreateInstance(spellType) is SpellEffect temp)
                    _spellEffects.Add(temp.Name, spellType);
            }
        }
        
        public static SpellEffect GetSpellEffect(string spellName)
        {
            InitializeFactory();

            if (!_spellEffects.ContainsKey(spellName)) return null;
            
            var type = _spellEffects[spellName];
            var spellEffect = Activator.CreateInstance(type) as SpellEffect;

            return spellEffect;

        }
    }
}