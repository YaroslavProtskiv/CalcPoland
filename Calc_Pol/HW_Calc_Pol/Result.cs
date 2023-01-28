using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_Calc_Pol
{
    class Result
    {
        public Value result = new Value();

        public Result(string value)
        {
            // видаляємо пробіли з виразу
            value = value.Replace(" ", "");
            // найпростіше виправлення баху при варіанті коли останнім значенням є ")"
            value = (value[value.Length - 1] == ')') ? value + "+0" : value;//bed code
            result = Ostend(value);
        }
           /// <summary>
           /// Перевіряємо чи кількість відкритих дужок = кількості закритих дужок. Якщо так, викликаємо медод відкриття дужок
           /// </summary>
           /// <param name="value"></param>
           /// <returns></returns>
        public Value Ostend(string value)
        {
            int nOpen = 0, nClose = 0;
            foreach (char item in value)
            {
                nOpen += (item == '(') ? 1 : 0;
                nClose += (item == ')') ? 1 : 0;
            }
            if (nOpen != nClose)
            {
                Console.WriteLine("This formula is not correct!!! Number '(' should be equal to number ')'!!!");
                return result;
            }
            int i = 0;
            return OpenBracket(value, ref i);

        }

        /// <summary>
        /// Метод відкриття дужок. Розбиває вираз на List чисел та List операцій. Якщо число є виразом в дужках - відкриває дужки. Запускаємо медод для проведення арифметичних операцій
        /// </summary>
        /// <param name="value"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private Value OpenBracket(string value, ref int n)
        {
            Value intermediate = new Value();
            List<char> operation = new List<char>();
            List<double> values = new List<double>();
            string _valueInValues = "";

            for (int i = n; i < value.Length; i++)
            {
                // відкриваємо дужки
                if (value [i] == '(')
                {
                    
                    int newIndex = i+1;
                    intermediate = OpenBracket(value, ref newIndex);
                    if (intermediate.isNumber == false)
                    {
                        Console.WriteLine("Expression in parentheses is not correct");
                        return new Value();
                    }
                    else
                    {
                        //intermediate = IsValueCorrect(_valueInValues);
                        values.Add(intermediate.number);
                        i = newIndex;
                        _valueInValues = "";
                    }
                }
                // закриваємо дужки
                else if (value[i] == ')')
                {
                    intermediate = IsValueCorrect(_valueInValues);
                    values.Add(intermediate.number);
                    n = i;
                    return Arithmetic(operation, values);
                }
                // залежно символу додаємо його в список мат. операцій чи формуємо як число
                else
                {
                    if (value[i] == '-' && _valueInValues == "" && values.Count == 0)
                    {
                        _valueInValues += value[i];
                    }
                    else if (value[i] == '+' || value[i] == '-' || value[i] == '*' || value[i] == '/')
                    {
                        if (_valueInValues != "")
                        {
                            intermediate = IsValueCorrect(_valueInValues);
                            values.Add(intermediate.number);
                        }

                        operation.Add(value[i]);

                        _valueInValues = "";
                    }
                    else
                    {
                        _valueInValues += value[i];
                    }
                }
            }
            

            intermediate = IsValueCorrect(_valueInValues);
            values.Add(intermediate.number);

            return Arithmetic(operation, values);
        }

        /// <summary>
        /// Метод арифметичних операцій. Виконує арифметичні дії. Першочергово виконуються операції "*" та "/" викликом методу для цих операцій
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private Value Arithmetic(List<char> operation, List<double> values)
        {
            Value result = new Value();
            // кількість дій має бути на 1 менше кількості чисел
            if (operation.Count+1 != values.Count)
            {
                Console.WriteLine("Check the entered values!! Operations too much for this values!!");
                return result;
            }
            // виконуємо першочергово множення та ділення
            if (operation.Contains('*') == true || operation.Contains('/') == true)
            {
                ProductAndPart(ref operation, ref values);  
            }
            // виконуємо додавання та віднімання в перше значення List
            for (int i = 0; i < operation.Count; i++)
            {
                values[0] += (operation[i] == '-') ? (-1) * values[i + 1] : values[i + 1]; 
            }
            result.number = values[0]; result.isNumber = true;
            return result;
        }

        /// <summary>
        /// Метод множення та ділення
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="values"></param>
        private void ProductAndPart (ref List<char> operation, ref List<double> values)
        {
            /*
             * 1.перевіряємо по черзі дії множення та ділення. 
             * 2.перемножуємо/ділимо числа, одне число в списку заміняємо на результат та видаляємо використані дію та інше число з List 
             * **/
            if (operation.Contains('*') == true)
            {
                int i = operation.IndexOf('*');
                values[i + 1] *= values[i];
                values.Remove(values[i]);// не видаляє даний рядок, а бере значення цього рядка і видаляє перше співпадіння
                operation.Remove(operation[i]);
                ProductAndPart(ref operation, ref values);
            }
            else if (operation.Contains('/') == true)
            {
                int i = operation.IndexOf('/');
                values[i + 1] = values[i] / values[i + 1];
                values.Remove(values[i]);
                operation.Remove(operation[i]);
                ProductAndPart(ref operation, ref values);
            }
            else
                return;
            
        }

        /// <summary>
        /// метод перевірки коректності числа
        /// </summary>
        /// <param name="_valueInValues"></param>
        /// <returns></returns>
        private Value IsValueCorrect(string _valueInValues)
        {
            Value result = new Value();
            result.isNumber = double.TryParse(_valueInValues, out result.number);
            if (result.isNumber == false)
                Console.WriteLine("The part \"" + _valueInValues + "\" is not value! Check the entered values!!");

            return result;
            
        }
    }
}
