using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MHeuristicProject
{
    class Program
    {
        // replace all the paths below with the proper values for you:
        
        //string path_sales_man = "C:\\Users\\Manuel\\Desktop\\MH\\Proyecto\\tsp_pr76.dat";
        static void Main(string[] args)
        {
            #region Probador de los continuos
            //int k = 0;
            //while (true)
            //{
            // Console.WriteLine(ReadingSalesMan("C:\\Users\\Manuel\\Desktop\\MH\\Proyecto\\tsp_pr76.dat"));
            //Console.WriteLine("INIT:");
            //double[] values = BeeColony(30);
            //Console.WriteLine("-------------------------");
            //double x = FunctionContinuosI(values);
            //foreach (var item in values.ToList())
            //    Console.WriteLine("Punto: " + item);
            //Console.WriteLine();
            //Console.WriteLine(FunctionContinuosI(values));

            //Console.WriteLine();
            //foreach (var item in values.ToList())
            //    Console.WriteLine("Punto: " + item);
            //Console.WriteLine("-------------------------");
            //double x = FunctionContinuosII(values);
            //Console.WriteLine("Solution "+FunctionContinuosII(values));
            //Console.WriteLine("END");
            //#region PRINTPATH

            //StreamWriter write = new StreamWriter("C:\\Users\\Arlet\\Desktop\\manu metaheuristica\\text"+k +".txt");
            //k++;
            //write.WriteLine(x.ToString());
            //Console.WriteLine();
            //foreach (var item in values.ToList())
            //{
            //    write.WriteLine(item.ToString());
            //}

            //write.Close();
            //#endregion
            //Console.ReadKey();

            //}

            #endregion

            //ReadingSalesMan("E:\\university\\4to\\Metaheuristica\\Proyecto\\tsp_pr76.dat");
            int k = 0;

            while (true)
            {
                // replace your path here (when you want to write the output)
                StreamWriter write = new StreamWriter("C:\\Users\\Manuel\\Desktop\\manu metaheuristica informe\\text" + k +".txt");
                k++;

                double better = double.MaxValue;
                List<double> result = GeneticSolution(30, 20, ref better);
                Console.WriteLine("value :" + better);
                write.WriteLine("value :" + better);

                foreach (var item in result)
                {
                    Console.WriteLine(item);
                    Console.WriteLine("====================================================");
                    write.WriteLine(item.ToString());
                    write.WriteLine("====================================================");
                }

                write.Close();

                Console.ReadKey();
            }

        }

        #region Continuos Function II

        private static double FunctionContinuosI(double[] vector)
        {
            double beesum = 0, beesum2 = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                beesum += Math.Cos(2 * Math.PI * vector[i]);
                beesum2 += Math.Pow(vector[i], 2) -
                10 * Math.Cos(2 * Math.PI * vector[i]);
            }
            return Math.Pow(beesum2, vector.Length) + beesum;
        }

        private static double FunctionContinuosII(double[] vector)
        {
            double beesum = 1, beesum2 = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                beesum *= Math.Sin(Math.Pow(vector[i], 2));
                beesum2 += Math.Pow(vector[i], 2) -
                10 * Math.Cos(2 * Math.PI * vector[i]);
            }
            return beesum2 + beesum;
        }

        #region Genetic Algorithm
        static List<double> GeneticSolution(int dimentions, int populations, ref double better)
        {
            Cromosoma[] population = new Cromosoma[populations];
            List<double> path = new List<double>();
            double max = double.MinValue;
            double min = double.MaxValue;
            for (int i = 0; i < populations; i++)
            {
                population[i] = new Cromosoma(dimentions);
                //population[i].value = FunctionContinuosI(population[i].population);
                population[i].value = FunctionContinuosII(population[i].population);
                max = population[i].value > max ? population[i].value : max;
                min = population[i].value < min ? population[i].value : min;
            }

            for (int i = 0; i < population.Length; i++)
            {
                population[i].prob = (max - population[i].value) / (max - min);
            }

            int count = 0;
            Array.Sort(population);
            while (count++ < 1000)
            {
                Console.WriteLine("Hola " + count);
                //seleccion:
                List<Cromosoma> selected = new List<Cromosoma>();
                int temp = 0;
                while (temp++ < 50)
                {
                    double p = r.NextDouble();
                    double acum = 0;
                    for (int i = 0; i < population.Length; i++)
                    {
                        acum += population[i].prob;
                        if (!population[i].have_partner && acum >= p)
                        {
                            selected.Add(population[i]);
                            population[i].have_partner = true;
                        }
                    }
                }

                for (int i = 0; i < selected.Count - 1; i++)
                {
                    Make_Crossover(selected[i].population, selected[i + 1].population);
                    Make_Mutation(selected[i].population);
                }
                Make_Mutation(selected[selected.Count - 1].population);
                population = new Cromosoma[selected.Count];
                for (int i = 0; i < selected.Count; i++)
                {
                    population[i] = new Cromosoma(dimentions);
                    selected[i].population.CopyTo(population[i].population, 0);
                    //population[i].value = FunctionContinuosI(population[i].population);
                    population[i].value = FunctionContinuosII(population[i].population);
                    max = population[i].value > max ? population[i].value : max;
                    min = population[i].value < min ? population[i].value : min;

                }

                for (int i = 0; i < population.Length; i++)
                {
                    population[i].prob = (max - population[i].value) / (max - min);
                }

                Array.Sort(population);

                if (population[0].value < better)
                {
                    path = new List<double>();
                    better = population[0].value;
                    for (int i = 0; i < population[0].population.Length; i++)
                    {
                        path.Add(population[0].population[i]);

                    }

                }
            }
            return path;
        }

        private static void Make_Mutation(double[] population)
        {
            int k = cross.Next(0, population.Length);
            int j = cross.Next(0, population.Length);
            population[k] = r.NextDouble() * population[k];
            population[j] = r.NextDouble();
            return;
            throw new NotImplementedException();
        }
        static Random cross = new Random();
        private static void Make_Crossover(double[] population1, double[] population2)
        {
            int k = cross.Next(0, population1.Length);
            for (int i = k; i < population1.Length; i++)
            {
                double temp = population1[i];
                population1[i] = population2[i];
                population2[i] = temp;
            }

            return;
            throw new NotImplementedException();
        }

        class Cromosoma : IComparable<Cromosoma>
        {
            public double[] population { get; set; }
            public double value { get; set; }
            public bool have_partner { get; set; }
            public int index { get; set; }

            public double prob { get; set; }
            public Cromosoma(int dimentions)
            {
                this.value = 0;
                this.prob = 0;
                this.index = -1;
                this.population = new double[dimentions];
                this.have_partner = false;
                for (int i = 0; i < population.Length; i++)
                    population[i] = r.NextDouble();
            }

            public int CompareTo(Cromosoma other)
            {
                return this.value.CompareTo(other.value);
            }
        }
        #endregion

        #region Bee Colony

        static Random r = new Random();

        static double[] BeeColony(int dimension)
        {
            double[] result = new double[dimension];
            double better = double.MaxValue;
            int count = 0;
            Bee[] employes = new Bee[50];
            int onlookers = 50;
            for (int i = 0; i < employes.Length; i++)
            {
                employes[i] = new Bee(dimension, 50);
                // employes[i].value_solution  = FunctionContinuosI(employes[i].associate_solution);
                employes[i].value_solution = FunctionContinuosII(employes[i].associate_solution);
            }
            for (int i = 0; i < result.Length; i++)
                result[i] = r.NextDouble();

            while (count++ <= 1000)
            {
                Console.WriteLine("Hola " + count);
                double[] vector = new double[dimension];
                for (int i = 0; i < vector.Length; i++)
                {
                    vector[i] = r.NextDouble();
                }
                for (int j = 0; j < employes.Length; j++)
                {
                    for (int i = 0; i < employes[j].associate_solution.Length; i++)
                    {
                        vector[i] = employes[j].associate_solution[i] + r.NextDouble() * r.Next(-1, 2) *
                            (employes[j].associate_solution[i] - employes[r.Next(0, employes.Length)].associate_solution[i]);
                    }
                    //double value_vector = FunctionContinuosI(vector);
                    double value_vector = FunctionContinuosII(vector);
                    if (value_vector < employes[j].value_solution || employes[j].count >= employes[j].maxrep)
                    {
                        vector.CopyTo(employes[j].associate_solution, 0);
                        employes[j].value_solution = value_vector;
                        employes[j].count = -1;
                    }
                    employes[j].count += 1;
                }
                Array.Sort(employes);
                bool[] takedsolutions = new bool[employes.Length];
                double prob = 0.5;
                for (int i = 0; i < onlookers; i++)
                {
                    double p = r.NextDouble();
                    double acumulator = 0;
                    int pot = 0;
                    for (int j = 0; j < employes.Length; j++)
                    {
                        if (!takedsolutions[j])
                        {
                            acumulator += prob / Math.Pow(2, pot);
                            pot++;
                        }
                        if (acumulator >= p)
                        {
                            takedsolutions[j] = true;
                            if (employes[j].value_solution < better)
                            {
                                better = employes[j].value_solution;
                                employes[j].associate_solution.CopyTo(result, 0);
                            }
                            break;
                        }
                    }
                }
            }

            return result;
        }

        class Bee : IComparable<Bee>
        {
            public double[] associate_solution { get; set; }

            public int count { get; set; }

            public int maxrep { get; set; }

            public double value_solution { get; set; }

            public Bee(int dimentions, int rep)
            {
                this.associate_solution = new double[dimentions];
                this.maxrep = rep;
                this.count = 0;
                for (int i = 0; i < associate_solution.Length; i++)
                    associate_solution[i] = r.NextDouble();
            }

            int IComparable<Bee>.CompareTo(Bee other)
            {
                return this.value_solution.CompareTo(other.value_solution);
            }
        }
        #endregion

        #endregion

        #region Sales Man
        static Random s = new Random();
        static double ReadingSalesMan(string path)
        {
            StreamReader reader = new StreamReader(path);
            string line = "";
            int count = 0;
            for (int i = 0; i < 6; i++)
            {
                if ((line = reader.ReadLine()).Contains("DIMENSION"))
                    count = int.Parse(line.Split()[2]);
            }
            List<Tuple<int, double, double>> result = new List<Tuple<int, double, double>>();
            int last = 0;
            while ((line = reader.ReadLine()) != "EOF")
            {
                string[] values = line.Split();
                Tuple<int, double, double> temp = new Tuple<int, double, double>(
                    int.Parse(values[0]), double.Parse(values[1]), double.Parse(values[2]));
                result.Add(temp);
                last = int.Parse(values[0]);
            }

            double[,] costs = new double[result.Count, result.Count];
            for (int i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < result.Count; j++)
                {
                    costs[i, j] = Math.Sqrt(Math.Pow(result[i].Item2 - result[j].Item2, 2) +
                        Math.Pow(result[i].Item3 - result[j].Item3, 2));
                }
            }
            List<Tuple<int, int>> final = new List<Tuple<int, int>>();
            Console.WriteLine("Greedy:");
            #region Print
            //StreamWriter write = new StreamWriter("C:\\Users\\Arlet\\Desktop\\manu metaheuristica\\textTSP.txt");
            ////k++;
            //double x = GreedySalesMan(result, ref final);
            //write.WriteLine(x.ToString());
            //Console.WriteLine();
            //foreach (var item in final.ToList())
            //{
            //    write.WriteLine(item.ToString());
            //}

            //write.Close();
            #endregion

            Console.WriteLine("ANTS");
            #region Print
            StreamWriter wrote = new StreamWriter("C:\\Users\\Arlet\\Desktop\\manu metaheuristica\\textTSPACO.txt");
            //k++;
            double y = MinCostSalesMan(result, costs, ref final); ;
            wrote.WriteLine(y.ToString());
            Console.WriteLine();
            foreach (var item in final)
            {
                wrote.WriteLine(item.ToString());
            }

            wrote.Close();
            #endregion

            double sol = MinCostSalesMan(result, costs, ref final);
            //foreach (var item in final)
            //{
            //    Console.WriteLine(item.Item1 + " " + item.Item2);
            //}
            return sol;
        }

        static double GreedySalesMan(List<Tuple<int, double, double>> cities, ref List<Tuple<int, int>> final)
        {
            double better = double.MaxValue;
            for (int k = 0; k < cities.Count; k++)
            {
                bool flag = true;
                bool[] taked = new bool[cities.Count];
                List<Tuple<int, int>> path = new List<Tuple<int, int>>();
                for (int i = k; flag;)
                {
                    flag = false;
                    taked[i] = true;
                    int index = -1;
                    double dist = double.MaxValue;
                    for (int j = 0; j < cities.Count; j++)
                    {
                        if (j != i && !taked[j])
                        {
                            double temp = Math.Sqrt(Math.Pow(cities[i].Item2 - cities[j].Item2, 2)
                                + Math.Pow(cities[i].Item3 - cities[j].Item3, 2));
                            if (dist > temp)
                            {
                                dist = temp;
                                flag = true;
                                index = j;
                            }

                        }
                    }
                    if (flag)
                    {
                        path.Add(new Tuple<int, int>(i, index));
                        i = index;
                    }
                }
                double actual = 0;
                for (int i = 0; i < path.Count; i++)
                {
                    actual += Math.Sqrt(Math.Pow(cities[path[i].Item1].Item2 - cities[path[i].Item2].Item2, 2)
                                + Math.Pow(cities[path[i].Item1].Item3 - cities[path[i].Item2].Item3, 2));
                }
                actual += Math.Sqrt(Math.Pow(cities[path[0].Item1].Item2 - cities[path[path.Count - 1].Item2].Item2, 2)
                                + Math.Pow(cities[path[0].Item1].Item3 - cities[path[path.Count - 1].Item2].Item3, 2));
                if (better > actual)
                {
                    better = actual;
                    final = new List<Tuple<int, int>>();
                    for (int i = 0; i < path.Count; i++)
                    {
                        final.Add(new Tuple<int, int>(path[i].Item1, path[i].Item2));
                    }
                }

            }

            return better;
        }

        static double MinCostSalesMan(List<Tuple<int, double, double>> cities, double[,] costs, ref List<Tuple<int, int>> final)
        {
            double[,] pheromones = new double[cities.Count, cities.Count];
            for (int i = 0; i < pheromones.GetLength(0); i++)
            {
                for (int j = 0; j < pheromones.GetLength(1); j++)
                {
                    pheromones[i, j] = 1;
                }
            }
            int count = 0, ants = -1;
            double better = double.MaxValue;
            while (count++ <= 3000)
            {
                Console.WriteLine("Hola " + count);
                List<Ant> antsjagger = new List<Ant>();
                ants = -1;
                while (ants++ <= 50)
                {
                    antsjagger.Add(new Ant(pheromones.GetLength(0)));
                    List<Tuple<int, double, double>> result = new List<Tuple<int, double, double>>();
                    bool[] taked_cities = new bool[cities.Count];
                    int index = s.Next(0, cities.Count);
                    result.Add(cities[index]);
                    taked_cities[index] = true;
                    double actual = 0;
                    for (int j = 1; j < cities.Count;)  //Empiezo j en 1 xq ya anadi una ciudad, lo escribo xq soy medio monguito
                    {
                        Tuple<int, double> selectedcity = new Tuple<int, double>(index, 2);
                        double prob = s.NextDouble();
                        int k = index;
                        double probsum = CalculateSum(pheromones, costs, index, taked_cities);
                        for (int i = 0; i < pheromones.GetLength(0); i++)
                        {
                            if (!taked_cities[i])
                            {
                                if (Math.Abs(prob - selectedcity.Item2) > Math.Abs(prob - ((pheromones[k, i] * (1 / costs[k, i])) / probsum)))
                                {
                                    selectedcity = new Tuple<int, double>(i,
                                    (pheromones[k, i] * (1 / costs[k, i])) / probsum);
                                    index = i;
                                }
                            }
                        }

                        taked_cities[index] = true;
                        result.Add(cities[index]);
                        antsjagger[antsjagger.Count - 1].UpdateEdge(k, index);
                        j++;
                    }

                    for (int i = 0; i < result.Count - 1; i++)
                    {
                        actual += Math.Sqrt(Math.Pow(result[i].Item2 - result[i + 1].Item2, 2) +
                        Math.Pow(result[i].Item3 - result[i + 1].Item3, 2));
                    }

                    actual += Math.Sqrt(Math.Pow(result[0].Item2 - result[result.Count - 1].Item2, 2) +
                        Math.Pow(result[0].Item3 - result[result.Count - 1].Item3, 2));

                    if (better >= actual)
                    {
                        final = new List<Tuple<int, int>>();
                        better = actual;
                        for (int i = 0; i < result.Count - 1; i++)
                        {
                            final.Add(new Tuple<int, int>(result[i].Item1, result[i + 1].Item1));
                        }
                        UpdatePheromones(pheromones, antsjagger[antsjagger.Count - 1], costs);
                    }
                }

                double p = 0.001;
                for (int i = 0; i < pheromones.GetLength(0); i++)
                {
                    for (int j = 0; j < pheromones.GetLength(1); j++)
                    {
                        pheromones[i, j] = pheromones[i, j] * (1 - p);
                    }
                }

            }
            return better;
        }

        private static void UpdatePheromones(double[,] pheromones, Ant antsjagger, double[,] costs)
        {
            for (int i = 0; i < pheromones.GetLength(0); i++)
            {
                for (int j = 0; j < pheromones.GetLength(1); j++)
                {
                    double sum = 0;
                    if (antsjagger.visited[i, j])
                    {
                        sum += 1 / costs[i, j];
                    }
                    pheromones[i, j] = pheromones[i, j] + sum;

                }
            }

        }

        private static double CalculateSum(double[,] pheromones, double[,] costs, int index, bool[] taked)
        {
            double result = 0;
            for (int i = 0; i < pheromones.GetLength(0); i++)
            {
                if (!taked[i])
                    result += pheromones[index, i] * costs[index, i];
            }
            return result;
        }

        class Ant
        {
            public bool[,] visited { get; set; }

            public Ant(int dimentions)
            {
                this.visited = new bool[dimentions, dimentions];
            }

            public void UpdateEdge(int i, int j)
            {
                this.visited[i, j] = true;
            }
        }

        #endregion

    }
}
