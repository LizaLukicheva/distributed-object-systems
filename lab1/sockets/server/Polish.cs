using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polska
{
    public class PolishString
    {
        public PolishString(String expression)
        {
            this.casual = expression;
            createPolska();
        }

        private Boolean isDigit(char c)
        { 
            return ( c >= '0' && 
                c <= '9');
        }

        private int priority(char c)
        {
            switch(c)
            {
                case '/':
                case '*': return 3;

                case '+':
                case '-':  return 2;

                case '(': return 1;

                case ')': return 0;

                default: return -1;
            }
        }

        public void createPolska() 
        {
            var tempStack = new Stack<char>();
            int maxStackPriority = 0;

            for (int i=0; i<this.casual.Length; i++)
            {
                char currentChar = this.casual[i];

                if (isDigit(currentChar))
                {
                    if (i>0 && (!isDigit(this.casual[i-1])))
                    {
                        this.polska = this.polska + ' ';
                    }
                    this.polska = this.polska + currentChar;
                } 
                else if (priority(currentChar) > 1)
                {
                    while (true)
                    {
                        if (tempStack.Count == 0 || priority(tempStack.First()) < priority(currentChar))
                        {
                            tempStack.Push(currentChar);
                            maxStackPriority = priority(currentChar);
                            break;
                        }
                        else if (priority(currentChar) <= priority(tempStack.First()))
                        {
                            do
                            {
                                this.polska = this.polska + ' ' + tempStack.Pop();
                            }
                            while (priority(currentChar) <= priority(tempStack.First()));
                        }
                        else break;
                    }
                }
                else if (currentChar == '(')
                {
                    tempStack.Push(currentChar);
                }
                else if (currentChar == ')')
                {
                    while(tempStack.First() != '(')
                    {
                        this.polska = this.polska + ' ' + tempStack.Pop();
                    }
                    tempStack.Pop();
                }
            }

            while (tempStack.Count != 0) 
            {
                char charFromTemp = tempStack.Pop();
                if (priority(charFromTemp) > 1)
                {
                    this.polska = this.polska + ' ' + charFromTemp;
                }
            }
        }

        public int calculatePolska()
        {
            var tempStack = new Stack<int>();

            int i = 0;
            int nextSpacePos = -1;

            while (i < polska.Length)
            {
                nextSpacePos = polska.IndexOf(' ', i + 1);
                if (nextSpacePos == -1) nextSpacePos = polska.Length;

                String readedVal = polska.Substring(i, nextSpacePos - i);

                if (readedVal.Length > 1 || isDigit(readedVal[0]))
                {
                    tempStack.Push(Convert.ToInt32(readedVal));
                }
                else
                {
                    int calcTwoNumbers = 0;
                    char sign = readedVal.First();
                    int second = tempStack.Pop(), first = tempStack.Pop();
                    switch (sign)
                    {
                        case '+': calcTwoNumbers = first + second; break;
                        case '-': calcTwoNumbers = first - second; break;
                        case '*': calcTwoNumbers = first * second; break;
                        case '/': calcTwoNumbers = first / second; break;
                    }
                    tempStack.Push(calcTwoNumbers);
                }
                i = nextSpacePos + 1;
            }

            return tempStack.First();
        }

        public String getExpression()
        {
            return this.casual;
        }

        public String getPolska()
        {
            return this.polska;
        }

        private String casual;
        private String polska;
        private string p;
    }

    //class Polish
    //{
    //    static void Main(string[] args)
    //    {
    //        while (true)
    //        {
    //            string readedString = System.Console.ReadLine();
    //            var p = new PolishString(readedString);
    //            System.Console.WriteLine(p.getPolska() + " = " + Convert.ToString(p.calculatePolska()));
    //        }
    //    }
    //}
}
