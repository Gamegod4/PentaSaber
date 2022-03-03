using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.XR;

namespace PentaSaber.InputControllers
{
    public class StandardInputController : IInputController
    {
        private InputDevice leftController; 
        private InputDevice rightController;
        public float TriggerThreshold = 0.85f;
        private static int rightFailedTries = 0;
        private static int leftFailedTries = 0;
        public bool SaberAToggled => LeftTriggerActive;

        public bool SaberBToggled => RightTriggerActive;

        internal InputDevice LeftController
        {
            get
            {
                if (leftController.isValid)
                {
                    leftFailedTries = 0;
                    return leftController;
                }
                leftFailedTries++;
                if (leftFailedTries > 100)
                {
                    leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                    leftFailedTries = 0;
                }
                return leftController;
            }
            set => leftController = value;
        }
        internal InputDevice RightController
        {
            get
            {
                if (rightController.isValid)
                {
                    rightFailedTries = 0;
                    return rightController;
                }
                rightFailedTries++;
                if (rightFailedTries > 100)
                {
                    rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                    rightFailedTries = 0;
                }
                return rightController;
            }
            set => rightController = value;
        }


        public bool RightTriggerActive
        {
            get
            {
                InputDevice device = RightController;
                if (device.isValid)
                {
                    if (device.TryGetFeatureValue(CommonUsages.trigger, out float value))
                    {
                        bool active = value > TriggerThreshold;
                        return active;
                    }
                }
                return false;
            }
        }

        public bool LeftTriggerActive
        {
            get
            {
                InputDevice device = LeftController;
                if (device.isValid)
                {
                    if (device.TryGetFeatureValue(CommonUsages.trigger, out float value))
                    {
                        bool active = value > TriggerThreshold;
                        return active;
                    }
                }
                return false;
            }
        }
    }
}
