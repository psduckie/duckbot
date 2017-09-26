using System;
using System.Collections.Generic;
using System.Text;

namespace Duckbot
{
    class BattleEntity
    {
        private string entityName;
        private int initiative;
        private int totalHealth;
        private int currentHealth;

        public string EntityName
        {
            get
            {
                return entityName;
            }
            set
            {
                entityName = value;
            }
        }
        public int Initiative
        {
            get
            {
                return initiative;
            }
            set
            {
                initiative = value;
            }
        }
        public int TotalHealth
        {
            get
            {
                return totalHealth;
            }
            set
            {
                totalHealth = value;
            }
        }
        public int CurrentHealth
        {
            get
            {
                return currentHealth;
            }
            set
            {
                currentHealth = value;
            }
        }

        public BattleEntity(string name, int health)
        {
            entityName = name;
            totalHealth = health;
            currentHealth = health;

            var rnd = new Random();
            initiative = rnd.Next(1001);
        }

        public int Damage(int amtDamage)
        {
            if (amtDamage > currentHealth)
                currentHealth = 0;
            else
                currentHealth -= amtDamage;
            return currentHealth;
        }
    }
}
