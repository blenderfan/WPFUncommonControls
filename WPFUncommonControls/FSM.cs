using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUncommonControls
{
    public class FSM
    {

        public List<object> States { get; set; }
        public List<Tuple<object, object, object, object>> Transitions { get; set; }

        public int StartState { get; set; }
        public int CurrentState { get; set; }
        public List<int> FinalStates { get; set; }

        public void AddTransition(object node, object input, object nextNode, object output)
        {
            Transitions.Add(new Tuple<object, object, object, object>(node, input, nextNode, output));
        }

        public FSM()
        {
            States = new List<object>();
            Transitions = new List<Tuple<object, object, object, object>>();
            StartState = 0;
            CurrentState = 0;
            FinalStates = new List<int>();
        }

    }
}
