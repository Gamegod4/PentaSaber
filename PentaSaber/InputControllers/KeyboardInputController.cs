using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PentaSaber.InputControllers
{
    public class KeyboardInputController : ITestInputController
    {
        public bool NoteAToggled => Input.GetKey(KeyCode.H);

        public bool NoteBToggled => Input.GetKey(KeyCode.J);

        public bool NeutralToggled => Input.GetKey(KeyCode.K);

        public bool SaberAToggled => Input.GetKey(KeyCode.B);

        public bool SaberBToggled => Input.GetKey(KeyCode.N);
    }
}
