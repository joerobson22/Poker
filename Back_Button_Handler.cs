using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA_Computer_Science_Poker__️__️__️__️
{
    public class Back_Button_Handler
    {
        public List<Form> Stack = new List<Form>();
        private int TopPointer = -1;

        public void Push(Form Window)
        {
            Stack.Add(Window);
            TopPointer += 1;
        }

        public Form Pop()
        {
            Form Window = Stack[TopPointer];
            Stack.RemoveAt(TopPointer);
            TopPointer -= 1;
            return Window;
        }

    }
}
