using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTimers
{
    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int x) { val = x; }
    }

    public class Solution
    {
        public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            int val = 0;
            ListNode prenode = new ListNode(0);
            ListNode lastnode = prenode;
            while (l1 != null || l2 != null || val != 0)
            {
                val = val + (l1 == null ? 0 : l1.val) + (l2 == null ? 0 : l2.val);
                lastnode.next = new ListNode(val % 10);
                lastnode = lastnode.next;
                val = val / 10;
                l1 = l1 == null ? null : l1.next;
                l2 = l2 == null ? null : l2.next;
            }
            return prenode.next;
        }
    }

    public class Test
    {
        static ListNode generateList(int[] vals)
        {
            ListNode res = null;
            ListNode last = null;
            foreach (var val in vals)
            {
                if (res is null)
                {
                    res = new ListNode(val);
                    last = res;
                }
                else
                {
                    last.next = new ListNode(val);
                    last = last.next;
                }
            }
            return res;
        }

        static void printList(ListNode l)
        {
            while (l != null)
            {
                Console.Write($"{l.val}, ");
                l = l.next;
            }
            Console.WriteLine("");
        }

        public static void Main()
        {
            //var l1 = generateList(new int[] { 1, 5, 7 });
            //var l2 = generateList(new int[] { 9, 8, 2, 9 });
            //printList(l1);
            //printList(l2);
            //Solution s = new Solution();
            //var sum = s.AddTwoNumbers(l1, l2);
            //printList(sum);

            string str = "au";
            int s_len = str.Length;
            int ret = 0;
            for (int i = 0; i < s_len; i++)
            {
                for (int y = i+1; y <= s_len; y++)
                {
                    string temp = str.Substring(i, y-i); 
                    string temp_next = "";
                    if (y < s_len)
                    {
                        temp_next = str.Substring(y, 1);
                    }
                    if (!string.IsNullOrEmpty(temp_next) && temp.IndexOf(temp_next)>-1)
                    {
                        if (ret< temp.Length)
                        {
                            ret = temp.Length;
                        }
                        break;
                    }
                    else
                    {
                        if (ret < temp.Length)
                        {
                            ret = temp.Length;
                        }
                    }
                }
            }

            Console.WriteLine(ret);

            Console.WriteLine("按任意键退出程序。");
            Console.ReadLine();
        }
    }
}
