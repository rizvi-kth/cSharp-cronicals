using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestConsole
{
    class Program
    {

        public interface IAnimal
        {
            void Feed();
        }

        public class Rabbit : IAnimal
        {
            public void Feed()
            {
                Console.WriteLine("Eating greens!");
            }
        }

        public class Lion : IAnimal
        {
            public void Feed()
            {
                Console.WriteLine("Eating meat!");
            }
        }

        public class Area<T> where T : 
            class,   // The T must be a value type
            IAnimal, // The T must be a type inharited from IAnimal
            new()    // The T must have default constractor
        {
            private List<T> _areaAnimals = new List<T>();
            public List<T> AreaAnimals
            {
                get { return _areaAnimals; }
                set { _areaAnimals = value; }
            }

            public void FeedAll()
            {
                foreach (T animal in AreaAnimals)
                {
                    // As Forest has a generic constraint which ensures that 
                    // T must be a type inharited from IAnimal so it should have Feed() method.
                    animal.Feed();
                }
            }

            public void AppearedInForest(T animal)
            {
                if (animal != null)
                {
                    this.AreaAnimals.Add(animal);
                }
            }

            public void FilledInForest()
            {
                for (int i = 0; i < 5; i++)
                {
                    // As Forest has a generic constraint which ensures that 
                    // T must have default constractor so it should have be instantiated like 'new T()' 
                    T newAnimal = new T();
                    this.AreaAnimals.Add(newAnimal);
                }

            }



            public int DisappearedFromForest()
            {
                if (AreaAnimals.Count == 0)
                {
                    return 0;
                }
                else
                {
                    var toBeRemoved = AreaAnimals.Count;
                    AreaAnimals.Clear();
                    return toBeRemoved;
                }
            }
        }

        public class Forest<TArea, TAnimal> 
            where TArea : class
            where TAnimal : IAnimal 
        {
            private readonly Queue<TArea> _animalHabitantsQueue = new Queue<TArea>();
            public Queue<TArea> AnimalHabitantsQueue
            {
                get { return _animalHabitantsQueue; }
            }

            public void PushAnimalHabitants(TArea area)
            {
                AnimalHabitantsQueue.Enqueue(area);
            }


        }


        static void Main(string[] args)
        {

            Area<Rabbit> rabbitArea = new Area<Rabbit>();
            rabbitArea.AppearedInForest(new Rabbit());
            rabbitArea.FilledInForest();
            Console.WriteLine("Rabbit count: {0}", rabbitArea.AreaAnimals.Count);

            Forest<Area<Rabbit>, Rabbit> bayForest = new Forest<Area<Rabbit>, Rabbit>();
            bayForest.PushAnimalHabitants(rabbitArea);


            Area<Lion> lionArea = new Area<Lion>();
            lionArea.FilledInForest();
            Console.WriteLine("Lion count: {0}", lionArea.AreaAnimals.Count);
           

            Forest<Area<Lion>, Lion> savanaForest = new Forest<Area<Lion>, Lion>();
            savanaForest.PushAnimalHabitants(lionArea);



        }


    }
}
