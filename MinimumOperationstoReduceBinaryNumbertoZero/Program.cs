namespace MinimumOperationstoReduceBinaryNumbertoZero;

/// <summary>
/// 提供將二進位數字減少到零的最少操作次數計算功能。
/// </summary>
public static class Solution
{
    /// <summary>
    /// 計算將正整數 n 變為 0 的最少操作次數。
    /// 每次操作可以加或減 2^i（i ≥ 0）。
    /// </summary>
    /// <param name="n">要處理的正整數</param>
    /// <returns>最少操作次數</returns>
    /// <example>
    /// <code>
    /// int result = Solution.MinOperations(7); // 返回 2
    ///  7 (111) -> 8 (1000) -> 0，操作：+1, -8
    /// </code>
    /// </example>
    /// <remarks>
    /// 解題思路：
    /// 1. 將問題看作二進位制處理
    /// 2. 從低位到高位處理每一位，同時維護進位
    /// 3. 當前位值 = 原位值 + 進位
    ///    - 若值為 0：不需要操作
    ///    - 若值為 1：需要一次操作（減法消掉或加法產生進位）
    ///    - 若值為 2（位 1 + 進位 1）：直接進位，不增加操作
    /// 4. 對於連續的 1，使用加法產生進位更優（例如 111 -> 1000 只需 2 步）
    /// </remarks>
    public static int MinOperations(int n)
    {
        if (n <= 0)
        {
            return 0;
        }

        int operations = 0;
        int carry = 0;

        // 從低位到高位處理每一位
        while (n > 0 || carry > 0)
        {
            // 當前位值 = 原位值 + 進位
            int currentBit = (n & 1) + carry;

            // 根據當前位值決定操作
            switch (currentBit)
            {
                // 值為 0：不需要操作，無進位
                case 0:
                    carry = 0;
                    break;

                // 值為 1：需要判斷下一位來決定最優策略
                // 如果下一位也是 1，則用加法產生進位更優（處理連續 1 的情況）
                case 1 when ((n >> 1) & 1) == 1:
                    operations++;
                    carry = 1;
                    break;

                // 值為 1 且下一位是 0：直接減法消掉
                case 1:
                    operations++;
                    carry = 0;
                    break;

                // 值為 2（位 1 + 進位 1）：直接進位，不增加操作
                case 2:
                    carry = 1;
                    break;

                // 值為 3（理論上不會出現，但為了完整性）
                default:
                    operations++;
                    carry = 1;
                    break;
            }

            // 右移處理下一位
            n >>= 1;
        }

        return operations;
    }
}

/// <summary>
/// 程式進入點，包含測試驗證。
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Minimum Operations to Reduce Binary Number to Zero ===\n");

        // 執行測試案例
        RunTests();
    }

    /// <summary>
    /// 執行所有測試案例並輸出結果。
    /// </summary>
    private static void RunTests()
    {
        var testCases = new (int Input, int Expected, string Description)[]
        {
            (1, 1, "1 (1) -> 0，操作：-1"),
            (2, 1, "2 (10) -> 0，操作：-2"),
            (3, 2, "3 (11) -> 4 (100) -> 0，操作：+1, -4"),
            (4, 1, "4 (100) -> 0，操作：-4"),
            (5, 2, "5 (101) -> 4 -> 0 或 5 -> 0（-4, -1）"),
            (6, 2, "6 (110) -> 8 (1000) -> 0，操作：+2, -8"),
            (7, 2, "7 (111) -> 8 (1000) -> 0，操作：+1, -8"),
            (8, 1, "8 (1000) -> 0，操作：-8"),
            (15, 2, "15 (1111) -> 16 (10000) -> 0，操作：+1, -16"),
            (16, 1, "16 (10000) -> 0，操作：-16"),
            (31, 2, "31 (11111) -> 32 -> 0"),
            (39, 3, "39 (100111) -> 40 (101000) -> 32 -> 0"),
            (100, 3, "100 (1100100) 需要 3 次操作"),
            (0, 0, "0 已經是 0，不需要操作"),
        };

        int passed = 0;
        int failed = 0;

        foreach (var (input, expected, description) in testCases)
        {
            int actual = Solution.MinOperations(input);
            bool success = actual == expected;

            if (success)
            {
                passed++;
                Console.WriteLine($"✅ PASS: n={input,-4} -> 操作次數={actual,-2} | {description}");
            }
            else
            {
                failed++;
                Console.WriteLine($"❌ FAIL: n={input,-4} -> 預期={expected}, 實際={actual} | {description}");
            }
        }

        Console.WriteLine($"\n=== 測試結果：{passed} 通過，{failed} 失敗 ===");

        // 額外展示二進位制轉換過程
        Console.WriteLine("\n=== 詳細二進位制分析 ===");
        ShowBinaryAnalysis(7);
        ShowBinaryAnalysis(39);
        ShowBinaryAnalysis(100);
    }

    /// <summary>
    /// 展示數字的二進位制分析過程。
    /// </summary>
    /// <param name="n">要分析的數字</param>
    private static void ShowBinaryAnalysis(int n)
    {
        Console.WriteLine($"\n數字 {n} 的二進位表示：{Convert.ToString(n, 2)}");
        Console.WriteLine($"最少操作次數：{Solution.MinOperations(n)}");
    }
}
