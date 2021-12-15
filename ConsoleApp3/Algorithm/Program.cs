using System;
using System.Collections;
using System.Collections.Generic;

namespace Algorithm
{
    class Program
    {
        static public void solutionDrawTriangle(int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
        }

        static void solutionComb(int[] nums)
        {
            List<int> results = new List<int>();
            bool[] used = new bool[nums.Length];

            recur(0);

            void recur(int idx)
            {
                if (idx == 3)
                {
                    for (int i = 0; i < results.Count; i++)
                    {
                        Console.Write(results[i]);
                    }
                    Console.WriteLine();

                    return;
                }

                for (int i = 0; i < nums.Length; i++)
                {
                    if (used[i])
                    {
                        continue;
                    }

                    used[i] = true;
                    results.Add(nums[i]);

                    recur(i + 1);

                    results.RemoveAt(results.Count - 1);
                    used[i] = false;
                }
            }
        }

        static public int solutionSpy(string[,] clothes)
        {
            Dictionary<string, int> typeByCnt = new Dictionary<string, int>();

            for (int i = 0; i < clothes.GetLength(0); i++)
            {
                var name = clothes[i, 0];
                var type = clothes[i, 1];

                if (typeByCnt.ContainsKey(type))
                {
                    typeByCnt[type]++;
                }
                else
                {
                    typeByCnt.Add(type, 1);
                }
            }

            int cnt = 1;

            foreach (var item in typeByCnt)
            {
                cnt *= (item.Value + 1);
            }

            return cnt - 1;
        }

        static public int[] solutionSimul01(int[,] v)
        {
            int[] answer = { 0, 0 };

            Dictionary<int, int> xCnt = new Dictionary<int, int>();
            Dictionary<int, int> yCnt = new Dictionary<int, int>();

            for (int i = 0; i < v.GetLength(0); i++)
            {
                if (xCnt.ContainsKey(v[i, 0]) == false)
                {
                    xCnt.Add(v[i, 0], 1);
                }
                else
                {
                    xCnt[v[i, 0]]++;
                }

                if (yCnt.ContainsKey(v[i, 1]) == false)
                {
                    yCnt.Add(v[i, 1], 1);
                }
                else
                {
                    yCnt[v[i, 1]]++;
                }
            }

            foreach (var x in xCnt)
            {
                if (x.Value == 1)
                {
                    answer[0] = x.Key;
                }
            }

            foreach (var y in yCnt)
            {
                if (y.Value == 1)
                {
                    answer[1] = y.Key;
                }
            }

            return answer;
        }

        static void Main(string[] args)
        {
            solutionComb(new int[] { 1, 2, 3 });

            return;

            int lotteryTotalCnt = 45;
            int lotterySelectNumCnt = 6;

            int lotteryDest = lotteryTotalCnt - lotterySelectNumCnt;

            ulong lotteryCases = 1;

            ulong divide = 1;
            ulong lotRes = 1;

            for (int i = 0; i < lotterySelectNumCnt; i++)
            {
                lotRes *= ((ulong)(lotteryTotalCnt - i)) / (ulong)(6 - i);
            }

            Console.WriteLine(lotRes); // / divide);

            return;

            solutionDrawTriangle(5);
            return;

            int[,] v = new int[,]
                {
                    { 1,1 },
                    {  2,2 },
                    {  1,2}
                };
            var resultSimul = solutionSimul01(v);

            Console.WriteLine(resultSimul[0] + " , " + resultSimul[1]);
            return;
            //var clothes = new string[,]
            //{
            //    {"A", "Q"}
            //    , {"B", "W"}
            //    , {"C", "Q" }
            //};

            //var result = solution(clothes);

            //var result02 = solution(4, new int[] { 4, 3, 3 });

            //Console.WriteLine(result02);

            int[] nums = new int[] { 1, 1 };
            int targetNum = 3;

            solution_makeTargetNum(nums, targetNum);
        }

        #region ====:: 숫자 조합해서 Target 숫자 만들기 ::====

        static public int solution_makeTargetNum(int[] numbers, int target)
        {
            int totalCnt = (int)MathF.Pow(2, numbers.Length);

            for (int i = 0; i < totalCnt; i++)
            {
                for (int j = 0; j < numbers.Length; j++)
                {
                    Console.Write(numbers[j] * (i % 2 == 0 ? 1 : -1));
                    Console.Write(" , ");
                }

                Console.WriteLine();
            }

            Console.WriteLine(totalCnt);

            int answer = 0;



            return answer;
        }

        #endregion

        #region ====:: 네트워크 ::====
        //int n = 3;
        //int[,] computers = new int[3, 3]
        //{
        //    { 1,1,0 },
        //    { 1,1,0 },
        //    { 0,0,1 }
        //};

        ///// 1
        //int n = 3;
        //int[,] computers = new int[3, 3]
        //{
        //    { 1,1,0 },
        //    { 1,1,1 },
        //    { 0,1,1 }
        //};

        ////// 2개
        //int n = 4;
        //int[,] computers = new int[,]
        //{
        //    { 1,1,1, 0 },
        //    { 1,1,0, 0 },
        //    { 1,0,1, 0 },
        //    { 0,0,0 ,1 }
        //};

        ///// 1개
        //int n = 4;
        //int[,] computers = new int[,]
        //{
        //    { 1, 1, 0, 1},
        //    { 1, 1, 0, 0},
        //    { 0, 0, 1, 1},
        //    {1, 0, 1, 1 }
        //};

        ///// 1개
        //int n = 3;
        //int[,] computers = new int[,]
        //{
        //    {1, 1, 0},
        //    { 1, 1, 1},
        //    { 0, 1, 1}
        //};

        //// 2개
        //int n = 3;
        //int[,] computers = new int[,]
        //{
        //    {1, 1, 0},
        //    { 1, 1, 0},
        //    { 0, 0, 1}
        //};

        //// 4 개
        //int n = 4;
        //int[,] computers = new int[,]
        //{
        //    {1, 0, 0, 0},
        //    { 0, 1, 0, 0},
        //    { 0, 0, 1, 0},
        //    {  0, 0, 0, 1 }
        //};

        /// 2개 
        //int n = 4;
        //int[,] computers = new int[,]
        //{
        //    { 1,0 ,0,1 },
        //    { 0,1,0,0 },
        //    { 0,0,1,1 },
        //    { 1,0,1,1 }
        //};
        static public bool TheseSameGroup(List<List<int>> groups, int src, int dst)
        {
            int srcGroup = groups.FindIndex(t => t.Exists(t02 => t02 == src));
            int dstGroup = groups.FindIndex(t => t.Exists(t02 => t02 == dst));

            return srcGroup == dstGroup;
        }

        /// <summary> https://programmers.co.kr/learn/courses/30/lessons/43162#qna </summary>
        /// 내가하는 테스트케이스는 다 되는데 자꾸 실패나서 중단 
        static public int solution_network(int n, int[,] computers)
        {
            int result = n;

            /// network 관계를 하나의 정수형 idx 로 변환하여 grouping 
            List<List<int>> groups = new List<List<int>>();
            HashSet<int> decreaseApplied = new HashSet<int>();

            /// 먼저 컴터 하나당 하나의 네트워크 그룹으로 묶음
            for (int i = 0; i < n; i++)
            {
                groups.Add(new List<int>() { i });
            }

            ///// 1 차원 배열로 나열했을때 src 되는 idx 
            for (int i = 0; i < n; i++)
            {
                int src = i;

                for (int j = i + 1; j < n; j++)
                {
                    int dst = j;

                    if (i == j)
                    {
                        continue;
                    }

                    bool isConnected = computers[src, dst] == 1;

                    if (isConnected)
                    {
                        bool isAlreadyInGroup = false;

                        if (TheseSameGroup(groups, src, dst) == false)
                        {
                            int removeAt = groups.FindIndex(t => t.Exists(t02 => t02 == dst));
                            if (groups[removeAt].Count == 1)
                            {
                                groups.RemoveAt(removeAt);
                            }
                            else
                            {
                                groups[removeAt].RemoveAt(groups[removeAt].FindIndex(t => t == dst));
                            }

                            var srcGroupIdx = groups.FindIndex(t => t.Exists(t02 => t02 == src));

                            if (groups[srcGroupIdx].Exists(t => t == dst) == false)
                            {
                                groups[srcGroupIdx].Add(dst);
                            }

                            result--;
                        }
                    }
                }
            }

            return result;

            /// Computer 개수만큼 Loop 
            //for (int i = 0; i < n * n; i++)
            //{
            //    /// 사실상 row , col
            //    int computerA = i / n;
            //    int computerB = i % n;
            //    bool isConnected = computers[computerA, computerB] == 1;

            //    Console.WriteLine($"A : {computerA} , B : {computerB} is Connected ?? : {isConnected}");

            //    bool self = computerA == computerB;

            //    /// self 인데 , 다른데 이미 link 정보가 있다 (?) 그럼 애는 이미 다른 
            //    /// Group 의 일부 노드로 이미 연결된거. 즉 
            //    ///     => 독립 network 노노 
            //    if (self)
            //    {

            //    }
            //    else
            //    {
            //        if (isConnected)
            //        {

            //        }
            //    }

            //    if (isConnected)
            //    {

            //    }
            //    else
            //    {

            //    }

            //    //Console.WriteLine($"idx : " + i + " , 's row is : " + row + " col is : " + col + $" , 즉 컴터 {row} 와 , {col} 는 {computers[row, col]} ");
            //}

            //return result;
        }
        #endregion

        static public long solution(int n, int[] works)
        {
            Array.Sort(works, (t1, t2) => t2.CompareTo(t1));

            int switchIdx = FindSwitchIdx(ref works, 1);

            while (n > 0)
            {
                works[0]--;
                n--;

                if (works[0] < 0)
                {
                    works[0] = 0;
                    break;
                }

                if (works[0] < works[switchIdx])
                {
                    var tmp = works[0];
                    works[0] = works[switchIdx];
                    works[switchIdx] = tmp;
                    switchIdx = FindSwitchIdx(ref works, switchIdx);
                }
            }

            int sum = 0;

            for (int i = 0; i < works.Length; i++)
            {
                if (works[i] == 0)
                {
                    break;
                }

                sum += (works[i] * works[i]);
            }

            return sum;
        }

        static int FindSwitchIdx(ref int[] target, int startIdx)
        {
            if (startIdx != 1 && target[startIdx - 1] != target[startIdx])
            {
                return startIdx - 1;
            }

            for (int i = startIdx; i < target.Length; i++)
            {
                if (i == target.Length - 1)
                {
                    break;
                }

                int t = target[i];
                int tNext = target[i + 1];

                if (t != tNext)
                {
                    return i;
                }
            }

            return target.Length - 1;
        }

        static int solution(string[,] clothes)
        {
            return 0;
        }
    }
}
