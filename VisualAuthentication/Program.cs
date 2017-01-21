using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VisualAuthentication
{
    public class Rastr
    {
        private bool[,] rastr;

        public bool this[int x, int y]
        {
            get { return rastr[x, y]; }
            set { rastr[x, y] = value; }
        }

        public Rastr(bool[,] rastr)
        {
            this.rastr = rastr;
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public override string ToString()
        {
            var res = new StringBuilder();
            for (var i = 0; i <= rastr.GetUpperBound(0); ++i)
            {
                for (var j = 0; j <= rastr.GetUpperBound(1); ++j)
                {
                    res.Append(rastr[i, j] ? "1" : "0");
                }
                res.AppendLine();
            }
            return res.ToString();
        }

        public int Lengh
        {
            get { return rastr.GetUpperBound(0) + 1; }
        }

        public string GetString(int index)
        {
            var res = new StringBuilder();
            for (var j = 0; j <= rastr.GetUpperBound(1); ++j)
            {
                res.Append(rastr[index, j] ? "1" : "0");
            }

            return res.ToString();
        }

        public override bool Equals(object obj)
        {
            var other = (Rastr) obj;
            if (other.rastr.GetUpperBound(0) != rastr.GetUpperBound(0) ||
                other.rastr.GetUpperBound(1) != rastr.GetUpperBound(1))
            {
                return false;
            }

            for (var i = 0; i <= rastr.GetUpperBound(0); ++i)
            {
                for (var j = 0; j <= rastr.GetUpperBound(1); ++j)
                {
                    if (rastr[i, j] != other.rastr[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    public class Table
    {
        private static Random random = new Random(new Guid().GetHashCode());
        private Rastr[,] rastrs;
        private int[,] nums;
        public Rastr this[int x, int y]
        {
            get { return rastrs[x, y]; }
            set { rastrs[x, y] = value; }
        }

        public Table(Rastr[,] rastrs)
        {
            this.rastrs = rastrs;
        }

        public override string ToString()
        {
            var res = new StringBuilder();
            res.Append("  ");
            for (var i = 0; i <= rastrs.GetUpperBound(0); ++i)
            {
                res.Append($"{random.Next(0, rastrs.GetUpperBound(0) + 1)}     ");
            }
            res.AppendLine();

            for (var i = 0; i <= rastrs.GetUpperBound(0); ++i)
            {
                var first = rastrs[i, 0];
                res.Append($"{random.Next(0, rastrs.GetUpperBound(0) + 1)}");
                for (var k = 0; k < first.Lengh; ++k)
                {
                    for (var j = 0; j <= rastrs.GetUpperBound(1); ++j)
                    {
                        res.Append((k == 0 && j == 0 ? " " : "  ") + rastrs[i, j].GetString(k));
                    }
                    res.AppendLine();
                }
            }

            return res.ToString();
        }

        public class Program
        {
            private static Random random = new Random(new Guid().GetHashCode());

            public static void Main(string[] args)
            {
                var u = 4;
                var v = 4;
                var n = 10;
                var x = 5;
                var y = 5;

                var aSet = Enumerable.Range(0, n).Select(x1 => GenerateRastr(u, v)).ToList();
                var nonASet = new List<Rastr>();
                while (nonASet.Count != n)
                {
                    var rastr = GenerateRastr(u, v);
                    if (!aSet.Contains(rastr))
                    {
                        nonASet.Add(rastr);
                    }
                }

                var result = new List<Table>();
                var r = new Random(new Guid().GetHashCode());

                for (var k = 0; k < 4*5; ++k)
                {
                    var table = new Table(new Rastr[x, y]);
                    for (var i = 0; i < x; ++i)
                    {
                        for (var j = 0; j < y; j++)
                        {
                            var fromA = r.NextDouble() < 0.5;
                            if (fromA)
                            {
                                table[i, j] = aSet[random.Next(0, aSet.Count)];
                            }
                            else
                            {
                                table[i, j] = nonASet[random.Next(0, nonASet.Count)];
                            }
                        }
                    }

                    result.Add(table);
                }

                File.WriteAllLines("tables.txt", result.Select(z => z.ToString()));
                File.WriteAllLines("key.txt", aSet.Select(z => z.ToString()));
                File.WriteAllLines("r_without_key.txt", nonASet.Select(z => z.ToString()));
            }

            public static Rastr GenerateRastr(int u, int v)
            {
                var rastr = new bool[u, v];
                for (var i = 0; i < u; ++i)
                {
                    for (var j = 0; j < v; ++j)
                    {
                        rastr[i, j] = random.Next(0, 2) == 1;
                    }
                }

                return new Rastr(rastr);
            }
        }
    }
}