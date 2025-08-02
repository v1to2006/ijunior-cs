namespace OOP.Assignments.Battle
{
    enum SoldierType
    {
        Basic,
        Powerful,
        MultiAttack,
        OverlappingAttack
    }

    static class SquadFactory
    {
        public static Squad CreateSquad(string name)
        {
            Squad squad = new Squad(name);

            int basicSoldiersCount = UserUtils.GetRandomNumber(20, 30);
            int powerfulSoldiersCount = UserUtils.GetRandomNumber(10, 20);
            int multiAttackSoldiersCount = UserUtils.GetRandomNumber(5, 15);
            int overlappingAttackSoldiersCount = UserUtils.GetRandomNumber(2, 10);

            AddSoldiers(squad, SoldierType.Basic, basicSoldiersCount);
            AddSoldiers(squad, SoldierType.Powerful, powerfulSoldiersCount);
            AddSoldiers(squad, SoldierType.MultiAttack, multiAttackSoldiersCount);
            AddSoldiers(squad, SoldierType.OverlappingAttack, overlappingAttackSoldiersCount);

            return squad;
        }

        private static void AddSoldiers(Squad squad, SoldierType type, int count)
        {
            for (int i = 0; i < count; i++)
            {
                squad.AddSoldier(SoldierFactory.CreateSoldier(type));
            }
        }
    }

    static class SoldierFactory
    {
        public static Soldier CreateSoldier(SoldierType type)
        {
            switch (type)
            {
                case SoldierType.Basic:
                    return new BasicSoldier();
                case SoldierType.Powerful:
                    return new PowerfulSoldier();
                case SoldierType.MultiAttack:
                    return new MultiAttackSoldier();
                case SoldierType.OverlappingAttack:
                    return new OverlappingAttackSoldier();
                default:
                    throw new ArgumentException("Invalid soldier type");
            }
        }
    }

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
                {
                    break;
                }

                PerformAttack(_squad2, _squad1);
            }

            AnnounceWinner();
        }

        private void InitSquads()
        {
            _squad1 = SquadFactory.CreateSquad("Squad 1");
            _squad2 = SquadFactory.CreateSquad("Squad 2");
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

        public string Name { get; private set; }

        public void Attack(Squad enemySquad)
        {
            foreach (Soldier soldier in _aliveSoldiers)
            {
                List<Soldier> targets;

                switch (soldier)
                {
                    case MultiAttackSoldier multiAttackSoldier:
                        targets = enemySquad.GetUniqueRandomSoldiers(multiAttackSoldier.TargetsCount);
                        multiAttackSoldier.Attack(targets);
                        break;
                    case OverlappingAttackSoldier overlappingAttackSoldier:
                        targets = enemySquad.GetRandomSoldiers(overlappingAttackSoldier.TargetsCount);
                        overlappingAttackSoldier.Attack(targets);
                        break;
                    default:
                        soldier.Attack(enemySquad.GetRandomSoldiers(soldier.TargetsCount));
                        break;
                }
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
                randomSoldier.Add(GetRandomSoldier());
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
            return _aliveSoldiers.Count > 0;
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
            Damage = 10;
            TargetsCount = 1;
        }

        protected int Damage;
        public int TargetsCount { get; protected set; }
        public bool IsAlive => _health > 0;

        public abstract void Attack(List<Soldier> targets);

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
        public override void Attack(List<Soldier> targets)
        {
            foreach (Soldier enemySoldier in targets)
            {
                enemySoldier.TakeDamage(Damage);
            }
        }
    }

    class PowerfulSoldier : Soldier
    {
        private double _damageMultiplier = 2;

        public override void Attack(List<Soldier> targets)
        {
            foreach (Soldier enemySoldier in targets)
            {
                enemySoldier.TakeDamage((int)(Damage * _damageMultiplier));
            }
        }
    }

    class MultiAttackSoldier : Soldier
    {
        public MultiAttackSoldier()
        {
            TargetsCount = 3;
        }

        public override void Attack(List<Soldier> targets)
        {
            foreach (Soldier enemySoldier in targets)
            {
                enemySoldier.TakeDamage(Damage);
            }
        }
    }

    class OverlappingAttackSoldier : Soldier
    {
        public OverlappingAttackSoldier()
        {
            TargetsCount = 5;
        }

        public override void Attack(List<Soldier> targets)
        {
            foreach (Soldier enemySoldier in targets)
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