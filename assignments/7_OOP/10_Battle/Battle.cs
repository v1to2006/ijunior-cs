namespace OOP.Assignments.Battle
{
    class Battle
    {
        private Squad _squad1;
        private Squad _squad2;

        public Battle()
        {
            InitSquads();
        }

        public void Execute()
        {
            while (_squad1.HasAliveSoldiers() && _squad2.HasAliveSoldiers())
            {
                PerformAttack(_squad1, _squad2);

                if (_squad2.HasAliveSoldiers() == false)
                    break;

                PerformAttack(_squad2, _squad1);
            }

            AnnounceWinner();
        }

        private void InitSquads()
        {
            int basicSoldiersCount = 25;
            int powerfulSoldiersCount = 15;
            int multiAttackSoldiersCount = 10;
            int overlappingAttackSoldiersCount = 5;

            _squad1 = new Squad("Squad 1");
            _squad2 = new Squad("Squad 2");

            for (int i = 0; i < basicSoldiersCount; i++)
            {
                _squad1.AddSoldier(new BasicSoldier());
                _squad2.AddSoldier(new BasicSoldier());
            }

            for (int i = 0; i < powerfulSoldiersCount; i++)
            {
                _squad1.AddSoldier(new PowerfulSoldier());
                _squad2.AddSoldier(new PowerfulSoldier());
            }

            for (int i = 0; i < multiAttackSoldiersCount; i++)
            {
                _squad1.AddSoldier(new MultiAttackSoldier());
                _squad2.AddSoldier(new MultiAttackSoldier());
            }

            for (int i = 0; i < overlappingAttackSoldiersCount; i++)
            {
                _squad1.AddSoldier(new OverlappingAttackSoldier());
                _squad2.AddSoldier(new OverlappingAttackSoldier());
            }
        }

        private void PerformAttack(Squad attackingSquad, Squad defendingSquad)
        {
            Console.Clear();
            Console.WriteLine($"{attackingSquad.Name} attacks\n");
            attackingSquad.Attack(defendingSquad);
            defendingSquad.RemoveDeadSoldiers();
            DisplaySquadsInfo();
        }

        private void DisplaySquadsInfo()
        {
            DisplaySquadInfo(_squad1);
            DisplaySquadInfo(_squad2);
            Console.ReadKey();
        }

        private void DisplaySquadInfo(Squad squad)
        {
            Console.WriteLine($"{squad.Name} has {squad.GetAliveSoldiersCount()} alive soldiers");
        }

        private void AnnounceWinner()
        {
            if (_squad1.HasAliveSoldiers() && _squad2.HasAliveSoldiers() == false)
            {
                Console.WriteLine($"\n{_squad1.Name} won!");
            }
            else
            {
                Console.WriteLine($"\n{_squad2.Name} won!");
            }
        }
    }

    class Squad
    {
        private List<Soldier> _aliveSoldiers = new List<Soldier>();

        public Squad(string name)
        {
            Name = name;
        }

        public string Name;

        public void Attack(Squad enemySquad)
        {
            foreach (Soldier soldier in _aliveSoldiers)
            {
                soldier.Attack(enemySquad);
            }
        }

        public void AddSoldier(Soldier soldier)
        {
            _aliveSoldiers.Add(soldier);
        }

        public Soldier GetRandomSoldier()
        {
            if (_aliveSoldiers.Count == 0)
            {
                return null;
            }

            return _aliveSoldiers[UserUtils.GetRandomNumber(0, _aliveSoldiers.Count)];
        }

        public List<Soldier> GetRandomSoldiers(int count)
        {
            count = ClampCountToAliveSoldiers(count);

            List<Soldier> randomSoldier = new List<Soldier>();

            for (int i = 0; i < count; i++)
            {
                randomSoldier.Add(_aliveSoldiers[UserUtils.GetRandomNumber(0, _aliveSoldiers.Count)]);
            }

            return randomSoldier;
        }

        public List<Soldier> GetUniqueRandomSoldiers(int count)
        {
            count = ClampCountToAliveSoldiers(count);

            List<Soldier> tempSoldiers = new List<Soldier>(_aliveSoldiers);
            List<Soldier> randomUniqueSoldiers = new List<Soldier>();

            for (int i = 0; i < count; i++)
            {
                Soldier randomSoldier = tempSoldiers[UserUtils.GetRandomNumber(0, tempSoldiers.Count)];

                randomUniqueSoldiers.Add(randomSoldier);
                tempSoldiers.Remove(randomSoldier);
            }

            return randomUniqueSoldiers;
        }

        public void RemoveDeadSoldiers()
        {
            List<Soldier> updatedSoldiers = new List<Soldier>();

            foreach (Soldier soldier in _aliveSoldiers)
            {
                if (soldier.IsAlive)
                {
                    updatedSoldiers.Add(soldier);
                }
            }

            _aliveSoldiers = updatedSoldiers;
        }

        public int GetAliveSoldiersCount()
        {
            return _aliveSoldiers.Count;
        }

        public bool HasAliveSoldiers()
        {
            if (_aliveSoldiers.Count > 0)
            {
                return true;
            }

            return false;
        }

        private int ClampCountToAliveSoldiers(int count)
        {
            return Math.Min(count, GetAliveSoldiersCount());
        }
    }

    abstract class Soldier
    {
        private int _health;
        private int _armor;

        public Soldier()
        {
            _health = 100;
            _armor = 50;
            Damage = 20;
        }

        protected int Damage;
        public bool IsAlive => _health > 0;

        public abstract void Attack(Squad enemySquad);

        public void TakeDamage(int damage)
        {
            if (damage <= 0)
                return;

            const int Scaling = 100;
            float damageReductionPercentage = _armor / (float)(_armor + Scaling);

            int effectiveDamage = (int)(damage * (1 - damageReductionPercentage));

            _health -= effectiveDamage;
        }
    }

    class BasicSoldier : Soldier
    {
        public override void Attack(Squad enemySquad)
        {
            enemySquad.GetRandomSoldier().TakeDamage(Damage);
        }
    }

    class PowerfulSoldier : Soldier
    {
        private double _damageMultiplier = 2;

        public override void Attack(Squad enemySquad)
        {
            enemySquad.GetRandomSoldier().TakeDamage((int)(Damage * _damageMultiplier));
        }
    }

    class MultiAttackSoldier : Soldier
    {
        private int _targetsCount = 3;

        public override void Attack(Squad enemySquad)
        {
            foreach (Soldier enemySoldier in enemySquad.GetUniqueRandomSoldiers(_targetsCount))
            {
                enemySoldier.TakeDamage(Damage);
            }
        }
    }

    class OverlappingAttackSoldier : Soldier
    {
        private int _targetsCount = 5;

        public override void Attack(Squad enemySquad)
        {
            foreach (Soldier enemySoldier in enemySquad.GetRandomSoldiers(_targetsCount))
            {
                enemySoldier.TakeDamage(Damage);
            }
        }
    }

    class UserUtils
    {
        private static Random s_random = new Random();

        public static int GetRandomNumber(int minNumber, int maxNumber)
        {
            return s_random.Next(minNumber, maxNumber);
        }
    }
}