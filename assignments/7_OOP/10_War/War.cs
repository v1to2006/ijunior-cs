namespace OOP.Assignments.War
{
    class SquadFactory
    {
        private SoldierFactory _soldierFactory = new SoldierFactory();

        public Squad Create(string name)
        {
            Squad squad = new Squad(name);

            Dictionary<Soldier, int> soldierCounts = new Dictionary<Soldier, int>
            {
                {new BasicSoldier(), UserUtils.GetRandomNumber(20, 30 + 1)},
                {new PowerfulSoldier(), UserUtils.GetRandomNumber(10, 20 + 1)},
                {new MultiAttackSoldier(), UserUtils.GetRandomNumber(5, 15 + 1)},
                {new OverlappingAttackSoldier(), UserUtils.GetRandomNumber(2, 10 + 1)},
            };

            foreach ((Soldier soldier, int count) in soldierCounts)
            {
                AddSoldiers(squad, soldier, count);
            }

            return squad;
        }

        private void AddSoldiers(Squad squad, Soldier soldier, int count)
        {
            for (int i = 0; i < count; i++)
            {
                squad.AddSoldier(_soldierFactory.Create(soldier));
            }
        }
    }

    class SoldierFactory
    {
        public Soldier Create(Soldier soldier)
        {
            return soldier.Clone();
        }
    }

    class War
    {
        private Squad _squad1;
        private Squad _squad2;

        public War()
        {
            InitSquads();
        }

        public void Run()
        {
            ShowIntro();
            DisplaySquadsInfo();

            while (_squad1.HasSoldiers && _squad2.HasSoldiers)
            {
                PerformAttack(_squad1, _squad2);

                if (_squad2.HasSoldiers)
                {
                    PerformAttack(_squad2, _squad1);
                }
            }

            AnnounceWinner();
        }

        private void InitSquads()
        {
            SquadFactory squadFactory = new SquadFactory();

            _squad1 = squadFactory.Create("Squad 1");
            _squad2 = squadFactory.Create("Squad 2");
        }

        private void PerformAttack(Squad attackingSquad, Squad defendingSquad)
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            Console.Clear();
            Console.WriteLine($"{attackingSquad.Name} attacks\n");
            attackingSquad.Attack(defendingSquad.Soldiers);
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
            Console.WriteLine($"{squad.Name} has {squad.SoldiersCount} alive soldiers");
        }

        private void AnnounceWinner()
        {
            if (_squad1.HasSoldiers && _squad2.HasSoldiers == false)
            {
                Console.WriteLine($"\n{_squad1.Name} won!");
            }
            else
            {
                Console.WriteLine($"\n{_squad2.Name} won!");
            }
        }

        private void ShowIntro()
        {
            Console.WriteLine("War started between Squad 1 and Squad 2!");
            Console.WriteLine("Press any key to start the war...\n");
        }
    }

    class Squad
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        public Squad(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public List<Soldier> Soldiers => new(_soldiers);
        public int SoldiersCount => _soldiers.Count;
        public bool HasSoldiers => SoldiersCount > 0;

        public void Attack(List<Soldier> enemySoldiers)
        {
            foreach (Soldier soldier in _soldiers)
            {
                soldier.Attack(enemySoldiers);
            }
        }

        public void AddSoldier(Soldier soldier)
        {
            _soldiers.Add(soldier);
        }

        public void RemoveDeadSoldiers()
        {
            List<Soldier> updatedSoldiers = new List<Soldier>();

            foreach (Soldier soldier in _soldiers)
            {
                if (soldier.IsAlive)
                {
                    updatedSoldiers.Add(soldier);
                }
            }

            _soldiers = updatedSoldiers;
        }
    }

    abstract class Soldier
    {
        private int _health;
        private int _armor;
        protected int Damage;

        public Soldier()
        {
            _health = 100;
            _armor = 50;
            Damage = 10;
            TargetsCount = 1;
        }

        public int TargetsCount { get; protected set; }
        public bool IsAlive => _health > 0;

        public abstract Soldier Clone();

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

        protected int ClampCountToAliveSoldiers(int count, List<Soldier> soldiers)
        {
            return Math.Min(count, soldiers.Count);
        }

        protected List<Soldier> GetRandomEnemies(int count, List<Soldier> soldiers)
        {
            count = ClampCountToAliveSoldiers(count, soldiers);

            List<Soldier> randomSoldiers = new List<Soldier>();

            for (int i = 0; i < count; i++)
            {
                randomSoldiers.Add(GetRandomEnemy(soldiers));
            }

            return randomSoldiers;
        }

        private Soldier GetRandomEnemy(List<Soldier> soldiers)
        {
            return soldiers[UserUtils.GetRandomNumber(0, soldiers.Count)];
        }
    }

    class BasicSoldier : Soldier
    {
        public override Soldier Clone()
        {
            return new BasicSoldier();
        }

        public override void Attack(List<Soldier> enemySoldiers)
        {
            List<Soldier> targets = GetRandomEnemies(TargetsCount, enemySoldiers);

            foreach (Soldier enemySoldier in targets)
            {
                enemySoldier.TakeDamage(Damage);
            }
        }
    }

    class PowerfulSoldier : Soldier
    {
        private double _damageMultiplier = 2;

        public override Soldier Clone()
        {
            return new PowerfulSoldier();
        }

        public override void Attack(List<Soldier> enemySoldiers)
        {
            List<Soldier> targets = GetRandomEnemies(TargetsCount, enemySoldiers);

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

        public override Soldier Clone()
        {
            return new MultiAttackSoldier();
        }

        public override void Attack(List<Soldier> enemySoldiers)
        {
            List<Soldier> targets = GetUniqueRandomEnemies(TargetsCount, enemySoldiers);

            foreach (Soldier enemySoldier in targets)
            {
                enemySoldier.TakeDamage(Damage);
            }
        }

        private List<Soldier> GetUniqueRandomEnemies(int count, List<Soldier> enemySoldiers)
        {
            count = ClampCountToAliveSoldiers(count, enemySoldiers);

            List<Soldier> tempSoldiers = new List<Soldier>(enemySoldiers);
            List<Soldier> randomUniqueSoldiers = new List<Soldier>();

            for (int i = 0; i < count; i++)
            {
                Soldier randomSoldier = tempSoldiers[UserUtils.GetRandomNumber(0, tempSoldiers.Count)];

                randomUniqueSoldiers.Add(randomSoldier);
                tempSoldiers.Remove(randomSoldier);
            }

            return randomUniqueSoldiers;
        }
    }

    class OverlappingAttackSoldier : Soldier
    {
        public OverlappingAttackSoldier()
        {
            TargetsCount = 5;
        }

        public override Soldier Clone()
        {
            return new OverlappingAttackSoldier();
        }

        public override void Attack(List<Soldier> enemySoldiers)
        {
            List<Soldier> targets = GetRandomEnemies(TargetsCount, enemySoldiers);

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